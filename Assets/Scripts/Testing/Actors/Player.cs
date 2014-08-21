using UnityEngine;
using System.Collections.Generic;

public class Player : Actor
{
	#region Variables
	public List<Item> invetoryList;
	#endregion

	public void pickupItem(int x, int y)
	{
		//TODO: Add item to inventory
		Item itemTemp = ItemController.itemMap[x, y];
		//invetoryList.Add(invenTemp);
		Destroy(itemTemp.gameObject);
	}
}
