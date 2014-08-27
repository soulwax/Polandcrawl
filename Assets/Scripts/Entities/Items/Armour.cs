using UnityEngine;
using System.Collections;

public class Armour : Item 
{
	public enum ArmourTypes
	{
		Body,
		Gloves,
		Boots,
		Headgear,
		Cloak
	}

	private ArmourTypes armourType;
	private int armourValue;

	public ArmourTypes ArmourType {
		get {return armourType;}
		set {armourType = value;}
	}

	public int ArmourValue {
		get {return armourValue;}
		set {armourValue = value;}
	}
}
