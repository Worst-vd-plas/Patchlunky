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
using System.Windows.Forms;

namespace Patchlunky
{
    public static class Msg
    {
        public static void Log(string msg)
        {
            string path = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/";
            string time = DateTime.Now.ToString("HH:mm:ss");
            string text = time + " - " + msg + Environment.NewLine;
            
            if(Program.mainForm != null)
                Program.mainForm.Log(text);

            File.AppendAllText(path + "Patchlunky.log", text);
        }

        public static DialogResult MsgBox(string msg, string caption = "Patchlunky", 
                                          MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            if (Program.mainForm != null)
                return MessageBox.Show(Program.mainForm, msg, caption, buttons);
            else
                return MessageBox.Show(msg, caption, buttons);
        }
    }
}
