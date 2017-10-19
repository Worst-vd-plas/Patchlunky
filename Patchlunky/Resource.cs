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
    }
}
