using UnityEngine;
using System.Collections.Generic;

public class Player : Actor
{

	#region Variables
	public List<Item> inventoryList;
	public GameObject inventoryObject;
	#endregion

    private int pX, pY;

    private InputHandler input;
    private TileMarkerScript tileMarkerScript;

    protected override void Awake()
    {
        base.Awake();
        input = new InputHandler();
        tileMarkerScript = view.GetComponent<TileMarkerScript>();       
    }

    
    void Update()
    {
        if (Input.anyKey) HandleControl(); //Handle input only if there is any input at all
        if (Input.mousePresent) input.MouseUpdate();
    }

    private void HandleControl()
    {
        pY = 0;
        pX = 0;

        if (Time.time > view.GetNextCycle())
        {
            input.KeyUpdate(); //updates currently pressed keys

            //apply input results from the last update
            if (input.up) pY = 1;
            if (input.down) pY = -1;
            if (input.left) pX = -1;
            if (input.right) pX = 1;
            if (input.upleft) { pX = -1; pY = 1; }
            if (input.upright) { pX = 1; pY = 1; }
            if (input.downleft) { pX = -1; pY = -1; }
            if (input.downright) { pX = 1; pY = -1; }
            if (input.wait) { view.NextCycle(); return; }

            if (input.lmb)
            {
                float x = tileMarkerScript.MarkerPosition.x;
                float y = tileMarkerScript.MarkerPosition.y;
                pX = (int)(x - xp);
                pY = (int)(y - yp);
            }

            //release all keys again
            input.ReleaseAll();

            if (pX == 0 && pY == 0) return; // No movement

            checkCollisions((int)xp + (int)pX, (int)yp + (int)pY);
            view.NextCycle();
        }   
    }

    public override void setPosition(float x, float y) //Set location, used by the player
    {
        base.setPosition(x,y);
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
        Color col = new Color(1f,1f,0f); //yellow for damage you deal as the player
        SpawnDamageText(dtemp, col);
    }

    //I am not sure whether this is better at all, but I deemed it to be a bit safer,
    //mostly because less ifs if tile is not a floor and more precise entity detection
    //as well as simultanous item and entity check (not sure if they will be able to pick 
    //items up, just to be open here) bonus: better type safety.
    private void checkCollisions(int moveToX, int moveToY)
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
            setPosition(moveToX, moveToY); //Nothing in our desired location thus can move freely.
        }      
    }
}
