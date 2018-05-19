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
using System.IO;
using System.Windows.Forms;

namespace Patchlunky
{
    public static class Msg
    {
        static object locker = new object();

        public static void Log(string msg, bool logfileonly = false)
        {
            string path = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/";
            string time = DateTime.Now.ToString("HH:mm:ss");
            string text = time + " - " + msg + Environment.NewLine;

            if ((Program.mainForm != null) && (logfileonly == false))
            {
                Program.mainForm.BeginInvoke((MethodInvoker)(() => Program.mainForm.Log(text)));
            }

            lock (locker)
            {
                File.AppendAllText(path + "Patchlunky.log", text);
            }
        }

        public static DialogResult MsgBox(string msg, string caption = "Patchlunky", 
                                          MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            if (Program.mainForm != null)
                return MessageBox.Show(Program.mainForm, msg, caption, buttons);
            else
                return MessageBox.Show(msg, caption, buttons);
        }
    }

    public enum LocalMsgType
    {
        None = 0,
        Url,
        DlNewDownload,
        DlProgress,
        DlState,
        DlMoveItem,
        DlFinished,
        DlRemove,
    }

    //Interface for local interprocess messages
    public interface ILocalMsg
    {
        LocalMsgType MsgType { get; }
        bool HasUserAction { get; }
    }

    //Message for a PatchlunkyUrl command
    public struct UrlMsg : ILocalMsg
    {
        private LocalMsgType msgtype;
        public LocalMsgType MsgType { get { return msgtype; } }
        private bool hasuseraction;
        public bool HasUserAction { get { return hasuseraction; } }

        public string PatchlunkyUrl;

        public UrlMsg(string url)
        {
            this.msgtype = LocalMsgType.Url;
            this.hasuseraction = true;
            this.PatchlunkyUrl = url;
        }
    }

    //Message for a new download
    public struct DlNewMsg : ILocalMsg
    {
        private LocalMsgType msgtype;
        public LocalMsgType MsgType { get { return msgtype; } }
        private bool hasuseraction;
        public bool HasUserAction { get { return hasuseraction; } }

        public long Id;
        public DownloadType Type;
        public DownloadState State;
        public string DownloadUrl;
        //public string FileName;

        public DlNewMsg(long id, DownloadType type, DownloadState state, string url)
        {
            this.msgtype = LocalMsgType.DlNewDownload;
            this.hasuseraction = false;
            this.Id = id;
            this.Type = type;
            this.State = state;
            this.DownloadUrl = url;
        }
    }

    //Message for download state change
    public struct DlStateMsg : ILocalMsg
    {
        private LocalMsgType msgtype;
        public LocalMsgType MsgType { get { return msgtype; } }
        private bool hasuseraction;
        public bool HasUserAction { get { return hasuseraction; } }

        public long Id;
        public DownloadState State;
        public string Message;

        public DlStateMsg(long id, DownloadState state, string message)
        {
            this.msgtype = LocalMsgType.DlState;
            this.hasuseraction = false;
            this.Id = id;
            this.State = state;
            this.Message = message;
        }
    }

    //Message for download progress update
    public struct DlProgressMsg : ILocalMsg
    {
        private LocalMsgType msgtype;
        public LocalMsgType MsgType { get { return msgtype; } }
        private bool hasuseraction;
        public bool HasUserAction { get { return hasuseraction; } }

        public long Id;
        public string Filename;
        public int ProgressPercentage;
        public long BytesReceived;
        public long TotalBytesToReceive;

        public DlProgressMsg(long id, string filename, int progress, long bytesreceived, long totalbytes)
        {
            this.msgtype = LocalMsgType.DlProgress;
            this.hasuseraction = false;
            this.Id = id;
            this.Filename = filename;
            this.ProgressPercentage = progress;
            this.BytesReceived = bytesreceived;
            this.TotalBytesToReceive = totalbytes;
        }
    }

    //Message for a finished download
    public struct DlFinishedMsg : ILocalMsg
    {
        private LocalMsgType msgtype;
        public LocalMsgType MsgType { get { return msgtype; } }
        private bool hasuseraction;
        public bool HasUserAction { get { return hasuseraction; } }

        public long Id;
        public DownloadState State;
        public string Message;
        public Byte[] Data;

        public DlFinishedMsg(long id, DownloadState state, string message, Byte[] data)
        {
            this.msgtype = LocalMsgType.DlFinished;
            this.hasuseraction = true;
            this.Id = id;
            this.State = state;
            this.Message = message;
            this.Data = data;
        }
    }

    //Message for removing a download
    public struct DlRemoveMsg : ILocalMsg
    {
        private LocalMsgType msgtype;
        public LocalMsgType MsgType { get { return msgtype; } }
        private bool hasuseraction;
        public bool HasUserAction { get { return hasuseraction; } }

        public long Id;

        public DlRemoveMsg(long id)
        {
            this.msgtype = LocalMsgType.DlRemove;
            this.hasuseraction = false;
            this.Id = id;
        }
    }

    //Message for moving a download in the queue
    public struct DlMoveMsg : ILocalMsg
    {
        private LocalMsgType msgtype;
        public LocalMsgType MsgType { get { return msgtype; } }
        private bool hasuseraction;
        public bool HasUserAction { get { return hasuseraction; } }

        public long Id;
        public bool Forward;

        public DlMoveMsg(long id, bool forward)
        {
            this.msgtype = LocalMsgType.DlMoveItem;
            this.hasuseraction = false;
            this.Id = id;
            this.Forward = forward;
        }
    }
}
