using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MapAlgorithm2;

namespace MapAlgorithm3
{
    public class trMap:djMap
    {
        public Queue<int> recorderof_DFS = new Queue<int>();
        public Queue<int> recorderof_BFS = new Queue<int>();
        public Queue<List<pa>> recorderof_conhull = new Queue<List<pa>>();

        public trMap()
        {
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    matri[i, j] = int.MaxValue;
                }
            }
            top = 0;
        }
        
        public void DFS_travel(int i)
        {  
            recorderof_DFS.Enqueue(i);
            listofnode[i].visited = true;
            for (int j = 0; j < numofnode; j++)
            {
                if (matri[i,j] <int.MaxValue && listofnode[j].visited == false)
                {
                    DFS_travel(j);
                }
            }
        }

        public void BFS_travel(int i)
        {
            Queue<int> Q = new Queue<int>();
            Q.Enqueue(i);
            listofnode[i].visited = true;
            while (Q.Count > 0)
            {
                int t = Q.Dequeue();
                recorderof_BFS.Enqueue(t);
                for (int j = 0; j < numofnode; j++)
                {
                    if (matri[t, j] < int.MaxValue && listofnode[j].visited == false)
                    {
                        //enqueue(q, j);
                        Q.Enqueue(j);
                        //a[j].bl = true;
                        listofnode[j].visited = true;
                    }

                }
            }

        }

        /// <summary>
        /// convex hull
        /// </summary>
        /// 
        public pa[] p = new pa[32];
        public pa[] ch = new pa[32];
        public static double cross(pa p0, pa p1, pa p2)
        { return (p1.x - p0.x) * (p2.y - p0.y) - (p1.y - p0.y) * (p2.x - p0.x); }

        public static double dist(pa a, pa b)
        { return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y); }

        public static double area(pa a, pa b)
        { return Math.Abs(a.x * b.y - a.y * b.x); }

        public int cmp(pa a, pa b)
        {
            double re = cross(p[0], a, b);
            if (re > 0) return -1;
            else if (re == 0 && dist(p[0], a) > dist(p[0], b))
                return -1;
            else return 1;
        }
        public int top = 0;
        public void f(int n)
        {
            int j = 0, i = 0;
            for (i = 1; i < n; i++)
            {
                if (p[i].y < p[j].y || p[i].y == p[j].y && p[i].x < p[j].x)
                {
                    j = i;
                }
            }
            pa temp = new pa(p[0]);
            p[0] = new pa(p[j]);
            p[j] = new pa(temp);
            //sort
            List<pa> list = new List<pa>(32);
            for (int ii = 1; ii < n; ii++)
            {
                list.Add(new pa(p[ii]));
            }
            list.Sort(cmp);
            for (int ii = 1; ii < n; ii++)
            {
                p[ii] = new pa(list[ii - 1]);
            }

            ch[0] = new pa(p[0]); ch[1] = new pa(p[1]); ch[2] = new pa(p[2]); 
            top = 2;
            List<pa> l = new List<pa>();
            for(int ii=0;ii<=2;ii++)
            {
                l.Add(new pa(ch[ii]));
            }
            recorderof_conhull.Enqueue(l);
            for (i = 3; i < n; i++)
            {
                while (cross(ch[top - 1], p[i], ch[top]) >= 0)
                {
                    top--;
                    if (top == 1)
                    {
                        break;
                    }
                }
                ch[++top] = new pa(p[i]);

                List<pa> l2 = new List<pa>();
                for (int ii = 0; ii <= top; ii++)
                {
                    l2.Add(new pa(ch[ii]));
                }
                recorderof_conhull.Enqueue(l2);
                
            }

        }

      
        


        
    }
    public class pa
    {
        public int x;
        public int y;
        //public double ang;
        public pa(int a, int b)
        {
            x = a;
            y = b;
            //ang = c;
        }
        public pa(pa b)
        {
            x = b.x;
            y = b.y;
            //ang = b.ang;
        }
        public pa(Point p)
        {
            x = p.X;
            y = p.Y;
        }
    }
}
