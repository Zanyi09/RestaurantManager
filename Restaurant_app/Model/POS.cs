using Restaurant_Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Restaurant_app.Model
{
    public partial class POS : Form
    {
        public POS()

        {
            InitializeComponent();
        }
        public int MainID = 0;
        public string OderType="";
        public int driverID = 0;
        public string customerName = "";
        public string customerPhone = "";
        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void POS_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();

            Productspanel.Controls.Clear();
            LoadProduct();
        }
        // Thêm loại sản phẩm bên góc trái
        private void AddCategory()
        {
            string qry = "Select * from category";
            SqlCommand cmd = new SqlCommand(qry,Mainclass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            CategoryPanel.Controls.Clear();
            Guna.UI2.WinForms.Guna2Button b2= new Guna.UI2.WinForms.Guna2Button();
            b2.FillColor = Color.FromArgb(50, 55, 89);
            b2.Size = new Size(134, 45);
            b2.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            b2.Text = "All Categories";
            b2.CheckedState.FillColor = Color.FromArgb(241, 85, 126);
            b2.Click += new EventHandler(_Click);
            CategoryPanel.Controls.Add(b2);
            
            if (dt.Rows.Count > 0)   
            {
                foreach( DataRow row in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.FillColor = Color.Orange;
                    b.Size = new Size(122, 40);
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = "All Categories".ToString();
                    b.Text = row["CateName"].ToString();

                    //Sự kiện khi click
                    b.Click += new EventHandler(_Click);
                    CategoryPanel.Controls.Add(b);
                }
            }

        }
        // Click loại sản phẩm
        private void _Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            if(b.Text == "All Categories")
            {
                txt_Search.Text = "1";
                txt_Search.Text = "";
                return;
            }
            foreach (var item in Productspanel.Controls)
            {
                var pro = (UcProduct)item;
                pro.Visible = pro.pCategory.ToLower().Contains(b.Text.Trim().ToLower());

            }
        }

        // Thêm thông tin sản phẩm vào DataGridView
        private void AddItems(string id, string proID, string name, string cat, string price , Image pimage)
        {
            var w = new UcProduct()
            {
                PName = name,
                PPrice = price,
                pCategory = cat,
                PImage = pimage,
                id = Convert.ToInt32(proID)
            };
            Productspanel.Controls.Add(w);

            w.onSelect += (ss, ee) =>
            {
                var wdg = (UcProduct)ss;
                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    // Kiểm tra sản phẩm có tồn tại không sau đó cập nhật số lượng và cật nhật giá
                    if (Convert.ToInt32(item.Cells["dgvproID"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) *
                                                        double.Parse(item.Cells["dgvPrice"].Value.ToString());
                        return;
                    }
                }
                // Thêm sản phẩm qua tạo mới new object
                // Đâu tiên cho Sr#(seri) và thứ 2 cho id
                guna2DataGridView1.Rows.Add(new object[] { 0,0, wdg.id, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                getTotal();
            };
        }
        // Load sản phẩm
        private void LoadProduct()
        {
            string qry = "Select * from products inner join category on cateID = categoryID";
            SqlCommand cmd = new SqlCommand(qry, Mainclass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                Byte[] imagearray = (byte[])item["pImage"];
                byte[] imagebytearray = imagearray;

                AddItems("0",item["pID"].ToString(), item["pName"].ToString(), item["cateName"].ToString(),
                    item["pPrice"].ToString(), Image.FromStream(new MemoryStream(imagebytearray)));
            }
        }
        // Tìm kiếm sản phẩm
        private void txt_Search_TextChanged(object sender, EventArgs e)
        {
            foreach(var item in Productspanel.Controls)
            {
                var pro = (UcProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txt_Search.Text.Trim().ToLower());

            }    
        }
        // Hiển thị seri products
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
        // Tính tổng lượt mua hàng
        private void getTotal()
        {
            double sum = 0;
            lbltotal.Text = "";
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                sum += double.Parse(item.Cells["dgvAmount"].Value.ToString());
            }
            lbltotal.Text = sum.ToString();
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            lbltable.Text = "";
            lblwaiter.Text = "";
            lbltable.Visible = false;
            lblwaiter.Visible = false;
            guna2DataGridView1.Rows.Clear();
            lbltotal.Text = "00";
        }

        private void btn_Delivery_Click(object sender, EventArgs e)
        {
            lbltable.Text = "";
            lblwaiter.Text = "";
            lbltable.Visible = false;
            lblwaiter.Visible = false;
            OderType = "Delivery";

            AddCustomer frm = new AddCustomer();
            frm.mainID = MainID;
            frm.orderType = OderType;
            Mainclass.Bluebackgournd(frm);

            if (frm.txt_Name.Text != "")
            {
                driverID = frm.driverID;
                lblDriverName.Text = "Customer Name: " + frm.txt_Name.Text + "  Phone: " + frm.txt_phone.Text + "  Driver:" + frm.cb_Driver.Text;
                lblDriverName.Visible = true;
                customerName = frm.txt_Name.Text;
                customerPhone = frm.txt_phone.Text;
            }
        }

        private void btn_takeaway_Click(object sender, EventArgs e)
        {
            lbltable.Text = "";
            lblwaiter.Text = "";
            lbltable.Visible = false;
            lblwaiter.Visible = false;
            OderType = "Take Away";

            AddCustomer frm = new AddCustomer();
            frm.mainID = MainID;
            frm.orderType = OderType;
            Mainclass.Bluebackgournd(frm);

            if (frm.txt_Name.Text !="")
            {
                driverID = frm.driverID;
                lblDriverName.Text = "Customer Name: " +frm.txt_Name.Text +"Phone: "+frm.txt_phone.Text +"Driver:"+frm.cb_Driver.Text;
                lblDriverName.Visible = true;
                customerName = frm.txt_Name.Text;
                customerPhone = frm.txt_phone.Text;
            }
        }

        private void btn_dinin_Click(object sender, EventArgs e)
        {
            OderType = "Din In";
            lblDriverName.Visible = false;
            // Cần tạo bảng Table selection và Waiter selection
            TableSelect table = new TableSelect();
            Mainclass.Bluebackgournd(table);
            if(table.TableName != "")
            {
                lbltable.Text = table.TableName;
                lbltable.Visible = true;
            }
            else
            {
                lbltable.Text = "";
                lbltable.Visible = false;
            }

            WaiterSelect waiter = new WaiterSelect();
            Mainclass.Bluebackgournd(waiter);
            if (waiter.WaiterName != "")
            {
                lblwaiter.Text = waiter.WaiterName;
                lblwaiter.Visible = true;
            }
            else
            {
                lblwaiter.Text = "";
                lblwaiter.Visible = false;
            }

        }

        private void btn_KOT_Click(object sender, EventArgs e)
        {
            string qry1 = ""; //Main table
            string qry2 = ""; //Detail table

            int detailID = 0;
            if(MainID == 0) // Insert
            {
                qry1 = @"Insert into tblMain values(@aDate,@aTime,@TableName,@WaiterName,
                    @status,@orderType,@total,@received,@change,@driverID,@CustName,@CustPhone);
                        Select SCOPE_IDENTITY()";
                //nhận giá trị id thêm gần đây
            }
            else //update
            {
                qry1 = @"Update tblMain Set status= @status,total = @total,
                        received= @received,change = @change where MainID =@ID";
            }

            SqlCommand cmd = new SqlCommand(qry1, Mainclass.con);
            cmd.Parameters.AddWithValue("@ID",MainID);
            cmd.Parameters.AddWithValue("@aDate",Convert.ToDateTime(DateTime.Now.Date));
            cmd.Parameters.AddWithValue("@aTime",DateTime.Now.ToShortTimeString());
            cmd.Parameters.AddWithValue("@TableName",lbltable.Text);
            cmd.Parameters.AddWithValue("@WaiterName",lblwaiter.Text);
            cmd.Parameters.AddWithValue("@status","Pending");
            cmd.Parameters.AddWithValue("@orderType",OderType);
            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lbltotal.Text));// lưu dữ liệu cho giá trị nhà bếp sẽ cập nhật khi nhận được thanh toán
            cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@driverID", driverID);
            cmd.Parameters.AddWithValue("@CustName", customerName);
            cmd.Parameters.AddWithValue("@CustPhone", customerPhone);

            if(Mainclass.con.State == ConnectionState.Closed) { Mainclass.con.Open(); }
            if (MainID == 0) { MainID = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }
            if(Mainclass.con.State == ConnectionState.Open) { Mainclass.con.Close(); }

            foreach(DataGridViewRow row in guna2DataGridView1.Rows)
            {
                detailID = Convert.ToInt32(row.Cells["dgvid"].Value);
                if(detailID == 0) // Insert
                {
                    qry2 = @"Insert into tblDetails values(@MainID,@proID,@qty,@price,@amount)";
                }
                else //Update
                {
                    qry2 = @"Update tblDetails Set proID = @proID,qty = @qty,price = @price,amount = @amount
                                    where DetailID = @ID";
                }
                SqlCommand cmd2 = new SqlCommand(qry2, Mainclass.con);
                cmd2.Parameters.AddWithValue("@ID", detailID);
                cmd2.Parameters.AddWithValue("@MainID", MainID);
                cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvproID"].Value));
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmount"].Value));

                if (Mainclass.con.State == ConnectionState.Closed) { Mainclass.con.Open(); }
                cmd2.ExecuteNonQuery(); 
                if (Mainclass.con.State == ConnectionState.Open) { Mainclass.con.Close(); }
                
            }
            guna2MessageDialog1.Show("Save successfully..");
            MainID = 0;
            detailID = 0;
            guna2DataGridView1.Rows.Clear();
            lbltable.Text = "";
            lblwaiter.Text = "";
            lbltable.Visible = false;
            lblwaiter.Visible = false;
            lbltotal.Text = "00";
            lblDriverName.Text = "";

        }
        public int id = 0;
 

        private void btn_Bill_Click(object sender, EventArgs e)
        {
            BillList frm = new BillList();
            Mainclass.Bluebackgournd(frm);
            if(frm.MainID > 0)
            {
                id = frm.MainID;
                MainID = frm.MainID;
                LoadEntries();
            }
        }

        private void LoadEntries()
        {
            string qry = @"Select * from tblMain m
                               inner join tblDetails d on m.MainID = d.MainID
                                inner join products p on p.pID = d.proID
                                    Where m.MainID = " + id + "";
            SqlCommand cmd2 = new SqlCommand(qry, Mainclass.con);
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);

            if (dt2.Rows[0]["orderType"].ToString() == "Delivery")
            {
                btn_Delivery.Checked = true;
                lblwaiter.Visible = false;
                lbltable.Visible = false;

            }
            else if(dt2.Rows[0]["orderType"].ToString() == "Take away")
            {
                btn_takeaway.Checked = true;
                lblwaiter.Visible = false;
                lbltable.Visible = false;
            }
            else
            {
                btn_dinin.Checked = true;
                lblwaiter.Visible = true;
                lbltable.Visible = true;
            }
            guna2DataGridView1.Rows.Clear();
            foreach (DataRow item in dt2.Rows)
            {
                lbltable.Text = item["TableName"].ToString();
                lblwaiter.Text = item["WaiterName"].ToString();


                string detailid = item["DetailID"].ToString();
                string proName = item["pName"].ToString();
                string proid = item["proID"].ToString();
                string qty = item["qty"].ToString();
                string price = item["price"].ToString();
                string amount = item["amount"].ToString();

                object[] obj = { 0, detailid, proid,proName, qty, price, amount };
                guna2DataGridView1.Rows.Add(obj);
            }
            getTotal();
        }

        private void btn_checkout_Click(object sender, EventArgs e)
        {
            CheckOut frm = new CheckOut();
            frm.MainID = id;
            frm.amt = Convert.ToDouble(lbltotal.Text);

            Mainclass.Bluebackgournd(frm);

            MainID = 0;
            guna2DataGridView1.Rows.Clear();
            lbltable.Text = "";
            lblwaiter.Text = "";
            lbltable.Visible = false;
            lblwaiter.Visible = false;
            lbltotal.Text = "00";
        }

        private void btn_Hold_Click(object sender, EventArgs e)
        {
            string qry1 = ""; //Main table
            string qry2 = ""; //Detail table

            int detailID = 0;
            if(OderType == "")
            {
                guna2MessageDialog1.Show("Please select order type");
                return;
            }
            if (MainID == 0) // Insert
            {
                qry1 = @"Insert into tblMain values(@aDate,@aTime,@TableName,@WaiterName,
                    @status,@orderType,@total,@received,@change,@driverID,@CustName,@CustPhone);
                        Select SCOPE_IDENTITY()";
                //nhận giá trị id thêm gần đây
            }
            else //update
            {
                qry1 = @"Update tblMain Set status= @status,total = @total,
                        received= @received,change = @change where MainID =@ID";
            }

            SqlCommand cmd = new SqlCommand(qry1, Mainclass.con);
            cmd.Parameters.AddWithValue("@ID", MainID);
            cmd.Parameters.AddWithValue("@aDate", Convert.ToDateTime(DateTime.Now.Date));
            cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
            cmd.Parameters.AddWithValue("@TableName", lbltable.Text);
            cmd.Parameters.AddWithValue("@WaiterName", lblwaiter.Text);
            cmd.Parameters.AddWithValue("@status", "Hold");
            cmd.Parameters.AddWithValue("@orderType", OderType);
            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lbltotal.Text));// lưu dữ liệu cho giá trị nhà bếp sẽ cập nhật khi nhận được thanh toán
            cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@driverID", driverID);
            cmd.Parameters.AddWithValue("@CustName", customerName);
            cmd.Parameters.AddWithValue("@CustPhone", customerPhone);

            if (Mainclass.con.State == ConnectionState.Closed) { Mainclass.con.Open(); }
            if (MainID == 0) { MainID = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }
            if (Mainclass.con.State == ConnectionState.Open) { Mainclass.con.Close(); }

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                detailID = Convert.ToInt32(row.Cells["dgvid"].Value);
                if (detailID == 0) // Insert
                {
                    qry2 = @"Insert into tblDetails values(@MainID,@proID,@qty,@price,@amount)";
                }
                else //Update
                {
                    qry2 = @"Update tblDetails Set proID = @proID,qty = @qty,price = @price,amount = @amount
                                    where DetailID = @ID";
                }
                SqlCommand cmd2 = new SqlCommand(qry2, Mainclass.con);
                cmd2.Parameters.AddWithValue("@ID", detailID);
                cmd2.Parameters.AddWithValue("@MainID", MainID);
                cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvproID"].Value));
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmount"].Value));

                if (Mainclass.con.State == ConnectionState.Closed) { Mainclass.con.Open(); }
                cmd2.ExecuteNonQuery();
                if (Mainclass.con.State == ConnectionState.Open) { Mainclass.con.Close(); }

            }
            guna2MessageDialog1.Show("Save successfully..");
            MainID = 0;
            detailID = 0;
            guna2DataGridView1.Rows.Clear();
            lbltable.Text = "";
            lblwaiter.Text = "";
            lbltable.Visible = false;
            lblwaiter.Visible = false;
            lbltotal.Text = "00";
            lblDriverName.Text = "";
        }
    }
}
