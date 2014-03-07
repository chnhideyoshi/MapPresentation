using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;

namespace MapAlgorithm2
{
    public class djMap
    {
        private static int comp(edge a, edge b)
        {
            return (int)a.weight - (int)b.weight;
        }
        public int start = 0;
        public int numofnode = 0;
        public int numofedge = 0;
        public List<node> listofnode = new List<node>();
        public List<edge> listofedge = new List<edge>();
        public int[,] matri = new int[32, 32];
        public djMap()
        {
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    matri[i, j] = int.MaxValue;
                }
            }
            for (int i = 0; i < 32; i++)
            {
                matri[i, i] = 0;
            }
            //for
            //path[i] = new List<int>();
        }

       // public int[] dist = new int[32];
        public int[,] dlist=new int[32,32];
        public List<int>[] path = new List<int>[32];
        public List<int> findsequence = new List<int>();
        public int iter = 0;
        public Queue<List<int>[]> pQ = new Queue<List<int>[]>();

        
        //
        public void dijkska(int v)
        {
            
            bool[] s = new bool[32];
            int[] dist = new int[32];
            for (int i = 0; i < numofnode; i++)
            {
                dist[i] = matri[v, i];
                s[i] = false;
            }
            s[v] = true;
            findsequence.Add(v);
            dist[v] = 0;
            for (int i = 0; i < numofnode ; i++)
            {
                path[i] = new List<int>(20);
                path[i].Add(v);
                //path[i].Add(65535);
            }
            List<int>[] np1 = new List<int>[20];
            for (int i = 0; i < numofnode; i++)
            {
                np1[i] = new List<int>(20);               
                np1[i].Add(v);          
            }
            pQ.Enqueue(np1);
            int min;
            iter = 0;
            for (int i = 0; i < numofnode; i++)
            {
                dlist[iter,i] = dist[i];
            }
            iter++;

            for (int i = 0; i < numofnode - 1; i++)
            {
                min = int.MaxValue;
                int u = v;
                for (int j = 0; j < numofnode; j++)
                {
                    if (s[j] == false && dist[j] < min)
                    { u = j; min = dist[j]; }
                }

                //dlist.Add();
                for (int uu = 0; uu < numofnode; uu++)
                {
                    dlist[iter,uu] = dist[uu];
                }
                iter++;
                s[u] = true;
                findsequence.Add(u);
                path[u].Add(u);
                List<int>[] np = new List<int>[20];
                for (int ii = 0; ii < numofnode; ii++)
                {
                    np[ii] = new List<int>(20);
                    for (int j = 0; j < path[ii].Count; j++)
                    {
                        np[ii].Add(path[ii][j]);
                    }
                }
                pQ.Enqueue(np);

                //int re;
                for (int k = 0; k < numofnode; k++)
                {
                    int w = matri[u, k];
                    if (s[k] == false && w < int.MaxValue && (dist[u] + w < dist[k]))
                    {
                        dist[k] = dist[u] + w;
                        path[k].Clear();
                        for (int j = 0; j < path[u].Count; j++)
                        {
                            path[k].Add(path[u][j]);
                        }
                    }
                }

            }





        }

    }
    public class node
    {
        
         public   Point position;
         public  string messsage="POINT";
         public  bool visited=false;
         public node(Point p, int hao)
         {
             position = p;
             messsage = "Point" + (hao+1);
         }

    }
    public class edge
    {
       public int fromvex;
       public  int tovex;
       public  double weight;
       public edge(int from, int to, int w)
       {
           fromvex = from;
           tovex = to;
           weight = (double)w;
       }
        public edge(edge d)
        {
            fromvex=d.fromvex;
            tovex=d.tovex;
            weight=d.weight;
        }

        
    }
}
