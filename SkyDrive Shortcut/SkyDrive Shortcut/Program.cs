﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyDrive_Shortcut
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            MouseTimer m = new MouseTimer();
            m.go();

            using (TrayIcon ti = new TrayIcon())
            {
                ti.Display();
                Application.Run();
            }
        }
    }
}
