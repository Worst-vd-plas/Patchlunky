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
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
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
                Directory.CreateDirectory(BackupPath + "Characters/");
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
            Settings settings = Program.mainForm.Settings;
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
                    {
                        Msg.Log("Patching files failed for mod: " + mod.Name + "!");
                        return false; //Stop patching
                    }

                    //Patch wad resources in the mod.
                    PatchArchive(mod, "Textures/alltex");
                    PatchArchive(mod, "Sounds/allsounds");

                    //Patch using patchlists provided by the mod
                    if (mod.Type.HasFlag(ModType.Xml))
                    {
                        //TODO!
                    }
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

                int count = 0;

                foreach(KeyValuePair<string, string> kvp in skinMan.SkinConfig)
                {
                    SkinData oldSkin = skinMan.GetSkin(kvp.Key);
                    SkinData newSkin = skinMan.GetSkin(kvp.Value);

                    if (oldSkin == null)
                        continue; //Skip unknown entry (this shouldn't happen)

                    if (newSkin == null)
                        continue; //Skip unknown skin (this may happen)

                    bool skinsMatch = kvp.Key.Equals(kvp.Value);

                    if (skinsMatch && settings.Check("ModsReplaceDefaultSkins", "True"))
                        continue; //Skip default skins

                    int j = archive.Groups[i].Entries.FindIndex(o => o.Name.Equals(oldSkin.Id + ".png", StringComparison.OrdinalIgnoreCase));
                    if (j == -1)
                        continue; //Skip missing character entry

                    //Read the new skin file
                    Byte[] new_data = Resource.LoadBytes(newSkin, newSkin.CharacterImgPath);
                    Entry new_entry = new Entry(oldSkin.Id + ".png", new_data);

                    //Replace entry in the archive.wad
                    archive.Groups[i].Entries[j] = new_entry;
                    //Msg.Log("Patching skin '" + oldSkin.Name + " to '" + newSkin.Name + "'.");

                    //Patch leaderboards image if included
                    if (newSkin.LeaderImgPath != null)
                    {
                        PatchLeaderpic(archive, oldSkin.Id, newSkin.Id);
                    }

                    if (skinsMatch == false)
                        count++;
                }

                if (count > 0) Msg.Log("Patched " + count + " character skins");

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
            if(mod.PatchDefaultFiles)
            {
                if (mod.Type.HasFlag(ModType.Zip))
                    return PatchZipFiles(mod);
                else
                    return PatchDirFiles(mod);
            }
            return true;
        }

        public bool PatchZipFiles(ModData mod)
        {
            ZipFile zip = new ZipFile(mod.ModPath);

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

                if (File.Exists(mod.ModPath + gmfile.FilePath) == true)
                {
                    //Try copying over modded file
                    try
                    {
                        string srcfile = mod.ModPath + gmfile.FilePath;
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
            if (mod.PatchDefaultFiles)
            {
                if (mod.Type.HasFlag(ModType.Zip))
                    PatchZipArchive(mod, archivepath);
                else
                    PatchDirArchive(mod, archivepath);
            }
        }

        public void PatchZipArchive(ModData mod, string archivepath)
        {
            ZipFile zip = new ZipFile(mod.ModPath);

            // Check if the mod has a directory for wad resources
            if (zip.ContainsEntry(archivepath + "/"))
            {
                Archive archive = new Archive(TempPath + archivepath + ".wad");
                archive.Load();

                int i, j;
                int count = 0;

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
                archive.Save();

                if (count > 0) Msg.Log("Patched " + count + " " + archivepath + " resources for " + mod.Name);
            }            
        }

        public void PatchDirArchive(ModData mod, string archivepath)
        {
            string archive_dir = mod.ModPath + archivepath;

            // Check if the mod has a directory for wad resources
            if (Directory.Exists(archive_dir))
            {
                Archive archive = new Archive(TempPath + archivepath + ".wad");
                archive.Load();

                int i, j;
                int count = 0;

                // Go through the groups in the archive file.
                for (i = 0; i < archive.Groups.Count; i++)
                {
                    var group = archive.Groups[i];

                    //Check for matching subdirectory and archive group.
                    if (Directory.Exists(archive_dir + "/" + group.Name))
                    {
                        //Go through the entries in the archive file
                        for (j = 0; j < archive.Groups[i].Entries.Count; j++)
                        {
                            var entry = archive.Groups[i].Entries[j];

                            //Check for matching file and archive entry.
                            string filepath = archive_dir + "/" + group.Name + "/" + entry.Name;
                            if (File.Exists(filepath))
                            {
                                Byte[] data = File.ReadAllBytes(filepath);
                                var new_entry = new Entry(entry.Name, data);

                                //Replace entry in the archive.wad with file in mod archive folder.
                                archive.Groups[i].Entries[j] = new_entry;

                                //Msg.Log("Patching '" + group.Name + "/" + entry.Name + "' from " + mod.Name);
                                count++;
                            }
                        }
                    }
                }

                archive.Save();

                if (count > 0) Msg.Log("Patched " + count + " " + archivepath + " resources for " + mod.Name);
            }
        }

        public void PatchLeaderpic(Archive archive, string oldId, string newId)
        {
            //oldId has to match one in the list (Hardcoded as currently new characters cannot be added to the game)
            string[] originalIds = { "char_orange", "char_red",   "char_green",  "char_blue",
                                     "char_white",  "char_pink",  "char_yellow", "char_brown",
                                     "char_purple", "char_black", "char_cyan",   "char_lime",
                                     "char_dlc1",   "char_dlc2",  "char_dlc3",   "char_dlc4",
                                     "char_dlc5",   "char_dlc6",  "char_dlc7",   "char_dlc8" };

            int char_index = Array.IndexOf(originalIds, oldId);
            if (char_index == -1)
                return; //Invalid character

            SkinData skin = Program.mainForm.SkinMan.GetSkin(newId);
            if (skin == null)
                return; //Unknown skin

            //Get the Leaderboards image
            Bitmap sourceBMP = Resource.LoadBitmap(archive, "LEADERBOARD/TU_leaderpics.png");
            if (sourceBMP == null)
                return; //Leaderboards image is missing

            //Need a valid leaderboard picture from the new skin
            Bitmap patchBMP = Resource.LoadBitmap(skin, skin.LeaderImgPath);
            if (patchBMP == null)
                return; //No image to patch

            //Patch the image
            int x = (char_index % 4) * 128;
            int y = (char_index / 4) * 48;
            Bitmap resultBMP = BitmapReplaceRegion(sourceBMP, patchBMP, x, y, 128, 48);

            //Convert bmp to PNG
            MemoryStream ms = new MemoryStream();
            resultBMP.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            Byte[] new_data = ms.ToArray();

            //Replace entry in the archive.wad
            Resource.ReplaceBytes(archive, "LEADERBOARD/TU_leaderpics.png", new_data);

            //File.WriteAllBytes(TempPath + "TU_leaderpics.png", new_data); //DEBUG

            ms.Dispose();
            resultBMP.Dispose();
            patchBMP.Dispose();
            sourceBMP.Dispose();
        }

        //Replace a bitmap region with the region of another
        public Bitmap BitmapReplaceRegion(Bitmap destBMP, Bitmap srcBMP, int x1, int y1, int width, int height)
        {
            //This is a bit of a hack, because System.Drawing.Graphics doesn't support drawing
            //transparent regions with much flexibility. The point is to have the transparent
            //regions in patchBMP still be transparent when the two images are combined, so
            //sourceBMP will not be visible through the transparent areas in patchBMP.
            Bitmap resultBMP = new Bitmap(destBMP);

            int x2 = x1 + width;
            int y2 = y1 + height;

            Rectangle rect_U = new Rectangle( 0,   0, destBMP.Width     ,                  y1); //Up
            Rectangle rect_L = new Rectangle( 0,  y1,                 x1,              height); //Left
            Rectangle rect_R = new Rectangle(x2,  y1, destBMP.Width - x2,              height); //Right
            Rectangle rect_D = new Rectangle( 0,  y2, destBMP.Width     , destBMP.Height - y2); //Down
            Rectangle rect_P = new Rectangle(x1,  y1,              width,              height); //Patch

            using (Graphics g = Graphics.FromImage(resultBMP))
            {
                g.PageUnit = GraphicsUnit.Pixel;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.Clear(Color.Transparent);

                g.DrawImage(destBMP, rect_U, rect_U.Left, rect_U.Top, rect_U.Width, rect_U.Height, GraphicsUnit.Pixel);
                g.DrawImage(destBMP, rect_L, rect_L.Left, rect_L.Top, rect_L.Width, rect_L.Height, GraphicsUnit.Pixel);
                g.DrawImage(destBMP, rect_R, rect_R.Left, rect_R.Top, rect_R.Width, rect_R.Height, GraphicsUnit.Pixel);
                g.DrawImage(destBMP, rect_D, rect_D.Left, rect_D.Top, rect_D.Width, rect_D.Height, GraphicsUnit.Pixel);
                g.DrawImage(srcBMP , rect_P,           0,          0,        width,        height, GraphicsUnit.Pixel);
            }

            return resultBMP;
        }

        //Draw on top of a bitmap region with the region of another
        public Bitmap BitmapOverlayRegion(Bitmap destBMP, Bitmap srcBMP, int x, int y, int width, int height)
        {
            Bitmap resultBMP = new Bitmap(destBMP);

            Rectangle rect_P = new Rectangle(x, y, width, height);

            using (Graphics g = Graphics.FromImage(resultBMP))
            {
                g.PageUnit = GraphicsUnit.Pixel;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                g.DrawImage(srcBMP, rect_P, 0, 0, width, height, GraphicsUnit.Pixel);
            }

            return resultBMP;
        }

        //-----------------------------------------------------------//
        // URL protocol wizard
        //-----------------------------------------------------------//

        // For handling the URLs passed with the custom URI scheme
        public void OpenPatchlunkyUrl(string patchlunkyUrl)
        {
            if (patchlunkyUrl.StartsWith("patchlunky:", StringComparison.OrdinalIgnoreCase) == false)
                return; //Invalid URL protocol, this should not happen.

            //Msg.Log("Received Patchlunky URL command: " + patchlunkyUrl);

            string downloadUrl = null;
            string downloadMod = "patchlunky://download/mod/";
            string downloadChar = "patchlunky://download/char/";
            int downloadType = 0;

            // Check for download/mod command
            if (patchlunkyUrl.StartsWith(downloadMod, StringComparison.OrdinalIgnoreCase) == true)
            {
                downloadType = 1;
                downloadUrl = patchlunkyUrl.Substring(downloadMod.Length);
            }
            // Check for download/char command
            else if (patchlunkyUrl.StartsWith(downloadChar, StringComparison.OrdinalIgnoreCase) == true)
            {
                downloadType = 2;
                downloadUrl = patchlunkyUrl.Substring(downloadChar.Length);
            }
            else
            {
                Msg.Log("Invalid Patchlunky URL command!: " + patchlunkyUrl);
                return;
            }


            //Check wether the url is valid
            Uri resultUri;
            bool valid = Uri.TryCreate(downloadUrl, UriKind.Absolute, out resultUri) &&
                          (resultUri.Scheme == Uri.UriSchemeHttp ||
                           resultUri.Scheme == Uri.UriSchemeHttps);

            if (!valid)
            {
                Msg.Log("Invalid download URL!: " + patchlunkyUrl);
                return;
            }

            //The URL should be fine if we get this far
            DialogResult result;
            string message;

            message = "Do you want to download the " + ((downloadType == 2) ? "character" : "mod") +
                      " from the internet address below?" + Environment.NewLine + Environment.NewLine +
                      downloadUrl + Environment.NewLine + Environment.NewLine +
                      "Warning: Files downloaded from the Internet could potentially harm you or your computer. " +
                      "You should only download files from sources you trust.";
            result = Msg.MsgBox(message, "Patchlunky URL protocol", MessageBoxButtons.YesNoCancel);

            if (result != DialogResult.Yes)
                return; //User chose NO or CANCEL

            using (WebClient client = new WebClient())
            {
                // Download the file.
                // TODO: Show progress, use background thread, etc.
                Byte[] data;

                try
                {
                    data = client.DownloadData(downloadUrl);
                }
                catch (Exception ex)
                {
                    Msg.MsgBox("Error downloading file:" + ex.Message, "Patchlunky URL protocol");
                    return;
                }

                string filepath = null;
                string filename = null;

                // This is a hacky workaround to get the filename since System.Net.Mime.ContentDisposition
                // does not appear to be parsing correctly..
                string contentDisp = client.ResponseHeaders["content-disposition"];
                string param = "filename=";
                int start_index = contentDisp.LastIndexOf(param, StringComparison.OrdinalIgnoreCase);
                int last_index = -1;
                if (start_index != -1)
                {
                    start_index += param.Length;
                    last_index = contentDisp.IndexOf(';', start_index);

                    if(last_index != -1)
                        filename = contentDisp.Substring(start_index, last_index-start_index);
                    else
                        filename = contentDisp.Substring(start_index);

                    filename = filename.Replace("\"", "");
                    filename = filename.TrimEnd(';');
                    //Msg.Log("Filename: '" + filename + "'");
                }

                //Just give it a generic filename if we couldn't get the filename from the response headers
                if (filename == null)
                {
                    filename = ((downloadType == 2) ? "character" : "mod") +
                              DateTime.Now.ToString("yyyyyMMddHHmmssffff") +
                               ((downloadType == 2) ? ".plc" : ".plm");
                }

                //Make sure the supplied filename is valid for patchlunky
                if (Xmf.PathIsValid(filename) == false)
                {
                    Msg.Log("filename: '" + filename + "' is not valid!");
                    return;
                }

                if (downloadType == 1)
                    filepath = this.AppPath + "Mods/" + filename;
                else if(downloadType == 2)
                    filepath = this.AppPath + "Skins/" + filename;

                //Check if the file already exists
                if (File.Exists(filepath) == true)
                {
                    message = "The " + ((downloadType == 2) ? "character" : "mod") + " '" + filename + "' already exists!" +
                                Environment.NewLine + Environment.NewLine + "Do you want to OVERWRITE it?";
                    result = Msg.MsgBox(message, "Patchlunky URL protocol", MessageBoxButtons.YesNoCancel);

                    if (result != DialogResult.Yes)
                        return;
                }

                //Save the file to disk
                try
                {
                    File.WriteAllBytes(filepath, data);
                    Msg.Log("Download finished, " + ((downloadType == 2) ? "character" : "mod") + ": " + filename);
                }
                catch (Exception ex)
                {
                    Msg.MsgBox("Error saving file: " + ex.Message);
                    return;
                }

                //Refresh the mods or characters
                if (downloadType == 1)
                {
                    Program.mainForm.ModMan.LoadAllMods();
                    Program.mainForm.UpdateModList();
                }
                else if (downloadType == 2)
                {
                    Program.mainForm.SkinMan.LoadAllSkins();
                    Program.mainForm.UpdateCharacterList();
                    Program.mainForm.UpdateSkinList();
                }
            }
        }

        //-----------------------------------------------------------//
        // Setup wizard
        //-----------------------------------------------------------//

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
        private string FindGameDir()
        {
            DialogResult result;
            string message;
            string path;

            //GOG Spelunky install
            RegistryKey GOGRegKey;
            const string GOGgameID = "1207659257"; //GOG gameID for Spelunky
            string path_gog = null;

            //Steam Spelunky install
            RegistryKey SteamRegKey;
            List<string> SteamDirs = new List<string>();
            string path_steam = null;


            //Check Registry for GOG install directory
            GOGRegKey = Registry.LocalMachine.OpenSubKey("Software\\GOG.com\\Games\\" + GOGgameID);
            if (GOGRegKey != null)
            {
                path = (string)GOGRegKey.GetValue("PATH");
                if (Directory.Exists(path))
                {
                    path_gog = path;
                }
            }

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

                    //Check the steam directories for Spelunky installs
                    for (int i = 0; i < SteamDirs.Count; i++)
                    {
                        path = SteamDirs[i] + "\\steamapps\\common\\Spelunky";
                        if (Directory.Exists(path))
                        {
                            path_steam = path;
                            break;
                        }
                    }
                }
            }

            //If both a Steam and GOG version is found.
            if ((path_steam != null) && (path_gog != null))
            {
                message = "Found BOTH the Steam and GOG versions of Spelunky." + Environment.NewLine
                + Environment.NewLine +
                "Choose which Spelunky version should be used with Patchlunky: " + Environment.NewLine +
                "(YES): Steam version " + Environment.NewLine +
                "(NO): GOG version " + Environment.NewLine;
                result = Msg.MsgBox(message, "Patchlunky Setup", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    Msg.Log("Using Steam spelunky directory.");
                    return path_steam;
                }
                else if (result == DialogResult.No)
                {
                    Msg.Log("Using GOG spelunky directory.");
                    return path_gog;
                }
                else /*if (result == DialogResult.Cancel)*/ return null;
            }
            //Steam version found
            else if (path_steam != null)
            {
                Msg.Log("Using Steam spelunky directory.");
                return path_steam;
            }
            //GOG version found
            else if (path_gog != null)
            {
                Msg.Log("Using GOG spelunky directory.");
                return path_gog;
            }
            //Found neither version
            return null;
        }
    }
}
