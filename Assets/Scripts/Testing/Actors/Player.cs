using UnityEngine;
using System.Collections;

public class Player : Actor
{
	public void pickupItem(int x, int y)
	{
		//TODO: Add item to inventory
		Item itemTemp = ItemController.itemMap[x, y];
		Destroy(itemTemp.gameObject);
	}
}
