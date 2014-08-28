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
            return g_value + h_value;
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
    private bool pathFound = false;
    private int xStart;
    private int yStart;
    private int xEnd;
    private int yEnd;
    private int lwidth;
    private int lheight;

    public bool isPathing = false;

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
        long lastTime = System.DateTime.Now.ToFileTime();
        this.xStart = xStart;
        this.yStart = yStart;
        this.xEnd = xEnd;
        this.yEnd = yEnd;
        pathFound = false;
        open.Clear();
        closed.Clear();
        isPathing = true;
        for (int y = 0; y < lheight; y++) {
            for (int x = 0; x < lwidth; x++) {
                if(nodes[x,y].passable)
                    nodes[x, y].Init(this.xEnd, this.yEnd);
            }
        }

        Node start = nodes[this.xStart, this.yStart];
        Node end = nodes[this.xEnd, this.yEnd];
        start.g_value = 0;
        open.Add(start);
        AddOpenNodeToClosed(start); //Put it into the closed list
           
        Node next1 = start;
        Node next2;
        //Look at surrounding nodes and add them to the open list 
        //if they are passable and not yet in any list
        //Assign the node A to them as a parent node and add them to the open list
        //Pick the cheapest node and add it to the closed list
        int attempts = 0;
        while (!pathFound) {          
            next2 = GetCheapestNodeAndInit(next1);        
            AddOpenNodeToClosed(next2);
            next1 = next2;

            if (attempts++ >= 1000) break;
        }
        
        //construct path
        attempts = 0;
        bool startFound = false;
        Node p = end.Parent;
        Node n;
        while(!startFound){
            n = p;         
            result.Add(n.vecPos);
            p = n.Parent;

            if (n.Equals(start)) startFound = true;
            if (attempts++ >= 1000) break;
        }

        result.Add(end.GetVector2());
        result.Reverse();
        isPathing = false;
        long now = System.DateTime.Now.ToFileTime();
        double duration = (now - lastTime)/1000000D;
        if (VERBOSE) Debug.Log("Iteration took "+duration+" ms.");
        return result;
    }

    //Returns the 'cheapest' passable node
    public Node GetCheapestNodeAndInit(Node pnode) {
        int x = pnode.x;
        int y = pnode.y;
        Node res = null;
        Node n;
        for (int yy = y - 1; yy <= y + 1; yy++) {
            if (yy >= lheight) break;
            for (int xx = x - 1; xx <= x + 1; xx++) {
                if (xx >= lwidth) break;      
                if (nodes[xx, yy] == null) continue;
                n = nodes[xx, yy];
                if ((xx == x && yy == y) || !n.passable) continue;
                if (closed.Contains(n)) continue;   
                
                bool isInOpen = open.Contains(n);
                int pncostG = GetGPathCost(pnode, n) + pnode.g_value;
                
                if ((isInOpen && pncostG < n.g_value) || !isInOpen) {                 
                    n.parent = pnode;
                    n.g_value = pncostG;
                }

                if (n.Equals(nodes[xEnd, yEnd])) {
                    pathFound = true;
                    return n;
                }

                if (!isInOpen && n != null) { 
                    open.Add(n);
                    if (res == null) res = n;
                    if (n.GetFValue() < res.GetFValue()) res = n;
                }
                
            }
        }
        if (res == null) {
            res = GetCheapestNode();
        }
        return res;
    }

    public void AddOpenNodeToClosed(Node n) {
        open.Remove(n);
        closed.Add(n);   
    }

    public int GetGPathCost(Node n1, Node n2) {
        if (n1.x == n2.x || n1.y == n2.y) return 10;
        else return 14;
    }

    public Node GetCheapestNode() {
        Node res = open[0];
        for (int i = 0; i < open.Count; i++) {
            Node n = open[i];
            if (res.GetFValue() > n.GetFValue()) res = n;
        }
        return res;
    }
}
