using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MapAlgorithm2;
using System.IO;

namespace MapPresentation
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = "DiamondBlue.ssk";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox2.Enabled = false;
            comboBox5.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = true;
            panel1.Enabled = true;
            maxsize = int.Parse(comboBox2.Text);
            for (int i = 1; i <= maxsize; i++)
            {
                comboBox3.Items.Add(i.ToString());
                comboBox4.Items.Add(i.ToString());
                comboBox5.Items.Add(i.ToString());
            }
            map.numofnode = maxsize;
            say(5);
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
        {
            if (sizeedge >= maxsize * (maxsize - 1))
            {
                MessageBox.Show("Too many edges!");
                return;
            }
            if (comboBox4.Text == "" || comboBox3.Text == "")
            {
                MessageBox.Show("please choose the two point");
                return;
            }
            int p1 = int.Parse(comboBox3.Text) - 1;
            int p2 = int.Parse(comboBox4.Text) - 1;
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
                map.listofedge.Add( new edge(p1, p2, w));
                sizeedge++;
                map.numofedge = sizeedge;
                map.matri[p1, p2] = w; map.matri[p2, p1] = w;
                richTextBox1.Text = "new edge added! from "+comboBox3.Text+" to "+comboBox4.Text+" weight:"+textBox1.Text+"\n" +richTextBox1.Text;
            }
            else
            {
                MessageBox.Show("They are the same Point!");
            }
            //say(2);
        }

        private void button6_Click(object sender, EventArgs e)
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

        private void button7_Click(object sender, EventArgs e)
        {
            map.numofnode = maxsize;
            map.numofedge = sizeedge;
            if (comboBox5.Text == "")
            {
                MessageBox.Show("Please choose the start point!");
                return;
            }
            map.start = int.Parse(comboBox5.Text)-1;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button2.Enabled = true;
            button1.Enabled = true;
            

            textBox1.Enabled = false;

            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            //map.start=int.Parse(comboBox5.Text);
            comboBox5.Enabled=false;
            comboBox1.Enabled = true;
            for (int i = 1; i <= maxsize; i++)
            {
                comboBox1.Items.Add(i.ToString());
            }
            comboBox1.Items.Remove(map.start.ToString());
            //

            map.dijkska(map.start);
            say(2);


        }
        private int it = 0;
        private int ts = 0;
        
        //private Queue<List<int>> Q=new Queue<List<int>>();
        private void button2_Click(object sender, EventArgs e)
        {
            say(4);
            if (it == map.iter)
            {
                comboBox1.Enabled = true;
                button1.Enabled = true;
                return;
            }
            richTextBox1.Text = "The count=" + it + " and the table shows the process\n" + richTextBox1.Text;

            dataGridView1.Enabled = true;
            dataGridView2.Enabled = true;
            dataGridView3.Enabled = true;
            
            button1.Enabled = false;
            comboBox1.Enabled = false;

            ///
            if (it == 0)
            {
                for (int lie = 0; lie < map.numofnode; lie++)
                {
                    dataGridView1.Columns.Add(lie.ToString(), "dist[" + (lie + 1) + "]");
                }
                for (int lie = 0; lie < map.numofnode; lie++)
                {
                    dataGridView3.Columns.Add(lie.ToString(), "Point" + (lie + 1) + " path[" + lie + "]");
                }
            }
            dataGridView1.Rows.Add(1);

            for (int lie = 0; lie < map.numofnode; lie++)
            {
                if (map.dlist[it, lie] != int.MaxValue)
                {
                    dataGridView1.Rows[it].Cells[lie].Value = map.dlist[it, lie];
                }
                else
                {
                    dataGridView1.Rows[it].Cells[lie].Value = "∞";
                }
            }
            it++;
            if (ts == 0)
            {
                for (int lie = 0; lie < map.numofnode; lie++)
                {
                    dataGridView2.Columns.Add("", "Point" + (lie + 1));
                }
                dataGridView2.Rows.Add(1);
                for (int i = 0; i < map.numofnode; i++)
                {
                    dataGridView2.Rows[0].Cells[i].Value = "NotFound";
                }
            }
            dataGridView2.Rows[0].Cells[ts].Value = (map.findsequence[ts] + 1).ToString();
            ts++;

            if (map.pQ.Count != 0)
            {
                List<int>[] newpath = new List<int>[20];
                for (int i = 0; i < 20; i++)
                {
                    newpath[i]=new List<int>(20);
                }
                for (int i = 0; i < map.numofnode; i++)
                {
                    for (int j = 0; j < map.pQ.Peek()[i].Count; j++)
                    {
                        newpath[i].Add(map.pQ.Peek()[i][j]);
                    }
                }

                for (int lie = 0; lie < map.numofnode; lie++)
                {
                    for (int hang = 0; hang < newpath[lie].Count; hang++)
                    {
                        if (hang >= dataGridView3.Rows.Count) ;
                        {
                            dataGridView3.Rows.Add(1);
                        }
                        dataGridView3.Rows[hang].Cells[lie].Value = "Point" + (newpath[lie][hang] + 1);
                    }
                }
                map.pQ.Dequeue();

            }









        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                int end = int.Parse(comboBox1.Text)-1;
                say(3);
                richTextBox1.Text = "Show the way to " + comboBox1.Text + "\n" + richTextBox1.Text;
                if (dataGridView1.Enabled != true)
                {
                    dataGridView1.Enabled = true;
                    //dataGridView2.Enabled = true;
                    dataGridView3.Enabled = true;
                    button2.Enabled = false;
                    //dataGridView1.Columns.Clear();
                    //dataGridView2.Columns.Clear();
                    //dataGridView3.Columns.Clear();

                    for (int lie = 0; lie < map.numofnode; lie++)
                    {
                        dataGridView1.Columns.Add(lie.ToString(), "dist[" + (lie+1) + "]");
                        for (int hang = 0; hang < map.iter; hang++)
                        {
                            dataGridView1.Rows.Add(1);
                            if (map.dlist[hang, lie] != int.MaxValue)
                            {
                                dataGridView1.Rows[hang].Cells[lie].Value = map.dlist[hang, lie];
                            }
                            else
                            {
                                dataGridView1.Rows[hang].Cells[lie].Value = "∞";
                            }

                        }
                    }
                    ///
                   /* for (int lie = 0; lie < map.numofnode; lie++)
                    {                     
                        dataGridView2.Columns.Add("", "sq[" + (lie+1) + "]");                                            
                    }
                    dataGridView2.Rows.Add(1);
                    for (int lie = 0; lie < map.numofnode; lie++)
                    {                     
                        dataGridView2.Rows[0].Cells[lie].Value= (map.findsequence[lie]+1).ToString();                                            
                    }*/
                    ///
                    for (int lie = 0; lie < map.numofnode; lie++)
                    {
                        dataGridView3.Columns.Add(lie.ToString(),"Point"+(lie+1)+" path[" + lie + "]");
                        for (int hang = 0; hang < map.path[lie].Count; hang++)
                        {
                            if (hang >= dataGridView3.Rows.Count) ;
                            {
                                dataGridView3.Rows.Add(1);
                            }
                            dataGridView3.Rows[hang].Cells[lie].Value = "Point"+(map.path[lie][hang]+1);
                        }           
                    }
                    
                    
                    
                }
                g=panel1.CreateGraphics();
                g.DrawImage(edgestack.Peek(), new Point(0, 0));
                for (int i = 0; i < map.path[end].Count - 1; i++)
                {
                    drawlinetree(new edge(map.path[end][i], map.path[end][i+1],0),Color.Blue);
                    drawnodewithnostack(map.listofnode[map.path[end][i]], g, 1);
                }
                drawnodewithnostack(map.listofnode[map.path[end][map.path[end].Count - 1]], g, 1);
                
                
                //button1.Enabled = false;
       

            }
            else
            {
                MessageBox.Show("Please choose the point as the final");
            }

        }
        //////
        private void say(int i)
        {
            switch (i)
            {
                case 0: richTextBox2.Text = "Welcome to the MapPresentation of Dijkstra Agorithm! Please choose the number of the Points"; break;
                case 1: richTextBox2.Text = "Well,you then need to make the egdes,you can set the weight or use the default distence"; break;
                case 2: richTextBox2.Text = "The Dijkstra Algorithm has done, you can choose to watch the result or the process"; break;
                case 3: richTextBox2.Text = "Tables of the greedy  shortest distence and the path to every points has been shown on the right. The way has been marked at the map"; break;
                case 4: richTextBox2.Text = "Tables are changing while you press the key!"; break;
                case 5: richTextBox2.Text = "You can click on the image to locate your Points! Don't forget to set the point to start!"; break;
            }

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



        /// <summary>
        /// form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private Graphics g;
        public int maxsize = 20;
        public int sizetemp = 0;
        public int delta = 16;
        public int sizeedge = 0;
        djMap map = new djMap();
        //bool success = false;
        /// <summary>
        /// stack and queue
        /// </summary>
        /// 
        Stack<Bitmap> nodestack = new Stack<Bitmap>();
        Stack<Bitmap> edgestack = new Stack<Bitmap>();
        //Stack<Bitmap> woca = new Stack<Bitmap>();
        private int jieduan = 1;
        private void Form3_Load(object sender, EventArgs e)
        {
            
            Init();
            say(0);
            timer1.Enabled = true;
            this.Opacity = 0;
        }
        private void Init()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button3.Enabled = true;
            richTextBox1.Text = "";
            say(0);
            panel1.Enabled = false;

            //dataGridView1.DataSource = null;
            //dataGridView2.DataSource = null;
            //dataGridView3.DataSource = null;
            dataGridView1.Enabled = false;
            dataGridView2.Enabled = false;
            dataGridView3.Enabled = false;
            textBox1.Enabled = false;

            comboBox1.Enabled = false;
            comboBox2.Enabled =true;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;

            g=panel1.CreateGraphics();
            
            sizeedge = 0;
            maxsize = 20;
            jieduan = 1;
            sizetemp = 0;
            it = 0;
            ts = 0;
            map = new djMap();
            nodestack.Clear();
            nodestack.Push(new Bitmap(panel1.BackgroundImage));
            edgestack.Clear();
            g.DrawImage(nodestack.Peek(), new Point(0, 0));
        

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

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (sizetemp != maxsize)
            {
                Point p = new Point(e.X, e.Y);
                playsound(1);
                map.listofnode.Add(new node(p, sizetemp));
                
                drawnode(map.listofnode[sizetemp]);
                jieduan = 1;
                
                
                richTextBox1.Text = "new point added! Postion: " + e.X + "," + e.Y + "\n" + richTextBox1.Text;
                sizetemp++;
                if (sizetemp == maxsize)
                {
                    button4.Enabled = false;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    button7.Enabled = true;

                    textBox1.Enabled = true;

                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;

                    edgestack.Push(nodestack.Peek());
                    say(1);
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
        private void drawnodewithnostack(node no,Graphics g,int ch)
        {
            Point p = new Point(no.position.X - delta, no.position.Y - delta);
            if (ch == 1)
            {
                p = new Point(no.position.X - delta + 1, no.position.Y - delta + 1);
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
            Point mid = new Point((from.X + to.X) / 2, (from.Y + to.Y) / 2);
            Font f = new Font(FontFamily.Families[6], 20);
            g.DrawString(textBox1.Text, f, b, mid);
            g = panel1.CreateGraphics();
            g.DrawImage(pc, new Point(0, 0));
            edgestack.Push(pc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text != "" && comboBox4.Text != "")
            {
                int p1 = int.Parse(comboBox3.Text) - 1;
                int p2 = int.Parse(comboBox4.Text) - 1;
                int x1 = map.listofnode[p1].position.X;
                int y1 = map.listofnode[p1].position.Y;
                int x2 = map.listofnode[p2].position.X;
                int y2 = map.listofnode[p2].position.Y;
                int distence = (int)System.Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
                textBox1.Text = distence.ToString();
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text != "" && comboBox4.Text != "")
            {
                int p1 = int.Parse(comboBox3.Text) - 1;
                int p2 = int.Parse(comboBox4.Text) - 1;
                int x1 = map.listofnode[p1].position.X;
                int y1 = map.listofnode[p1].position.Y;
                int x2 = map.listofnode[p2].position.X;
                int y2 = map.listofnode[p2].position.Y;
                int distence = (int)System.Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
                textBox1.Text = distence.ToString();
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = "Selected start point is " + comboBox5.Text + "\n" + richTextBox1.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = "The point-num has been set to " + comboBox2.Text + "\n" + richTextBox1.Text;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You are sure to make the map again from the begining?", "Remind", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            switch (result)
            {
                case DialogResult.OK:
                    {
                        
                        Init();
                    }
                    break;
                case DialogResult.Cancel:
                    {
                        return;
                    }
                    break;
            }
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
    }
}
