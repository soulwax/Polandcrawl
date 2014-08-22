using UnityEngine;
using System.Collections.Generic;

public class Player : Actor
{
	#region Variables
	public List<Item> inventoryList;
	public GameObject inventoryObject;
	#endregion

	public void pickupItem(int x, int y)
	{
		//TODO: Add item to inventory
		Item itemTemp = ItemController.itemMap[x, y];
		inventoryList.Add(itemTemp);
		itemTemp.gameObject.transform.parent = inventoryObject.transform;
		itemTemp.renderer.enabled = false;
	}
}
