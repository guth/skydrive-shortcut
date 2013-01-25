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

namespace SkyDrive_Shortcut
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
            this.Focus();
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

            TrayIcon.DisplaySuccessMessage();
            this.Close();
        }

        private void DropForm_KeyDown(object sender, KeyEventArgs e)
        {
            DateTime now = DateTime.Now;
            String timeString = now.ToString("M-d-yyyy h-mm-ss tt");

            if (e.KeyCode == Keys.V && Form.ModifierKeys == Keys.Control)
            {
                if(Clipboard.ContainsImage()) {
                    ImageConverter converter = new ImageConverter();
                    hideInstructions();
                    ApiHelper.UploadFileAndCopyLink("screenshot " + timeString, (byte[])converter.ConvertTo(Clipboard.GetImage(), typeof(byte[])));
                }
                else if (Clipboard.ContainsFileDropList())
                {
                    String[] s = (String[])Clipboard.GetData(DataFormats.FileDrop);

                    hideInstructions();
                    if (s.Length > 1) // batch upload
                    {
                        ApiHelper.BatchUpload(s);
                    }
                    else
                    {
                        String filePath = s[0];
                        ApiHelper.UploadFileAndCopyLink(filePath);
                    }
                }
                else if (Clipboard.ContainsText())
                {
                    hideInstructions();

                    String text = Clipboard.GetText();
                    byte[] data = new UTF8Encoding().GetBytes(text);

                    String fileName = "New File " + timeString + ".txt";
                    Console.WriteLine("Uploading " + fileName);
                    ApiHelper.UploadFileAndCopyLink(fileName, data);
                }
                TrayIcon.DisplaySuccessMessage();
                this.Close();
            }
        }

        private void hideInstructions()
        {
            InstructionLabel.Visible = false;
            ProgressLabel.Visible = true;
            // force UI update before sleep
            ProgressLabel.Refresh();

            Thread.Sleep(500);

            this.Hide();

            InstructionLabel.Visible = true;
            ProgressLabel.Visible = false;
        }

        /*private void fadeForm()
        {
            int NumberOfSteps = 200;
            float StepVal = (float)(100f / NumberOfSteps);
            float fOpacity = 100f;

            for (byte b = 0; b < NumberOfSteps; b++)
            {
                this.Opacity = fOpacity / 100;
                this.Refresh();
                fOpacity -= StepVal;
            }
        }*/
    }
}
