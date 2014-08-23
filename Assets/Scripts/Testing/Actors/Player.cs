using UnityEngine;
using System.Collections.Generic;

public class Player : Actor
{
	#region Variables
	public List<Item> inventoryList;
	public GameObject inventoryObject;
	#endregion

    //For a bit more polymorphism :3
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

    public override void OnAttack(int x, int y)
    {
        base.OnAttack(x, y);
        Color col = new Color(1f,1f,0f); //yellow for damage you deal as the player
        SpawnDamageText(damage, col);
    }
}
