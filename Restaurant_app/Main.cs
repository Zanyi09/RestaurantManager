using Guna.UI2.WinForms;
using Restaurant_app.Model;
using Restaurant_app.View;
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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        //để truy cập chính
        static Main _obj;
        public static Main Instance
        {
            get { if (_obj == null) { _obj = new Main(); } return _obj; }
        }
        //Phương Thức thêm Control trong Mainform
        //Xử lý nút buttom home,categories....
        public void AddControls(Form f)
        {
            CenterPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            CenterPanel.Controls.Add(f);
            f.Show();
        }
        private void btn_exit1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)// Category
        {
            AddControls(new CategoryView());
        }

        private void Main_Load(object sender, EventArgs e)
        {
            lbluser.Text = Mainclass.USER;
            _obj = this;
        }

        private void Centerpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_home_Click(object sender, EventArgs e) // Home
        {
            AddControls(new Home());
        }

        private void btn_table_Click(object sender, EventArgs e)
        {
            AddControls(new TableView());
        }

        private void btn_staff_Click(object sender, EventArgs e)
        {
            AddControls(new StaffView());
        }

        private void btn_products_Click(object sender, EventArgs e)
        {
            AddControls(new ProductView());
        }

        private void btn_pos_Click(object sender, EventArgs e)
        {
            POS ps = new POS();
            ps.ShowDialog();
        }

        private void btn_kitchen_Click(object sender, EventArgs e)
        {
            AddControls(new KitchenView());
        }
    }
}
