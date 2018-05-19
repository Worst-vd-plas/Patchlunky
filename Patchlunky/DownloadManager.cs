/*
 * Copyright (c) 2018, Worst-vd-plas
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Net;
using System.Windows.Forms;

namespace Patchlunky
{
    public enum DownloadState
    {
        None = 0,
        Queued,
        Downloading,
        Finished,
        Canceled,
        Failed,
    }

    public enum DownloadType
    {
        Unknown = 0,
        Mod = 1,
        Character = 2,
    }

    public struct FileDownload
    {
        public long Id;
        public DownloadState State;
        public DownloadState NewState; //Used to keep track of changes
        public DownloadType Type;
        public String DownloadUrl;
        public String Filename;
        public int ProgressPercentage;
        public long BytesReceived;
        public long TotalBytesToReceive;
        public string Message; //May contain error information

        public override string ToString()
        {
            return "#" + Id + " [" + Enum.GetName(typeof(DownloadType), Type) + "] " + (Filename != null ? Filename : DownloadUrl) + " ("
                       + ((State != DownloadState.Downloading) ? (((float)TotalBytesToReceive/1024/1024).ToString("0.##") + " MB") :
                       (((float)BytesReceived/1024/1024).ToString("0.##") + " / " + ((float)TotalBytesToReceive/1024/1024).ToString("0.##") + " MB")) + ") "
                       + ProgressPercentage + "% - " + Enum.GetName(typeof(DownloadState), State)
                       + (Message != null ? " - " + Message : "");
        }

        public string Name()
        {
            return (Filename != null ? Filename : DownloadUrl);
        }
    }

    public static class DlManagerProcess
    {
        public static DownloadManager downloadManager;
        public static AutoResetEvent WebClientARE;

        public static void StartThread(CancellationToken ct)
        {
            //Thread.CurrentThread.Name = "DownloadManager thread";
            Msg.Log("DownloadManager thread started.");

            WebClientARE = new AutoResetEvent(true);

            downloadManager = new DownloadManager(ct);
            downloadManager.Run();

            WebClientARE.Dispose();

            Msg.Log("DownloadManager thread stopping.", true);
        }
    }

    public class DownloadManager
    {
        private CancellationToken CancelToken;
        private List<FileDownload> Downloads;
        private WebClient WebClient;
        private long ActiveDownloadId;

        public DownloadManager(CancellationToken ct)
        {
            this.CancelToken = ct;
            this.Downloads = new List<FileDownload>();
            this.WebClient = new WebClient();
            this.ActiveDownloadId = -1;

            string useragent = "Patchlunky/" + Program.mainForm.PatchlunkyVersion + " (Mod Manager for SpelunkyHD)";
            WebClient.Headers.Add("user-agent", useragent);
            WebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
            WebClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(DownloadDataCompleted);
        }

        //Main loop for the DownloadManager
        public void Run()
        {
            while (!CancelToken.IsCancellationRequested)
            {
                //Go through the localmessage queue
                HandleInboundMessages();

                //Handle state changes in downloads
                HandleStateChanges();

                //Start queued downloads as appropiate
                HandleQueuedDownloads();

                Thread.Sleep(100);
            }

            //Cancel the active download, if there is one
            WebClient.CancelAsync();

            //Wait for the webclient to finish
            DlManagerProcess.WebClientARE.WaitOne();

            //Cleanup
            WebClient.Dispose();
        }

        //Handles starting queued downloads
        private void HandleQueuedDownloads()
        {
            if (Downloads.Count == 0)
                return;

            for (int i = 0; i < Downloads.Count; i++)
            {
                FileDownload dl = Downloads[i];

                //TODO: Concurrent downloads
                if (dl.State == DownloadState.Queued)
                {
                    if (ActiveDownloadId == -1) //If no active download
                    {
                        ActiveDownloadId = dl.Id;
                        dl.NewState = DownloadState.Downloading;

                        Downloads[i] = dl; //Update the list values
                    }
                }
            }
        }

        //Handles state changes in the download list
        private void HandleStateChanges()
        {
            if (Downloads.Count == 0)
                return;

            for (int i = 0; i < Downloads.Count; i++)
            {
                FileDownload dl = Downloads[i];

                //If the state has changed.
                if (dl.State != dl.NewState)
                {
                    //Msg.Log("Download #" + dl.Id + " state changed from '" + dl.State + "' to '" + dl.NewState + "'.");
                    dl.State = dl.NewState; //Update the state

                    if (dl.State == DownloadState.Downloading)
                    {
                        StartDownload(dl); //Start the download

                        Msg.Log("Download #" + dl.Id + " started.");
                    }
                    else if (dl.State == DownloadState.Canceled)
                    {
                        if (ActiveDownloadId == dl.Id)
                        {
                            ActiveDownloadId = -1;

                            WebClient.CancelAsync();
                        }

                        Msg.Log("Download #" + dl.Id + " canceled.");
                    }
                    else if (dl.State == DownloadState.Finished)
                    {
                        if (ActiveDownloadId == dl.Id)
                            ActiveDownloadId = -1;

                        Msg.Log("Download #" + dl.Id + " finished: " + dl.Name());
                    }
                    else if (dl.State == DownloadState.Failed)
                    {
                        if (ActiveDownloadId == dl.Id)
                            ActiveDownloadId = -1;

                        Msg.Log("Download #" + dl.Id + " failed: " + dl.Message);
                    }

                    //Let the main thread know about the updated state
                    SendOutboundMsg(new DlStateMsg(dl.Id, dl.State, dl.Message));

                    Downloads[i] = dl; //Update the list values
                }
            }
        }

        //Handles localmessages sent from other threads
        private void HandleInboundMessages()
        {
            if (Program.mainForm.DownloadMsgQueue.IsEmpty == false)
            {
                ILocalMsg localmsg;

                while (Program.mainForm.DownloadMsgQueue.TryDequeue(out localmsg))
                {
                    HandleInboundMsgType(localmsg);
                }
            }
        }

        //Handles a localmessage based on its type
        private void HandleInboundMsgType(ILocalMsg localmsg)
        {
            FileDownload fdl;

            if (localmsg.MsgType == LocalMsgType.None)
                return;

            if (localmsg.MsgType == LocalMsgType.DlNewDownload)
            {
                var dlmsg = (DlNewMsg)localmsg;
                int index = Downloads.FindIndex(o => o.Id == dlmsg.Id);

                if (index == -1) //Not an existing download
                {
                    fdl = new FileDownload();
                    fdl.Id = dlmsg.Id;
                    fdl.Type = dlmsg.Type;
                    fdl.NewState = dlmsg.State;
                    fdl.DownloadUrl = dlmsg.DownloadUrl;

                    Msg.Log("Download #" + dlmsg.Id + " '" + fdl.Name() + "' added.");

                    Downloads.Add(fdl); //Add the download to the list.

                    //Let the main UI thread know
                    SendOutboundMsg(new DlNewMsg(fdl.Id, fdl.Type, fdl.NewState, fdl.DownloadUrl));
                }
                else //This download exists already
                {
                    Msg.Log("DlNew: Download #" + dlmsg.Id + " '" + dlmsg.DownloadUrl + "' already exists!");
                }
            }
            //When the download state is going to be changed
            else if (localmsg.MsgType == LocalMsgType.DlState)
            {
                var dlmsg = (DlStateMsg)localmsg;
                int index = Downloads.FindIndex(o => o.Id == dlmsg.Id);

                if (index == -1)
                {
                    //Msg.Log("DlState: Download #" + dlmsg.Id + " is missing from list!");
                }
                else
                {
                    fdl = Downloads[index];
                    fdl.NewState = dlmsg.State;
                    fdl.Message = dlmsg.Message;

                    Downloads[index] = fdl; //Update the list data

                    //Main UI Thread is notified of this in HandleStateChanges()
                }
            }
            //When the download progress changes
            else if (localmsg.MsgType == LocalMsgType.DlProgress)
            {
                var dlmsg = (DlProgressMsg)localmsg;
                int index = Downloads.FindIndex(o => o.Id == dlmsg.Id);

                if (index == -1)
                {
                    //Msg.Log("DlProgress: Download #" + dlmsg.Id + " is missing from list!");
                }
                else
                {
                    fdl = Downloads[index];
                    fdl.BytesReceived = dlmsg.BytesReceived;
                    fdl.TotalBytesToReceive = dlmsg.TotalBytesToReceive;
                    fdl.ProgressPercentage = dlmsg.ProgressPercentage;

                    //Check if we've gotten a filename
                    if (fdl.Filename == null)
                    {
                        fdl.Filename = GetDownloadFilename();
                    }

                    Downloads[index] = fdl; //Update the list data

                    //Let the main UI thread know
                    SendOutboundMsg(new DlProgressMsg(fdl.Id, fdl.Filename, fdl.ProgressPercentage, fdl.BytesReceived, fdl.TotalBytesToReceive));
                }
            }
            //When the download finishes downloading
            else if (localmsg.MsgType == LocalMsgType.DlFinished)
            {
                var dlmsg = (DlFinishedMsg)localmsg;
                int index = Downloads.FindIndex(o => o.Id == dlmsg.Id);

                if (index == -1)
                {
                    //Msg.Log("DlFinished: Download #" + dlmsg.Id + " is missing from list!");
                }
                else
                {
                    fdl = Downloads[index];
                    fdl.NewState = dlmsg.State;
                    fdl.Message = dlmsg.Message;

                    Downloads[index] = fdl; //Update the list data

                    //Let the main UI thread know
                    SendOutboundMsg(new DlFinishedMsg(fdl.Id, fdl.NewState, fdl.Message, dlmsg.Data));
                }
            }
            //When the download is removed from the list
            else if (localmsg.MsgType == LocalMsgType.DlRemove)
            {
                var dlmsg = (DlRemoveMsg)localmsg;
                int index = Downloads.FindIndex(o => o.Id == dlmsg.Id);

                if (index == -1)
                {
                    //Msg.Log("DlRemove: Download #" + dlmsg.Id + " is missing from list!");
                }
                else
                {
                    fdl = Downloads[index];

                    if (ActiveDownloadId == fdl.Id)
                    {
                        ActiveDownloadId = -1;

                        WebClient.CancelAsync();
                    }

                    //Let the main thread know
                    SendOutboundMsg(new DlRemoveMsg(fdl.Id));

                    Msg.Log("Download #" + dlmsg.Id + " '" + fdl.Name() + "' removed from downloads.");

                    //Remove the download from the list
                    Downloads.RemoveAt(index);
                }
            }
            //When the download is moved forward/backward in the list
            else if (localmsg.MsgType == LocalMsgType.DlMoveItem)
            {
                var dlmsg = (DlMoveMsg)localmsg;
                int index = Downloads.FindIndex(o => o.Id == dlmsg.Id);

                if (index == -1)
                {
                    //Msg.Log("DlMove: Download #" + dlmsg.Id + " is missing from list!");
                }
                else
                {
                    if (dlmsg.Forward)
                        MoveDownloadItem(index, index + 1);
                    else
                        MoveDownloadItem(index, index - 1);
                }
            }
            else
            {
                Msg.Log("DlManager: Unsupported MsgType '" + localmsg.MsgType +
                        "' <" + (int)localmsg.MsgType + "> !");
            }
        }

        //Moves a download in the download list
        private void MoveDownloadItem(int index, int dest_index)
        {
            if ((     index < 0) || (     index >= Downloads.Count)) return;
            if ((dest_index < 0) || (dest_index >= Downloads.Count)) return;

            var item = Downloads[index];
            Downloads.RemoveAt(index);
            Downloads.Insert(dest_index, item);

            //Send a full update of the downloads list
            if (Program.mainForm != null)
            {
                Program.mainForm.BeginInvoke((MethodInvoker)(() => Program.mainForm.lstDownloads_UpdateAll(Downloads.ToArray())));
            }
        }

        //Send a localmessage to the Main UI thread
        private static void SendOutboundMsg(ILocalMsg localmsg)
        {
            if (Program.mainForm != null)
            {
                Program.mainForm.BeginInvoke((MethodInvoker)(() => Program.mainForm.HandleLocalMsg(localmsg)));
            }
        }

        //Send a localmessage to the DownloadManager thread
        private static void SendInboundMsg(ILocalMsg localmsg)
        {
            if (Program.mainForm != null)
            {
                Program.mainForm.DownloadMsgQueue.Enqueue(localmsg);
            }
        }

        //Callback for when Webclient download progress changes
        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            long DownloadId = (long)e.UserState;

            SendInboundMsg(new DlProgressMsg(DownloadId, null, e.ProgressPercentage, e.BytesReceived, e.TotalBytesToReceive));
        }

        //Callback for when Webclient completes downloading data
        private void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            long DownloadId = (long)e.UserState;
            DownloadState state;
            string message;
            byte[] data;

            if (e.Cancelled)
            {
                state = DownloadState.Canceled;
                message = null;
                data = null;
            }
            else if (e.Error != null)
            {
                state = DownloadState.Failed;
                message = e.Error.Message;
                data = null;
            }
            else
            {
                state = DownloadState.Finished;
                message = null;
                data = e.Result.ToArray(); //Make a copy of the array
            }

            SendInboundMsg(new DlFinishedMsg(DownloadId, state, message, data));

            DlManagerProcess.WebClientARE.Set();
        }

        //Start a download using the WebClient
        private void StartDownload(FileDownload fdl)
        {
            DlManagerProcess.WebClientARE.Reset();

            WebClient.DownloadDataAsync(new Uri(fdl.DownloadUrl), fdl.Id);
        }

        //Get a filename from WebClient ResponseHeaders content-disposition
        private string GetDownloadFilename()
        {
            string filename = null;

            // This is a hacky workaround to get the filename since System.Net.Mime.ContentDisposition
            // does not appear to be parsing correctly..
            if (WebClient.ResponseHeaders == null)
                return null;

            string contentDisp = WebClient.ResponseHeaders["content-disposition"];
            string param = "filename=";

            int start_index = contentDisp.LastIndexOf(param, StringComparison.OrdinalIgnoreCase);
            int last_index = -1;
            if (start_index != -1)
            {
                start_index += param.Length;
                last_index = contentDisp.IndexOf(';', start_index);

                if (last_index != -1)
                    filename = contentDisp.Substring(start_index, last_index-start_index);
                else
                    filename = contentDisp.Substring(start_index);

                filename = filename.Replace("\"", "");
                filename = filename.TrimEnd(';');
            }
            return filename;
        }
    }
}
