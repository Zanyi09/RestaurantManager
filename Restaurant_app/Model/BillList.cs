using Restaurant_app.Reports;
using Restaurant_Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_app.Model
{
    public partial class BillList : SampleAdd
    {
        public BillList()
        {
            InitializeComponent();
        }
        public int MainID = 0;
        private void BillList_Load(object sender, EventArgs e)
        {
            LoadDate();
        }
        private void LoadDate()
        {
            string qry = "select MainID,TableName,WaiterName,orderType,status,total from tblMain where status <> 'Pending' ";
            ListBox lb = new ListBox();
            lb.AutoSize = true;
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvtable);
            lb.Items.Add(dgvWaiter);
            lb.Items.Add(dgvType);
            lb.Items.Add(dgvStatus);
            lb.Items.Add(dgvTotal);

            Mainclass.LoadData(qry, guna2DataGridView1, lb);
        }
        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Hiển thị seri products
            int count = 0;

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                // Thay đổi thuộc tính textbox trước khi mở
                MainID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                this.Close();
            }
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                MainID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                string qry = @"Select * from tblMain m inner join 
                                tblDetails d on d.MainID = m.MainID
                                inner join products p on p.pID = d.proID
                                where m.MainID="+ MainID + "";

                SqlCommand cmd = new SqlCommand(qry, Mainclass.con);
                Mainclass.con.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                Mainclass.con.Close();

                Print frm = new Print();
                rpBill cr = new rpBill();
                cr.SetDatabaseLogon("sa", "sa123123");
                cr.SetDataSource(dt);
                frm.crystalReportViewer1.ReportSource = cr;
                frm.crystalReportViewer1.Refresh();
                frm.Show();

            }
        }

    }
}
