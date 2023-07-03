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
    public partial class CheckOut : SampleAdd
    {
        public CheckOut()
        {
            InitializeComponent();
        }
        public double amt;

        public int MainID = 0;

        private void txt_received_TextChanged(object sender, EventArgs e)
        {
            double amt = 0;
            double receipt = 0;
            double change = 0;

            double.TryParse(txt_billamount.Text, out amt);
            double.TryParse(txt_received.Text, out receipt);

            change = Math.Abs(amt - receipt);
            txt_change.Text = change.ToString();
        }

        public override void btn_save_Click(object sender, EventArgs e)
        {
            string qry = @"Update tblMain set total =@total, received = @rec, change =@change,
                            status ='Paid' where MainID = @id";

            Hashtable ht = new Hashtable();
            ht.Add("@id",MainID);
            ht.Add("@total",txt_billamount.Text);
            ht.Add("@rec", txt_received.Text);
            ht.Add("@change", txt_change.Text);
            if(Mainclass.SQL(qry,ht) > 0)
            {
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Saved Successfull");
                this.Close();
            }
        }

        private void CheckOut_Load(object sender, EventArgs e)
        {
            txt_billamount.Text = amt.ToString();
        }
    }
}
 