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

namespace Restaurant_app.Model
{
    public partial class AddCustomer : Form
    {
        public AddCustomer()
        {
            InitializeComponent();
        }
        public string orderType = "";
        public int driverID = 0;
        public string cusName = "";
        public int mainID = 0;

        private void AddCustomer_Load(object sender, EventArgs e)
        {
            if(orderType == "Take Away")
            {
                lbl_driver.Visible = false;
                cb_Driver.Visible = false;
            }
            string qry = "Select staffID 'id',sName 'name' from staff where sRole like 'Driver'";
            Mainclass.CBFill(qry,cb_Driver);

            if(mainID > 0)
            {
                cb_Driver.SelectedValue = driverID;
            }
        }

        private void cb_Driver_SelectedIndexChanged(object sender, EventArgs e)
        {
            driverID = Convert.ToInt32(cb_Driver.SelectedValue);
        }
    }
}
