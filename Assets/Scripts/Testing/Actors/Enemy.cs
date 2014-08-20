using UnityEngine;
using System.Collections;

public class Enemy : Actor
{
	private int pX, pY;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Turn()
	{
		pX = Random.Range(-1, 2);
		pY = Random.Range(-1, 2);

		checkCollisions(positionX + pX, positionY + pY);	
	}

	private void checkCollisions(int moveToX, int moveToY)
	{
		if(NPCController.npcMap[moveToX, moveToY] == 0) {
			if(ItemController.itemMap[moveToX, moveToY] == 0) {
				if(GameView.dungeonMap[moveToX, moveToY] == 1) {
					setPosition(moveToX, moveToY);
				} else {
					Debug.Log("Enemy Hit Wall!");
					return;
				}
			} else {
				//TODO: Pickup item ect.
				Debug.Log("Enemy Hit Item!");
			}
		} else {
			//TODO: Combat :D:D
			Debug.Log("Enemy Hit Enemy!");
		}
	}

	void OnEnable()
	{
		GameView.processTurn += Turn;
	}

	void OnDisable()
	{
		GameView.processTurn -= Turn;
	}
}
