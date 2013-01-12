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
            ni.Icon = Resources.TrayIconV2;
            ni.Text = "SkyDrive Shortcut";
            ni.Visible = true;

            // make a right-click menu
            ni.ContextMenuStrip = new ContextMenus().Create();
        }

        public void Dispose()
        {
            ni.Dispose();
        }

    }
}
