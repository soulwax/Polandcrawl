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
	
	public void setNPCMapPosition()
	{
		setPosition(xp, yp);
	}

	public void Turn()
	{
		pX = Random.Range(-1, 2);
		pY = Random.Range(-1, 2);
        checkCollisions((int)xp + pX, (int)yp + pY);
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
        if (NPCController.npcMap[moveToX, moveToY] == null)
        {
            if (ItemController.itemMap[moveToX, moveToY] == null)
            {
                if (GameView.dungeonMap[moveToX, moveToY] == 1)
                {
					setPosition(moveToX, moveToY);
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

    public override void OnAttack(int x, int y, float dmg)
    {
        base.OnAttack(x, y, dmg);
        
        Color col = new Color(1f, 0f, 0f); //red for damage dealt to you by the enemy
        SpawnDamageText(Damage, col);
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
