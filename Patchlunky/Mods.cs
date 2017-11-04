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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Drawing;
using System.Linq;

using Ionic.Zip;

namespace Patchlunky
{
    [Flags]
    public enum ModType
    {
        Dir = 1,
        Zip = 2, //.zip or .plm
        Xml = 4, //contains a mod.xml that describes the contents
    };

    public class ModData
    {
        public ModType Type;
        public bool Enabled;
        public int SortValue; //Used for sorting mods in the ModManager

        //Paths
        public string ModPath; //Path to directory or zip file
        public string PreviewImgPath;
        public string TextFilePath;

        //Mod information
        public string Name;
        public string Id;
        public string Version;
        public DateTime? Date;
        public string Author;
        public string Email;
        public string WebUrl;
        public string Description;
        public string PatchlunkyVersion;

        public bool PatchDefaultFiles;

        public Image PreviewImage;

        public ModData(ModType type, string modpath)
        {
            this.Type = type;
            this.ModPath = modpath;
            this.Enabled = false;
            this.SortValue = 9999;

            //Init values
            this.PreviewImgPath = null;
            this.TextFilePath = null;

            this.Name = null;
            this.Id = null;
            this.Version = null;
            this.Date = null;
            this.Author = null;
            this.Email = null;
            this.WebUrl = null;
            this.Description = null;
            this.PatchlunkyVersion = null;

            this.PatchDefaultFiles = false;

            this.PreviewImage = null;
        }

        public void LoadInfo()
        {
            //Simple mod in zip
            if (Type.HasFlag(ModType.Zip) && !Type.HasFlag(ModType.Xml))
            {
                ZipFile zip = new ZipFile(this.ModPath);

                string filename;

                //Look for a mod description
                filename = "readme.txt";
                if (zip.ContainsEntry(filename))
                {
                    this.TextFilePath = filename;
                }
                //Otherwise look for any .txt
                else
                {
                    var zip_entries = zip.SelectEntries("*.txt", "");
                    ZipEntry ze = zip_entries.FirstOrDefault();
                    if (ze != null)
                        this.TextFilePath = ze.FileName;
                }

                //Look for a mod image
                filename = "picture.png";
                if (zip.ContainsEntry(filename))
                {
                    this.PreviewImgPath = filename;
                }
                //Otherwise look for any .png or .jpg
                else
                {
                    var zip_entries = zip.SelectEntries("*.png", "");
                    ZipEntry ze = zip_entries.FirstOrDefault();
                    if (ze != null)
                    {
                        this.PreviewImgPath = ze.FileName;
                    }
                    else
                    {
                        zip_entries = zip.SelectEntries("*.jpg", "");
                        ze = zip_entries.FirstOrDefault();
                        if (ze != null)
                            this.PreviewImgPath = ze.FileName;
                    }
                }
                zip.Dispose();
            }
            //Simple mod in directory
            else if(Type.HasFlag(ModType.Dir) && !Type.HasFlag(ModType.Xml))
            {
                string filename;

                //Look for a mod description
                filename = "readme.txt";
                if (File.Exists(this.ModPath + filename))
                {
                    this.TextFilePath = filename;
                }
                //Otherwise look for any .txt
                else
                {
                    string[] files = Directory.GetFiles(this.ModPath, "*.txt");
                    string file = files.FirstOrDefault();
                    if (file != null)
                        this.TextFilePath = Path.GetFileName(file);
                }

                //Look for a mod image
                filename = "picture.png";
                if (File.Exists(this.ModPath + filename))
                {
                    this.PreviewImgPath = filename;
                }
                //Otherwise look for any .png or .jpg
                else
                {
                    string[] files = Directory.GetFiles(this.ModPath, "*.png");
                    string file = files.FirstOrDefault();
                    if (file != null)
                    {
                        this.PreviewImgPath = Path.GetFileName(file);
                    }
                    else
                    {
                        files = Directory.GetFiles(this.ModPath, "*.jpg");
                        file = files.FirstOrDefault();
                        if (file != null)
                            this.PreviewImgPath = Path.GetFileName(file);
                    }
                }
            }

            //Load the text file, if supplied
            if (this.TextFilePath != null)
            {
                this.Description = Resource.LoadText(this, this.TextFilePath);
            }
            //Load the preview image, if supplied
            if (this.PreviewImgPath != null)
            {
                this.PreviewImage = Resource.LoadBitmap(this, this.PreviewImgPath);
            }
        }
    }

    public class ModManager
    {
        public List<ModData> Mods;
        public List<string> Configs;
        public string CurrentConfig;

        public ModManager()
        {
            this.Mods = new List<ModData>();
            this.Configs = new List<string>();
        }

        //GetMod - Finds a mod by name
        public ModData GetMod(string modid)
        {
            ModData mod = Mods.Find(o => o.Id.Equals(modid, StringComparison.OrdinalIgnoreCase));
            if (mod != null && mod.Equals(default(ModData)))
                return null; //no mod with modname.

            return mod;
        }

        //LoadAllMods - Main function for loading mods to patchlunky
        public void LoadAllMods()
        {
            string path = Program.mainForm.Setup.AppPath + "Mods/";
            
            LoadMods(path);

            Msg.Log("Loaded " + Mods.Count + " mods.");
        }

        //LoadMods - Loads mods of given type
        private void LoadMods(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Msg.Log("Failed to load mods from: " + path);
                return;
            }

            string[] filepaths;

            //Get all the subdirectories
            filepaths = Directory.GetDirectories(path);
            foreach (string filepath in filepaths)
                LoadDirectoryMod(filepath + "/");

            //Get all the *.zip files
            filepaths = Directory.GetFiles(path, "*.zip");
            foreach (string filepath in filepaths)
                LoadZipMod(filepath);

            //Get all the *.plm files
            filepaths = Directory.GetFiles(path, "*.plm");
            foreach (string filepath in filepaths)
                LoadZipMod(filepath);
        }

        //Load mod stored in subdirectory
        private void LoadDirectoryMod(string dirpath)
        {
            if (Directory.Exists(dirpath) == false)
                return;

            //Check if this an xml-based mod
            string xmlpath = dirpath + "mod.xml";

            if (File.Exists(xmlpath) == true)
            {
                //Load the xml file
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(xmlpath);
                }
                catch (Exception ex)
                {
                    Msg.Log("Error loading xml: " + ex.Message);
                    return; //Skip invalid mod
                }

                ReadXmlMod(doc, ModType.Dir|ModType.Xml, dirpath);
                return;
            }
            else //else it is a simple mod
            {
                string name = new DirectoryInfo(dirpath).Name;
                string id = Xmf.StripSpecialChars(name);

                //Make sure the mod has a valid id
                if ((id == null) || (id.Length == 0))
                {
                    Msg.Log("Mod '" + dirpath + "' has an invalid name!");
                    return; //Skip invalid mod
                }

                //Make sure this id isn't already in use.
                if (this.GetMod(id) != null)
                {
                    //Msg.Log("Duplicate mod '" + id +"' skipped in: " + dirpath);
                    return; //Skip duplicate mod
                }

                //Add the mod
                ModData mod = new ModData(ModType.Dir, dirpath);
                //ModData mod = new ModData(ModType.Dir, dirpath + "/");

                mod.Name = name;
                mod.Id = id;
                mod.PatchDefaultFiles = true;

                mod.LoadInfo(); //Load more details

                this.Mods.Add(mod);
                //Msg.Log("Added mod: " + mod.Name + ", " + dirpath);
            }
        }

        //Load mod stored in zip file (as .zip or .plm)
        private void LoadZipMod(string filepath)
        {
            if (File.Exists(filepath) == false)
                return;

            //Check if this an xml-based mod
            string xmlpath = "mod.xml";

            ZipFile zip = new ZipFile(filepath);

            if (zip.ContainsEntry(xmlpath) == true)
            {
                //Extract the xml file
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
                    stream.Dispose();
                    return; //Skip invalid mod
                }

                ReadXmlMod(doc, ModType.Zip|ModType.Xml, filepath);
                stream.Dispose();
                return;
            }
            else //else it is a simple mod
            {
                zip.Dispose();
                string name = Path.GetFileNameWithoutExtension(filepath);
                string id = Xmf.StripSpecialChars(name);

                //Make sure the mod has a valid id
                if ((id == null) || (id.Length == 0))
                {
                    Msg.Log("Mod '" + filepath + "' has an invalid name!");
                    return; //Skip invalid mod
                }

                //Make sure this id isn't already in use.
                if (this.GetMod(id) != null)
                {
                    //Msg.Log("Duplicate mod '" + id +"' skipped in: " + filepath);
                    return; //Skip duplicate mod
                }

                //Add the mod
                ModData mod = new ModData(ModType.Zip, filepath);

                mod.Name = name;
                mod.Id = id;
                mod.PatchDefaultFiles = true;

                mod.LoadInfo(); //Load more details

                this.Mods.Add(mod);
                //Msg.Log("Added mod: " + mod.Name + ", " + filepath);
            }
        }

        //Read Mod data from an XML Document
        private void ReadXmlMod(XmlDocument doc, ModType modtype, string modpath)
        {
            //Get list of mod nodes
            XmlNodeList nodes = doc.SelectNodes("/Mods/Mod");

            if (nodes == null) return; //Skip invalid mod

            foreach (XmlNode node in nodes)
            {
                //Read required data from mod XML
                string id   = Xmf.StripSpecialChars(Xmf.GetXMLString(node, "./Id"));
                string name = Xmf.GetXMLString(node, "./Name");

                //Make sure it is a valid mod
                if ((id == null) || (name == null))
                {
                    Msg.Log("Invalid mod skipped in: " + modpath);
                    continue; //Skip invalid mod
                }

                //Make sure the mod id is not already in use.
                if (this.GetMod(id) != null)
                {
                    //Msg.Log("Duplicate mod '" + id +"' skipped in: " + modpath);
                    continue; //Skip duplicate mod
                }

                //Add the mod
                ModData mod = new ModData(modtype, modpath);

                //Set the values
                mod.Name = name;
                mod.Id = id;
                mod.Version = Xmf.GetXMLString(node, "./Version");
                DateTime date;
                if (DateTime.TryParse(Xmf.GetXMLString(node, "./Date"), out date))
                    mod.Date = date;
                mod.Author            = Xmf.GetXMLString(node, "./Author");
                mod.Email             = Xmf.GetXMLString(node, "./Email");
                mod.WebUrl            = Xmf.GetXMLString(node, "./WebUrl");
                mod.Description       = Xmf.GetXMLString(node, "./Description");
                mod.PatchlunkyVersion = Xmf.GetXMLString(node, "./PatchlunkyVersion");
                mod.PreviewImgPath    = Xmf.GetXMLString(node, "./PreviewImage");
                string patchdf        = Xmf.GetXMLString(node, "./PatchDefaultFiles");
                mod.PatchDefaultFiles = (patchdf ?? "false").Equals(Boolean.TrueString,StringComparison.OrdinalIgnoreCase);

                //Ensure that the paths are valid format
                if ((mod.PreviewImgPath != null) && (Xmf.PathIsValid(mod.PreviewImgPath) == false))
                {
                    mod.PreviewImgPath = null;
                    Msg.Log("Invalid PreviewImage path for '" + mod.Id + "'!");
                }

                //If the URLs don't include a protocol, add one.
                if (mod.WebUrl != null)
                {
                    if (mod.WebUrl.Contains("://") == false)
                        mod.WebUrl = "http://" + mod.WebUrl;
                }
                if (mod.Email != null)
                {
                    if (mod.Email.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) == false)
                        mod.Email = "mailto:" + mod.Email;
                }

                mod.LoadInfo(); //Load more details

                this.Mods.Add(mod);
                //Msg.Log("Added mod: " + mod.Name + ", " + modpath);
            }
        }

        //LoadConfigs - Loads the mod configuration list
        public void LoadConfigList()
        {
            Settings settings = Program.mainForm.Settings;

            this.CurrentConfig = settings.Get("ModConfig");
            string configs = settings.Get("ModConfigList");
            if (configs == null) return;

            this.Configs.Clear();

            string[] confignames = configs.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < confignames.Length; i++)
            {
                string configname = confignames[i];

                this.Configs.Add(configname);
            }

            Msg.Log("Loaded " + Configs.Count + " mod configurations.");
        }

        //SaveConfigs - Saves the mod configuration list
        public void SaveConfigList()
        {
            Settings settings = Program.mainForm.Settings;
            string configs = "";
            foreach (string conf in this.Configs)
            {
                configs += conf + "|";
            }
            settings.Set("ModConfigList", configs);
            settings.Set("ModConfig", this.CurrentConfig);

            Msg.Log("Saved modconfig list: " + Configs.Count + " mod configurations.");
        }

        //ConfigExists - Checks if a mod configuration exists
        public bool ConfigExists(string configname)
        {
            string config = Configs.Find(o => o.Equals(configname, StringComparison.OrdinalIgnoreCase));
            if (config == null)
                return false;

            return true;
        }

        //LoadConfig - Loads a mod configuration
        public void LoadConfig(string configname)
        {
            Settings settings = Program.mainForm.Settings;

            string modconfig = settings.Get("ModConfig_" + configname);
            if (modconfig == null) return;

            string[] modnames = modconfig.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < modnames.Length; i++)
            {
                string modname = modnames[i];
                bool enabled = false;

                if (modname.StartsWith("¤"))
                {
                    modname = modname.Substring(1); //Strip the special char from start of name
                    enabled = true;
                }

                ModData mod = GetMod(modname);
                if (mod != null)
                {
                    mod.Enabled = enabled;
                    mod.SortValue = -i; //Sort in reverse
                }
            }

            //Sort the mods
            Mods.Sort((a, b) => a.SortValue.CompareTo(b.SortValue));

            Msg.Log("Loaded modconfig '" + configname + "'.");
        }

        //SaveConfig - Saves a mod configuration
        public void SaveConfig(string configname)
        {
            Settings settings = Program.mainForm.Settings;
            string modconfig = "";
            foreach (ModData mod in Mods)
            {
                if (mod.Enabled)
                    modconfig += "¤" + mod.Name + "|";
                else
                    modconfig += mod.Name + "|";
            }
            settings.Add("ModConfig_" + configname, modconfig);
            settings.Set("ModConfig_" + configname, modconfig);

            Msg.Log("Saved modconfig '" + configname + "'.");
        }

        //RemoveConfig - Removes a mod configuration
        public void RemoveConfig(string configname)
        {
            Settings settings = Program.mainForm.Settings;
            settings.Remove("ModConfig_" + configname);

            Msg.Log("Removed modconfig '" + configname + "'.");
        }

    }
}