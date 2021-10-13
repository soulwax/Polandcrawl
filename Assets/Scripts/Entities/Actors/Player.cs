using UnityEngine;
using System.Collections.Generic;

public class Player : Actor
{

    #region Variables
    Gaussian rnd = new Gaussian();
    public List<Item> inventoryList;
    public GameObject inventoryObject;
    public Texture2D playerSprite;
    protected PathFinder pathFinder;
    private InputHandler input;
    private TileMarker tileMarker;

    private float manaRecoveryRate = 0.3f;

    //testing
    public GameObject pathMarker;
    List<Vector2> currentPath;
    private List<GameObject> markerInstances = new List<GameObject>();

    private bool hasNextOrder = false; //is true when there is a path to walk along
    private int turnOffset = 0; //the turn offset after the hasNextOrder was called, 
                                //to limit the pathfinder calls during a click spam
    #endregion

    protected override void Awake()
    {
        base.Awake();
        input = new InputHandler();
        tileMarker = view.GetComponent<TileMarker>();       
    }

    protected override void Start() {
        base.Start();
        pathFinder = new PathFinder(view);
    }
    
    protected override void Update()
    {
        input.KeyUpdate(); 
        input.MouseUpdate();
        if (Input.anyKey) HandleControl(); //Handle input only if there is any input at all
        if (turnOffset >= 0) HandleMouseControl();
        if (hasNextOrder && view.IsNextCycle()) {
            WalkAlongPath();
            turnOffset++;
        }


        if(mana < maxMana && view.IsNextCycle()) AdjustMana(manaRecoveryRate);
    }

    private void HandleControl()
    {
        float xa = 0;
        float ya = 0;

        if (input.up) ya = 1;
        if (input.down) ya = -1;
        if (input.left) xa = -1;
        if (input.right) xa = 1;
        if (input.upleft) { xa = -1; ya = 1; }
        if (input.upright) { xa = 1; ya = 1; }
        if (input.downleft) { xa = -1; ya = -1; }
        if (input.downright) { xa = 1; ya = -1; }
        if (input.wait) { view.NextCycle(); return; }
        if (xa == 0 && ya == 0) return; // No movement

        //just abort the movement when you attempt a standard move
        if (hasNextOrder) {
            StopWalking();
            return;
        }
        if (view.IsNextCycle())
        {         
            SimpleMove((int)xp + (int)xa, (int)yp + (int)ya);    
            view.NextCycle();
        }      
        
    }

    public void HandleMouseControl() {
        if (input.lmb && !pathFinder.isPathing) {
            xo = (int)tileMarker.MarkerPosition.x;
            yo = (int)tileMarker.MarkerPosition.y;          
            DisposePathMarkers();
            if (GameView.dungeonMap[xo, yo] == 1) {
                pathFinder.PrepareForNextUse();
                currentPath = pathFinder.GetPath((int)xp, (int)yp, xo, yo);
                MarkPath(currentPath);    
                turnOffset = -2;            
                hasNextOrder = true;                       
            } 
        }
    }

    //I am not sure whether this is better at all, but I deemed it to be a bit safer,
    //mostly because less ifs if tile is not a floor and more precise entity detection
    //as well as simultanous item and entity check (not sure if they will be able to pick 
    //items up, just to be open here) bonus: better type safety.
    protected void SimpleMove(int moveToX, int moveToY)
    {
        if (GameView.dungeonMap[moveToX, moveToY] == 1)
        { //Check to see if our our desired location is floor, thus moveable
            if (NPCController.npcMap[moveToX, moveToY] != null &&
                NPCController.npcMap[moveToX, moveToY].GetType() == typeof(Enemy))
            {
                //TODO: Combat :D:D
                OnAttack(moveToX, moveToY, Damage); //Found and enemy, so attack!
                return;
            }
            if (ItemController.itemMap[moveToX, moveToY] != null)
            {
                pickupItem(moveToX, moveToY);
            }
            SetPosition(moveToX, moveToY); //Nothing in our desired location thus can move freely.
        }
    }

    public override void SetPosition(float x, float y) //Set location, used by the player
    {
        base.SetPosition(x,y);
    }


    public void pickupItem(int x, int y)
    {
        //TODO: Add item to inventory
        Item itemTemp = ItemController.itemMap[x, y];  

        if(itemTemp is Stairs) {
            //Change level
        } else {        
            inventoryList.Add(itemTemp);
            itemTemp.gameObject.transform.parent = inventoryObject.transform;
            itemTemp.GetComponent<Renderer>().enabled = false;
        }
    }

    public override void OnAttack(int x, int y, float dmg)
    {
        float dtemp = dmg * (float)rnd.NextDouble();
        base.OnAttack(x, y, dtemp);     
    }

    public override void OnDamage(float dmg)
    {
        base.OnDamage(dmg);
        Color col = new Color(1f, 0f, 0f); //red for damage dealt to you by the enemy
        SpawnDamageText((int)dmg, col);
    }

    public void MarkPath(List<Vector2> path){
        for (int i = 0; i < path.Count; i++) {
            markerInstances.Add(Instantiate(pathMarker, new Vector3(path[i].x+0.5f, path[i].y+0.5f, -1f), Quaternion.Euler(0,0,0)) as GameObject);
        }
    }

    public void WalkAlongPath() {
        if (currentPath.Count > 0) {
            SimpleMove((int)currentPath[0].x, (int)currentPath[0].y);
            currentPath.RemoveAt(0);           
            view.NextCycle();          
        } else {
            StopWalking();
        }
    }

    public void StopWalking(){
        hasNextOrder = false;
        turnOffset = 0;
        DisposePathMarkers();
        if(currentPath != null) currentPath.Clear();
    }

    public void DisposePathMarkers(){
        for (int i = 0; i < markerInstances.Count; i++) {
            Destroy(markerInstances[i]);
        }
        markerInstances.Clear();
    }
}
