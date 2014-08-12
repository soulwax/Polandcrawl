using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour 
{
	public GameObject[] enemyList;
	public GameObject[] neutralList;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void populateEnemies(List<Vector2> viableLocations, int currentLevel)
	{
		// Add random enemys.
		int rndEnemyCount = Random.Range(currentLevel+10 - 3, currentLevel+10 + 3);
		for(int x = 0; x < rndEnemyCount; x++) {
			int rndIndex = Random.Range(0, viableLocations.Count);
			int rndNPC = Random.Range(0, enemyList.Length);
			(Instantiate(enemyList[rndNPC], new Vector3(viableLocations[rndIndex].x, viableLocations[rndIndex].y, 1), new Quaternion(0, 0, 0, 0)) as GameObject).transform.parent = this.gameObject.transform;
		}
	}
}
