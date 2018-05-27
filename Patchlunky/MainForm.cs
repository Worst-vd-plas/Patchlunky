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
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using SpelunkyWad;

namespace Patchlunky
{
    public partial class MainForm : Form
    {
        public string PatchlunkyVersion;
        public string PatchlunkyGuid;

        public Settings Settings;
        public Setup Setup;
        public ModManager ModMan;
        public SkinManager SkinMan;
        public ScriptManager ScriptMan;

        private ListViewItem CharItem;
        private ListViewItem SkinItem;
        private bool SkinBrowserActive;

        public Thread DownloadManagerThread;
        public CancellationTokenSource DownloadManagerThreadCts;
        public ConcurrentQueue<ILocalMsg> DownloadMsgQueue; //Downloadmanager thread reads this queue.
        public int DownloadIdCounter;

        private Queue<ILocalMsg> UILocalMsgQueue; //The Main UI thread reads this queue.
        private bool UserRoutineActive; //Wether or not the UI thread is in a routine that may require user input

        private string PatchlunkyUrl; //holds a PatchlunkyURL passed as a cmdline argument.

        public MainForm(string windowName, string version, string guid, string patchlunkyUrl)
        {
            InitializeComponent();

            //Store the passed arguments
            this.Text = windowName; //Window title
            PatchlunkyVersion = version;
            PatchlunkyGuid = guid;

            //Patchlunky URL protocol
            PatchlunkyUrl = patchlunkyUrl;

            //Characters tab
            CharItem = null;
            SkinItem = null;
            SkinBrowserActive = false;

            //Main UI thread local message queue
            UILocalMsgQueue = new Queue<ILocalMsg>();
            UserRoutineActive = false;

            //Downloadmanager thread
            DownloadMsgQueue = new ConcurrentQueue<ILocalMsg>();
            DownloadIdCounter = 1;
            DownloadManagerThreadCts = new CancellationTokenSource();
            DownloadManagerThread = new Thread(() => DlManagerProcess.StartThread(DownloadManagerThreadCts.Token));
        }

        //Override WndProc to handle WM_COPYDATA messages
        protected override void WndProc(ref Message m)
        {
            const Int32 WM_COPYDATA = 0x004A;

            if (m.Msg == WM_COPYDATA)
            {
                string message = CopyDataMessage.Read(ref m, PatchlunkyGuid);

                if (message != null)
                    HandleLocalMsg(new UrlMsg(message));
            }

            base.WndProc(ref m);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Msg.Log(this.Text + " started.");

            //Start the downloadmanager
            DownloadManagerThread.Start();

            Settings = new Settings();
            Setup = new Setup();
            ModMan = new ModManager();
            SkinMan = new SkinManager();
            ScriptMan = new ScriptManager();

            //Check if Patchlunky Setup should run
            if ( (Setup.CheckBackups() == false) || 
                 (Setup.CheckGameDir() == false) )
                Setup.DoSetup();

            //DebugPrintSettings();

            ModMan.LoadAllMods();
            SkinMan.LoadAllSkins();

            ModMan.LoadConfigList();            
            SkinMan.LoadConfigList();
            //ModMan.LoadConfig("Default");
            //SkinMan.LoadConfig("Default");

            UpdateModList();            
            UpdateModConfigList();

            UpdateCharacterList();
            UpdateSkinList();
            UpdateSkinConfigList();

            UpdateSettingsTab();

            //If a patchlunky URL was passed in the cmdline, ask to open it.
            if (PatchlunkyUrl != null)
            {
                HandleLocalMsg(new UrlMsg(PatchlunkyUrl));
                PatchlunkyUrl = null;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateModConfiguration();
            UpdateSkinConfiguration();

            if (Settings.Check("ModConfigAutosave", "True"))
                ModMan.SaveConfig(ModMan.CurrentConfig);

            if (Settings.Check("SkinConfigAutosave", "True"))            
                SkinMan.SaveConfig(SkinMan.CurrentConfig);
            
            ModMan.SaveConfigList();
            SkinMan.SaveConfigList();

            Settings.Set("GameDir", Setup.GamePath);            
            Settings.Save();

            //Stop the downloadmanager thread
            if (DownloadManagerThread != null)
            {
                if (DownloadManagerThread.IsAlive)
                {
                    //Msg.Log("Stopping the DownloadManager..");
                    DownloadManagerThreadCts.Cancel();
                    DownloadManagerThread.Join();
                }
            }
            DownloadManagerThreadCts.Dispose();

            Msg.Log(this.Text + " closing.");
        }

        //----------------------------------------------//
        // LOCAL MESSAGE QUEUE                          //
        //----------------------------------------------//

        // This function is only run from the main thread, but it may get executed from
        // multiple different messagepumps, which may result in it being run recursively.
        // The boolean 'UserRoutineActive' is for catching the recursive execution and
        // to prevent opening up dialogs on top of other possibly unrelated dialogs.
        // TODO: Make all the routines with dialogs execute from here.
        public void HandleLocalMsg(ILocalMsg newmsg)
        {
            ILocalMsg localmsg = newmsg;

            if (localmsg.MsgType == LocalMsgType.None)
                return;

            //If the message may involve user action
            if (localmsg.HasUserAction)
            {
                //If we are not currently processing a msg with useraction
                if (UserRoutineActive == false)
                {
                    UserRoutineActive = true;

                    HandleLocalMsgType(localmsg);

                    //Process any msgs that were added to the queue before the current one finished.
                    while (UILocalMsgQueue.Count > 0)
                    {
                        localmsg = UILocalMsgQueue.Dequeue();

                        HandleLocalMsgType(localmsg);
                    }
                    UserRoutineActive = false;
                }
                else //Else enqueue the msg for later retrieval
                {
                    // The only circumstance in which messages are enqueued to this queue is when
                    // this function is executed recursively. The previous call to this function
                    // that still hasn't finished will dequeue the messages being enqueued in here.
                    UILocalMsgQueue.Enqueue(localmsg);
                }
            }
            else //Else message does not involve user action
            {
                HandleLocalMsgType(localmsg);
            }
        }

        //Handles a localmessage based on its type
        private void HandleLocalMsgType(ILocalMsg localmsg)
        {
            if (localmsg.MsgType == LocalMsgType.Url)
            {
                var urlmsg = (UrlMsg)localmsg;

                Setup.OpenPatchlunkyUrl(urlmsg.PatchlunkyUrl);
            }
            else if (localmsg.MsgType == LocalMsgType.DlNewDownload)
            {
                var dlmsg = (DlNewMsg)localmsg;

                lstDownloads_NewDownload(dlmsg);
            }
            else if (localmsg.MsgType == LocalMsgType.DlState)
            {
                var dlmsg = (DlStateMsg)localmsg;

                lstDownloads_StateChange(dlmsg);
            }
            else if (localmsg.MsgType == LocalMsgType.DlProgress)
            {
                var dlmsg = (DlProgressMsg)localmsg;

                lstDownloads_ProgressUpdate(dlmsg);
            }
            else if (localmsg.MsgType == LocalMsgType.DlFinished)
            {
                var dlmsg = (DlFinishedMsg)localmsg;

                lstDownloads_DownloadFinished(dlmsg);
            }
            else if (localmsg.MsgType == LocalMsgType.DlRemove)
            {
                var dlmsg = (DlRemoveMsg)localmsg;

                lstDownloads_DownloadRemove(dlmsg);
            }
            else
            {
                Msg.Log("HandleLocalMsgType: Unsupported MsgType '" + localmsg.MsgType +
                        "' <" + (int)localmsg.MsgType + "> !");
            }
        }

        //----------------------------------------------//
        // MAIN CONTROLS                                //
        //----------------------------------------------//

        //Print message to log textbox
        public void Log(string text)
        {
            txtLog.AppendText(text);
        }

        //Sets the progressbar percentage
        public void SetProgress(int percent)
        {
            prgPatchGame.Value = percent;
        }

        //Patch mods to game
        private void btnPatch_Click(object sender, EventArgs e)
        {
            UpdateModConfiguration();
            UpdateSkinConfiguration();

            if (Settings.Check("ModConfigAutosave", "True"))
                ModMan.SaveConfig(ModMan.CurrentConfig);            

            if (Settings.Check("SkinConfigAutosave", "True"))
                SkinMan.SaveConfig(SkinMan.CurrentConfig);

            //Debug
            //foreach (var mod in ModMan.Mods)
            //    Msg.Log("mod '" + mod.Name + "' enabled: " + mod.Enabled + ", index: "+ mod.SortValue);
            
            prgPatchGame.Value = 100; //Test

            bool patch_ok;
            patch_ok = Setup.PatchGame(reset_only:false);
            
            Msg.Log(patch_ok ? "Game files successfully patched." :"Patching the game files failed.");
        }

        // Run the game
        private void btnRunGame_Click(object sender, EventArgs e)
        {
            if (File.Exists(Setup.GamePath + "Spelunky.exe"))
            {
                ProcessStartInfo startinfo = new ProcessStartInfo();
                startinfo.WorkingDirectory = Setup.GamePath;
                startinfo.FileName = Setup.GamePath + "Spelunky.exe";
                Process.Start(startinfo);
            }
        }

        //----------------------------------------------//
        // SETTINGS TAB                                 //
        //----------------------------------------------//

        //TODO: Get rid of this function
        private void DebugPrintSettings()
        {
            Msg.Log("Config dir: " + Settings.Config.FilePath);

            //if (ConfigurationManager.AppSettings.Count == 0)
            if (Settings.Config.AppSettings.Settings.Count == 0)
            {
                Msg.Log("AppSettings is empty.");
            }
            else
            {
                //foreach (var key in ConfigurationManager.AppSettings.AllKeys)
                foreach (string key in Settings.Config.AppSettings.Settings.AllKeys)
                {
                    Msg.Log(key + " = " + Settings.Config.AppSettings.Settings[key].Value);
                }
            }
        }

        //For verifying settings in the settings tab
        private void UpdateSettingsTab()
        {
            txtGamePath.Text = Setup.GamePath;

            lblStatusGamePath.Text = "Game path: " + (Setup.CheckGameDir() ? "OK." : "MISSING!");
            lblStatusBackup.Text = "Game backups: " + (Setup.CheckBackups() ? "OK." : "MISSING!");
            lblStatusUrlProtocol.Text = "URL Protocol: " + (Setup.CheckUriScheme() ? "OK." : "Not registered.");

            chkModsReplaceSkins.Checked = Settings.Check("ModsReplaceDefaultSkins", "True");
            chkModConfigAutosave.Checked = Settings.Check("ModConfigAutosave", "True");
            chkSkinConfigAutosave.Checked = Settings.Check("SkinConfigAutosave", "True");
        }

        //Starts patchlunky setup
        private void btnSetup_Click(object sender, EventArgs e)
        {
            Setup.DoSetup(forced: true);
            UpdateSettingsTab();
        }

        //Restores original game files
        private void btnRestore_Click(object sender, EventArgs e)
        {
            bool patch_ok;
            patch_ok = Setup.PatchGame(reset_only: true);

            Msg.Log(patch_ok ? "Game files successfully restored." : "Restoring the game files failed.");
        }

        //After game path is changed manually
        private void txtGamePath_Leave(object sender, EventArgs e)
        {
            Setup.GamePath = txtGamePath.Text;
            if ((Setup.GamePath.EndsWith("/") == false) && (Setup.GamePath.EndsWith("\\") == false))
            {
                Setup.GamePath += "/";
            }

            UpdateSettingsTab();
        }

        //Button for browsing to the game directory
        private void btnGamePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                Setup.GamePath = dialog.SelectedPath + "/";
                UpdateSettingsTab();
            }
        }

        //Extracts textures and sounds to a directory (for modders)
        private void btnExtractArchives_Click(object sender, EventArgs e)
        {
            if (Setup.CheckBackups() == false)
            {
                Msg.Log("Failed to extract the game archives.");
                string message = "Patchlunky is missing Spelunky backups!" +
                    Environment.NewLine + Environment.NewLine +
                "Patchlunky cannot extract the game archives without backups.";
                Msg.MsgBox(message);
                return; //Backups are missing, can't extract.
            }

            Archive alltex = new Archive(Setup.BackupPath + "Textures/alltex.wad");
            alltex.Load();

            foreach (var group in alltex.Groups)
            {
                string dirpath = Path.Combine(Setup.TempPath + "alltex/", group.Name);
                Directory.CreateDirectory(dirpath);

                foreach (var entry in group.Entries)
                {
                    string filepath = Path.Combine(dirpath, entry.Name);
                    File.WriteAllBytes(filepath, entry.Data);
                }
            }
            Msg.Log("Extracted alltex.wad to ./Patchlunky_Temp/alltex/");


            Archive allsounds = new Archive(Setup.BackupPath + "Sounds/allsounds.wad");
            allsounds.Load();

            foreach (var group in allsounds.Groups)
            {
                string dirpath = Path.Combine(Setup.TempPath + "allsounds/", group.Name);
                Directory.CreateDirectory(dirpath);

                foreach (var entry in group.Entries)
                {
                    string filepath = Path.Combine(dirpath, entry.Name);
                    File.WriteAllBytes(filepath, entry.Data);
                }
            }
            Msg.Log("Extracted allsounds.wad to ./Patchlunky_Temp/allsounds/");
        }

        //ModsReplaceDefaultSkins
        private void chkModsReplaceSkins_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Set("ModsReplaceDefaultSkins", chkModsReplaceSkins.Checked ? "True" : "False");            
        }

        //ModConfigAutosave
        private void chkModConfigAutosave_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Set("ModConfigAutosave", chkModConfigAutosave.Checked ? "True" : "False");
        }

        //SkinConfigAutosave
        private void chkSkinConfigAutosave_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Set("SkinConfigAutosave", chkSkinConfigAutosave.Checked ? "True" : "False");
        }

        //----------------------------------------------//
        // MODS TAB                                     //
        //----------------------------------------------//

        //Updates the mod configuration based on the checkedlistbox
        private void UpdateModConfiguration()
        {
            //Loop through checkedlistbox and enable/disable mods and set their order
            for (int i = 0; i < chklstMods.Items.Count; i++)
            {
                var kvp = (KeyValuePair<string, string>)chklstMods.Items[i];
                string modid = kvp.Key;
                bool check = chklstMods.GetItemChecked(i);

                ModData mod = ModMan.GetMod(modid);
                if (mod != null)
                {
                    mod.Enabled = check;
                    mod.SortValue = -i; //Sort in reverse
                }
            }

            //Sort the mods
            ModMan.Mods.Sort((a, b) => a.SortValue.CompareTo(b.SortValue));
        }

        //Clears and Recreates the checkedlist of mods
        public void UpdateModList()
        {
            chklstMods.Items.Clear();

            foreach (var mod in ModMan.Mods)
            {
                var kvp = new KeyValuePair<string, string>(mod.Id, mod.Name);
                chklstMods.Items.Add(kvp, mod.Enabled);
            }

            chklstMods.DisplayMember = "Value";
        }

        //Displays information about the currently selected mod
        private void UpdateModInfo()
        {
            //Reset the information
            rtfModInfo.Text = "";

            picModIconGraphics.Visible = false;
            picModIconSound.Visible = false;
            picModIconMusic.Visible = false;
            picModIconText.Visible = false;
            picModIconMisc.Visible = false;
            picModIconScripts.Visible = false;

            picModImage.Visible = false;
            picModImage.Image = null;

            lnkModWebUrl.Visible = false;
            lnkModWebUrl.Links.Clear();
            toolTip1.SetToolTip(lnkModWebUrl, "");

            lnkModEmail.Visible = false;
            lnkModEmail.Links.Clear();
            toolTip1.SetToolTip(lnkModEmail, "");

            //Get the selected mod
            int index = chklstMods.SelectedIndex;
            if ((index < 0) || (index > chklstMods.Items.Count)) return;

            var kvp = (KeyValuePair<string, string>)chklstMods.Items[index];
            string modid = kvp.Key;

            ModData mod = ModMan.GetMod(modid);
            if (mod == null)
                return;

            //Display basic information
            rtfModInfo.AppendText("Name: " + (mod.Name ?? "-") + Environment.NewLine);
            rtfModInfo.AppendText("Author: " + (mod.Author ?? "-") + Environment.NewLine);
            rtfModInfo.AppendText("Date: " + (mod.Date.HasValue ? mod.Date.Value.ToShortDateString() : "-") + Environment.NewLine);
            rtfModInfo.AppendText("Version: " + (mod.Version ?? "-") + Environment.NewLine);
            rtfModInfo.AppendText(Environment.NewLine);

            //Display a note if the mod has scripts
            if (mod.Contents.HasFlag(ModContents.Scripts))
            {
                rtfModInfo.SelectionStart = rtfModInfo.TextLength;
                rtfModInfo.SelectionLength = 0;
                rtfModInfo.SelectionColor = Color.DodgerBlue;
                rtfModInfo.AppendText("Mod contains Scripts" + Environment.NewLine);
                rtfModInfo.SelectionColor = rtfModInfo.ForeColor;
            }

            //Display supported version for this mod
            if (mod.PatchlunkyVersion != null)
            {
                rtfModInfo.SelectionStart = rtfModInfo.TextLength;
                rtfModInfo.SelectionLength = 0;

                if (VersionIsSupported(mod.PatchlunkyVersion))
                {
                    rtfModInfo.SelectionColor = Color.Green;
                    rtfModInfo.AppendText("Patchlunky version: " + mod.PatchlunkyVersion);
                }
                else
                {
                    rtfModInfo.SelectionColor = Color.Red;
                    rtfModInfo.AppendText("Patchlunky version needed: " + mod.PatchlunkyVersion);
                }
                rtfModInfo.SelectionColor = rtfModInfo.ForeColor;
                rtfModInfo.AppendText(Environment.NewLine + Environment.NewLine);
            }

            //Display description
            if (mod.Description != null)
            {
                rtfModInfo.AppendText(mod.Description);
                rtfModInfo.AppendText(Environment.NewLine);
            }

            //Display WebUrl
            if (mod.WebUrl != null)
            {
                lnkModWebUrl.Visible = true;
                lnkModWebUrl.Links.Add(0, lnkModWebUrl.Text.Length, mod.WebUrl);
                toolTip1.SetToolTip(lnkModWebUrl, mod.WebUrl);
            }

            //Display Email
            if (mod.Email != null)
            {
                lnkModEmail.Visible = true;
                lnkModEmail.Links.Add(0, lnkModEmail.Text.Length, mod.Email);
                toolTip1.SetToolTip(lnkModEmail, mod.Email);
            }

            //Display mod content icons
            if (mod.Contents.HasFlag(ModContents.Graphics)) picModIconGraphics.Visible = true;
            if (mod.Contents.HasFlag(ModContents.Sounds)) picModIconSound.Visible = true;
            if (mod.Contents.HasFlag(ModContents.Music)) picModIconMusic.Visible = true;
            if (mod.Contents.HasFlag(ModContents.Text)) picModIconText.Visible = true;
            if (mod.Contents.HasFlag(ModContents.Misc)) picModIconMisc.Visible = true;
            if (mod.Contents.HasFlag(ModContents.Scripts)) picModIconScripts.Visible = true;

            //Display preview image
            if (mod.PreviewImage != null)
            {
                picModImage.Visible = true;
                picModImage.Image = mod.PreviewImage;
            }

            //Adjust information panel size
            if (mod.PreviewImage != null) pnlModInfo.Top = picModImage.Bottom + 4;
            else                          pnlModInfo.Top = picModImage.Top;

            if ((mod.WebUrl != null) || (mod.Email != null))
                pnlModInfo.Height = lnkModWebUrl.Top - pnlModInfo.Top - 4;
            else
                pnlModInfo.Height = chklstMods.Bottom - pnlModInfo.Top;
        }

        // Move mod with focus up
        private void btnModUp_Click(object sender, EventArgs e)
        {
            int index = chklstMods.SelectedIndex;
            MoveModItem(index, index - 1);            
        }

        // Move mod with focus down
        private void btnModDown_Click(object sender, EventArgs e)
        {
            int index = chklstMods.SelectedIndex;
            MoveModItem(index, index + 1);
        }

        // Move mod with focus to top
        private void btnTop_Click(object sender, EventArgs e)
        {
            int index = chklstMods.SelectedIndex;
            MoveModItem(index, 0);
        }

        // Move mod with focus to bottom
        private void btnBottom_Click(object sender, EventArgs e)
        {
            int index = chklstMods.SelectedIndex;
            MoveModItem(index, chklstMods.Items.Count - 1);
        }

        // Function for moving mods around in the CheckedListBox
        private void MoveModItem(int index, int dest_index)
        {
            if (index < 0) return;

            object item = chklstMods.SelectedItem;
            if (item == null) return;

            bool check = chklstMods.GetItemChecked(index);

            if (dest_index < 0 || dest_index >= chklstMods.Items.Count)
                return;

            chklstMods.Items.Remove(item);
            chklstMods.Items.Insert(dest_index, item);
            chklstMods.SetItemChecked(dest_index, check);
            chklstMods.SetSelected(dest_index, true);

            SetProgress(0); //Reset patch progress bar
        }

        // Select all mods
        private void btnAll_Click(object sender, EventArgs e)
        {
            SetProgress(0); //Reset patch progress bar

            for (int index = 0; index < chklstMods.Items.Count; index++)
            {
                chklstMods.SetItemChecked(index, true);
            }
        }

        // Deselect all mods
        private void btnNone_Click(object sender, EventArgs e)
        {
            SetProgress(0); //Reset patch progress bar

            for (int index = 0; index < chklstMods.Items.Count; index++)
            {
                chklstMods.SetItemChecked(index, false);
            }
        }

        // When a Mod is checked/unchecked
        private void chklstMods_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            SetProgress(0); //Reset patch progress bar
        }

        //Clears and Recreates the list of modconfigs
        private void UpdateModConfigList()
        {
            cboModConfig.Items.Clear();

            foreach (var conf in ModMan.Configs)
            {
                cboModConfig.Items.Add(conf);
            }

            cboModConfig.SelectedItem = ModMan.CurrentConfig;
        }

        //Mod configuration list index changed
        private void cboModConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            string config = cboModConfig.Text;
            if (ModMan.ConfigExists(config))
            {
                ModMan.CurrentConfig = config;
                ModMan.LoadConfig(config);
                UpdateModList();
            }
        }

        //Save mod configuration button
        private void btnSaveModConfig_Click(object sender, EventArgs e)
        {
            UpdateModConfiguration();

            string modconfig = cboModConfig.Text;
            if (ModMan.ConfigExists(modconfig) == false)
            {
                ModMan.Configs.Add(modconfig);
                cboModConfig.Items.Add(modconfig);
            }
            ModMan.SaveConfig(modconfig);
        }

        //Delete mod configuration button
        private void btnDeleteModConfig_Click(object sender, EventArgs e)
        {
            string modconfig = cboModConfig.Text;
            
            if (modconfig.Equals("Default")) return;

            if (ModMan.ConfigExists(modconfig))
            {
                ModMan.RemoveConfig(modconfig);
                ModMan.Configs.Remove(modconfig);
                cboModConfig.Items.Remove(modconfig);

                //Switch back to default config
                cboModConfig.SelectedItem = "Default";
            }
        }

        //When a mod in the checkedlistbox is selected
        private void chklstMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateModInfo();
        }

        //Clink on a mod weburl link
        private void lnkModWebUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link.LinkData != null)
                OpenWebLink(e.Link.LinkData.ToString());
        }

        //Click on a mod email link
        private void lnkModEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link.LinkData != null)
                OpenWebLink(e.Link.LinkData.ToString());
        }

        //Click a link found in the description
        private void rtfModInfo_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            OpenWebLink(e.LinkText);
        }

        //----------------------------------------------//
        // SKINS TAB                                    //
        //----------------------------------------------//

        //Updates skin configuration based on the character listview
        private void UpdateSkinConfiguration()
        {
            SkinMan.SkinConfig.Clear();

            foreach (ListViewItem item in lvwCharacters.Items)
            {
                string oldSkin = item.Name;
                string newSkin = item.ImageKey;
                SkinMan.SkinConfig.Add(oldSkin, newSkin);
                //Msg.Log("Char: '" + item.Name + "', Skin: '" + item.ImageKey + "'.");
            }
        }

        //Clears and Recreates the listview of skins
        public void UpdateSkinList()
        {
            lvwSkins.Clear();
            lvwSkins.LargeImageList = SkinMan.SkinImageList;

            //Fill skin list
            foreach (var skin in SkinMan.Skins)
                lvwSkins.Items.Add(skin.Id, "", skin.Id);

            ListView_SetIconSpacing(lvwSkins, 80, 80);
        }

        //Clears and Recreates the listview of characters
        public void UpdateCharacterList()
        {
            lvwCharacters.Clear();
            lvwCharacters.LargeImageList = SkinMan.SkinImageList;
            
            //Fill character list
            foreach (var skin in SkinMan.Skins)
            {
                if (skin.IsDefault == true)
                {
                    lvwCharacters.Items.Add(skin.Id, "", skin.Id);
                }
            }

            //Adjust character list to match the skin config
            foreach (KeyValuePair<string, string> kvp in SkinMan.SkinConfig)
            {
                string oldSkin = kvp.Key;
                string newSkin = kvp.Value;
                
                foreach (ListViewItem item in lvwCharacters.Items)
                {
                    if (item.Name.Equals(oldSkin, StringComparison.OrdinalIgnoreCase))
                    {
                        item.ImageKey = newSkin;
                        break;
                    }
                }
            }

            ListView_SetIconSpacing(lvwCharacters, 80, 80);
        }

        //Switches beetween Character/Skin browsing in the character skin tab
        private void SwitchSkinView(bool characters)
        {
            lvwCharacters.Visible = characters;
            lblCharSelect.Visible = characters;
            cboSkinConfig.Enabled = characters;
            btnSaveSkinConfig.Enabled = characters;
            btnDeleteSkinConfig.Enabled = characters;
            btnResetSkins.Enabled = characters;
            btnRestoreSkin.Enabled = characters;

            lvwSkins.Visible = !characters;
            lblSkinSelect.Visible = !characters;
        }

        //Double-click on a character
        private void lvwCharacters_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CharItem = (lvwCharacters.SelectedItems.Count > 0) ? lvwCharacters.SelectedItems[0] : null;

            if (CharItem != null)
            {
                //Switch the view
                SwitchSkinView(characters: false);
                SkinBrowserActive = true;
            }            
            UpdateSkinTab();
        }

        //Double-click on a skin
        private void lvwSkins_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SkinItem = (lvwSkins.SelectedItems.Count > 0) ? lvwSkins.SelectedItems[0] : null;

            if (SkinItem != null)
            {
                //Set the new skin for the character
                CharItem.ImageKey = SkinItem.ImageKey;

                //Switch the view
                SwitchSkinView(characters: true);
                SkinBrowserActive = false;
            }
            UpdateSkinTab();
        }

        //Updates the character info view
        private void UpdateSkinTab()
        {
            //Clear previous information
            picCharacter.Image = null;
            picSkin.Image = null;

            rtfSkinInfo.Text = "";

            lnkSkinWebUrl.Visible = false;
            lnkSkinWebUrl.Links.Clear();
            toolTip1.SetToolTip(lnkSkinWebUrl, "");

            lnkSkinEmail.Visible = false;
            lnkSkinEmail.Links.Clear();
            toolTip1.SetToolTip(lnkSkinEmail, "");

            //Display the original character for the current item
            if (CharItem != null)
            {
                picCharacter.Image = CharItem.ImageList.Images[CharItem.Name];
            }

            //Get the selected character
            string skinName = null;
            if (SkinBrowserActive) //If browsing custom characters
            {
                if (SkinItem != null) skinName = SkinItem.ImageKey;
            }
            else
            {
                if (CharItem != null) skinName = CharItem.ImageKey;
            }

            SkinData skin = SkinMan.GetSkin(skinName);
            if (skin == null)
            {
                if (skinName != null) //Unknown skin
                {
                    rtfSkinInfo.AppendText("Unknown character: " + skinName + Environment.NewLine);
                    rtfSkinInfo.AppendText(Environment.NewLine);
                    rtfSkinInfo.AppendText("This character will not be patched, because the files are missing!" + Environment.NewLine);
                }
                return;
            }

            //Display basic information
            rtfSkinInfo.AppendText("Name: " + (skin.Name ?? "-") + Environment.NewLine);
            rtfSkinInfo.AppendText("Author: " + (skin.Author ?? "-") + Environment.NewLine);
            rtfSkinInfo.AppendText("Date: " + (skin.Date.HasValue ? skin.Date.Value.ToShortDateString() : "-") + Environment.NewLine);
            rtfSkinInfo.AppendText("Version: " + (skin.Version ?? "-") + Environment.NewLine);

            //Display supported version for this skin
            if (skin.PatchlunkyVersion != null)
            {
                rtfSkinInfo.AppendText(Environment.NewLine);
                rtfSkinInfo.SelectionStart = rtfSkinInfo.TextLength;
                rtfSkinInfo.SelectionLength = 0;

                if (VersionIsSupported(skin.PatchlunkyVersion))
                {
                    rtfSkinInfo.SelectionColor = Color.Green;
                    rtfSkinInfo.AppendText("Patchlunky version: " + skin.PatchlunkyVersion);
                }
                else
                {
                    rtfSkinInfo.SelectionColor = Color.Red;
                    rtfSkinInfo.AppendText("Patchlunky version needed: " + skin.PatchlunkyVersion);
                }
                rtfSkinInfo.SelectionColor = rtfSkinInfo.ForeColor;
                rtfSkinInfo.AppendText(Environment.NewLine);
            }

            //Display description
            if (skin.Description != null)
            {
                rtfSkinInfo.AppendText(Environment.NewLine);
                rtfSkinInfo.AppendText(skin.Description);
                rtfSkinInfo.AppendText(Environment.NewLine);
            }

            //Display WebUrl
            if (skin.WebUrl != null)
            {
                lnkSkinWebUrl.Visible = true;
                lnkSkinWebUrl.Links.Add(0, lnkSkinWebUrl.Text.Length, skin.WebUrl);
                toolTip1.SetToolTip(lnkSkinWebUrl, skin.WebUrl);
            }

            //Display Email
            if (skin.Email != null)
            {
                lnkSkinEmail.Visible = true;
                lnkSkinEmail.Links.Add(0, lnkSkinEmail.Text.Length, skin.Email);
                toolTip1.SetToolTip(lnkSkinEmail, skin.Email);
            }

            //Display the new character for the current item
            picSkin.Image = CharItem.ImageList.Images[skinName];

            //Adjust information panel size
            if ((skin.WebUrl != null) || (skin.Email != null))
                pnlSkinInfo.Height = lnkSkinWebUrl.Top - pnlSkinInfo.Top - 4;
            else
                pnlSkinInfo.Height = lnkSkinWebUrl.Bottom - pnlSkinInfo.Top;
        }

        //Change selection in character listview.
        private void lvwCharacters_SelectedIndexChanged(object sender, EventArgs e)
        {
            CharItem = (lvwCharacters.SelectedItems.Count > 0) ? lvwCharacters.SelectedItems[0] : null;
            UpdateSkinTab();
        }

        //Change selection in skin listview.
        private void lvwSkins_SelectedIndexChanged(object sender, EventArgs e)
        {
            SkinItem = (lvwSkins.SelectedItems.Count > 0) ? lvwSkins.SelectedItems[0] : null;
            UpdateSkinTab();
        }

        //Resets selected skin back to default
        private void btnRestoreSkin_Click(object sender, EventArgs e)
        {
            if (CharItem != null)
            {
                CharItem.ImageKey = CharItem.Name;
                UpdateSkinTab();
            }
        }

        //Resets all skins back to default
        private void btnResetSkins_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvwCharacters.Items)
            {
                item.ImageKey = item.Name;
            }
            UpdateSkinTab();
        }

        //Clears and Recreates the list of skinconfigs
        private void UpdateSkinConfigList()
        {
            cboSkinConfig.Items.Clear();

            foreach (var conf in SkinMan.Configs)
            {
                cboSkinConfig.Items.Add(conf);
            }

            cboSkinConfig.SelectedItem = SkinMan.CurrentConfig;
        }

        //Skin configuration list index changed
        private void cboSkinConfig_SelectedIndexChanged(object sender, EventArgs e)
        {                    
            string config = cboSkinConfig.Text;
            if (SkinMan.ConfigExists(config))
            {
                SkinMan.CurrentConfig = config;
                SkinMan.LoadConfig(config);
                UpdateCharacterList();
            }
        }

        //Save skin configuration button
        private void btnSaveSkinConfig_Click(object sender, EventArgs e)
        {
            UpdateSkinConfiguration();

            string config = cboSkinConfig.Text;
            if (SkinMan.ConfigExists(config) == false)
            {
                SkinMan.Configs.Add(config);
                cboSkinConfig.Items.Add(config);
            }
            SkinMan.SaveConfig(config);
        }

        //Delete skin configuration button
        private void btnDeleteSkinConfig_Click(object sender, EventArgs e)
        {
            string config = cboSkinConfig.Text;

            if (config.Equals("Default")) return;

            if (SkinMan.ConfigExists(config))
            {
                SkinMan.RemoveConfig(config);
                SkinMan.Configs.Remove(config);
                cboSkinConfig.Items.Remove(config);

                //Switch back to default config
                cboSkinConfig.SelectedItem = "Default";
            }
        }

        //Click on a skin weburl link
        private void lnkSkinWebUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(e.Link.LinkData != null)
                OpenWebLink(e.Link.LinkData.ToString());
        }

        //Click on a skin email link
        private void lnkSkinEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link.LinkData != null)
                OpenWebLink(e.Link.LinkData.ToString());
        }

        //Click a link found in the description
        private void rtfSkinInfo_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            OpenWebLink(e.LinkText);
        }

        //----------------------------------------------//
        // DOWNLOADS                                    //
        //----------------------------------------------//

        //Returns the list index of the download with the given Id
        private int lstDownloads_GetIndex(long Id)
        {
            for (int i=0; i < lstDownloads.Items.Count; i++)
            {
                if (((FileDownload)lstDownloads.Items[i]).Id == Id)
                    return i;
            }
            return -1;
        }

        //Updates the entire list of downloads - Invoked by DownloadManager
        public void lstDownloads_UpdateAll(FileDownload[] downloads)
        {
            long selected_id = -1;
            int selected_index = lstDownloads.SelectedIndex;
            if ((selected_index >= 0) || (selected_index < lstDownloads.Items.Count))
            {
                selected_id = ((FileDownload)lstDownloads.Items[selected_index]).Id;
            }

            lstDownloads.Items.Clear();
            lstDownloads.Items.AddRange(downloads.Cast<object>().ToArray());

            if (selected_id != -1)
            {
                //Set the selected index again, if GetIndex returns -1, no item is selected.
                lstDownloads.SelectedIndex = lstDownloads_GetIndex(selected_id);
            }
        }

        //Adds a new download to the list
        private void lstDownloads_NewDownload(DlNewMsg dlmsg)
        {
            int item_index = lstDownloads_GetIndex(dlmsg.Id);

            if (item_index != -1)
            {
                Msg.Log("Error adding Download #" + dlmsg.Id + ", it already exists!");
                return;
            }

            FileDownload item = new FileDownload();
            item.Id = dlmsg.Id;
            item.Type = dlmsg.Type;
            item.State = dlmsg.State;
            item.DownloadUrl = dlmsg.DownloadUrl;

            lstDownloads.Items.Add(item);
        }

        //Removes a download from the list
        private void lstDownloads_DownloadRemove(DlRemoveMsg dlmsg)
        {
            int item_index = lstDownloads_GetIndex(dlmsg.Id);

            if (item_index == -1)
            {
                Msg.Log("Error removing Download #" +  dlmsg.Id + ", it does not exist!");
                return;
            }

            lstDownloads.Items.RemoveAt(item_index);
        }

        //Updates the state of a download
        private void lstDownloads_StateChange(DlStateMsg dlmsg)
        {
            int item_index = lstDownloads_GetIndex(dlmsg.Id);

            if (item_index == -1)
            {
                Msg.Log("Error changing download state, Download #" +  dlmsg.Id + " does not exist!");
                return;
            }

            FileDownload item = (FileDownload)lstDownloads.Items[item_index];
            //Only update if the state has changed
            if (item.State != dlmsg.State)
            {
                //Msg.Log("Download #" + dlmsg.Id + ", state changed: " + dlmsg.State);
                item.State = dlmsg.State;
                item.Message = dlmsg.Message;

                lstDownloads.SuspendLayout();
                lstDownloads.Items[item_index] = item; //Update the item in the list
                lstDownloads.ResumeLayout();
            }
        }

        //Updates the progress of a download
        private void lstDownloads_ProgressUpdate(DlProgressMsg dlmsg)
        {
            int item_index = lstDownloads_GetIndex(dlmsg.Id);

            if (item_index == -1)
            {
                Msg.Log("Error updating download progress, Download #" +  dlmsg.Id + " does not exist!");
                return;
            }

            FileDownload item = (FileDownload)lstDownloads.Items[item_index];
            //Only update if the progress has changed
            if (item.ProgressPercentage != dlmsg.ProgressPercentage)
            {
                item.ProgressPercentage = dlmsg.ProgressPercentage;
                item.BytesReceived = dlmsg.BytesReceived;
                item.TotalBytesToReceive = dlmsg.TotalBytesToReceive;
                item.Filename = dlmsg.Filename;

                lstDownloads.SuspendLayout();
                lstDownloads.Items[item_index] = item; //Update the item in the list
                lstDownloads.ResumeLayout();
            }
        }

        //Updates the state of a finished download and saves it to a file
        private void lstDownloads_DownloadFinished(DlFinishedMsg dlmsg)
        {
            int item_index = lstDownloads_GetIndex(dlmsg.Id);

            if (item_index == -1)
            {
                Msg.Log("Error finishing Download #" +  dlmsg.Id + ", it does not exist!");
                return;
            }

            FileDownload item = (FileDownload)lstDownloads.Items[item_index];
            item.State = dlmsg.State;
            item.Message = dlmsg.Message;

            lstDownloads.SuspendLayout();
            lstDownloads.Items[item_index] = item; //Update the item in the list
            lstDownloads.ResumeLayout();

            //If the download finished normally and there is data
            if((dlmsg.State == DownloadState.Finished) && (dlmsg.Data != null))
                Setup.DownloadSaveToFile(item, dlmsg.Data);
        }

        //Updates the rtfDownloadInfo when a download is selected
        private void lstDownloads_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lstDownloads.SelectedIndex;
            if ((index < 0) || (index >= lstDownloads.Items.Count))
            {
                rtfDownloadInfo.Text = "";
                return;
            }

            FileDownload fdl = (FileDownload)lstDownloads.Items[index];

            rtfDownloadInfo.Text = "";
            rtfDownloadInfo.AppendText("Id: #" + fdl.Id + Environment.NewLine);
            rtfDownloadInfo.AppendText("Type: " + (Enum.GetName(typeof(DownloadType), fdl.Type)) + Environment.NewLine);
            rtfDownloadInfo.AppendText("Url: " + (fdl.DownloadUrl ?? "-") + Environment.NewLine);
            rtfDownloadInfo.AppendText("Filename: " + (fdl.Filename ?? "-") + Environment.NewLine);

            string progress = fdl.ProgressPercentage + "%" + " (" + ((fdl.State != DownloadState.Downloading) ?
                       (((float)fdl.TotalBytesToReceive/1024/1024).ToString("0.##") + " MB") :
                       (((float)fdl.BytesReceived/1024/1024).ToString("0.##") + " / " +
                        ((float)fdl.TotalBytesToReceive/1024/1024).ToString("0.##") + " MB")) + ")";
            rtfDownloadInfo.AppendText("Progress: " + progress + Environment.NewLine);

            string status = Enum.GetName(typeof(DownloadState), fdl.State) +
                            (fdl.Message != null ? " - " + fdl.Message : "");
            rtfDownloadInfo.AppendText("Status: " + status + Environment.NewLine);
        }

        //Click a link in the download info
        private void rtfDownloadInfo_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            OpenWebLink(e.LinkText);
        }

        //Cancels the selected download
        private void btnDLCancel_Click(object sender, EventArgs e)
        {
            int index = lstDownloads.SelectedIndex;
            if ((index < 0) || (index >= lstDownloads.Items.Count)) return;

            FileDownload fdl = (FileDownload)lstDownloads.Items[index];

            if((fdl.State == DownloadState.Downloading) || (fdl.State == DownloadState.Queued))
            {
                DownloadMsgQueue.Enqueue(new DlStateMsg(fdl.Id, DownloadState.Canceled, null));
            }
        }

        //Queues a canceled or failed download again
        private void btnDLRestart_Click(object sender, EventArgs e)
        {
            int index = lstDownloads.SelectedIndex;
            if ((index < 0) || (index >= lstDownloads.Items.Count)) return;

            FileDownload fdl = (FileDownload)lstDownloads.Items[index];

            if ((fdl.State == DownloadState.Canceled) || (fdl.State == DownloadState.Failed))
            {
                DownloadMsgQueue.Enqueue(new DlStateMsg(fdl.Id, DownloadState.Queued, null));
            }
        }

        //Removes a download from the download queue (Does not remove files)
        private void btnDLRemove_Click(object sender, EventArgs e)
        {
            int index = lstDownloads.SelectedIndex;
            if ((index < 0) || (index >= lstDownloads.Items.Count)) return;

            FileDownload fdl = (FileDownload)lstDownloads.Items[index];

            string message = "Remove Download #" + fdl.Id + " '" + (fdl.Filename != null ? fdl.Filename : fdl.DownloadUrl) +
                            "' from the download list?" + Environment.NewLine + Environment.NewLine +
                            "NOTE: This only removes the download from the download list. Downloaded files are not removed.";
            DialogResult result = Msg.MsgBox(message, "Patchlunky DownloadManager", MessageBoxButtons.YesNoCancel);

            if (result != DialogResult.Yes)
                return;

            DownloadMsgQueue.Enqueue(new DlRemoveMsg(fdl.Id));
        }

        //Moves the download down in the queue
        private void btnDLMoveDown_Click(object sender, EventArgs e)
        {
            int index = lstDownloads.SelectedIndex;
            if ((index < 0) || (index >= lstDownloads.Items.Count)) return;

            FileDownload fdl = (FileDownload)lstDownloads.Items[index];

            DownloadMsgQueue.Enqueue(new DlMoveMsg(fdl.Id, true));
        }

        //Moves the download up in the queue
        private void btnDLMoveUp_Click(object sender, EventArgs e)
        {
            int index = lstDownloads.SelectedIndex;
            if ((index < 0) || (index >= lstDownloads.Items.Count)) return;

            FileDownload fdl = (FileDownload)lstDownloads.Items[index];

            DownloadMsgQueue.Enqueue(new DlMoveMsg(fdl.Id, false));
        }

        //Reloads the mods
        private void btnReloadMods_Click(object sender, EventArgs e)
        {
            //TODO: Save current config?
            Program.mainForm.ModMan.LoadAllMods();
            Program.mainForm.UpdateModList();
        }

        //Reloads the characters
        private void btnReloadChars_Click(object sender, EventArgs e)
        {
            //TODO: Save current config?
            Program.mainForm.SkinMan.LoadAllSkins();
            Program.mainForm.UpdateCharacterList();
            Program.mainForm.UpdateSkinList();
        }

        //Open the Mods folder in explorer
        private void btnOpenMods_Click(object sender, EventArgs e)
        {
            Process.Start(Setup.AppPath + "Mods");
        }

        //Open the Characters folder in explorer
        private void btnOpenChars_Click(object sender, EventArgs e)
        {
            Process.Start(Setup.AppPath + "Skins");
        }

        //----------------------------------------------//
        // MISC                                         //
        //----------------------------------------------//

        //Check if a version is supported in the current Patchlunky version
        private bool VersionIsSupported(string version)
        {
            try
            {
                Version verPatchlunky = new Version(this.PatchlunkyVersion);
                Version verTarget = new Version(version);

                if (verPatchlunky < verTarget) return false;
            }
            catch (Exception ex)
            {
                Msg.Log("Error comparing versions: " + ex.Message);
                return false;
            }
            return true;
        }

        //For visiting a link
        private void OpenWebLink(string target)
        {
            if (target == null)
                return;

            //Check wether the url is valid
            Uri result;
            bool valid = Uri.TryCreate(target, UriKind.Absolute, out result) &&
                          (result.Scheme == Uri.UriSchemeHttp ||
                           result.Scheme == Uri.UriSchemeHttps ||
                           result.Scheme == Uri.UriSchemeMailto);

            if (valid)
                Process.Start(target);
        }

        //ListView_SetIconSpacing, see: https://msdn.microsoft.com/en-us/library/windows/desktop/bb761176%28v=vs.85%29.aspx
        private void ListView_SetIconSpacing(ListView listView, int cx, int cy)
        {
            const Int32 LVM_FIRST = 0x1000;
            const Int32 LVM_SETICONSPACING = (LVM_FIRST + 53);

            IntPtr lParam = (IntPtr)((UInt16)cx | ((UInt16)cy << 16));

            NativeMethods.SendMessage(listView.Handle, LVM_SETICONSPACING, IntPtr.Zero, lParam);
        }
    }
}
