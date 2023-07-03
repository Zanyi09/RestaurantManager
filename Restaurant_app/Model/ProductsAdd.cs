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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_app.Model
{
    public partial class ProductsAdd : SampleAdd
    {
        public ProductsAdd()
        {
            InitializeComponent();
        }

        public int id = 0;
        public int cID = 0;
        private void ProductsAdd_Load(object sender, EventArgs e)
        {
            // Điền combobox Category products
            string qry = "Select cateID 'id', cateName 'name' from category ";
            Mainclass.CBFill(qry, cb_Cate);
            if (cID > 0)// Update
            {
                cb_Cate.SelectedValue= cID;
            }
            if (id > 0)
            {
                ForUpdateLoadData();
            }
        }
        // Thêm ảnh products
        string filepath;
        Byte[] imageByteArray;
        private void btn_Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images(.jpg,.png)|* .png; *.jpg;";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filepath = ofd.FileName;
                txt_Image.Image = new Bitmap(filepath);
            }
        }
        public override void btn_save_Click(object sender, EventArgs e)
        {
            string qry = "";
            if (id == 0) // Insert
            {
                qry = "Insert into products values(@Name,@price,@cate,@img)";
            }
            else // Update
            {
                qry = "Update products Set pName = @Name, pPrice =@price,CategoryID =@cate,pImage = @img where pID = @id";
            }
            //Image
            Image temp = new Bitmap(txt_Image.Image);
            MemoryStream ms = new MemoryStream();
            temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            imageByteArray = ms.ToArray();


            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txt_Name.Text);
            ht.Add("@price", txt_Price.Text);
            ht.Add("@cate", Convert.ToInt32(cb_Cate.SelectedValue));
            ht.Add("@img", imageByteArray);
            if (Mainclass.SQL(qry, ht) > 0)
            {
                guna2MessageDialog1.Show("Save sucessfull...");
                id = 0;
                cID = 0;
                txt_Name.Text = "";
                txt_Price.Text = "";
                cb_Cate.SelectedIndex = 0;
                txt_Image.Image = Restaurant_app.Properties.Resources.no_photo;
                txt_Name.Focus();
            }
        }
        private void ForUpdateLoadData()
        {
            string qry = @"Select * from products where pID =" + id + "";
            SqlCommand cmd = new SqlCommand(qry, Mainclass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                txt_Name.Text = dt.Rows[0]["pName"].ToString();
                txt_Price.Text = dt.Rows[0]["pPrice"].ToString();

                Byte[] imageArray = (Byte[])(dt.Rows[0])["pImage"];
                Byte[] imageByteArray = imageArray;
                txt_Image.Image = Image.FromStream(new MemoryStream(imageArray));
            }
        }
    }
}
