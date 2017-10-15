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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;

using SpelunkyWad;

using Ionic.Zip;

namespace Patchlunky
{
    public enum SkinType
    {
        Png = 0,
        Zip,
        Dir,
    };

    public class SkinData
    {
        public SkinType Type;
        public bool IsDefault;

        //Paths
        public string ModPath; //Path to directory or zip file that contains the character
        public string CharacterImgPath;
        public string PreviewImgPath;
        public string LeaderImgPath;

        //Character information
        public string Name;
        public string Id;
        public string Version;
        public DateTime? Date;
        public string Author;
        public string Email;
        public string WebUrl;
        public string Description;
        public string PatchlunkyVersion;

        public Image PreviewImage;

        public SkinData(SkinType type, string modpath, bool isdefault)
        {
            this.Type = type;
            this.ModPath = modpath;
            this.IsDefault = isdefault;

            //Init values
            this.CharacterImgPath = null;
            this.PreviewImgPath = null;
            this.LeaderImgPath = null;

            this.Name = null;
            this.Id = null;
            this.Version = null;
            this.Date = null;
            this.Author = null;
            this.Email = null;
            this.WebUrl = null;
            this.Description = null;
            this.PatchlunkyVersion = null;

            this.PreviewImage = null;
        }

        public void LoadPreview()
        {
            string imgfile;
            Image sourceImg;

            if (this.Type == SkinType.Zip)
            {
                if (this.PreviewImgPath != null)
                    imgfile = this.PreviewImgPath;
                else
                    imgfile = this.CharacterImgPath;
                
                ZipFile zip = new ZipFile(this.ModPath);

                if (zip.ContainsEntry(imgfile) == false)
                {
                    zip.Dispose();
                    Msg.Log("Preview image missing for '" + this.Id + "'!");
                    return;
                }

                MemoryStream stream = new MemoryStream();
                zip[imgfile].Extract(stream);
                zip.Dispose();
                stream.Position = 0;

                sourceImg = Image.FromStream(stream);
            }
            else //image file or directory
            {
                if (this.PreviewImgPath != null)
                    imgfile = this.ModPath + "/" + this.PreviewImgPath;
                else
                    imgfile = this.ModPath + "/" + this.CharacterImgPath;

                if (File.Exists(imgfile) == false)
                {
                    Msg.Log("Preview image missing for '" + this.Id + "'!");
                    return;
                }

                sourceImg = Image.FromFile(imgfile);
            }

            Bitmap sourceBMP = new Bitmap(sourceImg);
            Rectangle previewRect = new Rectangle(0, 0, 80, 80);

            //TODO: Add error handling here
            this.PreviewImage = sourceBMP.Clone(previewRect, sourceBMP.PixelFormat);

            sourceImg.Dispose();
            sourceBMP.Dispose();
        }
    }

    public class SkinManager
    {
        public List<SkinData> Skins;
        public ImageList SkinImageList;
        public Dictionary<string, string> SkinConfig;
        public List<string> Configs;
        public string CurrentConfig;

        public SkinManager()
        {
            this.Skins = new List<SkinData>();
            this.SkinImageList = new ImageList();
            this.SkinConfig = new Dictionary<string, string>();
            this.Configs = new List<string>();
        }
        
        //Extracts the original skins from the backups
        private void ExtractDefaultSkins()
        {
            Setup setup = Program.mainForm.Setup;

            if (setup.CheckBackups() == false)
            {
                Msg.Log("Failed to extract default skins.");
                string message = "Patchlunky is missing Spelunky backups!" +
                    Environment.NewLine + Environment.NewLine +
                "Patchlunky character skins will not work without backups.";
                Msg.MsgBox(message, "Patchlunky");
                return; //Backups are missing, can't extract.
            }

            Archive alltex = new Archive(setup.BackupPath + "Textures/alltex.wad");
            alltex.Load();

            var group = alltex.Groups.Find(o => o.Name.Equals("PLAYERS"));
            if (group.Equals(default(SpelunkyWad.Group)))
                return; //Group not found

            string dirpath = setup.BackupPath + "Characters/";
            Directory.CreateDirectory(dirpath);

            foreach (var entry in group.Entries)
            {
                string filepath = Path.Combine(dirpath, entry.Name);
                File.WriteAllBytes(filepath, entry.Data);
            }
        }

        //GetSkin - Finds a skin by id
        public SkinData GetSkin(string skinid)
        {
            SkinData skin = Skins.Find(o => o.Id.Equals(skinid, StringComparison.OrdinalIgnoreCase));
            if (skin != null && skin.Equals(default(SkinData)))
                return null; //no skin with skinname.

            return skin;
        }

        //LoadAllSkins - Main function for loading skins to patchlunky
        public void LoadAllSkins()
        {
            Setup setup = Program.mainForm.Setup;

            ExtractDefaultSkins();
            

            LoadSkins(setup.BackupPath + "Characters/", true); //Original skins
            LoadSkins(setup.AppPath + "Skins/", false); //Custom skins

            Msg.Log("Loaded " + Skins.Count + " character skins.");

            //Create imagelist
            this.SkinImageList.ImageSize = new Size(80, 80);
            this.SkinImageList.ColorDepth = ColorDepth.Depth24Bit;

            foreach (var skin in this.Skins)
            {
                SkinImageList.Images.Add(skin.Id, skin.PreviewImage);
            }
        }

        //LoadSkins - Loads skins from path
        private void LoadSkins(string path, bool isdefault)
        {
            LoadImageSkins(path, isdefault);
            LoadDirectorySkins(path, isdefault);
            LoadZipSkins(path, isdefault);
        }

        //Load characters that are plain .png files
        private void LoadImageSkins(string path, bool isdefault)
        {
            if (Directory.Exists(path) == false)
            {
                Msg.Log("Failed to load characters from: " + path);
                return;
            }

            //Get all the *.png files
            string[] skins = Directory.GetFiles(path, "*.png");

            foreach (string skinpath in skins)
            {
                if (File.Exists(skinpath))
                {
                    string id = Path.GetFileNameWithoutExtension(skinpath);
                    Image skinImg = Image.FromFile(skinpath);
                    int width = skinImg.Width;
                    int height = skinImg.Height;
                    skinImg.Dispose();

                    //Make sure it is a valid character
                    if ((width < 80) || (height < 80))
                    {
                        Msg.Log("Character '" + skinpath + "' has invalid dimensions!");
                        continue; //Skip invalid character
                    }

                    //Make sure this id isn't already in use.
                    if (Program.mainForm.SkinMan.GetSkin(id) != null)
                    {
                        Msg.Log("Duplicate character '" + id +"' in: " + skinpath);
                        continue;
                    }

                    //Add the character
                    SkinData skin = new SkinData(SkinType.Png, path, isdefault);

                    skin.Name = id;
                    skin.Id = id;
                    skin.CharacterImgPath = Path.GetFileName(skinpath);

                    if (isdefault == true)
                        skin.Author = "Mossmouth";

                    skin.LoadPreview(); //Load the preview image

                    this.Skins.Add(skin);
                    //Msg.Log("Added character: " + skin.Name + ", " + skin.CharacterImgPath);
                }
            }
        }

        //Load characters stored in subdirectories (with XML files)
        private void LoadDirectorySkins(string path, bool isdefault)
        {
            if (Directory.Exists(path) == false)
            {
                Msg.Log("Failed to load characters from: " + path);
                return;
            }

            //Get all the subdirectories
            string[] skins = Directory.GetDirectories(path);

            foreach (string skinpath in skins)
            {
                //Check for the xml file
                string xmlpath = skinpath + "/character.xml";

                if (File.Exists(xmlpath) == false)
                {
                    Msg.Log("No characters found inside: " + skinpath);
                    continue; //Invalid skin folder
                }

                //Load the xml file
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(xmlpath);
                }
                catch (Exception ex)
                {
                    Msg.Log("Error loading xml: " + ex.Message);
                    continue; //Skip invalid character mod
                }

                ReadXmlSkins(doc, SkinType.Dir, skinpath, isdefault);
            }
        }

        //Load characters stored in zip files (as .plc files)
        private void LoadZipSkins(string path, bool isdefault)
        {
            if (Directory.Exists(path) == false)
            {
                Msg.Log("Failed to load characters from: " + path);
                return;
            }

            //Get all the *.plc files
            string[] skins = Directory.GetFiles(path, "*.plc");

            foreach (string skinpath in skins)
            {
                if (File.Exists(skinpath))
                {
                    //Check for the xml file
                    string xmlpath = "character.xml";

                    ZipFile zip = new ZipFile(skinpath);

                    if (zip.ContainsEntry(xmlpath) == false)
                    {
                        zip.Dispose();
                        Msg.Log("No characters found inside: " + skinpath);
                        continue; //Invalid skin file
                    }

                    MemoryStream stream = new MemoryStream();
                    zip[xmlpath].Extract(stream);
                    zip.Dispose();
                    stream.Position = 0;

                    //Load the xml file
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        doc.Load(stream);
                    }
                    catch (Exception ex)
                    {
                        Msg.Log("Error loading xml: " + ex.Message);
                        continue; //Skip invalid character mod
                    }

                    ReadXmlSkins(doc, SkinType.Zip, skinpath, isdefault);
                }
            }
        }

        //Read Character data from an XML document
        private void ReadXmlSkins(XmlDocument doc, SkinType skintype, string modpath, bool isdefault)
        {
            //Get list of character nodes
            XmlNodeList nodes = doc.SelectNodes("/Characters/Character");

            if (nodes == null) return; //Skip invalid character mod

            foreach (XmlNode node in nodes)
            {
                //Read required data from character XML
                string id   = GetXMLString(node, "./Id");
                string name = GetXMLString(node, "./Name");
                string charimgpath = GetXMLString(node, "./CharacterImage");

                //Ensure that the path is valid
                if ((charimgpath != null) && (PathIsValid(charimgpath) == false))
                {
                    charimgpath = null;
                    Msg.Log("Invalid CharacterImage path in: " + modpath);
                }

                //Make sure it is a valid character
                if ((id == null) || (name == null) || (charimgpath == null))
                {
                    Msg.Log("Invalid character skipped in: " + modpath);
                    continue; //Skip invalid character
                }

                //Make sure the character id is not already in use.
                if (Program.mainForm.SkinMan.GetSkin(id) != null)
                {
                    Msg.Log("Duplicate character '" + id +"' skipped in: " + modpath);
                    continue; //Skip duplicate character
                }

                //Add the character
                SkinData skin = new SkinData(skintype, modpath, isdefault);

                //Set the values
                skin.Name = name;
                skin.Id = id;
                skin.Version = GetXMLString(node, "./Version");
                DateTime date;
                if (DateTime.TryParse(GetXMLString(node, "./Date"), out date))
                    skin.Date = date;
                skin.Author  = GetXMLString(node, "./Author");
                skin.Email = GetXMLString(node, "./Email");
                skin.WebUrl     = GetXMLString(node, "./WebUrl");
                skin.Description    = GetXMLString(node, "./Description");
                skin.PatchlunkyVersion = GetXMLString(node, "./PatchlunkyVersion");
                skin.CharacterImgPath = charimgpath;
                skin.PreviewImgPath   = GetXMLString(node, "./PreviewImage");
                skin.LeaderImgPath    = GetXMLString(node, "./LeaderboardImage");

                //Ensure that the paths are valid
                // CharacterImgPath is checked earlier.
                if (PathIsValid(skin.PreviewImgPath) == false)
                {
                    skin.PreviewImgPath = null;
                    Msg.Log("Invalid PreviewImage path for '" + skin.Id + "'!");
                }
                if (PathIsValid(skin.LeaderImgPath) == false)
                {
                    skin.LeaderImgPath = null;
                    Msg.Log("Invalid LeaderboardImage path for '" + skin.Id + "'!");
                }

                //If the URLs don't include a protocol, add one.
                if (skin.WebUrl != null)
                {
                    if (skin.WebUrl.Contains("://") == false)
                        skin.WebUrl = "http://" + skin.WebUrl;
                }
                if (skin.Email != null)
                {
                    if (skin.Email.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) == false)
                        skin.Email = "mailto:" + skin.Email;
                }

                skin.LoadPreview(); //Load the preview image

                this.Skins.Add(skin);
                //Msg.Log("Added character: " + skin.Name + ", " + skin.CharacterImgPath);
            }
        }

        //PathIsValid - Enforce some rules to (hopefully) avoid path traversal attacks
        private bool PathIsValid(string path)
        {
            //Blacklisting
            if (path == null) return false; //No null strings
            if (path.Length == 0) return false; //No empty strings
            if (path.Contains("..")) return false; //No double dots
            if (path.Contains("//") || path.Contains("\\\\")) return false; //No double slashes
            if (path.Contains("/\\") || path.Contains("\\/")) return false; //No double slashes
            if (path.Contains("/.") || path.Contains("\\.")) return false; //No slashes followed by a dot
            if (path.Contains("./") || path.Contains(".\\")) return false; //No dots followed by a slash
            if (path.Contains(":")) return false; //No colons (Windows root)
            if (path.StartsWith("/") || path.StartsWith("\\")) return false; //No beginning slash
            if (path.Contains("%")) return false; //No URL encoding

            //Whitelisting
            foreach (char symbol in path)
            {
                if (char.IsLetterOrDigit(symbol)) continue; //Allow letters and digits
                else if (symbol == '.') continue; //Allow dots
                else if ((symbol == '-') || (symbol == '_')) continue; //Allow dashes and underscores
                else if ((symbol == '/') || (symbol == '\\')) continue; //Allow slashes
                else if (symbol == ' ') continue; //Allow spaces
                else return false; //Invalid character in path
            }

            return true; //if we get this far, the path is OK.
        }

        //GetXMLString - Return an XML text value from a child node
        private string GetXMLString(XmlNode node, string xpath)
        {
            XmlNode strnode = node.SelectSingleNode(xpath + "/text()");

            if (strnode == null) return null;

            return strnode.Value.Trim();
        }

        //LoadConfigs - Loads the skin configuration list
        public void LoadConfigList()
        {
            Settings settings = Program.mainForm.Settings;

            this.CurrentConfig = settings.Get("SkinConfig");
            string configs = settings.Get("SkinConfigList");
            if (configs == null) return;

            this.Configs.Clear();

            string[] confignames = configs.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < confignames.Length; i++)
            {
                string configname = confignames[i];

                this.Configs.Add(configname);
            }

            Msg.Log("Loaded " + Configs.Count + " skin configurations.");
        }

        //SaveConfigs - Saves the skin configuration list
        public void SaveConfigList()
        {
            Settings settings = Program.mainForm.Settings;
            string configs = "";
            foreach (string conf in this.Configs)
            {
                configs += conf + "|";
            }
            settings.Set("SkinConfigList", configs);
            settings.Set("SkinConfig", this.CurrentConfig);

            Msg.Log("Saved skinconfig list: " + Configs.Count + " skin configurations.");
        }

        //ConfigExists - Checks if a skin configuration exists
        public bool ConfigExists(string configname)
        {
            string config = Configs.Find(o => o.Equals(configname, StringComparison.OrdinalIgnoreCase));
            if (config == null)
                return false;

            return true;
        }

        //LoadConfig - Loads a skin configuration
        public void LoadConfig(string configname)
        {
            Settings settings = Program.mainForm.Settings;

            string config = settings.Get("SkinConfig_" + configname);
            if (config == null) return;

            SkinConfig.Clear();

            string[] keyvaluepairs = config.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < keyvaluepairs.Length; i++)
            {
                int eq = keyvaluepairs[i].IndexOf('=');
                if(eq == -1) continue;

                string key = keyvaluepairs[i].Substring(0, eq);
                string val = keyvaluepairs[i].Substring(eq+1);
                //Msg.Log("key = '" + key + "', val= '" + val + "'.");

                SkinConfig.Add(key, val);
            }
            Msg.Log("Loaded skinconfig '" + configname + "'.");
        }

        //SaveConfig - Saves a skin configuration
        public void SaveConfig(string configname)
        {
            Settings settings = Program.mainForm.Settings;
            string config = "";
            foreach(KeyValuePair<string, string> kvp in this.SkinConfig)
            {
                config += kvp.Key + "=" + kvp.Value + "|";
            }
            settings.Add("SkinConfig_" + configname, config);
            settings.Set("SkinConfig_" + configname, config);

            Msg.Log("Saved skinconfig '" + configname + "'.");
        }

        //RemoveConfig - Removes a skin configuration
        public void RemoveConfig(string configname)
        {
            Settings settings = Program.mainForm.Settings;
            settings.Remove("SkinConfig_" + configname);

            Msg.Log("Removed skinconfig '" + configname + "'.");
        }

    }
}
