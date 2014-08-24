using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour 
{
	public GameObject[] enemyList;
	public GameObject[] neutralList;

	public static Actor[,] npcMap;
	
	public void populateEnemies(List<Vector2> viableLocations, int currentLevel, int width, int height)
	{
		//Insialise Map.
		npcMap = new Actor[width, height];
		GameObject tempObject;
		Actor tempActor;

		// Add random enemys.
		int rndEnemyCount = Random.Range(currentLevel+30 - 3, currentLevel+30 + 3);
		//int rndEnemyCount = 2;
		for(int x = 0; x < rndEnemyCount; x++) {
			int rndIndex = Random.Range(0, viableLocations.Count);
			int rndNPC = Random.Range(0, enemyList.Length);
			tempObject = Instantiate(enemyList[rndNPC], new Vector3(viableLocations[rndIndex].x, viableLocations[rndIndex].y, 1), new Quaternion(0, 0, 0, 0)) as GameObject;
			tempObject.gameObject.transform.parent = transform;
			tempActor = tempObject.GetComponent<Actor>();
			npcMap[(int)viableLocations[rndIndex].x, (int)viableLocations[rndIndex].y] = tempActor;
		}
	}
}
