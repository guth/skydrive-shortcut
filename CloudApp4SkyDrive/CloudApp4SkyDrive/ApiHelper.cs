using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CloudApp4SkyDrive
{
    class ApiHelper
    {
        public void authenticateUser()
        {
            BrowserWindow bw = new BrowserWindow();
            bw.Show();
        }

        public static void UploadFile(String filePath)
        {
            String[] parts = filePath.Split(new char[]{'\\'});
            String fileName = parts[parts.Length-1];
            
            FileStream fileStream = File.OpenRead(filePath);
            byte[] arr = new byte[fileStream.Length];
            fileStream.Read(arr, 0, (int)fileStream.Length)         ;
            fileStream.Close();

            String url = Globals.ApiUrl + "me/skydrive/files/" + fileName + "?access_token="
                            + Globals.AccessToken;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "PUT";
            using (Stream output = req.GetRequestStream())
                output.Write(arr, 0, arr.Length);
            //Stream dataStream = req.GetRequestStream();
            //dataStream.Write(arr, 0, arr.Length);
            //dataStream.Close();

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            resp.Close();
            
            Console.WriteLine("Response: " + resp.ToString());
        }

        public void makeRequest(String url)
        {
            //WebClient wc = new WebClient();
            
            //wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            //wc.DownloadStringAsync(new Uri(url));
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "PUT";
            req.ContentType = "application/octet-stream";
            Stream dataStream = req.GetRequestStream();
            
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
