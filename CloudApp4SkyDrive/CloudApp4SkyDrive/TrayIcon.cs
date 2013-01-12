// TODO: some imports probably not necessary
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloudApp4SkyDrive
{
    class TrayIcon : IDisposable
    {
        NotifyIcon ni;

        public TrayIcon()
        {
            ni = new NotifyIcon();
        }
    }
}
