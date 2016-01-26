using System;
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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(mainForm = new MainForm());
        }
    }
}
