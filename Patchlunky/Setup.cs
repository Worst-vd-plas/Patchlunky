﻿/* 
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
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.Win32;

using SpelunkyWad;
using Ionic.Zip;

namespace Patchlunky
{
    public class GameFile
    {
        public string Name;
        public string FilePath;
        public bool IsModdable;

        public GameFile(string path, bool ismoddable=true)
        {
            this.Name = Path.GetFileName(path);
            this.FilePath = path;
            this.IsModdable = ismoddable;
        }
    }

    public class Setup
    {
        public List<GameFile> DefaultFiles;
        
        public string GamePath; //Path to Spelunky
        public string AppPath; //Path to Patchlunky
        public string BackupPath; //Path to Patchlunky backups
        public string TempPath; //Path to Patchlunky temp folder

        public Setup()
        {
            Settings settings = Program.mainForm.Settings;

            this.DefaultFiles = new List<GameFile>();

            this.GamePath = settings.Get("GameDir");                        
            this.AppPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/";
            this.TempPath   = AppPath + "Patchlunky_Temp/";
            this.BackupPath = AppPath + "Patchlunky_Backup/";
            
            this.CreateSubDirs();
            this.InitDefaultFiles();            
        }

        //Create some needed subdirectories inside patchlunky folder
        private void CreateSubDirs()
        {
            if (Directory.Exists(this.AppPath))
            {
                Directory.CreateDirectory(AppPath + "Mods/");
                Directory.CreateDirectory(AppPath + "Skins/");
                Directory.CreateDirectory(TempPath);
                Directory.CreateDirectory(BackupPath);
                Directory.CreateDirectory(BackupPath + "Textures/");
                Directory.CreateDirectory(BackupPath + "Sounds/");
                Directory.CreateDirectory(BackupPath + "Music/");
                Directory.CreateDirectory(BackupPath + "Localization/");
                Directory.CreateDirectory(BackupPath + "Animations/");
                Directory.CreateDirectory(BackupPath + "Shaders/");
            }
        }

        //Create the list of game files to backup / patch
        private void InitDefaultFiles()
        {
            string[] musicfiles = { "Music/A01_A.ogg", 
                                    "Music/A01_B.ogg",
                                    "Music/A01_C.ogg",
                                    "Music/A01_dark.ogg",
                                    "Music/A01_egg.ogg",
                                    "Music/A01_first.ogg",
                                    "Music/A02_A.ogg",
                                    "Music/A02_B.ogg",
                                    "Music/A02_C.ogg",
                                    "Music/A02_cemetary.ogg",
                                    "Music/A02_dark.ogg",
                                    "Music/A02_egg.ogg",
                                    "Music/A02_haunted.ogg",
                                    "Music/A02_market.ogg",
                                    "Music/A02_worm.ogg",
                                    "Music/A03_A.ogg",
                                    "Music/A03_B.ogg",
                                    "Music/A03_C.ogg",
                                    "Music/A03_egg.ogg",
                                    "Music/A03_mother.ogg",
                                    "Music/A03_yeti.ogg",
                                    "Music/A04_A.ogg",
                                    "Music/A04_B.ogg",
                                    "Music/A04_C.ogg",
                                    "Music/A04_boss.ogg",
                                    "Music/A04_dark.ogg", 
                                    "Music/A04_egg.ogg",
                                    "Music/A04_gold.ogg",
                                    "Music/A05_A.ogg",
                                    "Music/A05_boss.ogg",
                                    "Music/A05_yama.ogg",
                                    "Music/Action_mxA.ogg",
                                    "Music/Action_mxB.ogg",
                                    "Music/adventure.ogg",
                                    "Music/amb_desert.ogg",
                                    "Music/amb_jungle.ogg",
                                    "Music/amb_mines.ogg",
                                    "Music/Credits.ogg",
                                    "Music/Credits_2.ogg",
                                    "Music/Credits_2b.ogg",
                                    "Music/Deathmatch_drums.ogg",
                                    "Music/Deathmatch_melody.ogg",
                                    "Music/endamb_in.ogg",
                                    "Music/endamb_out.ogg",
                                    "Music/Lobby.ogg",
                                    "Music/Lobby_jingle.ogg",
                                    "Music/Menu.ogg",
                                    "Music/olmec_intro.ogg",
                                    "Music/Shop_brothel.ogg",
                                    "Music/Shop_gameshow.ogg",
                                    "Music/Shop_slave.ogg",
                                    "Music/Shop_radio_1.ogg",
                                    "Music/Shop_radio_2.ogg",
                                    "Music/Shop_radio_3.ogg",
                                    "Music/Shop_radio_4.ogg",
                                    "Music/Title.ogg",
                                    "Music/Title_stripped.ogg",
                                    "Music/Trial_nag.ogg",
                                    "Music/ultraegg.ogg",
                                    "Music/yamaamb.ogg" };

            DefaultFiles.Add(new GameFile("Textures/alltex.wad", false));
            DefaultFiles.Add(new GameFile("Textures/alltex.wad.wix", false));
            DefaultFiles.Add(new GameFile("Textures/fonthud.fnt"));
            DefaultFiles.Add(new GameFile("Textures/fontkid.fnt"));
            DefaultFiles.Add(new GameFile("Textures/fontmain.fnt"));
            DefaultFiles.Add(new GameFile("Textures/loadring.png"));
            DefaultFiles.Add(new GameFile("Textures/mossmouth.png"));

            DefaultFiles.Add(new GameFile("Sounds/allsounds.wad", false));
            DefaultFiles.Add(new GameFile("Sounds/allsounds.wad.wix", false));
            DefaultFiles.Add(new GameFile("Sounds/reverbsettings.dat"));
            DefaultFiles.Add(new GameFile("Sounds/soundlist.dat"));

            DefaultFiles.Add(new GameFile("Animations/allanimations.wad"));
            DefaultFiles.Add(new GameFile("Localization/strings.pct"));
            DefaultFiles.Add(new GameFile("Shaders/infinite.fxb"));

            foreach (string musfile in musicfiles)
            {
                DefaultFiles.Add(new GameFile(musfile));
            }
        }

        //Checks if GameDir is valid
        public bool CheckGameDir() 
        {
            if (File.Exists(this.GamePath + "Spelunky.exe")) return true;
            return false;
        }

        //Check if backups have been done
        public bool CheckBackups() 
        {
            foreach (GameFile gmfile in DefaultFiles)
            {
                if (File.Exists(this.BackupPath + gmfile.FilePath) == false) return false;
            }
            return true;
        }
        
        //Backups the game files
        private bool DoBackups() 
        {
            string message;

            if (CheckGameDir() == false)
            {
                message = "The Spelunky directory is not set!" +
                    Environment.NewLine + Environment.NewLine +
                "Patchlunky cannot backup files until the directory is set.";
                Msg.MsgBox(message, "Patchlunky Setup");
                return false; //Path to game has not been set, can't backup.
            }

            foreach (GameFile gmfile in DefaultFiles)
            {
                if (File.Exists(this.BackupPath + gmfile.FilePath) == true)
                    continue; //This file has already been backed up.

                try
                {
                    string srcfile = this.GamePath + "Data/" + gmfile.FilePath;
                    string dstfile = this.BackupPath + gmfile.FilePath;
                    File.Copy(srcfile, dstfile, true);
                }
                catch (Exception ex)
                {
                    Msg.MsgBox("Error backing up file: " + ex.Message, "Patchlunky Setup");
                    return false; //Stop doing backups
                }
            }
 
            return true;
        }

        // Patch the game files
        public bool PatchGame(bool reset_only)
        {
            ModManager modMan = Program.mainForm.ModMan;
            SkinManager skinMan = Program.mainForm.SkinMan;
            
            string message;

            if (CheckGameDir() == false)
            {
                message = "The Spelunky directory is not set!" +
                    Environment.NewLine + Environment.NewLine +
                "Patchlunky cannot patch the game until the directory is set.";
                Msg.MsgBox(message, "Patchlunky Setup");
                return false; //Path to game has not been set, can't patch.
            }

            if (CheckBackups() == false)
            {
                message = "Patchlunky is missing Spelunky backups!" +
                    Environment.NewLine + Environment.NewLine +
                "Patchlunky cannot patch the game without backups.";
                Msg.MsgBox(message, "Patchlunky Setup");
                return false; //Backups are missing, can't patch.
            }

            Program.mainForm.SetProgress(0);

            // STEP 1 OF 4 - Copy default data files to temp folder
            foreach (GameFile gmfile in DefaultFiles)
            {
                try
                {
                    string srcfile = this.BackupPath + gmfile.FilePath;
                    string dstfile = this.TempPath + gmfile.FilePath;
                    Directory.CreateDirectory(Path.GetDirectoryName(dstfile));
                    File.Copy(srcfile, dstfile, true);
                }
                catch (Exception ex)
                {
                    Msg.MsgBox("Error trying to copy file: " + ex.Message, "Patchlunky Setup");
                    return false; //Stop patching
                }
            }

            Program.mainForm.SetProgress(33);

            // STEP 2 OF 4 - Copy mod files to temp folder
            if ((reset_only == false) && (modMan.Mods.Count > 0))
            {
                foreach (ModData mod in modMan.Mods)
                {
                    if (mod.Enabled == false)
                        continue;

                    //Patch game files in the mod.
                    if (PatchFiles(mod) == false)
                        return false; //Stop patching

                    //Patch wad resources in the mod.
                    PatchArchive(mod, "Textures/alltex");
                    PatchArchive(mod, "Sounds/allsounds");
                }
            }

            // STEP 3 OF 4 - Patch alltex.wad in temp folder with skin files
            if ((reset_only == false) && (skinMan.SkinConfig.Count > 0))
            {
                Archive archive = new Archive(TempPath + "Textures/alltex" + ".wad");
                archive.Load();

                int i = archive.Groups.FindIndex(o => o.Name.Equals("PLAYERS", StringComparison.OrdinalIgnoreCase));
                if (i == -1)
                    return false; //Missing "PLAYERS" group in archive, stop patching                

                foreach(KeyValuePair<string, string> kvp in skinMan.SkinConfig)
                {
                    SkinData oldSkin = skinMan.GetSkin(kvp.Key);
                    SkinData newSkin = skinMan.GetSkin(kvp.Value);

                    int j = archive.Groups[i].Entries.FindIndex(o => o.Name.Equals(oldSkin.Name + ".png", StringComparison.OrdinalIgnoreCase));
                    if (j == -1)
                        continue; //Skip missing character entry

                    Byte[] data = File.ReadAllBytes(newSkin.Path);
                    var new_entry = new Entry(oldSkin.Name + ".png", data);

                    //Replace entry in the archive.wad
                    archive.Groups[i].Entries[j] = new_entry;
                    //Msg.Log("Patching skin '" + oldSkin.Name + " to '" + newSkin.Name + "'.");
                }

                archive.Save();
            }

            Program.mainForm.SetProgress(66);

            // STEP 4 OF 4 - Copy files to Spelunky data folders.
            foreach (GameFile gmfile in DefaultFiles)
            {
                try
                {
                    string srcfile = this.TempPath + gmfile.FilePath;
                    string dstfile = this.GamePath + "Data/" + gmfile.FilePath;
                    File.Copy(srcfile, dstfile, true);
                }
                catch (Exception ex)
                {
                    Msg.MsgBox("Error patching file: " + ex.Message, "Patchlunky Setup");
                    return false; //Stop patching
                }
            }

            Program.mainForm.SetProgress(100);

            return true;
        }

        // Patch game files in the temp folder with files of a mod.
        public bool PatchFiles(ModData mod)
        {
            if (mod.IsZip == true)
                return PatchZipFiles(mod);
            else
                return PatchDirFiles(mod);
        }

        public bool PatchZipFiles(ModData mod)
        {
            ZipFile zip = new ZipFile(mod.Path);

            int count = 0;

            // Only mod files that match the original files are copied.
            foreach (GameFile gmfile in DefaultFiles)
            {
                if (zip.ContainsEntry(gmfile.FilePath) == true)
                {
                    //Msg.Log("zip: " + mod.Name + ", matching entry: " + gmfile.FilePath);

                    try
                    {
                        zip[gmfile.FilePath].Extract(this.TempPath, ExtractExistingFileAction.OverwriteSilently);
                        count++;
                    }
                    catch (Exception ex)
                    {
                        Msg.MsgBox("Error patching file: " + ex.Message, "Patchlunky Setup");
                        return false; //Stop patching
                    }
                }
            }

            if (count > 0) Msg.Log("Patched " + count + " files for " + mod.Name);
            return true;
        }

        public bool PatchDirFiles(ModData mod)
        {
            int count = 0;

            // Only mod files that match the original files are copied.
            foreach (GameFile gmfile in DefaultFiles)
            {
                // The full alltex.wad and allsounds.wad files are intentionally disabled from
                // being used in mods, as including the entire files works against the idea of
                // being able to combine multiple mods that change different resources inside 
                // the wad files. Not to mention that it makes the mods unnecessarily big in  
                // filesize. (although maybe this could be made to work anyways with some kind
                // of hash comparison with the entries of the wad files..)
                //NOTE: Commented this out, so old mods can work.
                //if (gmfile.IsModdable == false)
                //    continue;

                if (File.Exists(mod.Path + gmfile.FilePath) == true)
                {
                    //Try copying over modded file
                    try
                    {
                        string srcfile = mod.Path + gmfile.FilePath;
                        string dstfile = this.TempPath + gmfile.FilePath;
                        File.Copy(srcfile, dstfile, true);
                        //Msg.Log("Patching file '" + gmfile.FilePath + "' from " + mod.Name);
                        count++;
                    }
                    catch (Exception ex)
                    {
                        Msg.MsgBox("Error patching file: " + ex.Message, "Patchlunky Setup");
                        return false; //Stop patching
                    }
                }
            }
            if (count > 0) Msg.Log("Patched " + count + " files for " + mod.Name);
            return true;
        }

        // Patches an archive wad file in the temp folder with resources of a mod.
        public void PatchArchive(ModData mod, string archivepath)
        {
            if (mod.IsZip == true)
                PatchZipArchive(mod, archivepath);
            else
                PatchDirArchive(mod, archivepath);
        }

        public void PatchZipArchive(ModData mod, string archivepath)
        {
            Archive archive = new Archive(TempPath + archivepath + ".wad");
            archive.Load();

            ZipFile zip = new ZipFile(mod.Path);

            int count = 0;

            // Check if the mod has a directory for wad resources
            if (zip.ContainsEntry(archivepath + "/"))
            {
                //Msg.Log("zip has '" + archivepath + "' folder.");

                int i, j;

                // Go through the groups in the archive file.
                for (i = 0; i < archive.Groups.Count; i++)
                {
                    var group = archive.Groups[i];

                    //Check for matching subdirectory and archive group.
                    string grouppath = archivepath + "/" + group.Name + "/";
                    if (zip.ContainsEntry(grouppath))
                    {
                        //Go through the entries in the archive file
                        for (j = 0; j < archive.Groups[i].Entries.Count; j++)
                        {
                            var entry = archive.Groups[i].Entries[j];

                            //Check for matching file and archive entry.
                            string entrypath = grouppath + entry.Name;
                            if (zip.ContainsEntry(entrypath))
                            {
                                //Msg.Log("zip: " + mod.Name + ", matching res-entry: " + entrypath);

                                MemoryStream stream = new MemoryStream();
                                zip[entrypath].Extract(stream);

                                Byte[] data = stream.GetBuffer();
                                var new_entry = new Entry(entry.Name, data);

                                //Replace entry in the archive.wad with file in mod archive folder.
                                archive.Groups[i].Entries[j] = new_entry;
                                //Msg.Log("Patching '" + group.Name + "/" + entry.Name + "' from zip " + mod.Name);
                                count++;
                            }
                        }
                    }
                }
            }
            archive.Save();

            if (count > 0)  Msg.Log("Patched " + count + " " + archivepath + " resources for " + mod.Name);
        }

        public void PatchDirArchive(ModData mod, string archivepath)
        {
            Archive archive = new Archive(TempPath + archivepath + ".wad");
            archive.Load();

            string archive_dir = mod.Path + archivepath;

            int count = 0;

            // Check if the mod has a directory for wad resources
            if (Directory.Exists(archive_dir))
            {
                int i, j;
                string[] directories = Directory.GetDirectories(archive_dir);

                // Go through the subdirectories of the archive folder.
                foreach (string dir in directories)
                {
                    // Go through the groups in the archive file.
                    for (i = 0; i < archive.Groups.Count; i++)
                    {
                        var group = archive.Groups[i];

                        //Check for matching subdirectory and archive group.
                        string dirname = new DirectoryInfo(dir).Name;
                        if (dirname.Equals(group.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            string[] files = Directory.GetFiles(dir);

                            // Go through the files in the archive subfolders.
                            foreach (string file in files)
                            {
                                //Go through the entries in the archive file
                                for (j = 0; j < archive.Groups[i].Entries.Count; j++)
                                {
                                    var entry = archive.Groups[i].Entries[j];

                                    //Check for matching file and archive entry.
                                    string filename = Path.GetFileName(file);
                                    if (filename.Equals(entry.Name, StringComparison.OrdinalIgnoreCase))
                                    {
                                        Byte[] data = File.ReadAllBytes(file);
                                        var new_entry = new Entry(filename, data);

                                        //Replace entry in the archive.wad with file in mod archive folder.
                                        archive.Groups[i].Entries[j] = new_entry;
                                        //Msg.Log("Patching '" + group.Name + "/" + entry.Name + "' from " + mod.Name);
                                        count++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            archive.Save();

            if (count > 0) Msg.Log("Patched " + count + " " + archivepath + " resources for " + mod.Name);
        }

        //Displays dialogs for setting the game path and doing backups
        public void DoSetup(bool forced=false) 
        {
            DialogResult result;
            string message;

            if ((CheckGameDir() == false) || forced)
            {
                string path = null;

                message = "Before Patchlunky can be used it needs to know where Spelunky is installed to." +
                          Environment.NewLine + Environment.NewLine +
                          "Do you want Patchlunky to automatically search for Spelunky from known locations?";
                result = Msg.MsgBox(message, "Patchlunky Setup", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    path = FindGameDir();
                    if (path == null)
                    {
                        message = "Patchlunky could not find the Spelunky install directory." +
                           Environment.NewLine + Environment.NewLine +
                           "Do you want to manually locate the Spelunky directory?";
                        result = Msg.MsgBox(message, "Patchlunky Setup", MessageBoxButtons.YesNoCancel);

                        if (result == DialogResult.Yes)
                            path = BrowseGameDir();
                        
                    }
                    this.GamePath = path + "/";
                }
                else if (result == DialogResult.No)
                {
                    message = "Do you want to manually locate the Spelunky directory?";
                    result = Msg.MsgBox(message, "Patchlunky Setup", MessageBoxButtons.YesNoCancel);

                    if (result == DialogResult.Yes)
                    {
                        path = BrowseGameDir();
                        this.GamePath = path + "/";
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                }

                if(CheckGameDir() == false)
                {
                    message = "The Spelunky directory is not set!" +
                            Environment.NewLine + Environment.NewLine +
                            "Patchlunky will not work until the directory is set.";
                    Msg.MsgBox(message, "Patchlunky Setup");
                    return;
                }
            }
            
            if ((CheckBackups() == false) || forced)
            {
                message = "For Patchlunky to work, it needs to backup the original Spelunky data files." +
                    Environment.NewLine + Environment.NewLine +
                    "Do you want to backup the Spelunky data files now? (THE FILES SHOULD NOT BE MODDED!)";
                result = Msg.MsgBox(message, "Patchlunky Setup", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    DoBackups();
                }
                else if (result == DialogResult.No)
                {
                }
                else if (result == DialogResult.Cancel)
                {
                }

                if (CheckBackups() == false)
                {
                    message = "The Spelunky data files have not been backed up!" +
                    Environment.NewLine + Environment.NewLine +
                    "Patchlunky will not work until the files are backed up.";
                    Msg.MsgBox(message, "Patchlunky Setup");
                    return;
                }
            }

            message = "Patchlunky setup finished.";
            Msg.MsgBox(message, "Patchlunky Setup");
        }

        //Opens a dialog for browsing for the game directory
        private string BrowseGameDir()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Browse to Spelunky directory";
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            return null;
        }

        //Attempts to find where the game is installed
        //TODO: Add support for finding GOG version too
        private string FindGameDir()
        {
            RegistryKey SteamRegKey;
            List<string> SteamDirs = new List<string>();
            string path;

            //Check Registry for steam install directory            
            SteamRegKey = Registry.LocalMachine.OpenSubKey("Software\\Valve\\Steam");
            if (SteamRegKey != null)
            {
                path = (string)SteamRegKey.GetValue("InstallPath");
                if (Directory.Exists(path))
                {
                    SteamDirs.Add(path);

                    //Check config.vdf for additional steam libraries
                    if (File.Exists(path + "\\config\\config.vdf"))
                    {
                        try
                        {
                            using (StreamReader sr = new StreamReader(path + "\\config\\config.vdf"))
                            {
                                string read;
                                while ((read = sr.ReadLine()) != null)
                                {
                                    Match match = Regex.Match(read, "\"BaseInstallFolder_[\\d]+\"[\\s]*\"([^\"]*)\"");
                                    if (match.Success)
                                    {
                                        if (Directory.Exists(Regex.Unescape(match.Groups[1].Value)))
                                            SteamDirs.Add(Regex.Unescape(match.Groups[1].Value));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Msg.MsgBox("Error: config.vdf could not be read: " + ex.Message);
                            return null;
                        }
                    }
                }
            }

            for (int i = 0; i < SteamDirs.Count; i++)
            {
                path = SteamDirs[i] + "\\steamapps\\common\\Spelunky";
                if (Directory.Exists(path))
                {
                    return path;
                }
            }
            return null;
        }
    }
}
