

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class SimSelect : Form
    {
        public SimSelect()
        {
            InitializeComponent();
        }

        private void lab1_Click(object sender, EventArgs e)
        {
            this.Hide();

            lab445 lab445 = new lab445();

            lab445.ShowDialog();
        }

            

        private void lab2_Click(object sender, EventArgs e)
        {
            this.Hide();

            lab452 lab452 = new lab452();

            lab452.ShowDialog();

        }

        private void lab3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Lab430 lab430 = new Lab430();
            lab430.ShowDialog();
        }

        private void lab4_Click(object sender, EventArgs e)
        {
            this.Hide();
            lab449 lab449 = new lab449();
            lab449.ShowDialog();
        }
    

        private void lab5_Click(object sender, EventArgs e)
        {
        this.Hide();
        lab455 lab455 = new lab455();
        lab455.ShowDialog();
    }
    }
}
