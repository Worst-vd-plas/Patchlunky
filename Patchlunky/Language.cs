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
using System.Linq;
using System.Text;
using System.IO;

namespace Patchlunky
{
    public class LanguageFile
    {
        private bool Modified;
        public string Path;
        private Dictionary<string, string> Strings;

        public LanguageFile(string path)
        {
            this.Modified = false;
            this.Path = path;
            this.Strings = new Dictionary<string, string>();
        }

        //Load the keys and values from the strings.pct file
        public void Load()
        {
            string key = null;
            string value = null;
            int lineNum = 0;

            using (StreamReader sr = new StreamReader(this.Path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    // Spelunky expects even numbered lines to be keys and odd numbered
                    // lines to be values.
                    if (lineNum % 2 == 0) //Even
                    {
                        key = line;
                    }
                    else //Odd
                    {
                        value = line;

                        //If the key does not start with "IDS_" the language file is probably broken
                        if (key.StartsWith("IDS_", StringComparison.OrdinalIgnoreCase) == false)
                        {
                            Msg.Log("Bad key encountered in Language file, parsing aborted!");
                            break;
                        }

                        Strings[key] = value;
                    }
                    lineNum++;
                }
            }
        }

        //Get the value of a key
        public string Get(string key)
        {
            if (Strings.ContainsKey(key))
            {
                return Strings[key];
            }
            return null;
        }

        //Set the value of a key, returns false if the key doesn't exist.
        public bool Set(string key, string value)
        {
            if (Strings.ContainsKey(key))
            {
                Strings[key] = value;
                this.Modified = true;
                return true;
            }
            return false;
        }

        //Overwrite the strings.pct file with dictionary values
        public void Save()
        {
            //Dont save if the dictionary has not been changed.
            if (Modified == false) return;

            using (StreamWriter sw = new StreamWriter(this.Path, false))
            {
                foreach (var entry in this.Strings)
                {
                    //Spelunky strings.pct has LF after each key and CRLF after each value. (mostly)
                    sw.Write(entry.Key  + "\n");
                    sw.Write(entry.Value + "\r\n");
                }
            }
        }
    }
}
