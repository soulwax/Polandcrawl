using UnityEngine;
using System.Collections.Generic;

public class PathFinder  {
    public static bool VERBOSE = true;

    public class Node {
        public int x, y;
        public int xEnd, yEnd;
        public int g_value; 
        public int h_value; //heuristic
        public int f_value;
        public bool passable = false;
        public Node parent;
        public Vector2 vecPos;

        public Node(int x, int y, bool passable) {
            this.x = x;
            this.y = y;
            this.vecPos = new Vector2(x,y);
            this.passable = passable;
        }

        public void Init(int xEnd, int yEnd) {
            this.xEnd = xEnd;
            this.yEnd = yEnd;

            int xd = Mathf.Abs(x-xEnd);
            int yd = Mathf.Abs(y-yEnd);
            if (xd > yd){
                h_value = 14 * yd + 10 * (xd - yd);
            } else {
                h_value = 14 * xd + 10 * (yd - xd);
            }
        }

        public int GetFValue() {
            f_value = g_value + h_value;
            return f_value;
        }

        public Node Parent {
            get { return parent; }
            set { this.parent = value; }
        }

        public Vector2 GetVector2(){
            return vecPos;
        }

        public bool Equals(Node n) {
            if (this.x == n.x && this.y == n.y) return true;
            return false;
        }
    }
    private List<Vector2> result = new List<Vector2>();
    private List<Node> open = new List<Node>();
    private List<Node> closed = new List<Node>();

    private Node[,] nodes;
    private Node start, end, next, n, p;
    private bool pathFound = false;
    private int xStart;
    private int yStart;
    private int xEnd;
    private int yEnd;
    private int lwidth;
    private int lheight;
    public bool isPathing = false;

    //constructor preloads the nodes
    public PathFinder(GameView view) {
        this.lwidth = view.levelWidth;
        this.lheight = view.levelHeight;
        nodes = new Node[lwidth, lheight];
        for (int y = 0; y < lheight; y++) {
            for (int x = 0; x < lwidth; x++) {
                if(GameView.dungeonMap[x,y] == 1) //is the tile a floor?
                    nodes[x, y] = new Node(x,y, true);
                else
                    nodes[x, y] = new Node(x, y, false);
            }
        }

        isPathing = false;
    }

    public List<Vector2> GetPath(int xStart, int yStart, int xEnd, int yEnd) {
        long lastTime = System.DateTime.Now.ToFileTime(); //for the iteration time measuring
        this.xStart = xStart;
        this.yStart = yStart;
        this.xEnd = xEnd;
        this.yEnd = yEnd;
        pathFound = false;       
        isPathing = true;
        start = nodes[this.xStart, this.yStart];
        end = nodes[this.xEnd, this.yEnd];
        result.Clear();
        open.Clear();
        closed.Clear();

        open.Add(start);
        AddOpenNodeToClosed(start); //Put it into the closed list           
        next = start;

        //Look at surrounding nodes and add them to the open list 
        //if they are passable and not yet in any list
        //Assign the node A to them as a parent node and add them to the open list
        //Pick the cheapest node and add it to the closed list
        int attempts = 0;
        while (!pathFound) {
            InitSurroundingNodes(next);
            AddOpenNodeToClosed(next);
            next = GetCheapestNode();

            if (attempts++ >= 10000) break;
        }
        
        attempts = 0;
        p = end.Parent;

        //reconstruct path by begginning at the end node and looking for parent nodes
        while (true) {
            if(p != null) {
                if (p.Equals(start)) break;
                n = p;
                result.Add(n.vecPos);
                p = n.Parent;               
            }
            if (attempts++ >= 10000) break;
        }

        result.Reverse();
        isPathing = false;
        long now = System.DateTime.Now.ToFileTime();
        double duration = (now - lastTime)/10000D;
        if (VERBOSE) Debug.Log("Iteration took "+duration+" ms.");
           
        return result;
    }

    //Initializes the neighbouring eight nodes of a parent node
    //and assigns the values as necessary and ignoring not passable
    //nodes. This is the core A* algorithm, it is getting repeated until
    //the end node is found
    public void InitSurroundingNodes(Node pnode) {
        int x = pnode.x;
        int y = pnode.y;
        Node n;
        for (int yy = y - 1; yy <= y + 1; yy++) {
            if (yy >= lheight) break;
            for (int xx = x - 1; xx <= x + 1; xx++) {
                if (xx >= lwidth) break;      
                n = nodes[xx, yy];
                n.Init(this.xEnd, this.yEnd);
                if ((xx == x && yy == y) || !n.passable) continue;
                if (closed.Contains(n)) continue;                  
                bool isInOpen = open.Contains(n);
                int pncostG = GetGPathCost(pnode, n) + pnode.g_value;

                //assign g value or update if costs are cheaper 
                if ((isInOpen && pncostG < n.g_value) || !isInOpen) {                 
                    n.parent = pnode;
                    n.g_value = pncostG;
                }

                if (n.Equals(end)) pathFound = true;
                if (!isInOpen) open.Add(n);                                            
            }
        }
    }

    //removes node n from the open list and puts it into the closed
    //So_einfach_ist_das.jpg
    public void AddOpenNodeToClosed(Node n) {
        open.Remove(n);
        closed.Add(n);   
    }

    //only use for two neighbouring nodes!
    public int GetGPathCost(Node n1, Node n2) {
        if (n1.x == n2.x || n1.y == n2.y) return 10;
        else return 14;
    }

    //this is the simplest possible 'sorting' algorithm and 
    //thus a performance bottleneck, but for the maps of our 
    //size it doesn't matter at all (I cannot into binary heaps
    //yet)
    public Node GetCheapestNode() {
        Node res = open[0];
        for (int i = 0; i < open.Count; i++) {
            Node n = open[i];
            if (res.GetFValue() > n.GetFValue()) res = n;
        }
        return res;
    }
}
