using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_app
{
   

    public partial class UcProduct : UserControl
    {
        public UcProduct()
        {
            InitializeComponent();
        }
        public event EventHandler onSelect = null;
        public int id { set; get; }
        public string PPrice { set; get; }
        public string pCategory { set; get; }
        public string PName 
        { 
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }
        public Image PImage
        {
            get { return txt_Image.Image; }
            set { txt_Image.Image = value; }
        }

        private void txt_Image_Click(object sender, EventArgs e)
        {
            onSelect?.Invoke(this, e);
        }
    }
}
