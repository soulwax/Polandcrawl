using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
	private string itemName;
	private string itemDescription;
	private int itemID;
	private Texture2D itemTile;

	public enum ItemTypes
	{
		Weapon,
		Armour,
		Potion,
		Scroll
	}

	private ItemTypes itemType;

	public string ItemName {
		get {return itemName;}
		set {itemName = value;}
	}

	public string ItemDescription {
		get {return itemDescription;}
		set {itemDescription = value;}
	}

	public int ItemID {
		get {return itemID;}
		set {itemID = value;}
	}

	public Texture2D ItemTile {
		get {return itemTile;}
		set {itemTile = value;}
	}

	public ItemTypes ItemType {
		get {return itemType;}
		set {itemType = value;}
	}
}
