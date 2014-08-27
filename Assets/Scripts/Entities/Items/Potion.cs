using UnityEngine;
using System.Collections;

public class Potion : Item 
{
	public enum PotionTypes
	{
		Health,
		Mana,
		Attribute
	}

	private PotionTypes potionType;

	public PotionTypes PotionType {
		get {return potionType;}
		set {potionType = value;}
	}
}
