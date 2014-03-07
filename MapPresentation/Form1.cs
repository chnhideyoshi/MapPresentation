using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MapAlgorithm;


namespace MapPresentation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //this.MdiParent = new MDIParent1();
            this.skinEngine1.SkinFile = "DiamondGreen.ssk";
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_CENTER = 0x0010;
        const int AW_ACTIVATE = 0x20000;
        /// <summary>
        /// 
        /// </summary>
        private Graphics g;
        private int jieduan=1;
        public int maxsize=10;
        public int sizetemp=0;
        public int delta = 16;
        public int sizeedge = 0;
        Map map = new Map();
        bool success = false;
        /// <summary>
        /// stack and queue
        /// </summary>
        /// 
        Stack<Bitmap> nodestack=new Stack<Bitmap>();
        Stack<Bitmap> edgestack=new Stack<Bitmap>();
        

        ///
        private void init()
        {
            sizeedge = 0;
            sizetemp = 0;
            jieduan = 1;
            button1.Enabled = true;button2.Enabled = false;button3.Enabled = false;button4.Enabled = false;
            button5.Enabled = false; button6.Enabled = false; button8.Enabled = false;
            button9.Enabled = false;
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox1.Enabled = true;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            panel1.Enabled = false;
            textBox1.Enabled = false;
            for (int i = 0; i < 31; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    map.matri[i, j] = int.MaxValue;
                }
            }
            g = panel1.CreateGraphics();
            nodestack.Clear();
            nodestack.Push(new Bitmap(panel1.BackgroundImage));

            edgestack.Clear();
            map.primQ.Clear();
            g.DrawImage(nodestack.Peek(), new Point(0, 0));
            say(1);
        }
        private void say(int i)
        {
            switch (i)
            {
                case 1:
                    richTextBox1.Text = "这个框是提示框!\n 现在请设定顶点数.";
                    break;
                case 2:
                    richTextBox1.Text = "点击右边的图片定位点";
                    break;
                case 3:
                    richTextBox1.Text = "请设定边属性.你可以修改权值也可以使用默认的图示距离为权值";
                    break;
                case 4:
                    richTextBox1.Text = " Prim算法完成! 你可以看结果了";
                    break;
                case 5:
                    richTextBox1.Text = "Kruskal 算法完成! 你可以看结果了";
                    break;
                case 6:
                    richTextBox1.Text = "粉红是候选边.\n红色是已确定为树边的边.\n";
                    break;
                case 7:
                    richTextBox1.Text = " 红色是已确定为树边的边.\n";
                    break;

                default:
                    break;
            }     
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            init();
            
            AnimateWindow(this.Handle, 1000, AW_CENTER | AW_ACTIVATE);
            delta = imageList1.ImageSize.Height / 2;
                    
        }
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (sizetemp != maxsize)
            {
                Point p = new Point(e.X, e.Y);
                playsound(1);
                map.arryofnode[sizetemp] = new node(p, sizetemp);
                drawnode(map.arryofnode[sizetemp]);
                sizetemp++;
                if (sizetemp == maxsize)
                {
                    button3.Enabled = false;
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;                   
                    button8.Enabled = true;
                    button9.Enabled = true;                 
                    button2.Enabled = true;                    
                    textBox1.Enabled = true;
                    edgestack.Push(nodestack.Peek());
                    say(3);
                }
            }          
        }
        ///
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
        private void drawnode(node no)
        {
            Point p=new Point(no.position.X-delta,no.position.Y-delta);
            Bitmap pc = new Bitmap(nodestack.Peek());
            g=Graphics.FromImage(pc);
            SolidBrush b=new SolidBrush(Color.Blue);
            g.DrawImage(imageList1.Images[0], p);
            Font f=new Font(FontFamily.Families[6],14);
            g.DrawString(no.messsage,f, b, new Point(p.X,p.Y+30));
            g = panel1.CreateGraphics();
            g.DrawImage(pc,new Point(0,0));
            nodestack.Push(pc);
        }
        private void drawnodewithnostack(node no,Graphics g,int ch)
        {
            Point p = new Point(no.position.X - delta, no.position.Y - delta);
            if (ch == 1)
            {
                 p = new Point(no.position.X - delta+1, no.position.Y - delta+1);
            }           
            SolidBrush b = new SolidBrush(Color.Blue);         
            g.DrawImage(imageList1.Images[ch], p);
            //Font f = new Font(FontFamily.Families[6], 14);
            //g.DrawString(no.messsage, f, b, new Point(p.X, p.Y + 30));
        }
        private void drawlinetree(edge e,Color c)
        {
            Pen p = new Pen(c, 7);
            g.DrawLine(p, map.arryofnode[e.fromvex].position, map.arryofnode[e.tovex].position);
        }

        private void drawline(int p1,int p2)
        {
            Point from = map.arryofnode[p1].position;
            Point to = map.arryofnode[p2].position;
            Bitmap pc = new Bitmap(edgestack.Peek());
            g = Graphics.FromImage(pc);
            Pen p = new Pen(Color.GreenYellow, 4);
            g.DrawLine(p, from, to);
            drawnodewithnostack(map.arryofnode[p1],g,0);
            drawnodewithnostack(map.arryofnode[p2],g,0);
            SolidBrush b = new SolidBrush(Color.Brown);
            Point mid = new Point((from.X + to.X) / 2, (from.Y + to.Y) / 2);
            Font f = new Font(FontFamily.Families[6], 20);
            g.DrawString(textBox1.Text, f, b, mid);
            g = panel1.CreateGraphics();
            g.DrawImage(pc, new Point(0, 0));
            edgestack.Push(pc);
        }
        /// <summary>
        /// button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                playsound(2);
                panel1.Enabled = true;
                button3.Enabled = true;
                comboBox1.Enabled = false;
                maxsize = int.Parse(comboBox1.Text);
                button1.Enabled = false;
                map.numofedge = maxsize * maxsize - 1;
                map.numofnode = maxsize;
                for (int i = 1; i <= maxsize; i++)
                {
                    comboBox2.Items.Add(i.ToString());
                    comboBox3.Items.Add(i.ToString());
                }
                say(2);
            }
            else
            {
                MessageBox.Show("please decide the number of the point");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            playsound(2);
            if (radioButton1.Checked || radioButton2.Checked)
            {
                button2.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;

                if (radioButton2.Checked == true)
                {
                    if (comboBox4.Text != "")
                    {
                        for (int i = 0; i < maxsize - 1; i++)
                        {
                            map.t[i] = new edge(0, 0, 1);
                        }
                        map.numofedge = sizeedge;
                        say(4);
                        success = map.prim(int.Parse(comboBox4.Text) - 1);
                    }
                    else
                    {
                        MessageBox.Show("Prim Algorithm need a start points");
                    }
                }
                else
                {
                    map.numofedge = sizeedge;
                    map.numofnode = sizetemp;
                    success = map.krusk();
                    say(5);
                }
            }


            else
            {
                MessageBox.Show("You must choose an algorithm");
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (sizeedge >= maxsize * (maxsize - 1))
            {
                MessageBox.Show("Too many edges!");
                return; 
            }
            
            int p1 = int.Parse(comboBox2.Text)-1;
            int p2 = int.Parse(comboBox3.Text)-1;
            for (int i = 0; i < sizeedge; i++)
            {            
                if (map.arryofedge[i].fromvex == p1 && map.arryofedge[i].tovex == p2)
                {
                    MessageBox.Show("Alreay got the edge!"); return;
                }
                if (map.arryofedge[i].fromvex == p2 && map.arryofedge[i].tovex == p1)
                {
                    MessageBox.Show("Alreay got the edge!"); return;
                }
            }                     
            if (p1 != p2)
            {
                playsound(2);
                drawline(p1,p2);
                jieduan=2;
                int w=int.Parse(textBox1.Text);
                map.arryofedge[sizeedge] = new edge(p1, p2, w);
                sizeedge++;
                map.numofedge = sizeedge;
                map.matri[p1, p2] = w; map.matri[p2, p1] = w;            
            }
            else
            {
                MessageBox.Show("They are the same vertex!");
            }        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (sizetemp > 0)
            {
                Point toerase = new Point(map.arryofnode[sizetemp - 1].position.X, map.arryofnode[sizetemp - 1].position.Y);
                nodestack.Pop();
                g.DrawImage(nodestack.Peek(),new Point(0,0));               
                map.arryofnode[sizetemp-1] = null;
                sizetemp--;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                comboBox4.Enabled =false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                comboBox4.Enabled = true;
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (edgestack.Count > 1)
            {
                edgestack.Pop();
                g.DrawImage(edgestack.Peek(), new Point(0, 0));
                map.arryofedge[sizeedge - 1] = null;
                sizeedge--;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            playsound(1);
            button5.Enabled = false;
            g = panel1.CreateGraphics();
            if (radioButton2.Checked == true)
            {
                for (int i = 0; i < maxsize - 1; i++)
                {
                    drawlinetree(map.t[i], Color.Blue);
                }
                for (int i = 0; i < maxsize; i++)
                {
                    drawnodewithnostack(map.arryofnode[i], g, 0);
                }
            }
            if (radioButton1.Checked == true)
            {
                for (int i = 0; i < map.re.Count; i++)
                {
                    drawlinetree(map.re[i], Color.Blue);
                }
                for (int i = 0; i < maxsize; i++)
                {
                    drawnodewithnostack(map.arryofnode[i], g, 0);
                }
            }
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
            playsound(2);
            button4.Enabled = false;
            if (radioButton2.Checked == true)
            {
                
                if (map.primQ.Count > 0)
                {
                    g = panel1.CreateGraphics();
                    primaction ac = map.primQ.Peek();
                    for (int i = 0; i < ac.edgeoftree.Count; i++)
                    {
                        drawlinetree((edge)ac.edgeoftree[i], Color.Red);
                    }
                    for (int i = 0; i < ac.alter.Count; i++)
                    {
                        drawlinetree((edge)ac.alter[i], Color.Purple);
                    }
                    for (int i = 0; i < ac.renode.Count; i++)
                    {
                        drawnodewithnostack((node)ac.renode[i], g, 1);
                    }
                    map.primQ.Dequeue();
                    if (map.primQ.Count == 0)
                    {
                        button5.Enabled = false;
                    }
                    say(6);
                }
            }
            if (radioButton1.Checked == true)
            {
                if (map.krusQ.Count > 0)
                {
                    g = panel1.CreateGraphics();
                    kruskaction ac = map.krusQ.Peek();
                    for (int i = 0; i < ac.reedge.Count; i++)
                    {
                        drawlinetree(ac.reedge[i], Color.Red);
                    }                   
                    for (int i = 0; i < ac.reedge.Count; i++)
                    {
                        drawnodewithnostack(map.arryofnode[ac.reedge[i].fromvex], g, 1);
                        drawnodewithnostack(map.arryofnode[ac.reedge[i].tovex], g, 1);
                    }
                    map.krusQ.Dequeue();
                    if (map.krusQ.Count == 0)
                    {
                        button5.Enabled = false;
                    }
                    say(6);
                }
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            init();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "" && comboBox3.Text != "")
            {
                int p1 = int.Parse(comboBox2.Text) - 1;
                int p2 = int.Parse(comboBox3.Text) - 1;
                int x1 = map.arryofnode[p1].position.X;
                int y1 = map.arryofnode[p1].position.Y;
                int x2 = map.arryofnode[p2].position.X;
                int y2 = map.arryofnode[p2].position.Y;
                int distence = (int)System.Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
                textBox1.Text = distence.ToString();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "" && comboBox3.Text != "")
            {
                int p1 = int.Parse(comboBox2.Text) - 1;
                int p2 = int.Parse(comboBox3.Text) - 1;
                int x1 = map.arryofnode[p1].position.X;
                int y1 = map.arryofnode[p1].position.Y;
                int x2 = map.arryofnode[p2].position.X;
                int y2 = map.arryofnode[p2].position.Y;
                int distence = (int)System.Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
                textBox1.Text = distence.ToString();
            }
        }

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
    }
    
}
