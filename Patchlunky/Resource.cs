/*
 * Copyright (c) 2017, Worst-vd-plas
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
using System.Drawing;
using System.IO;

using Ionic.Zip;
using SpelunkyWad;

namespace Patchlunky
{
    // Resource handling functions
    public static class Resource
    {
        //Load a bitmap copy of a skin resource
        public static Bitmap LoadBitmap(SkinData skin, string path)
        {
            Bitmap result = null;

            if ((skin.Type == SkinType.Png) || (skin.Type == SkinType.Dir))
            {
                string filepath = skin.ModPath + "/" + path;

                if (File.Exists(filepath) == false)
                {
                    Msg.Log("ResourceLoadBitmap: '" + filepath + "' not found!");
                    return null;
                }

                result = new Bitmap((Bitmap)Image.FromFile(filepath));
            }
            else if (skin.Type == SkinType.Zip)
            {
                ZipFile zip = new ZipFile(skin.ModPath);

                if (zip.ContainsEntry(path) == false)
                {
                    zip.Dispose();
                    Msg.Log("ResourceLoadBitmap: '" + path + "' not found!");
                    return null;
                }

                MemoryStream stream = new MemoryStream();
                zip[path].Extract(stream);
                zip.Dispose();
                //stream.Position = 0;

                result = new Bitmap((Bitmap)Image.FromStream(stream));
                stream.Dispose();
            }

            return result;
        }

        //Load bytes of a skin resource
        public static Byte[] LoadBytes(SkinData skin, string path)
        {
            Byte[] result = null;

            if ((skin.Type == SkinType.Png) || (skin.Type == SkinType.Dir))
            {
                string filepath = skin.ModPath + "/" + path;

                if (File.Exists(filepath) == false)
                {
                    Msg.Log("ResourceLoadBytes: '" + filepath + "' not found!");
                    return null;
                }

                result = File.ReadAllBytes(filepath);
            }
            else if (skin.Type == SkinType.Zip)
            {
                ZipFile zip = new ZipFile(skin.ModPath);

                if (zip.ContainsEntry(path) == false)
                {
                    zip.Dispose();
                    Msg.Log("ResourceLoadBytes: '" + path + "' not found!");
                    return null;
                }

                MemoryStream stream = new MemoryStream();
                zip[path].Extract(stream);
                zip.Dispose();

                result = stream.GetBuffer();
                stream.Dispose();
            }

            return result;
        }

        //Load a bitmap copy of an archive resource
        public static Bitmap LoadBitmap(Archive archive, string path)
        {
            Bitmap result = null;

            //Path needs to have a single slash separating the group and file
            if ((path.Contains('/') || path.Contains('\\')) == false)
            {
                Msg.Log("ResourceLoadBitmap: path '" + path + "' is invalid!");
                return null;
            }

            //Split the path to group and file
            int split = path.IndexOfAny(new char[] {'/', '\\'});
            string group = path.Substring(0, split);
            string file = path.Substring(split+1);

            //Msg.Log("Group: " + group);
            //Msg.Log("File: " + file);

            int grp_index = archive.Groups.FindIndex(o => o.Name.Equals(group, StringComparison.OrdinalIgnoreCase));
            if (grp_index == -1)
            {
                Msg.Log("ResourceLoadBitmap: Group '" + group + "' not found!");
                return null;
            }

            int ent_index = archive.Groups[grp_index].Entries.FindIndex(o => o.Name.Equals(file, StringComparison.OrdinalIgnoreCase));
            if (ent_index == -1)
            {
                Msg.Log("ResourceLoadBitmap: Entry '" + file + "' not found!");
                return null;
            }

            //Get the entry
            MemoryStream stream = new MemoryStream(archive.Groups[grp_index].Entries[ent_index].Data);
            result = new Bitmap((Bitmap)Image.FromStream(stream));
            stream.Dispose();

            return result;
        }

        //Replace an archive resource - returns true if the entry was replaced.
        public static bool ReplaceBytes(Archive archive, string path, Byte[] bytes)
        {
            //Path needs to have a single slash separating the group and file
            if ((path.Contains('/') || path.Contains('\\')) == false)
            {
                Msg.Log("ResourceSaveBytes: path '" + path + "' is invalid!");
                return false;
            }

            //Split the path to group and file
            int split = path.IndexOfAny(new char[] { '/', '\\' });
            string group = path.Substring(0, split);
            string file = path.Substring(split+1);

            int grp_index = archive.Groups.FindIndex(o => o.Name.Equals(group, StringComparison.OrdinalIgnoreCase));
            if (grp_index == -1)
            {
                Msg.Log("ResourceSaveBytes: Group '" + group + "' not found!");
                return false;
            }

            int ent_index = archive.Groups[grp_index].Entries.FindIndex(o => o.Name.Equals(file, StringComparison.OrdinalIgnoreCase));
            if (ent_index == -1)
            {
                Msg.Log("ResourceSaveBytes: Entry '" + file + "' not found!");
                return false;
            }

            //Save the entry
            Entry new_entry = new Entry(archive.Groups[grp_index].Entries[ent_index].Name, bytes);
            archive.Groups[grp_index].Entries[ent_index] = new_entry;

            return true;
        }
    }
}
