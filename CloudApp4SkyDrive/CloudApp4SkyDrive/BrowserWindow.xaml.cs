using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace CloudApp4SkyDrive
{
    public partial class BrowserWindow : Window
    {
        public BrowserWindow()
        {
            InitializeComponent();
            String signInUrl = @"https://login.live.com/oauth20_authorize.srf?client_id={0}&redirect_uri=https://login.live.com/oauth20_desktop.srf&response_type=token&scope={1}";
            signInUrl = String.Format(signInUrl, Globals.ClientID, Globals.Scope);
            webBrowser.Navigate(signInUrl);

            // bring the window to front
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DropForm));
            //this.Icon = this.Icon = ((System.Windows.Media.ImageSource)(resources.GetObject("$this.Icon")));
            this.Show();
            this.Activate();
            // hack to keep it open (by preventing the thread from ending)
            this.Hide();
            this.ShowDialog();
        }

        private void webBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Console.WriteLine("Code: " + e.Uri.AbsoluteUri);
            if (e.Uri.AbsoluteUri.Contains("access_token="))
            {
                //if (App.Current.Properties.Contains("auth_code"))
                //{
                //    App.Current.Properties.Clear();
                //}
                String accessToken = Regex.Split(e.Uri.AbsoluteUri, "access_token=")[1];
                accessToken = Regex.Split(accessToken, "&token_type")[0];
                Console.WriteLine("Access token: " + accessToken);
                Globals.AccessToken = accessToken;
                //App.Current.Properties.Add("auth_code", auth_code);
                this.Close();
            }
        }
    }
}