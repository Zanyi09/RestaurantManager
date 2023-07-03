﻿using Restaurant_Manager;
using System;
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
    public partial class WaiterSelect : Form
    {
        public WaiterSelect()
        {
            InitializeComponent();
        }
        public string WaiterName;
        private void WaiterSelect_Load(object sender, EventArgs e)
        {
            string qry = "Select * from staff where sRole like 'Waiter' ";
            SqlCommand cmd = new SqlCommand(qry, Mainclass.con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.Text = row["sName"].ToString();
                b.Width = 150;
                b.Height = 50;
                b.FillColor = Color.FromArgb(241, 85, 126);
                b.HoverState.FillColor = Color.Orange;

                //event for click
                b.Click += new EventHandler(_Click);
                flowLayoutPanel1.Controls.Add(b);
            }
        }
        private void _Click(object sender, EventArgs e)
        {
            WaiterName = (sender as Guna.UI2.WinForms.Guna2Button).Text.ToString();
            this.Close();
        }
    }
}
