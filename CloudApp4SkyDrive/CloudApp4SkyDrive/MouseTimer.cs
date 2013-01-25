using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace CloudApp4SkyDrive
{
    class MouseTimer
    {
        public void go()
        {
            System.Timers.Timer t = new System.Timers.Timer(500.0);
            t.Elapsed += OnTimedEvent;
            t.Enabled = true;
            t.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (Cursor.Position.X < 5 && Cursor.Position.Y < 5
                    && Globals.HotCornerEnabled && !DropForm.isOpen)
            {
                Thread t = new Thread(new ThreadStart(ShowDropDownDialog));
                t.SetApartmentState(ApartmentState.STA);
                t.Start(); 
            }
        }

        private void ShowDropDownDialog()
        {
            if (Globals.AccessToken == null) // Not logged in
            {
                if (!Globals.LoginWindowOpen)
                {
                    Globals.LoginWindowOpen = true;
                    BrowserWindow bw = new BrowserWindow();
                    Globals.LoginWindowOpen = false;
                }
            }
            else
            {
                if (!DropForm.isOpen)
                {
                    DropForm.isOpen = true;
                    DropForm df = new DropForm();

                    // bring the form to front
                    df.Show();
                    df.Activate();
                    // hack to keep it open (by preventing the thread from ending)
                    df.Hide();
                    df.ShowDialog();

                    DropForm.isOpen = false;
                }
            }
        }
    }
}
