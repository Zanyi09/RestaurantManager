using Restaurant_app;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_Manager
{
    class Mainclass
    {
        public static readonly string strconn = "database=Restaurant;Server=DESKTOP-8QKTB1I\\SQLEXPRESS; User id=sa; " +
            "password=sa123123";
        public static SqlConnection con = new SqlConnection(strconn);

        // Check người dùng có tồn tại.
        public static bool IsValidUser(string user, string pass)
        {
            bool isValid = false;

            string qry = @"Select * from users where username ='" + user + "' and upass ='" + pass + "' ";
            SqlCommand cmd = new SqlCommand(qry, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                isValid = true;
                USER = dt.Rows[0]["uName"].ToString();
            }
            return isValid;
        }
        //tạo thuộc tính cho tên người dùng
        public static string user;
        public static string USER
        {
            get { return user; }
            private set { user = value; }
        }
        // Phương thức Curd(tạo ,đọc, sữa xóa)
        public static int SQL(string qry, Hashtable ht)
        {
            int res =0;
            try
            {
                SqlCommand cmd = new SqlCommand(qry,con);
                cmd.CommandType = CommandType.Text;

                foreach(DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                }
                if (con.State == ConnectionState.Closed) { con.Open(); }
                res = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open) { con.Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }
            return res;
        }
        //Tải dữ liệu từ Database

        public static void LoadData(string qry, DataGridView gv, ListBox lb)
        {
            // Sr# (seri) trong gridview
            gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);
            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i =0; i < lb.Items.Count; i++)
                {
                    string ColName1 = ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[ColName1].DataPropertyName = dt.Columns[i].ToString();
                }

                gv.DataSource= dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }
        }

        private static void gv_CellFormatting(object sender,DataGridViewCellFormattingEventArgs e)
        {
            Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
            int count = 0;

            foreach (DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }
        // Thêm hiệu ứng backgound cho các add, save
        public static void Bluebackgournd(Form Model)
        {
            Form Backgound = new Form();
            using (Model)
            {
                Backgound.StartPosition = FormStartPosition.Manual;
                Backgound.FormBorderStyle = FormBorderStyle.None;
                Backgound.Opacity = 0.5d;
                Backgound.BackColor = Color.Black;
                Backgound.Size = Main.Instance.Size;
                Backgound.Location = Main.Instance.Location;
                Backgound.ShowInTaskbar = false;
                Backgound.Show();
                Model.Owner = Backgound;
                Model.ShowDialog(Backgound);
                Backgound.Dispose();
            }
        }
        // Combobox products category
        public static void CBFill(string qry,ComboBox cb)
        {
            SqlCommand cmd = new SqlCommand(qry,con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cb.DisplayMember = "name";
            cb.ValueMember= "id";
            cb.DataSource = dt;
            cb.SelectedIndex = -1;
        }
    }
}
