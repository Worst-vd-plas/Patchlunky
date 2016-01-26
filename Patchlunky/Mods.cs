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

using Ionic.Zip;

namespace Patchlunky
{
    public class ModData
    {
        public string Name;
        public string Path;
        public bool IsZip;
        public bool Enabled;
        public int SortValue; //Used for sorting mods in the ModManager

        public string Text;
        public System.Drawing.Image Image;

        public ModData(string name, string path, bool iszip=false)
        {
            this.Name = StripSpecialChars(name);
            this.Path = path;
            this.IsZip = iszip;
            this.Enabled = false;
            this.SortValue = 9999; //New mods are sorted at bottom of list

            this.Text = "";
            this.Image = null;

            LoadModInfo();
        }

        private static string StripSpecialChars(string str)
        {
            return Regex.Replace(str, "[|¤]+", "", RegexOptions.Compiled);
            //return Regex.Replace(str, "[^0-9A-Za-z *^~!?&#%€£$@<>[]()/\\;,:._+-]+", "", RegexOptions.Compiled);
        }

        //TODO: Clean this up
        private void LoadModInfo()
        {
            if (this.IsZip == true)
            {
                string entrypath;
                ZipFile zip = new ZipFile(this.Path);
                
                entrypath = "readme.txt";
                if (zip.ContainsEntry(entrypath))
                {
                    MemoryStream stream = new MemoryStream();
                    zip[entrypath].Extract(stream);
                    stream.Position = 0;
                    var sr = new StreamReader(stream);
                    this.Text = sr.ReadToEnd();
                }
                entrypath = "picture.png";
                if (zip.ContainsEntry(entrypath))
                {
                    MemoryStream stream = new MemoryStream();
                    zip[entrypath].Extract(stream);
                    //stream.Position = 0;
                    this.Image = System.Drawing.Image.FromStream(stream);
                }
            }
            else
            {
                string filepath;
                //Look for mod description
                filepath = this.Path + "readme.txt";
                if (File.Exists(filepath))
                {
                    this.Text = File.ReadAllText(filepath);
                }
                else
                {
                    string[] files = Directory.GetFiles(this.Path, "*.txt");
                    foreach (string fpath in files)
                    {
                        if (File.Exists(fpath))
                        {
                            this.Text = File.ReadAllText(fpath);
                            break;
                        }
                    }
                }
                //Look for mod picture
                //TODO: Look for .jpg and also any picture if 'picture.png' is not found.
                filepath = this.Path + "picture.png";
                if (File.Exists(filepath))
                {
                    this.Image = System.Drawing.Image.FromFile(filepath);
                }
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
        public ModData GetMod(string modname)
        {
            ModData mod = Mods.Find(o => o.Name.Equals(modname, StringComparison.OrdinalIgnoreCase));
            if (mod != null && mod.Equals(default(ModData)))
                return null; //no mod with modname.

            return mod;
        }

        //LoadAllMods - Main function for loading mods to patchlunky
        public void LoadAllMods()
        {
            string path = Program.mainForm.Setup.AppPath + "Mods/";

            Msg.Log("Loading mods.");

            LoadMods(path, "/");
            LoadMods(path, "*.zip");
            LoadMods(path, "*.plm");

            Msg.Log("Found " + Mods.Count + " mods.");
        }

        //LoadMods - Loads mods of given type
        private void LoadMods(string path, string type)
        {            
            string[] mods;
            if (type.Equals("/")) mods = Directory.GetDirectories(path);
            else                  mods = Directory.GetFiles(path, type);

            foreach (string modpath in mods)
            {
                ModData mod;
                string name;
                if (type.Equals("/")) //Mods as directories
                {
                    name = new DirectoryInfo(modpath).Name;
                    mod = new ModData(name, modpath + "/", false);
                }
                else //Zip files
                {
                    name = Path.GetFileNameWithoutExtension(modpath);
                    mod = new ModData(name, modpath, true);
                }

                this.Mods.Add(mod);

                //Msg.Log("Added mod: " + name + ", " + modpath);
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

            Msg.Log("Loaded modconfig list: " + Configs.Count + " mod configurations.");
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