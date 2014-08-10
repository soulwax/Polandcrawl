using UnityEngine;
using System.Collections;

public class PlayerManager : TileRenderer 
{
	#region Base Variables
	public int
		playerX,
		playerY,
		viewRadius;

	private int
		pX = 0,
		pY = 0;
	#endregion

	#region Other Variables
	public int 
		strength,
		intelligence,
		dexterity,
		baseHP = 100,
		baseMP = 100,
		baseArmour = 0,
		baseEvasion = 0,
		baseBlock = 0;

	#endregion
	
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void applyDamage(int damage)
	{
		// Armour calculations used from Crawl 
		float calcDamage = damage - (damage * ((14 * Mathf.Sqrt(baseArmour-2))/100));
		baseHP -= (int)calcDamage;
	}

	public void setPlayerPosition(int x, int y)
	{
		playerX = x;
		playerY = y;
	}
}
