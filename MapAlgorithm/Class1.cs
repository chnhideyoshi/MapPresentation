using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;


namespace MapAlgorithm
{
   
    public class Map
    {
        private static int comp(edge a, edge b)
        {
            return (int)a.weight - (int)b.weight;
        }
        public int maxnumofnode=30; 
        public int[,] matri = new int[32, 32];
        public node[] arryofnode = new node[32];
        public edge[] arryofedge = new edge[32 * 31];
        public int numofnode = 30;
        public int numofedge = 900;
        public edge[] t = new edge[32];
        public List<edge> re = new List<edge>();
        public primaction action=new primaction();
        public Queue<primaction> primQ = new Queue<primaction>();
        public Queue<kruskaction>krusQ=new Queue<kruskaction>();

        /// <summary>
        /// prim
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public bool prim(int root)
        {
            Point temp=new Point(0,0);
            int n = numofnode;
            edge e;
            int  k = 0, minpos = 0, v;
            double min;
            for (int i = 0; i < n; i++)
            {
                if (i != root)
                {
                    t[k].fromvex = root;
                    t[k].tovex = i;
                    t[k].weight = matri[root, i];
                    k++;
                }
                
            }
            //
            action.renode.Add(new node(this.arryofnode[root].position,root));
            for (int i = 0; i < n-1; i++)
            {
                if (t[i].weight != int.MaxValue)
                {
                    action.alter.Add(new edge(t[i]));
                }
            }
            primQ.Enqueue(action);
            for (k = 0; k < n - 1; k++)
            {
                min = int.MaxValue;
                for (int i = k; i < n - 1; i++)
                {
                    if (t[i].weight < min)
                    {
                        min = t[i].weight;
                        minpos = i;
                    }
                }

                if (min == int.MaxValue)
                {
                    return false;
                }

                e = t[minpos];
                t[minpos] = t[k];
                t[k] = e;
                v = t[k].tovex;

                action = new primaction(primQ.Peek().renode, primQ.Peek().edgeoftree, primQ.Peek().alter);
                action.renode.Add( new node(this.arryofnode[v].position, v));
                action.edgeoftree.Add(new edge(t[k]));

                if (action.alter.Contains(t[k]))
                {
                    action.alter.Remove(t[k]);
                }
                

                for (int i = k + 1; i < n - 1; i++)
                {
                    if (matri[v, t[i].tovex] < t[i].weight)
                    {
                        t[i].weight = matri[v, t[i].tovex];
                        t[i].fromvex = v;
                    }
                }
               
                for (int i = k + 1; i < n - 1; i++)
                {
                    if (matri[v, t[i].tovex] !=int.MaxValue)
                    {                       
                        action.alter.Add(new edge(v, t[i].tovex, matri[v, t[i].tovex]));                       
                    }
                }

                primQ.Enqueue(action);
                
                
                
            }
            return true;
        }
        ////

        public bool krusk()
        {
            re.Clear();
            List < edge > list = new List<edge>();
            //List < edge > re = new List<edge>();
            int[] a = new int[32];
            for (int i = 0; i < 32; i++)
            {
                a[i] = 0;
            }
            for (int i = 0; i < numofedge; i++)
            {
                list.Add(arryofedge[i]);
            }
            list.Sort(comp);
            int iter = 0; int count = 1;
            while (iter < numofedge)
            {
                edge e = new edge(list[iter]);
                if (a[e.fromvex] == 0 || a[e.tovex] == 0)
                {
                    if ((a[e.fromvex] == 0 && a[e.tovex] != 0) || (a[e.fromvex] != 0 && a[e.tovex] == 0))
                    {
                        if (a[e.fromvex] == 0)
                        {
                            a[e.fromvex] = a[e.tovex];

                            iter++;
                        }
                        else
                        {
                            a[e.tovex] = a[e.fromvex];
                            iter++;
                        }

                    }
                    else
                    {
                        a[e.fromvex] = count;
                        a[e.tovex] = count;
                        count++;
                        iter++;
                    }
                    re.Add(e);
                    kruskaction action=new kruskaction(re);
                    krusQ.Enqueue(action);
                }
                else
                {
                    if (a[e.fromvex] == a[e.tovex])
                    {
                        iter++;
                    }
                    else
                    {
                        re.Add(e);
                        kruskaction action = new kruskaction(re);
                        krusQ.Enqueue(action);
                        int max, min;
                        if (a[e.fromvex] < a[e.tovex])
                        {
                            max = a[e.tovex];
                            min = a[e.fromvex];
                        }
                        else
                        {
                            max = a[e.fromvex];
                            min = a[e.tovex];
                        }
                        for (int i = 0; i < numofnode; i++)
                        {
                            if (a[i] == max)
                            {
                                a[i] = min;
                            }
                        }
                        iter++;
                    }
                }

            }
            if (re.Count == numofnode - 1)
            {
                return true;
            }
            else
            {
                return false;
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
    public class primaction
    {
        public ArrayList renode=new ArrayList(32);
        public ArrayList edgeoftree = new ArrayList(32);
        public ArrayList alter = new ArrayList(32);
        public  primaction(ArrayList n,ArrayList e,ArrayList a)
        {
            ArrayList renode = new ArrayList(32);
            ArrayList edgeoftree = new ArrayList(32);
            ArrayList alter = new ArrayList(32);
            for (int i = 0; i < n.Count; i++)
            {
                renode.Add(n[i]);
            }
            for (int i = 0; i < e.Count; i++)
            {
                edgeoftree.Add(e[i]);
            }
            for (int i = 0; i < a.Count; i++)
            {
                alter.Add(a[i]) ;
            }          
        }
        public primaction()
        {
             ArrayList renode = new ArrayList(32);
             ArrayList edgeoftree = new ArrayList(32);
             ArrayList alter = new ArrayList(32);
        }

    }
    public class kruskaction
    {
        public List<edge> reedge=new List<edge>(32);
        public kruskaction()
        {
             List<edge> reedge=new List<edge>(32);
        }
        public kruskaction(List<edge> n)
        {
            List<edge> renode = new List<edge>(32);
            for (int i = 0; i < n.Count; i++)
            {
                reedge.Add(n[i]);
            }
        }
    }
    public class comp : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            return (int)((edge)x).weight - (int)((edge)y).weight;
        }

    }
}
