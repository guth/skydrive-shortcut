using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace CloudApp4SkyDrive
{
    class ApiHelper
    {
        public void authenticateUser()
        {
            BrowserWindow bw = new BrowserWindow();
            bw.Show();
        }

        public static void BatchUpload(String[] files)
        {
            CreateFolder("NewFolder111");
            String folderId = getFolderId("NewFolder111");
            Console.WriteLine("Folder ID: " + folderId);

            for(int k=0; k<files.Length; k++)
            {
                String fileName = files[k];
                UploadFile(fileName, folderId);
            }

            String link = getFileLink("NewFolder111");
            Console.WriteLine("Link to folder: " + link);

            while (true)
            {
                try
                {
                    Clipboard.SetDataObject(link, true);
                    break;
                }
                catch (Exception e)
                {
                }
            }
            Console.WriteLine("Clipboard text is set.");

        }

        private static String folderUrl = @"https://apis.live.net/v5.0/me/skydrive/";
        public static void CreateFolder(String folderName)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(folderUrl);
            req.Method = "POST";
            req.ContentType = "application/json";
            req.Headers.Add("Authorization", "Bearer " + Globals.AccessToken);
            
            String jsonBody = String.Format("{{ \"name\": \"{0}\" }}", folderName);

            StreamWriter sw = new StreamWriter(req.GetRequestStream());
            sw.Write(jsonBody);
            sw.Close();

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Console.WriteLine(resp.Headers.Keys);
            resp.Close();
        }

        public static void UploadFile(String filePath, String folderId)
        {
            String[] parts = filePath.Split(new char[] { '\\' });
            String fileName = parts[parts.Length - 1];

            FileStream fileStream = File.OpenRead(filePath);
            byte[] arr = new byte[fileStream.Length];
            fileStream.Read(arr, 0, (int)fileStream.Length);
            fileStream.Close();

            String url = Globals.ApiUrl + folderId + "/files/" + fileName + "?access_token="
                            + Globals.AccessToken;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "PUT";
            using (Stream output = req.GetRequestStream())
                output.Write(arr, 0, arr.Length);

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            resp.Close();
            String fileId = getFileId(fileName);
            Console.WriteLine("File id: " + fileId);

            String link = getFileLink(fileId);
            Console.WriteLine("Link to file: " + link);

            while (true)
            {
                try
                {
                    Clipboard.SetDataObject(link, true);
                    break;
                }
                catch (Exception e)
                {
                }
            }
            Console.WriteLine("Clipboard text is set.");
        }

        public static void UploadFile(String filePath)
        {
            String[] parts = filePath.Split(new char[] { '\\' });
            String fileName = parts[parts.Length - 1];

            FileStream fileStream = File.OpenRead(filePath);
            byte[] arr = new byte[fileStream.Length];
            fileStream.Read(arr, 0, (int)fileStream.Length);
            fileStream.Close();

            UploadFile(fileName, arr);
        }

        public static void UploadFile(String fileName, byte[] arr) {
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
            String fileId = getFileId(fileName);
            Console.WriteLine("File id: " + fileId);

            String link = getFileLink(fileId);
            Console.WriteLine("Link to file: " + link);

            while (true)
            {
                try
                {
                    Clipboard.SetDataObject(link, true);
                    break;
                }
                catch (Exception e)
                {
                }
            }
            Console.WriteLine("Clipboard text is set.");
        }

        private static String searchUrl = @"https://apis.live.net/v5.0/me/skydrive/search?q={0}&access_token={1}";
        public static String getFileId(String fileName)
        {
            String url = String.Format(searchUrl, fileName, Globals.AccessToken);
            String jsonResponse = getJsonResult(url);
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonResponse));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName && reader.Value.Equals("id"))
                    {
                        reader.Read();
                        if (reader.TokenType == JsonToken.String
                            && reader.Value.ToString().Substring(0, 4).Equals("file"))
                            return reader.Value.ToString();
                    }
                }
            }
            return "Failed";
        }

        public static String getFolderId(String folderName)
        {
            String url = String.Format(searchUrl, folderName, Globals.AccessToken);
            String jsonResponse = getJsonResult(url);
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonResponse));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName && reader.Value.Equals("id"))
                    {
                        reader.Read();
                        if (reader.TokenType == JsonToken.String
                            && reader.Value.ToString().Substring(0, 6).Equals("folder"))
                            return reader.Value.ToString();
                    }
                }
            }
            return "Failed";
        }

        private static string fileLinkUrl = @"https://apis.live.net/v5.0/{0}/shared_read_link?access_token={1}";
        private static String getFileLink(String fileId)
        {
            String url = String.Format(fileLinkUrl, fileId, Globals.AccessToken);
            String jsonResponse = getJsonResult(url);
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonResponse));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                    if (reader.TokenType == JsonToken.PropertyName && reader.Value.Equals("link"))
                    {
                        reader.Read();
                        return reader.Value.ToString();
                    }
                }
            }
            return "Failed to get link for file ID: " + fileId;
        }

        private static String getJsonResult(String url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "GET";
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            String jsonContent = sr.ReadToEnd();
            resp.Close();

            return jsonContent;
        }
    }
}
