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
    public partial class StaffAdd : SampleAdd
    {
        public StaffAdd()
        {
            InitializeComponent();
        }
        public int id = 0;

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void StaffAdd_Load(object sender, EventArgs e)
        {

        }
        public override void btn_save_Click(object sender, EventArgs e)
        {
            string qry = "";
            if (id == 0) // Insert
            {
                qry = "Insert into Staff values(@Name,@phone, @role)";
            }
            else // Update
            {
                qry = "Update Staff Set sName = @Name, sPhone =@phone,sRole =@role where staffID = @id";
            }
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txt_Name.Text);
            ht.Add("@phone", txt_Phone.Text);
            ht.Add("@role", cb_Role.Text);
            if (Mainclass.SQL(qry, ht) > 0)
            {
                guna2MessageDialog1.Show("Save sucessfull...");
                id = 0;
                txt_Name.Text = "";
                txt_Phone.Text = "";
                cb_Role.SelectedIndex = -1;
                txt_Name.Focus();
            }
        }
    }
}
