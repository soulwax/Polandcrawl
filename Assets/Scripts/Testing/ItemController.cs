using UnityEngine;
using System.Collections.Generic;

public class ItemController : MonoBehaviour 
{
	public GameObject[] itemList;

	public static int[,] itemMap;

	public void populateItems(List<Vector2> viableLocations, int width, int height)
	{
		//Insialise Map.
		itemMap = new int[width, height];

		// Add random items.
		int rndItemCount = Random.Range(5, 10);
		for(int x = 0; x < rndItemCount; x++) {
			int rndIndex = Random.Range(0, viableLocations.Count);
			int rndItem = Random.Range(0, itemList.Length);
			(Instantiate(itemList[rndItem], new Vector3(viableLocations[rndIndex].x, viableLocations[rndIndex].y, 1), new Quaternion(0, 0, 0, 0)) as GameObject).transform.parent = this.gameObject.transform;
			itemMap[(int)viableLocations[rndIndex].x, (int)viableLocations[rndIndex].y] = rndItem + 1;
		}
	}
}
