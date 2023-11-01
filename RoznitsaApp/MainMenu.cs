using Roznitsa.RoznitsaClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoznitsaApp
{
    public partial class MainMenu : Form
    {
        User user;
        string sqlConnectionLine;
        public MainMenu(User user, string SQLconLine)
        {
            InitializeComponent();
            this.user = user;
            foreach(var button in this.Controls)
            {
                if(button is Button)
                {
                    ((Button)button).BackColor = Color.FromArgb(45, 62, 80);
                }
            } 
            if(user.Direction == 2)
            {
                button5.Visible= false;
                button4.Visible= false;
                button3.Visible= false;
                button1.Visible= false;
            }
            this.sqlConnectionLine = SQLconLine;
        }


        private void button6_Click(object sender, EventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Hide();
        }

        private void EnterButton_Click_1(object sender, EventArgs e)
        {
            Tables.TablesForm tablesForm = new Tables.TablesForm(user, Tables.Tables.Товары, sqlConnectionLine);
            tablesForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Tables.TablesForm tablesForm = new Tables.TablesForm(user, Tables.Tables.Продажи, sqlConnectionLine);
            tablesForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tables.TablesForm tablesForm = new Tables.TablesForm(user, Tables.Tables.Фирмы, sqlConnectionLine);
            tablesForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Tables.TablesForm tablesForm = new Tables.TablesForm(user, Tables.Tables.Поставщики, sqlConnectionLine);
            tablesForm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Tables.TablesForm tablesForm = new Tables.TablesForm(user, Tables.Tables.Поставки, sqlConnectionLine);
            tablesForm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Tables.Users usersForm = new Tables.Users(user, sqlConnectionLine);
            usersForm.Show();
            this.Hide();
        }
    }
}
