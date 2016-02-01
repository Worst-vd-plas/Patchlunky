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
using System.Configuration;

namespace Patchlunky
{
    public class Settings
    {
        public Configuration Config;

        public Settings()
        {
            try
            {
               Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
               Msg.Log("Loading config file \"" + Path.GetFileName(Config.FilePath) + "\".");
            }
            catch (ConfigurationErrorsException)
            {
                Msg.Log   ("Error reading config file!");
                Msg.MsgBox("Error reading config file!");
                return;
            }

            //Create keys with default values in config file if they don't exist yet.
            this.Add("GameDir",      null);
            this.Add("ModConfig",  "Default");
            this.Add("ModConfigList", "Default");
            this.Add("ModConfig_Default", "");
            this.Add("SkinConfig_Default", "");
            this.Add("ModsReplaceDefaultSkins", "True");

            this.Save(); //Save config to disk.
        }

        public void Save() //Save configuration to disk
        {
            Config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public void Add(string key, string value) //Adds a key if it does NOT exist
        {
            if (Config.AppSettings.Settings.AllKeys.Contains(key) == false)
                Config.AppSettings.Settings.Add(key, value); // Create new key
        }

        public void Remove(string key)
        {
            if (Config.AppSettings.Settings.AllKeys.Contains(key))
            {
                Config.AppSettings.Settings.Remove(key); //Remove the key
            }
        }

        public void Set(string key, string value) //Set value for existing key
        {
            //if (ConfigurationManager.AppSettings.AllKeys.Contains(theKey))
            if (Config.AppSettings.Settings.AllKeys.Contains(key))
            {
                Config.AppSettings.Settings[key].Value = value;
            }
        }

        public string Get(string key) //Get key value
        {
            if (Config.AppSettings.Settings.AllKeys.Contains(key))
            {
                return Config.AppSettings.Settings[key].Value;
            }
            return null;
        }

        public bool Check(string key, string check) //Check if key value matches string
        {
            if (Config.AppSettings.Settings.AllKeys.Contains(key))
            {
                string value = Config.AppSettings.Settings[key].Value;

                     if ((value == null) && (check == null)) return true;
                else if ((value == null) || (check == null)) return false;

                //Check if key equals to what is being checked
                if (value.Equals(check, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
