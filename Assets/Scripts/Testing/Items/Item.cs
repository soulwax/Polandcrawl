using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
	public enum ItemTypes
	{
		Weapon,
		Armour,
		Potion,
		Scroll
	}

	public ItemTypes itemType;
	public string itemName;
	public string itemDescription;

	public void Apply()
	{
		//TODO: Do shit with the item, ie consume
		Destroy(this.gameObject);
	}


}
