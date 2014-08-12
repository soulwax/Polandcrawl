using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour 
{
	public GameObject[] EnemyList;

	private List<Vector2> viableLocations;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void InitialiseNPCController(List<Vector2> viableLocations, int currentLevel)
	{
		this.viableLocations = viableLocations;
		populateEnemies(currentLevel);
	}

	private void populateEnemies(int currentLevel)
	{
		// Add random enemys.
		int rndEnemyCount = Random.Range(currentLevel+10 - 3, currentLevel+10 + 3);
		for(int x = 0; x < rndEnemyCount; x++) {
			int rndIndex = Random.Range(0, viableLocations.Count);
			int rndNPC = Random.Range(0, EnemyList.Length);
			(Instantiate(EnemyList[rndNPC], new Vector3(viableLocations[rndIndex].x, viableLocations[rndIndex].y, 1), new Quaternion(0, 0, 0, 0)) as GameObject).transform.parent = this.gameObject.transform;
		}
	}
}
