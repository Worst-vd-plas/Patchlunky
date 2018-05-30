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
using System.Drawing;
using System.IO;

using SpelunkyWad;
using NLua;

namespace Patchlunky
{
    public class ScriptManager
    {
        private Lua State;

        //Archives for scripts
        public Archive alltex;

        //For keeping track of script resource usage.
        private List<Bitmap> Bitmaps; //Holds all bitmaps used by ScriptImages
        private List<string> SavePathsUsed; //A script may only overwrite each file once
        private int PrintCounter; //Used to prevent log message spam

        //Limits
        private const int BitmapLimit = 50;
        private const int PrintLimit = 50;

        public ScriptManager()
        {
            Bitmaps = new List<Bitmap>();
            SavePathsUsed = new List<string>();
            PrintCounter = 0;
        }

        public bool RunScript(ModData mod, string script)
        {
            bool success = true;

            //Create the Lua state
            State = new Lua();
            object sc_ext = new ScriptExtensions(this, mod);

            //Load archives
            alltex = new Archive(Program.mainForm.Setup.TempPath + "Textures/alltex.wad");
            alltex.Load();

            try
            {
                //Set up the Lua environment
                PrepareLuaEnvironment(sc_ext);

                //Run the script
                RunLuaScript(script, mod.Id);
            }
            catch (NLua.Exceptions.LuaScriptException ex)
            {
                Msg.Log("Lua (" + mod.Name + ") script exception: " + ex.Message);
                success = false;
            }
            catch (NLua.Exceptions.LuaException ex)
            {
                Msg.Log("Lua (" + mod.Name + ") exception: " + ex.Message);
                success = false;
            }

            //Save archives
            alltex.Save();

            //Perform cleanup of the resources
            CleanUp();

            return success;
        }

        //Disposes of all the resources after the script has finished running
        private void CleanUp()
        {
            //Dispose of all the bitmaps
            foreach (Bitmap bmp in Bitmaps)
            {
                bmp.Dispose();
            }
            Bitmaps.Clear();

            //Clear the savepath list
            SavePathsUsed.Clear();

            //Reset the print counter
            PrintCounter = 0;

            //Dispose of the Lua state
            State.Dispose();
        }

        //Prepares the Lua environment and sandbox prior to calling scripts
        private void PrepareLuaEnvironment(object sc_ext)
        {
            string lua_string = @"
            -- Patchlunky functions
            function _print(msg)
              local ok = sc_ext:Print(tostring(msg))
              if not ok then error('PrintLimit exceeded!', 2) end
            end

            function _image_load(path)
              local img = sc_ext:Image_Load(tostring(path))
              if not img then error('image.load failed!', 2) end
              return img
            end

            function _image_new(width, height, color)
              local img = sc_ext:Image_New(width, height, color)
              if not img then error('image.new failed!', 2) end
              return img
            end

            function _math_random(m, n)
              if m and n then
                return sc_ext:Math_Random(m, n)
              end
              if m then
                return sc_ext:Math_Random(m)
              end
              return sc_ext:Math_Random()
            end

            function _math_randomseed(x)
              sc_ext:Math_RandomSeed(x)
            end

            -- Sandbox environment for patch scripts
            local sandbox_env = {
              -- Basic functions
              assert = _G.assert,
              error = _G.error, -- Should be ok for scripts to call this, although the level argument can
                                -- expose a little bit of information in the Lua call stack, that info is
                                -- not really secret anyways.
              ipairs = _G.ipairs,
              next = _G.next,
              pairs = _G.pairs,
              print = _print, -- Calls the patchlunky print method
              select = _G.select,
              tonumber = _G.tonumber,
              tostring = _G.tostring,
              type = _G.type,
              -- unpack = _G.unpack, -- Has been moved to table.unpack in Lua 5.2
              _VERSION = _G._VERSION,

              -- String Manipulation
              string = {
                byte = _G.string.byte,
                char = _G.string.char,
                find = _G.string.find,
                format = _G.string.format,
                gmatch = _G.string.gmatch,
                gsub = _G.string.gsub,
                len = _G.string.len,
                lower = _G.string.lower,
                match = _G.string.match,
                rep = _G.string.rep,
                reverse = _G.string.reverse,
                sub = _G.string.sub,
                upper = _G.string.upper
              },

              -- Table Manipulation
              table = {
                concat = _G.table.concat,
                insert = _G.table.insert,
                -- maxn = _G.table.maxn, -- Deprecated in Lua 5.2
                pack = _G.table.pack,
                remove = _G.table.remove,
                sort = _G.table.sort,
                unpack = _G.table.unpack
              },

              -- Mathematical Functions
              math = {
                abs = _G.math.abs,
                acos = _G.math.acos,
                asin = _G.math.asin,
                atan = _G.math.atan,
                atan2 = _G.math.atan2,
                ceil = _G.math.ceil,
                cos = _G.math.cos,
                cosh = _G.math.cosh,
                deg = _G.math.deg,
                exp = _G.math.exp,
                floor = _G.math.floor,
                fmod = _G.math.fmod,
                frexp = _G.math.frexp,
                huge = _G.math.huge,
                ldexp = _G.math.ldexp,
                log = _G.math.log,
                -- log10 = _G.math.log10, -- Deprecated in Lua 5.2
                max = _G.math.max,
                min = _G.math.min,
                modf = _G.math.modf,
                pi = _G.math.pi,
                pow = _G.math.pow,
                rad = _G.math.rad,
                random = _math_random, -- Calls the patchlunky version of this
                randomseed = _math_randomseed, -- Calls the patchlunky version of this
                sin = _G.math.sin,
                sinh = _G.math.sinh,
                sqrt = _G.math.sqrt,
                tan = _G.math.tan,
                tanh = _G.math.tanh
              },

              -- Bitwise Operations
              bit32 = {
                arshift = _G.bit32.arshift,
                band = _G.bit32.band,
                bnot = _G.bit32.bnot,
                bor = _G.bit32.bor,
                btest = _G.bit32.btest,
                bxor = _G.bit32.bxor,
                extract = _G.bit32.extract,
                replace = _G.bit32.replace,
                lrotate = _G.bit32.lrotate,
                lshift = _G.bit32.lshift,
                rrotate = _G.bit32.rrotate,
                rshift = _G.bit32.rshift
              },

              -- Patchlunky Image library
              image = {
                load = _image_load,
                new = _image_new,
                MODE_REPLACE = 0,
                MODE_OVERLAY = 1
              },

              -- No I/O library
              -- No debug library
              -- No coroutines
              -- No os.* library
              -- No pcall or xpcall (might be added later)
            }

            -- Run a script using the sandbox environment
            function run_sandbox_script(script_chunk, script_name)
              local userfunction, message = load(script_chunk, script_name, 't', sandbox_env)
              if not userfunction then return nil, message end
              return pcall(userfunction)
            end";

            State["sc_ext"] = sc_ext;
            State.DoString(lua_string, "LuaPrepEnv");
        }

        //Runs the Lua script in the sandbox
        private void RunLuaScript(string script_chunk, string script_name)
        {
            string lua_string = @"
            local status, message = run_sandbox_script(script_chunk, script_name)
            if not status then error(message, 0) end";

            State["script_chunk"] = script_chunk;
            State["script_name"] = script_name;
            State.DoString(lua_string, script_name);
        }


        //-----------------------------------------------------//
        // Functions for handling resources or tracking limits //
        //-----------------------------------------------------//

        //Handles script prints, returns false if the PrintLimit was reached.
        public bool Print(string msg)
        {
            if (PrintCounter >= PrintLimit)
                return false;

            Msg.Log(msg);
            PrintCounter++;
            return true;
        }

        //Adds a save path to the list
        public void AddSavePath(string path)
        {
            SavePathsUsed.Add(path);
        }

        //Checks if the save path already exists
        public bool CheckSavePathExists(string path)
        {
            int index = SavePathsUsed.FindIndex(o => o.Equals(path, StringComparison.OrdinalIgnoreCase));
            if (index == -1)
                return false;

            return true;
        }

        //Add a new bitmap to the list
        public int NewBitmap(Bitmap bmp)
        {
            //Check if the limit has been reached
            if (Bitmaps.Count >= BitmapLimit)
                return -1;

            //If not, add the bitmap to the list and return the index
            Bitmaps.Add(bmp);
            return Bitmaps.Count-1;
        }

        //Get a bitmap from the list
        public Bitmap GetBitmap(int index)
        {
            if ((index < 0) || (index >= Bitmaps.Count))
                return null;

            return Bitmaps[index];
        }

        //Dispose of a bitmap in the list
        public void FreeBitmap(int index)
        {
            if ((index < 0) || (index >= Bitmaps.Count))
                return;

            Bitmap bmp = Bitmaps[index];
            Bitmaps.RemoveAt(index);

            bmp.Dispose();
        }
    }

    // Contains functions that Lua will have access to
    public class ScriptExtensions
    {
        private ScriptManager ScriptMan;
        private ModData Mod;
        private PseudoRandom.MersenneTwister PsRng;

        public ScriptExtensions(ScriptManager sc_man, ModData mod)
        {
            this.ScriptMan = sc_man;
            this.Mod = mod;

            ulong seed = unchecked((ulong)DateTime.Now.Ticks);
            this.PsRng = new PseudoRandom.MersenneTwister(seed);
        }

        //Print a message from Lua
        public bool Print(string msg)
        {
            return ScriptMan.Print("Lua (" + Mod.Name + "): " + msg);
        }

        //math.random(), returns real number in the range [0,1)
        public double Math_Random()
        {
            return PsRng.genrand_real2();
        }

        //math.random(m), returns a integer in the range [1, m]
        public int Math_Random(int m)
        {
            return PsRng.genrand_N(m)+1;
        }

        //math.random(m, n), returns a integer in the range [m, n]
        public int Math_Random(int min, int max)
        {
            int value;

            if(min == max) return min;

            //Swap the values if they are in reverse
            if(max < min) { value = max; max = min; min = value; }

            value = PsRng.genrand_N((max-min) + 1); //Get a random int 0 to N-1.
            return min + value; //Return a value in the initial range.
        }

        //Seed the random generator for this script
        public void Math_RandomSeed(long value)
        {
            ulong seed = unchecked((ulong)value);
            PsRng.init_genrand(seed);
        }

        //Load an image from path
        public ScriptImage Image_Load(string fullpath)
        {
            string path;
            string group = "default";

            //Check if there is a group identifier in the path
            if (fullpath.Contains(':'))
            {
                //Split the fullpath to group and path
                int split = fullpath.IndexOf(':');
                group = fullpath.Substring(0, split);
                path  = fullpath.Substring(split+1);
            }
            else path = fullpath;

            //Make sure the path is ok
            if (Xmf.PathIsValid(path) == false)
                return null;

            Bitmap bmp = null;

            //Mod folder/zip
            if (group.Equals("default", StringComparison.OrdinalIgnoreCase))
            {
                bmp = Resource.LoadBitmap(this.Mod, path);
            }
            //Spelunky Data folder (in Patchlunky_Temp)
            else if (group.Equals("game", StringComparison.OrdinalIgnoreCase))
            {
                bmp = Resource.LoadBitmap(path);
            }
            //Spelunky alltex.wad (in Patchlunky_Temp)
            else if (group.Equals("alltex.wad", StringComparison.OrdinalIgnoreCase))
            {
                bmp = Resource.LoadBitmap(ScriptMan.alltex, path);
            }
            else return null; //Invalid group.

            //Check if the resource was loaded
            if (bmp == null)
                return null;

            //Try adding the bitmap to the list
            int bmp_index = ScriptMan.NewBitmap(bmp);
            if (bmp_index == -1)
            {
                bmp.Dispose();
                return null;
            }

            return new ScriptImage(ScriptMan, bmp_index);
        }

        //Create a new image with given dimensions and color
        public ScriptImage Image_New(int width, int height, LuaTable color)
        {
            //Make sure width and height are within reasonable(?) limits
            if ((width < 0) || (width > 4096)) return null;
            if ((height < 0)|| (height > 4096)) return null;

            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //Set the background color
            Color backColor = Color.Transparent;
            if (color != null)
                backColor = Color.FromArgb( ((int?)(color["a"] as double?)) ?? 255,
                                            ((int?)(color["r"] as double?)) ?? 0,
                                            ((int?)(color["g"] as double?)) ?? 0,
                                            ((int?)(color["b"] as double?)) ?? 0);
            using (Graphics g = Graphics.FromImage(bmp))
                g.Clear(backColor);

            //Try adding the bitmap to the list
            int bmp_index = ScriptMan.NewBitmap(bmp);
            if (bmp_index == -1)
            {
                bmp.Dispose();
                return null;
            }

            return new ScriptImage(ScriptMan, bmp_index);
        }
    }

    // The method names of this class aside from the constructor are
    // in lowercase to keep with the lua convention.
    public class ScriptImage
    {
        private ScriptManager ScriptMan;
        private int bitmapIndex;

        public int ImageIndex { get { return bitmapIndex; } }

        public ScriptImage(ScriptManager sc_man, int bmp_index)
        {
            this.ScriptMan = sc_man;
            this.bitmapIndex = bmp_index;
        }

        //Returns a new copy of the ScriptImage.
        public ScriptImage clone()
        {
            Bitmap bmp = new Bitmap(ScriptMan.GetBitmap(bitmapIndex));
            if (bmp == null)
                return null;

            //Try adding the bitmap to the list
            int bmp_index = ScriptMan.NewBitmap(bmp);
            if (bmp_index == -1)
            {
                bmp.Dispose();
                return null;
            }

            return new ScriptImage(ScriptMan, bmp_index);
        }

        //Disposes of the bitmap for this ScriptImage.
        public void free()
        {
            ScriptMan.FreeBitmap(bitmapIndex);
            this.bitmapIndex = -1;
        }

        //Saves the image. (Overwriting whatever there is already)
        public void save(string fullpath)
        {
            string path;
            string group = "game"; //Default to 'game'

            //Check if there is a group identifier in the path
            if (fullpath.Contains(':'))
            {
                //Split the fullpath to group and path
                int split = fullpath.IndexOf(':');
                group = fullpath.Substring(0, split);
                path  = fullpath.Substring(split+1);
            }
            else
            {
                path = fullpath;
                fullpath = group + ":" + path;
            }

            //Make sure the path is ok
            if (Xmf.PathIsValid(path) == false)
                return;

            //Check if it the path is still allowed to be written to
            if (ScriptMan.CheckSavePathExists(fullpath))
                return;

            //Get the bitmap
            Bitmap bmp = ScriptMan.GetBitmap(bitmapIndex);
            if (bmp == null)
                return;

            //Convert bmp to PNG or JPG based on extension
            MemoryStream ms = new MemoryStream();
            if      (path.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            else if (path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            else //Unsupported file extension
            {
                ms.Dispose();
                return;
            }
            Byte[] new_data = ms.ToArray();
            ms.Dispose();

            //Spelunky Data folder (in Patchlunky_Temp)
            if (group.Equals("game", StringComparison.OrdinalIgnoreCase))
            {
                Resource.ReplaceFile(path, new_data);
            }
            //Spelunky alltex.wad (in Patchlunky_Temp)
            else if (group.Equals("alltex.wad", StringComparison.OrdinalIgnoreCase))
            {
                Resource.ReplaceBytes(ScriptMan.alltex, path, new_data);
            }
            else return; //Invalid group.

            //Add the save path to the list
            ScriptMan.AddSavePath(fullpath);
        }

        //Draws the specified image on top of this image using the specified location and mode.
        public void draw_image(ScriptImage img, int x, int y, int mode)
        {
            DrawImage(img, x, y, 0, 0, 0, 0, 0, 0, mode);
        }

        //Draws the specified image on top of this image using the specified location, size and mode.
        public void draw_image(ScriptImage img, int x, int y, int w, int h, int mode)
        {
            DrawImage(img, x, y, w, h, 0, 0, 0, 0, mode);
        }

        //Draws the specified portion of the specified image on top of this image using the specified location, size and mode.
        public void draw_image(ScriptImage img, int x, int y, int w, int h, int srcx, int srcy, int srcw, int srch, int mode)
        {
            DrawImage(img, x, y, w, h, srcx, srcy, srcw, srch, mode);
        }

        private enum DrawMode
        {
            Replace = 0,
            Overlay = 1,
        }

        //Handles all the draw_image cases of ScriptImage
        private void DrawImage(ScriptImage img, int x, int y, int width, int height, int srcx, int srcy, int srcw, int srch, int mode)
        {
            //First check if both bitmaps are ok
            Bitmap DestBMP = ScriptMan.GetBitmap(bitmapIndex);
            Bitmap SourceBMP = ScriptMan.GetBitmap(img.ImageIndex);

            if (DestBMP == null) return;
            if (SourceBMP == null) return;

            //Check for empty arguments
            if (srcw == 0) srcw = SourceBMP.Width;
            if (srch == 0) srch = SourceBMP.Height;
            if (width == 0) width = srcw;
            if (height == 0) height = srch;

            /* Graphics.DrawImage seems to handle odd values just fine, so no need to check for them.
            if ((     x < 0) || (        x >    DestBMP.Width)) return; //bad x
            if ((     y < 0) || (        y >   DestBMP.Height)) return; //bad y
            if (( width < 0) || (  width+x >    DestBMP.Width)) return; //bad width
            if ((height < 0) || ( height+y >   DestBMP.Height)) return; //bad height
            if ((  srcx < 0) || (srcx      >  SourceBMP.Width)) return; //bad source x
            if ((  srcy < 0) || (srcy      > SourceBMP.Height)) return; //bad source y
            if ((  srcw < 0) || (srcw+srcx >  SourceBMP.Width)) return; //bad source width
            if ((  srch < 0) || (srch+srcy > SourceBMP.Height)) return; //bad source height
            */

            //Set the drawing mode
            System.Drawing.Drawing2D.CompositingMode compmode;
            if ((DrawMode)mode == DrawMode.Replace)
                compmode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            else if ((DrawMode)mode == DrawMode.Overlay)
                compmode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            else return; //Unknown drawing mode

            Rectangle rect_P = new Rectangle(x, y, width, height);

            using (Graphics g = Graphics.FromImage(DestBMP))
            {
                g.PageUnit = GraphicsUnit.Pixel;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.CompositingMode = compmode;

                g.DrawImage(SourceBMP, rect_P, srcx, srcy, srcw, srch, GraphicsUnit.Pixel);
            }
        }
    }
}
