using UnityEngine;
using System.Collections;

public class Scroll : Item
{
	public enum ScrollTypes
	{
		EnchantWeapon,
		EnchantArmour,
		Teleportation,
		Curse
	}

	private ScrollTypes scrollType;

	public ScrollTypes ScrollType {
		get {return scrollType;}
		set {scrollType = value;}
	}
}
