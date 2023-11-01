using Roznitsa.RoznitsaClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoznitsaApp
{
    public partial class AfterAuth : Form
    {
        User user;
        int ticks = 0;
        string sqlConnectionLine;
        public AfterAuth(User user, string SQLconLine)
        {
            InitializeComponent();

            this.user = user;
            using (MemoryStream stream = new MemoryStream(user.Photo))
            {
                pictureBox1.Image = Image.FromStream(stream);
            }
            label2.Text = user.Name + ", " +  (User.Roles)user.Direction;
            timer1.Start();
            this.sqlConnectionLine= SQLconLine;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(ticks == 1)
            {
                ticks = 0;
                MainMenu mainMenu = new MainMenu(user, sqlConnectionLine);
                mainMenu.Show();
                timer1.Stop();
                this.Hide();
            }
            else
            {
                ticks++;
            }
        }
    }
}
