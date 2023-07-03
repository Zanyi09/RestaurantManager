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
    public partial class ProductView : SampleView
    {
        public ProductView()
        {
            InitializeComponent();
        }

        private void ProductView_Load(object sender, EventArgs e)
        {
            GetData();
        }
        public void GetData()
        {
            string qry = "select pID,pName,pPrice,CategoryID,c.CateName from products p inner join category c on c.cateID = p.categoryID where pName like '%" + txt_Search.Text + "%' ";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvPrice);
            lb.Items.Add(dgvCateID);
            lb.Items.Add(dgvCate);
            Mainclass.LoadData(qry, guna2DataGridView1, lb);
        }
        public override void btn_add_Click(object sender, EventArgs e)
        {
            //thêm hiệu ứng màu xanh
            //TablesAdd tbadd = new TablesAdd();
            //tbadd.ShowDialog();
            Mainclass.Bluebackgournd(new ProductsAdd());
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
                ProductsAdd cate = new ProductsAdd();
                cate.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                cate.cID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvCateID"].Value);
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
                    string qry = "Delete From products where pID= " + id + " ";
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
