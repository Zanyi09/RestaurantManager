using Guna.UI2.WinForms;
using Restaurant_app.Model;
using Restaurant_Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_app.View
{
    public partial class StaffView : SampleView
    {
        public StaffView()
        {
            InitializeComponent();
        }

        private void StaffView_Load(object sender, EventArgs e)
        {
            GetData();
        }
        public void GetData()
        {
            string qry = "Select * From Staff where sName like '%" + txt_Search.Text + "%' ";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvPhone);
            lb.Items.Add(dgvRole);
            Mainclass.LoadData(qry, guna2DataGridView1, lb);
        }
        public override void btn_add_Click(object sender, EventArgs e)
        {
            //thêm hiệu ứng màu xanh
            //TablesAdd tbadd = new TablesAdd();
            //tbadd.ShowDialog();
            Mainclass.Bluebackgournd(new StaffAdd());
            GetData();
        }
        public override void txt_Search_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                StaffAdd cate = new StaffAdd();
                cate.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                cate.txt_Name.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                cate.txt_Phone.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvPhone"].Value);
                cate.cb_Role.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvRole"].Value);
                cate.ShowDialog();
                GetData();
            }
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                // need to confirm before delete
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                if (guna2MessageDialog1.Show("Are you sure you want to delete?") == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                    string qry = "Delete From Staff where staffid= " + id + " ";
                    Hashtable ht = new Hashtable();
                    Mainclass.SQL(qry, ht);

                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show("Delete Successfully...");
                    GetData();
                }


            }
        }

    }
}
