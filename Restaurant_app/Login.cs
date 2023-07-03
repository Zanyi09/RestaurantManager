using Guna.UI2.WinForms;
using Restaurant_Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_app
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            if (Mainclass.IsValidUser(txt_user.Text, txt_password.Text) == false)
            {
                guna2MessageDialog1.Show("invalid username or password");
                return;
            }
            else
            {
                this.Hide();
                Main mi = new Main();
                mi.Show();
            }
        }
    }
}
