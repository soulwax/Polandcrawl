﻿using UnityEngine;
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

		GameObject tempEnemy;
		Actor tempActor;

		// Add random enemys.
		int rndEnemyCount = Random.Range(currentLevel+10 - 3, currentLevel+10 + 3);
		for(int x = 0; x < rndEnemyCount; x++) {
			int rndIndex = Random.Range(0, viableLocations.Count);
			int rndNPC = Random.Range(0, enemyList.Length);
			tempEnemy = Instantiate(enemyList[rndNPC], new Vector3(viableLocations[rndIndex].x, viableLocations[rndIndex].y, 1), new Quaternion(0, 0, 0, 0)) as GameObject;
			tempEnemy.gameObject.transform.parent = transform;
			tempActor = tempEnemy.AddComponent<Enemy>();
			npcMap[(int)viableLocations[rndIndex].x, (int)viableLocations[rndIndex].y] = tempActor;
		}
	}
}
