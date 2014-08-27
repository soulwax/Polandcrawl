using UnityEngine;
using System.Collections.Generic;

public class ItemController : MonoBehaviour 
{
	public GameObject[] potionList;
	public GameObject[] weaponList;
	public GameObject[] armourList;
	public GameObject[] scrollList;

	public GameObject stairs;

	public static Item[,] itemMap;

	public void populateItems(List<Vector2> viableLocations, int width, int height)
	{
		//Insialise Map.
		itemMap = new Item[width, height];

		GameObject tempObject;
		Item tempPotion;

		//Add random items.
		int rndItemCount = Random.Range(5, 10);
		for(int x = 0; x < rndItemCount; x++) {
			int rndIndex = Random.Range(0, viableLocations.Count);
			int rndItem = Random.Range(0, potionList.Length);
			tempObject = Instantiate(potionList[rndItem], new Vector3(viableLocations[rndIndex].x, viableLocations[rndIndex].y, 1), new Quaternion(0, 0, 0, 0)) as GameObject;
			tempObject.gameObject.transform.parent = transform;
			tempPotion = tempObject.GetComponent<Item>();
			itemMap[(int)viableLocations[rndIndex].x, (int)viableLocations[rndIndex].y] = tempPotion;
		}

		//Add the stairs - just one.
		int rndLoc = Random.Range(0, viableLocations.Count);
		tempObject = Instantiate(stairs, new Vector3(viableLocations[rndLoc].x, viableLocations[rndLoc].y, 1), new Quaternion(0, 0, 0, 0)) as GameObject;
		tempObject.gameObject.transform.parent = transform;
		Item tempStairs = tempObject.GetComponent<Item>();
		itemMap[(int)viableLocations[rndLoc].x, (int)viableLocations[rndLoc].y] = tempStairs;
	}
}
