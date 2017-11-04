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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Patchlunky
{
    internal static class CopyDataMessage
    {
        private const Int32 WM_COPYDATA = 0x004A;

        //Struct containing data passed with the WM_COPYDATA message
        [StructLayout(LayoutKind.Sequential)]
        internal struct COPYDATASTRUCT
        {
            public IntPtr dwData; //A number that can be used to identify the data
            public int    cbData; //The size of the data in bytes
            public IntPtr lpData; //Pointer to the data
        }

        //Sends a string to another window using WM_COPYDATA
        public static void Send(string lpWindowName, string signature, string message)
        {
            //Find the window
            IntPtr hWnd = IntPtr.Zero;
            hWnd = NativeMethods.FindWindow(null, lpWindowName);
            if (hWnd == IntPtr.Zero)
            {
                Msg.Log("Running instance of Patchlunky not found!");
                return;
            }

            //Create the message
            string MsgData = signature + message;

            //Pointer for the unmanaged memory block that will hold the message
            IntPtr pMsgData = IntPtr.Zero;

            try
            {
                //Allocate and copy message to unmanaged memory
                int MsgDataSize = System.Text.Encoding.Unicode.GetByteCount(MsgData) + 1;
                pMsgData = Marshal.StringToHGlobalUni(MsgData);

                //Fill COPYDATASTRUCT members
                COPYDATASTRUCT cds = new COPYDATASTRUCT();
                cds.dwData = IntPtr.Zero;
                cds.cbData = MsgDataSize;
                cds.lpData = pMsgData;

                // Send the WM_COPYDATA message
                NativeMethods.SendMessage(hWnd, WM_COPYDATA, IntPtr.Zero, ref cds);

                // Check if there was an error
                int error = Marshal.GetLastWin32Error();
                if (error != 0)
                {
                    Msg.Log("Error in SendMessage(WM_COPYDATA)!: " + error);
                }
            }
            catch (Exception ex)
            {
                Msg.Log("Error sending CopyDataMessage: " + ex.Message);
            }
            finally
            {
                //Free the previously allocated memory
                Marshal.FreeHGlobal(pMsgData);
            }
        }

        public static string Read(ref Message m, string signature)
        {
            if (m.Msg != WM_COPYDATA)
                return null;

            COPYDATASTRUCT cds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));

            if (cds.dwData != IntPtr.Zero)
                return null; //dwData should always be zero.

            string MsgData = Marshal.PtrToStringUni(cds.lpData);

            if (MsgData == null)
                return null;

            //The message should be longer than its signature.
            if (MsgData.Length <= signature.Length)
            {
                Msg.Log("Invalid data in WM_COPYDATA message!");
                return null;
            }

            //The signature in the message must match our signature
            string MsgSig = MsgData.Substring(0, signature.Length);
            if (MsgSig.Equals(signature, StringComparison.Ordinal) == false)
            {
                Msg.Log("Invalid signature in WM_COPYDATA message!");
                return null;
            }

            //Return the message without its signature
            return MsgData.Substring(signature.Length);
        }
    }

    internal static class NativeMethods
    {
        //Import the SendMessage function from user32.dll
        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        //Import SendMessage function for use with WM_COPYDATA message
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, ref CopyDataMessage.COPYDATASTRUCT lParam);

        //Import the FindWindow function from user32.dll
        [DllImport("user32.dll", SetLastError=true)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    }
}

