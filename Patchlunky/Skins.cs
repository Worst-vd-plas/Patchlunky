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
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

using SpelunkyWad;

namespace Patchlunky
{
    public class SkinData
    {
        public string Name;
        public string Path;
        public bool IsDefault;

        public Image PreviewImage;

        public SkinData(string name, string path, bool isdefault)
        {
            this.Name = name;
            this.Path = path;
            this.IsDefault = isdefault;

            this.PreviewImage = null;

            LoadSkinPreview();
        }

        private void LoadSkinPreview()
        {
            if (File.Exists(this.Path))
            {
                Image sourceImg = Image.FromFile(this.Path);
                
                Bitmap sourceBMP = new Bitmap(sourceImg);
                Rectangle previewRect = new Rectangle(0, 0, 80, 80);                

                this.PreviewImage = sourceBMP.Clone(previewRect, sourceBMP.PixelFormat);
            }
        }
    }

    public class SkinManager
    {
        public List<SkinData> Skins;
        public ImageList SkinImageList;
        public Dictionary<string, string> SkinConfig;

        public SkinManager()
        {
            this.Skins = new List<SkinData>();
            this.SkinImageList = new ImageList();
            this.SkinConfig = new Dictionary<string, string>();
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

        //GetSkin - Finds a skin by name
        public SkinData GetSkin(string skinname)
        {
            SkinData skin = Skins.Find(o => o.Name.Equals(skinname, StringComparison.OrdinalIgnoreCase));
            if (skin != null && skin.Equals(default(SkinData)))
                return null; //no skin with skinname.

            return skin;
        }

        //LoadAllSkins - Main function for loading skins to patchlunky
        public void LoadAllSkins()
        {
            Setup setup = Program.mainForm.Setup;

            ExtractDefaultSkins();

            Msg.Log("Loading character skins.");

            LoadSkins(setup.BackupPath + "Characters/", "*.png", true); //Original skins
            LoadSkins(setup.AppPath + "Skins/", "*.png", false); //Custom skins

            Msg.Log("Found " + Skins.Count + " skins.");

            //Create imagelist
            this.SkinImageList.ImageSize = new Size(80, 80);
            this.SkinImageList.ColorDepth = ColorDepth.Depth24Bit;

            foreach (var skin in this.Skins)
            {
                SkinImageList.Images.Add(skin.Name, skin.PreviewImage);
            }
        }

        //LoadSkins - Loads skins from path by type
        private void LoadSkins(string path, string type, bool isdefault)
        {
            string[] skins;
            skins = Directory.GetFiles(path, type);

            foreach (string skinpath in skins)
            {
                SkinData skin;
                string name;

                name = Path.GetFileNameWithoutExtension(skinpath);
                skin = new SkinData(name, skinpath, isdefault);

                this.Skins.Add(skin);
                //Msg.Log("Added skin: " + name + ", " + skinpath);
            }
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

    }
}
