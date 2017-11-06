using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Patchlunky
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static MainForm mainForm;

        [STAThread]
        static void Main(string[] args)
        {
            //Get the version and the GUID string of the program
            Assembly exAss = Assembly.GetExecutingAssembly();
            string version = FileVersionInfo.GetVersionInfo(exAss.Location).ProductVersion;
            string appGuid = ((GuidAttribute)exAss.GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value;
            string windowName = "Patchlunky " + version + " Beta";

            //The patchlunkyUrl is for use with a custom URI scheme
            string patchlunkyUrl = null;

            if (args.Length > 0)
            {
                //If the first argument starts with the URL protocol, merge all arguments together.
                if (args[0].StartsWith("patchlunky:", StringComparison.OrdinalIgnoreCase))
                {
                    patchlunkyUrl = String.Join(" ", args);
                }
                //else //Handle other commandline arguments
            }

            //Prevent multiple instances by using a global mutex with the application GUID.
            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
            {
                //Use Mutex to check if there is another instance running
                if (!mutex.WaitOne(0, false))
                {
                    //If Patchlunky was started via URL protocol, send the URL to the main running instance.
                    if (patchlunkyUrl != null)
                    {
                        CopyDataMessage.Send(windowName, appGuid, patchlunkyUrl);
                    }
                    else Msg.MsgBox("Another instance of Patchlunky is already running!");

                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(mainForm = new MainForm(windowName, version, appGuid, patchlunkyUrl));
            }
        }
    }
}
