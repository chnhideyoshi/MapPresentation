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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = "DiamondGreen.ssk";
        }
       // const int AW_HOR_POSITIVE = 0x0001;
        //const int AW_HOR_NEGATIVE = 0x0002;
        //const int AW_VER_POSITIVE = 0X0004;
        //const int AW_VER_NEGATIVE = 0x0008;
        //[System.Runtime.InteropServices.DllImport("user32")]
        //private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        public trMap map = new trMap();
        private Graphics g;
        private int maxsize = 20;
        private int sizetemp = 0;
        private int delta = 16;
        private int sizeedge = 0;

        public int jieduan = 1;
        Stack<Bitmap> nodestack = new Stack<Bitmap>();
        Stack<Bitmap> edgestack = new Stack<Bitmap>();

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            g = panel1.CreateGraphics();
            switch (jieduan)
            {
                case 1:
                    {
                        g.DrawImage(nodestack.Peek(), new Point(0, 0));
                    }
                    break;
                case 2:
                    {
                        g.DrawImage(edgestack.Peek(), new Point(0, 0));
                    }
                    break;
                case 3:
                    {
                        //g.DrawImage(woca.Peek(), new Point(0, 0));
                    }
                    break;
                default:
                    {

                    }
                    break;
            }
        }
        private void init()
        {
            comboBox1.Enabled = true;

            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            panel1.Enabled = false;
            comboBox2.Enabled = false;       
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;

            radioButton1.Enabled = false;
            radioButton2.Enabled = false;

            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button5.Enabled = false;

            textBox1.Enabled = false;
            

            g=panel1.CreateGraphics();
            maxsize = 0;
            sizetemp = 0;
            //delta = 16;
            sizeedge = 0;
            jieduan = 1;
            cho = 0;
            richTextBox1.Text = "Wellcome to the Presentation of Map's travel!\n";
            map = new trMap();
            nodestack.Clear();
            nodestack.Push(new Bitmap(panel1.BackgroundImage));
            edgestack.Clear();
            g.DrawImage(nodestack.Peek(), new Point(0, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>

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
        private void drawnodewithnostack(node no, Graphics g, int ch)
        {
            Point p = new Point(no.position.X - delta, no.position.Y - delta);
            if (ch == 1)
            {
                p = new Point(no.position.X - delta+1, no.position.Y - delta+1);
            }
            SolidBrush b = new SolidBrush(Color.Blue);
            g.DrawImage(imageList1.Images[ch], p);
            Font f = new Font(FontFamily.Families[6], 14);
            g.DrawString(no.messsage, f, b, new Point(p.X, p.Y + 30));
        }

        private void drawlinetree(edge e, Color c)
        {
            Pen p = new Pen(c, 7);
            g.DrawLine(p, map.listofnode[e.fromvex].position, map.listofnode[e.tovex].position);
        }

        private void drawline(int p1, int p2)
        {
            Point from = map.listofnode[p1].position;
            Point to = map.listofnode[p2].position;
            Bitmap pc = new Bitmap(edgestack.Peek());
            g = Graphics.FromImage(pc);
            Pen p = new Pen(Color.GreenYellow, 4);
            g.DrawLine(p, from, to);
            drawnodewithnostack(map.listofnode[p1], g, 0);
            drawnodewithnostack(map.listofnode[p2], g, 0);
            SolidBrush b = new SolidBrush(Color.Brown);
            //Point mid = new Point((from.X + to.X) / 2, (from.Y + to.Y) / 2);
            //Font f = new Font(FontFamily.Families[6], 20);
            //g.DrawString(textBox1.Text, f, b, mid);
            g = panel1.CreateGraphics();
            g.DrawImage(pc, new Point(0, 0));
            edgestack.Push(pc);
        }
   

        private void Form5_Load(object sender, EventArgs e)
        {
            init();
           // AnimateWindow(this.Handle, 2000, AW_HOR_POSITIVE | AW_VER_POSITIVE);
            timer1.Enabled = true;
            this.Opacity = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                maxsize = int.Parse(comboBox1.Text);
                map.numofnode = maxsize;

                panel1.Enabled = true;
                button1.Enabled = false;
                button5.Enabled = true;
                comboBox1.Enabled = false;

                for (int i = 1; i <= maxsize; i++)
                {
                    comboBox2.Items.Add(i.ToString());
                    comboBox3.Items.Add(i.ToString());
                    comboBox4.Items.Add(i.ToString());
                }

                richTextBox1.Text = "Please click the panel left to locate those points.\n"+richTextBox1.Text;
                /*
                radioButton2.Enabled = true;
                radioButton1.Enabled = true;

                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;*/


            }
            else
            {
                MessageBox.Show("please choose the number of the points!");
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (sizetemp != maxsize)
            {
                Point p = new Point(e.X, e.Y);
                playsound(1);
                map.listofnode.Add(new node(p, sizetemp));
                drawnode(map.listofnode[sizetemp]);
                jieduan = 1;
                richTextBox1.Text = "new point added! Position: " + e.X + "," + e.Y + "\n" + richTextBox1.Text;
                sizetemp++;
                if (sizetemp == maxsize)
                {
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button2.Enabled = true;
                    button5.Enabled = false;
                    textBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    radioButton1.Enabled = true;
                    radioButton2.Enabled = true;
                    edgestack.Push(nodestack.Peek());
                    richTextBox1.Text = "All points are ready,then you can add the edges!\n" + richTextBox1.Text;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text != "" && comboBox2.Text != "")
            {
                int p1 = int.Parse(comboBox3.Text) - 1;
                int p2 = int.Parse(comboBox2.Text) - 1;
                int x1 = map.listofnode[p1].position.X;
                int y1 = map.listofnode[p1].position.Y;
                int x2 = map.listofnode[p2].position.X;
                int y2 = map.listofnode[p2].position.Y;
                int distence = (int)System.Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
                textBox1.Text = distence.ToString();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text != "" && comboBox2.Text != "")
            {
                int p1 = int.Parse(comboBox3.Text) - 1;
                int p2 = int.Parse(comboBox2.Text) - 1;
                int x1 = map.listofnode[p1].position.X;
                int y1 = map.listofnode[p1].position.Y;
                int x2 = map.listofnode[p2].position.X;
                int y2 = map.listofnode[p2].position.Y;
                int distence = (int)System.Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
                textBox1.Text = distence.ToString();
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (sizeedge >= maxsize * (maxsize - 1))
            {
                MessageBox.Show("Too many edges!");
                return;
            }
            if (comboBox3.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("please choose the two points");
                return;
            }
            int p1 = int.Parse(comboBox3.Text) - 1;
            int p2 = int.Parse(comboBox2.Text) - 1;
            for (int i = 0; i < sizeedge; i++)
            {
                if (map.listofedge[i].fromvex == p1 && map.listofedge[i].tovex == p2)
                {
                    MessageBox.Show("Alreay got the edge!"); return;
                }
                if (map.listofedge[i].fromvex == p2 && map.listofedge[i].tovex == p1)
                {
                    MessageBox.Show("Alreay got the edge!"); return;
                }
            }
            if (p1 != p2)
            {
                playsound(2);
                drawline(p1, p2);
                jieduan = 2;
                int w = int.Parse(textBox1.Text);
                map.listofedge.Add(new edge(p1, p2, w));
                sizeedge++;
                map.numofedge = sizeedge;
                map.matri[p1, p2] = w; map.matri[p2, p1] = w;
                richTextBox1.Text = "new edge added! from " + comboBox3.Text + " to " + comboBox4.Text + "\n" + richTextBox1.Text;
            }
            else
            {
                MessageBox.Show("They are the same vertex!");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (edgestack.Count > 1)
            {
                edgestack.Pop();
                g.DrawImage(edgestack.Peek(), new Point(0, 0));
                richTextBox1.Text = "Remove the edge from Point " + (map.listofedge[sizeedge - 1].fromvex + 1) + " to Point" + (map.listofedge[sizeedge - 1].tovex + 1) + "\n" + richTextBox1.Text;
                //map.arryofedge[sizeedge - 1] = null;
                map.listofedge.RemoveAt(sizeedge - 1);

                sizeedge--;
            }
        }
        private int cho=0;
        private void button2_Click(object sender, EventArgs e)
        {
            map.numofnode = maxsize;
            map.numofedge = sizeedge;
            
            if (comboBox4.Text != ""&&(radioButton1.Checked==true||radioButton2.Checked==true))
            {
                map.start = int.Parse(comboBox4.Text) - 1;
            }
            else
            {
                MessageBox.Show("you must choose the start point and the travel algorithm!");
                return;
            }
            
            button8.Enabled = false;
            button9.Enabled = false;
            button3.Enabled = true;
            button2.Enabled = false;

            textBox1.Enabled = false;

            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;

            if (radioButton1.Checked == true)
            {
                map.DFS_travel(map.start);
                
            }
            else
            {
                map.BFS_travel(map.start);
                cho=1;
            }
            richTextBox1.Text = "The Algorithm has done !  You can click the button blow to see the process!\n" + richTextBox1.Text;
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            if (cho == 0)
            {
                if (map.recorderof_DFS.Count > 0)
                {
                    int s = map.recorderof_DFS.Dequeue();
                    drawnodewithnostack(map.listofnode[s], g, 1);
                }
            }
            else
            {
                if (map.recorderof_BFS.Count > 0)
                {
                    int s = map.recorderof_BFS.Dequeue();
                    drawnodewithnostack(map.listofnode[s], g, 1);
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            init();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity = this.Opacity + 0.05;
            }
            else
            {
                timer1.Enabled = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (sizetemp > 0)
            {
                Point toerase = new Point(map.listofnode[sizetemp - 1].position.X, map.listofnode[sizetemp - 1].position.Y);
                nodestack.Pop();
                g.DrawImage(nodestack.Peek(), new Point(0, 0));
                //map.listofnode[sizetemp - 1] = null;
                richTextBox1.Text = "The Point " + sizetemp + " has been deleted!" + richTextBox1.Text;
                map.listofnode.RemoveAt(sizetemp - 1);
                sizetemp--;

            }
        }

        
    }
}
