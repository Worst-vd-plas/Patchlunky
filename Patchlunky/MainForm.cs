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

using SpelunkyWad;

namespace Patchlunky
{
    public partial class MainForm : Form
    {
        public Settings Settings;
        public Setup Setup;
        public ModManager ModMan;
        public SkinManager SkinMan;
        private ListViewItem CharItem;
        private ListViewItem SkinItem;

        public MainForm()
        {
            InitializeComponent();

            System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
            this.Text = "Patchlunky " + System.Diagnostics.FileVersionInfo.GetVersionInfo(ass.Location).ProductVersion.ToString();

            CharItem = null;
            SkinItem = null;
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
            ModMan.LoadConfigList();
            //ModMan.LoadConfig("Default");
            
            SkinMan.LoadAllSkins();
            SkinMan.LoadConfig("Default");


            UpdateModList();            
            UpdateModConfigList();

            UpdateCharacterList();

            UpdateSettingsTab();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateModConfiguration();
            ModMan.SaveConfig(ModMan.CurrentConfig); //TODO: Only auto-save default
            ModMan.SaveConfigList();

            UpdateSkinConfiguration();
            SkinMan.SaveConfig("Default");


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
            ModMan.SaveConfig(ModMan.CurrentConfig); //TODO: Only auto-save default

            UpdateSkinConfiguration();
            SkinMan.SaveConfig("Default");

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
            cboConfig.Items.Clear();

            foreach (var conf in ModMan.Configs)
            {
                cboConfig.Items.Add(conf);
            }

            cboConfig.SelectedItem = ModMan.CurrentConfig;
        }

        //Save mod configuration button
        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateModConfiguration();

            string modconfig = cboConfig.Text;
            if (ModMan.ConfigExists(modconfig) == false)
            {
                ModMan.Configs.Add(modconfig);
                cboConfig.Items.Add(modconfig);                
            }
            ModMan.SaveConfig(modconfig);
        }

        //Mod configuration list index changed
        private void cboConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            string modconfig = cboConfig.Text;
            if (ModMan.ConfigExists(modconfig))
            {
                ModMan.CurrentConfig = modconfig;
                ModMan.LoadConfig(modconfig);
                UpdateModList();
            }
        }

        //Delete mod configuration button
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string modconfig = cboConfig.Text;
            
            if (modconfig.Equals("Default")) return;

            if (ModMan.ConfigExists(modconfig))
            {
                ModMan.RemoveConfig(modconfig);
                ModMan.Configs.Remove(modconfig);
                cboConfig.Items.Remove(modconfig);

                //Switch back to default config
                cboConfig.SelectedItem = "Default";
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

        //Clears and Recreates the listviews of characters and skins
        private void UpdateCharacterList()
        {
            lvwCharacters.Clear();
            lvwSkins.Clear();

            lvwCharacters.LargeImageList = SkinMan.SkinImageList;
            lvwSkins.LargeImageList = SkinMan.SkinImageList;

            //Fill character and skin list
            foreach (var skin in SkinMan.Skins)
            {
                if (skin.IsDefault == true)
                {
                    lvwCharacters.Items.Add(skin.Name, skin.Name, skin.Name);
                }
                lvwSkins.Items.Add(skin.Name, skin.Name);
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
        }

        //Switches beetween Character/Skin browsing in the character skin tab
        private void SwitchSkinView(bool characters)
        {
            lvwCharacters.Visible = characters;
            lblCharSelect.Visible = characters;  

            lvwSkins.Visible = !characters;
            lblSkinSelect.Visible = !characters;
        }

        //Double-click on a character
        private void lvwCharacters_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CharItem = (lvwCharacters.SelectedItems.Count > 0) ? lvwCharacters.SelectedItems[0] : null;

            if (CharItem != null)
            {
                SwitchSkinView(characters: false);
            }            
            UpdateSkinTab();
        }

        //Double-click on a skin
        private void lvwSkins_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SkinItem = (lvwSkins.SelectedItems.Count > 0) ? lvwSkins.SelectedItems[0] : null;

            if (SkinItem != null)
            {
                CharItem.ImageKey = SkinItem.ImageKey;
                SwitchSkinView(characters: true);
            }
            UpdateSkinTab();
        }

        //TODO: Remove this
        private void UpdateSkinTab()
        {
            picCharacter.Image = null;
            picSkin.Image = null;

            if (CharItem != null)
            {
                picCharacter.Image = CharItem.ImageList.Images[CharItem.Name];
                picSkin.Image = CharItem.ImageList.Images[CharItem.ImageKey];
            }

            //if (SkinItem != null)
            //    picSkin.Image = SkinItem.ImageList.Images[SkinItem.ImageKey];
        }

        //Change selection in character listview. TODO: Remove
        private void lvwCharacters_SelectedIndexChanged(object sender, EventArgs e)
        {
            CharItem = (lvwCharacters.SelectedItems.Count > 0) ? lvwCharacters.SelectedItems[0] : null;
            UpdateSkinTab();
        }

        //Change selection in skin listview. TODO: Remove
        private void lvwSkins_SelectedIndexChanged(object sender, EventArgs e)
        {
            SkinItem = (lvwSkins.SelectedItems.Count > 0) ? lvwSkins.SelectedItems[0] : null;
            UpdateSkinTab();
        }

        //Resets selected skin back to default
        private void btnRestoreSkin_Click(object sender, EventArgs e)
        {
            if (lvwCharacters.SelectedItems.Count > 0)
            {
                lvwCharacters.SelectedItems[0].ImageKey = lvwCharacters.SelectedItems[0].Name;
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

    }
}
