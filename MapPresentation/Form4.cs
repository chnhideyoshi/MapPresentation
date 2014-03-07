using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MapAlgorithm3;
using MapAlgorithm2;

namespace MapPresentation
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = "DiamondBlue.ssk";
        }
        //const int AW_HOR_POSITIVE = 0x0001;
        //const int AW_HOR_NEGATIVE = 0x0002;
        //const int AW_VER_POSITIVE = 0X0004;
            //const int AW_VER_NEGATIVE = 0x0008;
       // [System.Runtime.InteropServices.DllImport("user32")]private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        private void Form4_Load(object sender, EventArgs e)
        {
             init();

           //AnimateWindow(this.Handle, 2000, AW_HOR_POSITIVE | AW_VER_POSITIVE);
        }
        private Graphics g;
        public int maxsize = 20;
        public int sizetemp = 0;
        public int delta = 16;
        trMap map = new trMap();
        //private int jieduan = 1;
        Stack<Bitmap> nodestack = new Stack<Bitmap>();
        private void init()
        {

            comboBox1.Enabled = true;
            button1.Enabled = true;
            button2.Enabled =false;
            button3.Enabled = false;
            button5.Enabled = false;
            panel1.Enabled = false;
            maxsize = 0;
            g = panel1.CreateGraphics();
            sizetemp = 0;
            map = new trMap();
            nodestack.Clear();
            nodestack.Push(new Bitmap(panel1.BackgroundImage));
            g.DrawImage(nodestack.Peek(), new Point(0, 0));
            richTextBox1.Text = "Welcome,you can solve the problem of convex hull here,please choose the number of the points\n";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                button1.Enabled = false;
                button2.Enabled = true;
                panel1.Enabled = true;
                maxsize = int.Parse(comboBox1.Text);
                comboBox1.Enabled = false;
                map.numofnode = maxsize;
                richTextBox1.Text = "OK and you can click the panel to locate your points\n" + richTextBox1.Text;


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sizetemp > 0)
            {
                Point toerase = new Point(map.listofnode[sizetemp - 1].position.X, map.listofnode[sizetemp - 1].position.Y);
                nodestack.Pop();
                g.DrawImage(nodestack.Peek(), new Point(0, 0));
                //map.listofnode[sizetemp - 1] = null;
                richTextBox1.Text = "The Point " + sizetemp + " has been deleted!\n" + richTextBox1.Text;
                map.listofnode.RemoveAt(sizetemp - 1);
                sizetemp--;

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
             g = panel1.CreateGraphics();
             g.DrawImage(nodestack.Peek(), new Point(0, 0));
        }
        private void playsound(int i)
        {
            string dname = "1.wav";
            switch (i)
            {
                case 1: break;
                case 2: dname = "2.wav"; break;
                case 3: dname = "3.wav"; break;
            }
            FileStream fsf = new FileStream(dname, FileMode.Open);
            System.Media.SoundPlayer sp = new System.Media.SoundPlayer(fsf);
            sp.Play();
            fsf.Close();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (sizetemp != maxsize)
            {
                Point p = new Point(e.X, e.Y);
                playsound(1);
                map.listofnode.Add(new node(p, sizetemp));

                drawnode(map.listofnode[sizetemp]);
                //jieduan = 1;


                richTextBox1.Text = "new point added! Postion: " + e.X + "," + e.Y + "\n" + richTextBox1.Text;
                sizetemp++;
                if (sizetemp == maxsize)
                {
                    button3.Enabled = true;
                    button5.Enabled = true;
                    button2.Enabled = false;

                    //panel1.Enabled = true;
                    richTextBox1.Text = "All points OK and you can choose to see the result or the process\n" + richTextBox1.Text;
                    
 
                }
            }
        }
        private void drawnode(node no)
        {
            Point p = new Point(no.position.X - delta, no.position.Y - delta);
            Bitmap pc = new Bitmap(nodestack.Peek());
            g = Graphics.FromImage(pc);
            SolidBrush b = new SolidBrush(Color.Blue);
            g.DrawImage(imageList1.Images[0], p);
            Font f = new Font(FontFamily.Families[6], 14);
            g.DrawString(no.messsage, f, b, new Point(p.X, p.Y + 30));
            g = panel1.CreateGraphics();
            g.DrawImage(pc, new Point(0, 0));
            nodestack.Push(pc);
        }
        private void drawnodewithnostack(Point no, Graphics g)
        {
            Point p = new Point(no.X - delta, no.Y - delta);
            SolidBrush b = new SolidBrush(Color.Blue);
            g.DrawImage(imageList1.Images[0], p);
            //Font f = new Font(FontFamily.Families[6], 14);
            //g.DrawString(no.messsage, f, b, new Point(p.X, p.Y + 30));
        }
        private void drawline(Point from, Point  to)
        {
            //Point from = map.listofnode[p1].position;
            //Point to = map.listofnode[p2].position;
            Bitmap pc = new Bitmap(nodestack.Peek());
            g = Graphics.FromImage(pc);
            Pen p = new Pen(Color.GreenYellow, 4);
            g.DrawLine(p, from, to);
            drawnodewithnostack(from, g);
            drawnodewithnostack(to, g);
            //SolidBrush b = new SolidBrush(Color.Brown);
            //Point mid = new Point((from.X + to.X) / 2, (from.Y + to.Y) / 2);
            //Font f = new Font(FontFamily.Families[6], 20);
           // g.DrawString(textBox1.Text, f, b, mid);
            g = panel1.CreateGraphics();
            g.DrawImage(pc, new Point(0, 0));
            nodestack.Push(pc);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Point[] s = new Point[32];
           // Point[] cs = new Point[32];
            button5.Enabled = false;
            for (int i = 0; i < maxsize; i++)
            {
                //map.s[i] = new Point(map.listofnode[i].position.X, map.listofnode[i].position.Y);
                map.p[i] = new pa(map.listofnode[i].position);
                //map.cs[i] = new Point(0, 0);
                map.ch[i] = new pa(0, 0);
            }
           map.f(maxsize);
            for (int i = 0; i < map.top; i++)
            {
                Point from = new Point(map.ch[i].x, map.ch[i].y);
                Point to = new Point(map.ch[i+1].x,map.ch[i+1].y);
                drawline(from,to);
            }
            drawline(new Point(map.ch[map.top ].x, map.ch[map.top ].y), new Point(map.ch[0].x, map.ch[0].y));
            richTextBox1.Text = "The algorithm has done!\n" + richTextBox1.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button3.Enabled == true)
            {
                for (int i = 0; i < maxsize; i++)
                {
                    //map.s[i] = new Point(map.listofnode[i].position.X, map.listofnode[i].position.Y);
                    map.p[i] = new pa(map.listofnode[i].position);
                    //map.cs[i] = new Point(0, 0);
                    map.ch[i] = new pa(0, 0);
                }
                map.f(maxsize);
            }
            richTextBox1.Text = "one step on,and the trail of the edge has been left!\n" + richTextBox1.Text;
            if (map.recorderof_conhull.Count > 0)
            {
                button3.Enabled = false;
                
                List<pa> temp = map.recorderof_conhull.Dequeue();
                for (int i = 0; i < temp.Count-1; i++)
                {
                    drawline(new Point(temp[i].x, temp[i].y), new Point(temp[i + 1].x, temp[i + 1].y));
                }                          
            }
            else
            {
                drawline(new Point(map.ch[map.top].x, map.ch[map.top].y), new Point(map.ch[0].x, map.ch[0].y));
                MessageBox.Show("all over!");


            }

        }
    }
}
