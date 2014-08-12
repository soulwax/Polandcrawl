using UnityEngine;
using System.Collections;

public class Weapon : Item 
{
	public enum WeaponTypes 
	{
		ShortBlade,
		LongBlade,
		Axe,
		Staff,
		Bow
	}

	private WeaponTypes weaponType;
	private int weaponDamage;
	private int weaponRange;
	private bool twoHanded;

	public WeaponTypes WeaponType {
		get {return weaponType;}
		set {weaponType = value;}
	}

	public int WeaponDamage {
		get {return weaponDamage;}
		set {weaponDamage = value;}
	}

	public int WeaponRange {
		get {return weaponRange;}
		set {weaponRange = value;}
	}

	public bool TwoHanded {
		get {return twoHanded;}
		set {twoHanded = value;}
	}
}
