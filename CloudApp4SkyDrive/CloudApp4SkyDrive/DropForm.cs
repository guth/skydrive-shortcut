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
            this.DragDrop += new
                System.Windows.Forms.DragEventHandler(this.DropForm_DragDrop);
            this.DragEnter += new
                System.Windows.Forms.DragEventHandler(this.DropForm_DragEnter);
        }

        private void DropForm_Load(object sender, EventArgs e)
        {
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
            InstructionLabel.Visible = false;

            // code for actually dealing with files
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            //int i;
            //for (i = 0; i < s.Length; i++)
            //    Console.WriteLine(s[i]);
            String filePath = s[0];
            ApiHelper.UploadFile(filePath);
            // comments below here are for a progress bar
            //this.Height = 87;
            
            /*progressBar1.Visible = true;

            for (int i = 0; i < 100; i+=10)
            {
                progressBar1.Value = i;
                Thread.Sleep(100);
            }

            this.Height = 309;*/
            SuccessLabel.Visible = true;
            SuccessLabel.Refresh();

            Thread.Sleep(1500);

            SuccessLabel.Visible = false;
            //InstructionLabel.Visible = true;
            progressBar1.Visible = false;
            this.Hide();
        }
    }
}
