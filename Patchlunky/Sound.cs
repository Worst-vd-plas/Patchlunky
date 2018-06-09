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
    public class SoundListFile
    {
        private bool Modified;
        public string Path;
        private Dictionary<string, SoundReverb> Entries;

        private struct SoundReverb
        {
            public int val1;
            public int val2;
            public int val3;

            public SoundReverb(int arg1, int arg2, int arg3)
            {
                this.val1 = arg1;
                this.val2 = arg2;
                this.val3 = arg3;
            }
        }

        public SoundListFile(string path)
        {
            this.Modified = false;
            this.Path = path;
            this.Entries = new Dictionary<string, SoundReverb>();
        }

        //Load the keys and values from the soundlist.dat file
        public void Load()
        {
            using (StreamReader sr = new StreamReader(this.Path))
            {
                while (!sr.EndOfStream)
                {
                    string key = null;
                    SoundReverb values;

                    string line = sr.ReadLine();
                    string[] parts = line.Split(' ');

                    //If the line has less than 4 parts, the soundlist file is probably broken.
                    if (parts.Length < 4)
                    {
                        Msg.Log("Bad line encountered in Soundlist file, parsing aborted!");
                        break;
                    }

                    key = parts[0];
                    int.TryParse(parts[1], out values.val1);
                    int.TryParse(parts[2], out values.val2);
                    int.TryParse(parts[3], out values.val3);

                    Entries[key] = values;
                }
            }
        }

        //Set the values of a key, returns false if the key doesn't exist.
        public bool Set(string key, int arg1, int arg2, int arg3)
        {
            if (Entries.ContainsKey(key))
            {
                SoundReverb value = new SoundReverb(arg1, arg2, arg3);
                Entries[key] = value;
                this.Modified = true;
                return true;
            }
            return false;
        }

        //Overwrite the soundlist.dat file with dictionary values
        public void Save()
        {
            //Dont save if the dictionary has not been changed.
            if (Modified == false) return;

            using (StreamWriter sw = new StreamWriter(this.Path, false))
            {
                foreach (var entry in this.Entries)
                {
                    //Spelunky soundlist.dat has spaces between keys and values, and a CRLF after each line. (mostly)
                    sw.Write(entry.Key + " " +
                             entry.Value.val1 + " " +
                             entry.Value.val2 + " " +
                             entry.Value.val3 + "\r\n");
                }
            }
        }
    }
}
