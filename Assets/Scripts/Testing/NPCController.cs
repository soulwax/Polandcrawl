using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour 
{
	public GameObject[] enemyList;
	public GameObject[] neutralList;

	public static int[,] npcMap;
	
	public void populateEnemies(List<Vector2> viableLocations, int currentLevel, int width, int height)
	{
		//Insialise Map.
		npcMap = new int[width, height];

		// Add random enemys.
		int rndEnemyCount = Random.Range(currentLevel+10 - 3, currentLevel+10 + 3);
		for(int x = 0; x < rndEnemyCount; x++) {
			int rndIndex = Random.Range(0, viableLocations.Count);
			int rndNPC = Random.Range(0, enemyList.Length);
			(Instantiate(enemyList[rndNPC], new Vector3(viableLocations[rndIndex].x, viableLocations[rndIndex].y, 1), new Quaternion(0, 0, 0, 0)) as GameObject).transform.parent = this.gameObject.transform;
			npcMap[(int)viableLocations[rndIndex].x, (int)viableLocations[rndIndex].y] = rndNPC + 1;
		}
	}
}
