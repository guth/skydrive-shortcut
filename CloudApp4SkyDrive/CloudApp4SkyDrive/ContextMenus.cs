// TODO: some imports probably not necessary

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using System.Windows;
using System.Windows.Forms;
using CloudApp4SkyDrive.Properties;

namespace CloudApp4SkyDrive
{
    class ContextMenus
    {
        public ContextMenuStrip Create()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;

            // Login button
            ToolStripMenuItem loginItem = new ToolStripMenuItem();
            loginItem.Text = "Login";
            loginItem.Click += new System.EventHandler(Login_Click);
            menu.Items.Add(loginItem);

            // Exit button
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            // TODO: use your own icon
            item.Image = Resources.Exit;
            menu.Items.Add(item);

            return menu;
        }

        void Login_Click(object sender, EventArgs e)
        {
            BrowserWindow bw = new BrowserWindow();
        }

        void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
