/* 
 * Copyright (c) 2016, Worst-vd-plas
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

using SpelunkyWad;

namespace Patchlunky
{
    public partial class MainForm : Form
    {
        public string PatchlunkyVersion;

        public Settings Settings;
        public Setup Setup;
        public ModManager ModMan;
        public SkinManager SkinMan;
        private ListViewItem CharItem;
        private ListViewItem SkinItem;
        private bool SkinBrowserActive;

        public MainForm()
        {
            InitializeComponent();

            //Title bar
            System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
            PatchlunkyVersion = FileVersionInfo.GetVersionInfo(ass.Location).ProductVersion;
            this.Text = "Patchlunky " + PatchlunkyVersion + " Beta";

            //Characters tab
            CharItem = null;
            SkinItem = null;
            SkinBrowserActive = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Msg.Log(this.Text + " started.");

            Settings = new Settings();
            Setup = new Setup();
            ModMan = new ModManager();
            SkinMan = new SkinManager();

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
        }

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
                Msg.MsgBox(message, "Patchlunky");
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
                string modname = (string)chklstMods.Items[i];
                bool check = chklstMods.GetItemChecked(i);

                ModData mod = ModMan.GetMod(modname);
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
        private void UpdateModList()
        {
            chklstMods.Items.Clear();

            foreach (var mod in ModMan.Mods)
            {
                chklstMods.Items.Add(mod.Name, mod.Enabled);
            }
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
            int index = chklstMods.SelectedIndex;
            if ((index < 0) || (index > chklstMods.Items.Count)) return;

            string modname = (string)chklstMods.Items[index];

            ModData mod = ModMan.GetMod(modname);
            if (mod != null)
            {
                lblModName.Text = mod.Name;
                txtModInfo.Text = mod.Text;
                if (mod.Image != null)
                {
                    picModImage.Image = mod.Image;
                    picModImage.Height = 144;
                    txtModInfo.Top = picModImage.Bottom + 4;
                }
                else
                {
                    picModImage.Height = 0;
                    txtModInfo.Top = picModImage.Bottom;
                }

                txtModInfo.Height = chklstMods.Bottom - txtModInfo.Top;
            }
            else
            {
                lblModName.Text = "";
                txtModInfo.Text = "";
                picModImage.Image = null;
            }
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
        private void UpdateSkinList()
        {
            lvwSkins.Clear();
            lvwSkins.LargeImageList = SkinMan.SkinImageList;

            //Fill skin list
            foreach (var skin in SkinMan.Skins)
                lvwSkins.Items.Add(skin.Id, "", skin.Id);

            ListView_SetIconSpacing(lvwSkins, 80, 80);
        }

        //Clears and Recreates the listview of characters
        private void UpdateCharacterList()
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

            lblSkinInfo.Text = ""; //Main fields
            lblSkinVersion.Text = ""; //Supported version

            lnkSkinWebUrl.Text = ""; //WebUrl link
            lnkSkinWebUrl.Links.Clear();
            toolTip1.SetToolTip(lnkSkinWebUrl, "");

            lnkSkinEmail.Text = ""; //Email link
            lnkSkinEmail.Links.Clear();
            toolTip1.SetToolTip(lnkSkinEmail, "");

            txtSkinInfo.Text = ""; //Description


            //Display the original character for the current item
            if (CharItem != null)
            {
                picCharacter.Image = CharItem.ImageList.Images[CharItem.Name];
            }

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
            if (skin != null)
            {
                lblSkinInfo.Text += "Name: " + (skin.Name ?? "-") + Environment.NewLine;
                lblSkinInfo.Text += "Author: " + (skin.Author ?? "-") + Environment.NewLine;
                lblSkinInfo.Text += "Date: " + (skin.Date.HasValue ? skin.Date.Value.ToShortDateString() : "-") + Environment.NewLine;
                lblSkinInfo.Text += "Version: " + (skin.Version ?? "-") + Environment.NewLine;

                //Display supported version for this skin
                if (skin.PatchlunkyVersion != null)
                {
                    try
                    {
                        Version verPatchlunky = new Version(this.PatchlunkyVersion);
                        Version verSkinNeeds = new Version(skin.PatchlunkyVersion);

                        if (verSkinNeeds > verPatchlunky)
                        {
                            lblSkinVersion.ForeColor = Color.Red;
                            lblSkinVersion.Text += "Patchlunky version needed: " + skin.PatchlunkyVersion;
                        }
                        else
                        {
                            lblSkinVersion.ForeColor = Color.Green;
                            lblSkinVersion.Text += "Patchlunky version: " + skin.PatchlunkyVersion;
                        }
                    }
                    catch (Exception ex)
                    {
                        Msg.Log("Error comparing versions: " + ex.Message);
                    }
                }

                if (skin.WebUrl != null)
                {
                    lnkSkinWebUrl.Text = "Link";
                    lnkSkinWebUrl.Links.Add(0, 4, skin.WebUrl);
                    toolTip1.SetToolTip(lnkSkinWebUrl, skin.WebUrl);
                }

                if (skin.Email != null)
                {
                    lnkSkinEmail.Text = "Email";
                    lnkSkinEmail.Links.Add(0, 5, skin.Email);
                    toolTip1.SetToolTip(lnkSkinEmail, skin.Email);
                }

                txtSkinInfo.Text = skin.Description;

                //Display the new character for the current item
                picSkin.Image = CharItem.ImageList.Images[skinName];
            }
            else if(skinName != null) //Unknown skin
            {
                lblSkinInfo.Text += "Unknown character: " + skinName + Environment.NewLine;
                lblSkinInfo.Text += Environment.NewLine;
                lblSkinInfo.Text += "This character will not be patched, because the files are missing!" + Environment.NewLine;
            }
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

        //----------------------------------------------//
        // MISC                                         //
        //----------------------------------------------//

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

        //Import the SendMessage function from user32.dll
        [DllImport("user32.dll")]
        //private static extern IntPtr SendMessage(HandleRef hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        //ListView_SetIconSpacing, see: https://msdn.microsoft.com/en-us/library/windows/desktop/bb761176%28v=vs.85%29.aspx
        private void ListView_SetIconSpacing(ListView listView, int cx, int cy)
        {
            const Int32 LVM_FIRST = 0x1000;
            const Int32 LVM_SETICONSPACING = (LVM_FIRST + 53);

            IntPtr lParam = (IntPtr)((UInt16)cx | ((UInt16)cy << 16));
            //HandleRef hr = new HandleRef(listView, listView.Handle);

            SendMessage(listView.Handle, LVM_SETICONSPACING, IntPtr.Zero, lParam);
            //SendMessage(hr, LVM_SETICONSPACING, IntPtr.Zero, lParam);
        }
    }
}
