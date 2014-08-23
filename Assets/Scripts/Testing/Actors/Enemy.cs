﻿using UnityEngine;
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

    public void setPosition(float originX, float originY, float newX, float newY) //Set location, used by NPCs
    {
        NPCController.npcMap[(int)originX, (int)originY] = null; //Empty old location
        NPCController.npcMap[(int)newX, (int)newY] = this; //Store enemy in the new location
        base.setPosition(newX, newY);
    }

	public void setNPCMapPosition()
	{
		setPosition(xp, yp, xp, yp);
	}

	public void Turn()
	{
		/*pX = Random.Range(-1, 2);
		pY = Random.Range(-1, 2);

		checkCollisions(xp + pX, yp + pY);
		Actor temp = checkForEnemies();
		if(temp != null)
				Debug.Log("Found Enemy: + " + this.gameObject.name + " - " + temp.gameObject.name);*/
	}

	private Actor checkForEnemies()
	{
		//TODO: Previous version was utter poop.
		return null;
	}

	private void checkCollisions(float moveToX, float moveToY)
	{
        if (NPCController.npcMap[(int)moveToX, (int)moveToY] == null)
        {
            if (ItemController.itemMap[(int)moveToX, (int)moveToY] == null)
            {
                if (GameView.dungeonMap[(int)moveToX, (int)moveToY] == 1)
                {
					setPosition(xp, yp, moveToX, moveToY);
					//setPosition(moveToX, moveToY);
				} else {
					//Debug.Log("Enemy Hit Wall!");
					return;
				}
			} else {
				//TODO: Pickup item ect.
			}
		} else {
			//TODO: Combat :D:D
		}
	}

    public override void OnAttack(int x, int y)
    {
        base.OnAttack(x, y);
        
        Color col = new Color(1f, 0f, 0f); //red for damage dealt to you by the enemy
        SpawnDamageText(damage, col);
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
