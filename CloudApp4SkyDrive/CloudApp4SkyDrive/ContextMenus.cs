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
        private CheckBox hotCornerCheckBox;

        public ContextMenuStrip Create()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;

            // Check box to enable/disable the hot corner
            hotCornerCheckBox = new CheckBox();
            hotCornerCheckBox.Checked = true;
            hotCornerCheckBox.CheckedChanged += new EventHandler(hotCornerCheckBox_CheckChanged);
            ToolStripControlHost itemHost = new ToolStripControlHost(hotCornerCheckBox);
            itemHost.Text = "Hot Corner Enabled";
            menu.Items.Add(itemHost);

            // Exit button
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            // TODO: use your own icon
            item.Image = Resources.Exit;
            menu.Items.Add(item);

            return menu;
        }

        void hotCornerCheckBox_CheckChanged(object sender, EventArgs e)
        {
            Globals.HotCornerEnabled = hotCornerCheckBox.Checked;
        }

        void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
