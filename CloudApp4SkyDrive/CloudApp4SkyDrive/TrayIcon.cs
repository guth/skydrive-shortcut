// TODO: some imports probably not necessary
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using CloudApp4SkyDrive.Properties;

namespace CloudApp4SkyDrive
{
    class TrayIcon : IDisposable
    {
        NotifyIcon ni;

        public TrayIcon()
        {
            ni = new NotifyIcon();
        }

        public void Display()
        {
            // put the icon in the system tray
            // TODO: use your own icon
            ni.Icon = Resources.TrayIcon;
            ni.Text = "SkyDrive Shortcut";
            ni.Visible = true;

            ni.MouseClick += new MouseEventHandler(ni_MouseClick);

            // make a right-click menu
            ni.ContextMenuStrip = new ContextMenus().Create();
        }

        public void Dispose()
        {
            ni.Dispose();
        }

        void ni_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Globals.AccessToken == null) // Not logged in
                {
                    if (!Globals.LoginWindowOpen)
                    {
                        Globals.LoginWindowOpen = true;
                        BrowserWindow bw = new BrowserWindow();
                        Globals.LoginWindowOpen = false;
                    }
                }
                else
                {
                    //MessageBox.Show("content", "title");
                    if (!DropForm.isOpen)
                    {
                        DropForm.isOpen = true;
                        new DropForm().ShowDialog();
                        DropForm.isOpen = false;
                    }
                }
            }
        }

    }
}
