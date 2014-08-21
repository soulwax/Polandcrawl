using UnityEngine;
using System.Collections;

public class Enemy : Actor
{
	public enum EnemyState {
		Wandering,
		Attacking
	}

	public EnemyState enemyState;
	public int viewRange;
	private int pX, pY;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void setNPCMapPosition()
	{
		setPosition(positionX, positionY, positionX, positionY);
	}

	public void Turn()
	{
		/*pX = Random.Range(-1, 2);
		pY = Random.Range(-1, 2);

		checkCollisions(positionX + pX, positionY + pY);*/
		Actor temp = checkForEnemies();
		if(temp != null)
				Debug.Log("Found Enemy: + " + this.gameObject.name + " - " + temp.gameObject.name);
	}

	private Actor checkForEnemies()
	{
		//TODO: Previous version was utter poop.
		return null;
	}

	private void checkCollisions(int moveToX, int moveToY)
	{
		if(NPCController.npcMap[moveToX, moveToY] == null) {
			if(ItemController.itemMap[moveToX, moveToY] == null) {
				if(GameView.dungeonMap[moveToX, moveToY] == 1) {
					setPosition(positionX, positionY, moveToX, moveToY);
					//setPosition(moveToX, moveToY);
				} else {
					//Debug.Log("Enemy Hit Wall!");
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
