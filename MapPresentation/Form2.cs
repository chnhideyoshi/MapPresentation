using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MapPresentation
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = "vista1.ssk";
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //pictureBox1.Image = new Bitmap("PIC/SHOW.jpg");
        }

 
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            new Form3().ShowDialog();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            new Form4().ShowDialog();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            new Form1().ShowDialog();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            new Form5().ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            new Form6().ShowDialog();
        }
    }
}
