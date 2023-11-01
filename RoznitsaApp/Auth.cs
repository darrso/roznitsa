using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Roznitsa;
using Roznitsa.RoznitsaClasses;

namespace RoznitsaApp
{
    public partial class Auth : Form
    {
        string sqlConnectionString = @"Data Source=DARRSO\DARRSO;Initial Catalog=""Розничная торговля"";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        User user;
        string captcha = "";
        bool captcha_enabled = false;
        int ticks = 0;
        public Auth()
        {
            InitializeComponent();
            EnterButton.BackColor = Color.FromArgb(45, 62, 80); 
            ShowPassword.BackColor = Color.FromArgb(45, 62, 80); 
        }
        private Bitmap CreateImage(int w, int h)
        {
            Random rnd = new Random();
            Bitmap res = new Bitmap(w, h);

            int XPosText = rnd.Next(0, w - 50);
            int YPosText = rnd.Next(9, h - 15);

            Graphics g = Graphics.FromImage((Image)res);
            g.Clear(Color.LightCyan);

            captcha = "";
            string symbols = "ABCDEFGHUJKLMNOPQRSTUVWXYZ1234567890";
            for(int i = 0; i < 4; i++)
            {
                captcha += symbols[rnd.Next(symbols.Length)];
            }
            g.DrawString(captcha, new Font("Arial", 15), Brushes.Black, new PointF(XPosText, YPosText));


            // Помехи
            g.DrawLine(Pens.White, new Point(0, 0), new Point(w - 1, h - 1));
            g.DrawLine(Pens.White, new Point(0, h-1), new Point(w - 1, 0));
            for(int i = 0; i < w; i++)
            {
                for(int j = 0; j < h; j++)
                {
                    if(rnd.Next() % 20 == 0)
                    {
                        res.SetPixel(i, j, Color.White);
                    }
                }
            }
            return res;
        }
        private void ShowPassword_Click(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '*')
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            bool res = CheckData(textBox1.Text, textBox2.Text);
            if (res && (captcha == textBox3.Text))
            {
                this.Hide();
                AfterAuth afterAuth = new AfterAuth(user, sqlConnectionString);
                afterAuth.Show();
                Database.InsertQuery(sqlConnectionString, "INSERT INTO [dbo].[История входов] VALUES (@Login, @Time, @Succ)", new Parametr[]
                {
                    new Parametr("@Login", textBox1.Text),
                    new Parametr("@Time", Database.OpDate(DateTime.Now)),
                    new Parametr("@Succ", 1)
                });
            }
            else
            {
                if (!captcha_enabled)
                {
                    groupBox1.Visible = true;
                    Bitmap bitmap = CreateImage(pictureBox2.Width, pictureBox2.Height);
                    pictureBox2.Image = bitmap;
                    captcha_enabled = true;
                    Database.InsertQuery(sqlConnectionString, "INSERT INTO [dbo].[История входов] VALUES (@Login, @Time, @Succ)", new Parametr[]
                {
                    new Parametr("@Login", textBox1.Text),
                    new Parametr("@Time", Database.OpDate(DateTime.Now)),
                    new Parametr("@Succ", 0)
                });
                }

                else
                {
                    EnterButton.Enabled = false;
                    timer1.Start();
                    MessageBox.Show("Таймаут 10 секунд");
                    Database.InsertQuery(sqlConnectionString, "INSERT INTO [dbo].[История входов] VALUES (@Login, @Time, @Succ)", new Parametr[]
                {
                    new Parametr("@Login", textBox1.Text),
                    new Parametr("@Time", Database.OpDate(DateTime.Now)),
                    new Parametr("@Succ", 0)
                });
                }
            }
        }
        private bool CheckData(string login, string password)
        {
            string hash_pass_user = Database.Crypt(password);
            this.user = Database.GetUser(User.Queries.CheckUser, sqlConnectionString, new Parametr[2] { new Parametr("@Login", login), new Parametr("@Password", hash_pass_user) });
            if (user.Login != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(ticks == 10)
            {
                timer1.Stop();
                EnterButton.Enabled = true;
                Bitmap bitmap = CreateImage(pictureBox2.Width, pictureBox2.Height);
                pictureBox2.Image = bitmap;
                ticks= 0;
            }
            else
            {
                ticks++;
            }
        }
    }
}
