using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public event RadiusChanged RC;
        public static int r = 20;
        private void Form2_Load(object sender, EventArgs e)
        {
            trackBar1.Maximum = 50;
            trackBar1.Minimum = 0;
            trackBar1.Value = r;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            RadiusEventArgs radius = new RadiusEventArgs(trackBar1.Value);
            RC(this, radius);
            r = radius.Radius;
        }

        
    }
    public class OpenEventArgs : EventArgs
    {
        public bool open;
        public OpenEventArgs(bool open)
        {
            this.open = open;
        }
    }
    public class RadiusEventArgs : EventArgs
    {
        public int Radius;
        public RadiusEventArgs(int Radius)
        {
            this.Radius = Radius;
        }
    }
    public delegate void RadiusChanged(object sender, RadiusEventArgs e);
}
