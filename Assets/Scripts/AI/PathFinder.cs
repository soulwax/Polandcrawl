using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A* pathfinder
/// </summary>

public class PathFinder  {
    public static bool VERBOSE = true;

    public class Node {
        public int x, y;
        public int xEnd, yEnd;
        public int g_value; 
        public int h_value; //heuristic
        public int f_value;
        public int open_i;
        public bool passable = false;
        public Node parent;
        public Vector2 vecPos;
        
        public bool inopen = false;
        public bool inclosed = false;

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

        public void SetGValue(int g_value) {
            this.g_value = g_value;
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

        public void Reset() {
            this.inopen = this.inclosed = false;
        }
    }
    private List<Vector2> result = new List<Vector2>();
    private Node[] open;
    private List<Node> closed = new List<Node>();

    private Node[,] nodes;
    private Node start, end, next, p, n;
    private bool pathFound = false;
    private int xStart;
    private int yStart;
    private int xEnd;
    private int yEnd;
    private int lwidth;
    private int lheight;
    public bool isPathing = false;
    public int len_open;
    public int attempts;

    //constructor preloads the nodes
    public PathFinder(GameView view) {
        this.lwidth = view.levelWidth;
        this.lheight = view.levelHeight;
        nodes = new Node[lwidth, lheight];
        open = new Node[lwidth*lheight];
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
        //reversing it
        this.xStart = xEnd;
        this.yStart = yEnd;
        this.xEnd = xStart;
        this.yEnd = yStart;
        pathFound = false;       
        isPathing = true;
        start = nodes[this.xStart, this.yStart];
        end = nodes[this.xEnd, this.yEnd];
        len_open = 0;
        if (start.Equals(end)) goto pastPathing;

        closed.Add(start);
        start.inclosed = true;
        start.inopen = false;
        start.Init(xEnd, yEnd);
        next = start;

        //Look at surrounding nodes and add them to the open list 
        //if they are passable and not yet in any list
        //Assign the node A to them as a parent node and add them to the open list
        //Pick the cheapest node and add it to the closed list
        attempts = 0;
        while (!pathFound) {
            ExpandNode(next);
            next = DrawFromBHeap();
            if (attempts++ >= 10000) break;
        }
    pastPathing: 
        p = end;
        attempts = 0;
        //reconstruct path by begginning at the end node and looking for parent nodes
        while (true) {
            if (p != null) {
                if (p.Equals(start)) break;
                p = p.Parent;
                result.Add(p.vecPos);            
            }
            if (attempts++ >= 10000) break;
        }


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
    public void ExpandNode(Node pnode) {
        int x = pnode.x;
        int y = pnode.y;
        for (int yy = y - 1; yy <= y + 1; yy++) {
            if (yy < 0 || yy >= lheight) continue;
            for (int xx = x - 1; xx <= x + 1; xx++) {
                if (xx < 0 || xx >= lwidth) continue;      
                n = nodes[xx, yy];
                if ((xx == x && yy == y) || !n.passable) continue;
                if (n.inclosed) continue;
                n.Init(this.xEnd, this.yEnd);
                bool isInOpen = n.inopen;
                int newG = GetGPathCost(pnode, n) + pnode.g_value;

                //assign g value or update if costs are cheaper 
                if ((isInOpen && newG < n.g_value) || !isInOpen) {                   
                    n.Parent = pnode;
                    n.SetGValue(newG);
                    if (isInOpen) SetBHeapPosition(n.open_i);
                    else AddToBHeap(n); 
                }

                if (n.Equals(end)) pathFound = true;                                    
            }
        }
    }

    //only use for two neighbouring nodes!
    public int GetGPathCost(Node n1, Node n2) {
        if (n1.x == n2.x || n1.y == n2.y) return 10;
        else return 14;
    }

    //Here comes the binary heap, a technique used by all professional
    //RTS games. It uses an arithmetic insertion and extraction so the heap
    //is always sorted, without having to constantly sort all over again.
    //Speeds up the process 2-3 times at short routes and 10-100 times at long
    //routes

    public void AddToBHeap(Node node) {
        int m = len_open+1;
        node.open_i = m;
        open[m] = node;
        open[m].inopen = true;
        SetBHeapPosition(m);
        len_open = len_open + 1;
    }

    public void SetBHeapPosition(int index) {
        int m = index;
        while (m != 1) {

            if (open[m].GetFValue() <= open[m / 2].GetFValue()) {
                Swap(m, m / 2);
                m /= 2;
            } else break;
        }
    }

    public Node DrawFromBHeap() {
        Node res = open[1]; //take the first = cheapest node out of the heap
        Swap(1, len_open);  //swap the first with the last node in the heap
        open[len_open--] = null; //throw it out and reduce the open index count
        closed.Add(res); //add the cheapest node to the closed list
        res.inclosed = true; //set marker to closed
        res.inopen = false;  
        int u, v, a = 0;
        v = 1;
        
        //bubble the parent, now first node up until it 
        //has reached its proper position in the heap
        //note: this has nothing to do with bubble sort, heap competes
        //with qsort and introsort with efficiency O(n*log n)
 
        do{
            u = v;
            if((2*u+1) <= len_open) { //two children reachable?
                if (open[u].GetFValue() >= open[2*u].GetFValue()) v = 2 * u;
                if (open[v].GetFValue() >= open[2*u+1].GetFValue()) v = 2 * u + 1;
                //good, pick the cheapest child then
            } else { //only one child reachable? look if it's cheaper
                if (2 * u <= len_open)
                    if (open[u].GetFValue() >= open[2*u].GetFValue()) v = 2 * u;
            } 

            //bubble parent up
            if (u != v) {              
                Swap(u, v);
            } else break; //this is the exit that gets taken 100% of the time
                          //unless there are millions of nodes, then 'a' can reach 10k
                          //but your RAM might even run out first then
        } while(a++ <= 10000);    
        return res;
    }


    //Just an array swapper
    public void Swap(int u, int v) {
        //swap the index
        open[u].open_i = v;
        open[v].open_i = u;

        //swap the actual nodes
        Node temp = open[u];
        open[u] = open[v];
        open[v] = temp;
    }

    public void PrepareForNextUse() {
        ResetAllNodes();
        System.Array.Clear(open, 0, len_open);
        closed.Clear();
        result.Clear();
    }

    public void ResetAllNodes() {
        for (int i = 1; i <= len_open; i++) {
            open[i].Reset();
        }
        for (int i = 0; i < closed.Count; i++) {
            closed[i].Reset();
        }
    }
}
