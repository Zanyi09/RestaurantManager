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

namespace Restaurant_app.Model
{
    public partial class CategoryAdd : SampleAdd
    {
        public CategoryAdd()
        {
            InitializeComponent();
        }
        public int id = 0;
        private void btn_close_Click_1(object sender, EventArgs e)
        {

        }
        public override void btn_save_Click(object sender, EventArgs e)
        {
            string qry = "";
            if (id == 0) // Insert
            {
                qry = "Insert into category values(@Name)";
            }
            else // Update
            {
                qry = "Update category Set cateName = @Name where CateID = @id";
            }
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txt_Name.Text);
            if (Mainclass.SQL(qry, ht) > 0)
            {
                guna2MessageDialog1.Show("Save sucessfull...");
                id = 0;
                txt_Name.Text = "";
                txt_Name.Focus();
            }
        }
    }
}
