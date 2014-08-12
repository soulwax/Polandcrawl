using UnityEngine;
using System.Collections.Generic;

public class ItemController : MonoBehaviour 
{
	public GameObject[] itemList;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void populateItems(List<Vector2> viableLocations)
	{
		// Add random items.
		int rndItemCount = Random.Range(5, 10);
		for(int x = 0; x < rndItemCount; x++) {
			int rndIndex = Random.Range(0, viableLocations.Count);
			int rndItem = Random.Range(0, itemList.Length);
			(Instantiate(itemList[rndItem], new Vector3(viableLocations[rndIndex].x, viableLocations[rndIndex].y, 1), new Quaternion(0, 0, 0, 0)) as GameObject).transform.parent = this.gameObject.transform;
		}

	}
}
