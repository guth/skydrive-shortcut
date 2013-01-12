using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace CloudApp4SkyDrive
{
    public partial class DropForm : Form
    {
        public static bool isOpen = false;

        public DropForm()
        {
            InitializeComponent();
            //this.SetDesktopLocation(30, 30);
            this.DesktopLocation = new Point(0, 0);
            this.DragDrop += new
                System.Windows.Forms.DragEventHandler(this.DropForm_DragDrop);
            this.DragEnter += new
                System.Windows.Forms.DragEventHandler(this.DropForm_DragEnter);
        }

        private void DropForm_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void DropForm_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            hideInstructions();

            // code for actually dealing with files
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            //int i;
            //for (i = 0; i < s.Length; i++)
            //    Console.WriteLine(s[i]);
            
            if (s.Length > 1) // batch upload
            {
                ApiHelper.BatchUpload(s);
            }
            else
            {
                String filePath = s[0];
                ApiHelper.UploadFileAndCopyLink(filePath);
            }
            
            animateUpload();
        }

        private void DropForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && Form.ModifierKeys == Keys.Control)
            {
                if(Clipboard.ContainsImage()) {
                hideInstructions();

                DateTime now = DateTime.Now;
                String timeString = now.ToString("M-d-yyyy h-mm-ss tt");
                ImageConverter converter = new ImageConverter();
                Console.WriteLine("screenshot " + timeString);
                ApiHelper.UploadFileAndCopyLink("screenshot " + timeString, (byte[])converter.ConvertTo(Clipboard.GetImage(), typeof(byte[])));

                animateUpload();
                
                }
                else if (Clipboard.ContainsFileDropList())
                {
                    String[] s = (String[])Clipboard.GetData(DataFormats.FileDrop);

                    if (s.Length > 1) // batch upload
                    {
                        ApiHelper.BatchUpload(s);
                    }
                    else
                    {
                        String filePath = s[0];
                        ApiHelper.UploadFileAndCopyLink(filePath);
                    }
                    this.Close();
                }
            }
        }

        private void hideInstructions()
        {
            InstructionLabel.Visible = false;
        }

        private void animateUpload()
        {
            SuccessLabel.Visible = true;
            SuccessLabel.Refresh();

            Thread.Sleep(1500);

            SuccessLabel.Visible = false;
            InstructionLabel.Visible = true;
            this.Hide();
        }
    }
}
