using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CloudApp4SkyDrive
{
    class LoginHelper
    {
        public void authenticateUser()
        {
            BrowserWindow bw = new BrowserWindow();
            bw.Show();
        }

        public void makeRequest(String url)
        {
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            wc.DownloadStringAsync(new Uri(url));
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                Console.WriteLine(e.Result);
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                Console.WriteLine(tie.InnerException.ToString());
            }
            //userData = deserializeJson(e.Result);
            //changeView();
        }
    }
}
