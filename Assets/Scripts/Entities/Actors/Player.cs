using UnityEngine;
using System.Collections.Generic;

public class Player : Actor
{

	#region Variables
	public List<Item> inventoryList;
	public GameObject inventoryObject;
	#endregion

    private InputHandler input;
    private TileMarker tileMarker;

    //testing
    public GameObject pathMarker;
    List<Vector2> path_visual = new List<Vector2>();
    private List<GameObject> markerInstances = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        input = new InputHandler();
        tileMarker = view.GetComponent<TileMarker>();       
    }

    
    void Update()
    {
        if (Input.anyKey) HandleControl(); //Handle input only if there is any input at all
        if (Input.mousePresent) input.MouseUpdate();
    }

    private void HandleControl()
    {
        float xa = 0;
        float ya = 0;

        if (Time.time > view.GetNextCycle())
        {
            input.KeyUpdate(); //updates currently pressed keys

            //apply input results from the last update
            if (input.up) xa = 1;
            if (input.down) xa = -1;
            if (input.left) ya = -1;
            if (input.right) ya = 1;
            if (input.upleft) { ya = -1; xa = 1; }
            if (input.upright) { ya = 1; xa = 1; }
            if (input.downleft) { ya = -1; xa = -1; }
            if (input.downright) { ya = 1; xa = -1; }
            if (input.wait) { view.NextCycle(); return; }

            if (input.lmb) //This block is purely for testing yet
            {
                for (int i = 0; i < markerInstances.Count; i++) {
                    Destroy(markerInstances[i]);
                }
                markerInstances.Clear();
                path_visual.Clear();

                float xEnd = tileMarker.MarkerPosition.x;
                float yEnd = tileMarker.MarkerPosition.y;
                xa = xEnd - xp;
                ya = yEnd - yp;
                int xx = (int)xp;
                int yy = (int)yp;
                while (xx != (int)xEnd) {
                    path_visual.Add(new Vector2(xx, yy));
                    xx += xx < (int)xEnd ? 1 : -1;
                }

                
                while (yy != (int)yEnd) {
                    path_visual.Add(new Vector2(xx, yy));
                    yy += yy < (int)yEnd ? 1 : -1;
                }
                MarkPath(path_visual);
                //MarkPath(pathFinder.GetPath(GetTravelCosts(), (int)xp, (int)yp, (int)xEnd, (int)yEnd));
            }

            //release all keys again
            input.ReleaseAll();

            if (xa == 0 && ya == 0) return; // No movement

            SimpleMove((int)xp + (int)xa, (int)yp + (int)ya);
            view.NextCycle();
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
        
		inventoryList.Add(itemTemp);
		itemTemp.gameObject.transform.parent = inventoryObject.transform;
		itemTemp.renderer.enabled = false;
	}

    public override void OnAttack(int x, int y, float dmg)
    {
        float dtemp = dmg * (float)Gaussian.NextDouble();
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
}
