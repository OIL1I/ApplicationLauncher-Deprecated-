﻿using System;
using System.Windows.Forms;

namespace ApplicationLauncher
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var arg = "";
            if (args.Length == 0)
            {
                arg = "Start";
            }
            else
            {
                arg = args[0];
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.Favorites(arg));
        }
    }
}
