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
		SetPosition(xp, yp);
	}

	public void Turn()
	{
		Actor somePlayer = checkForPlayer();

		if(somePlayer != null) {
			Debug.Log("Found Enemy: + " + this.gameObject.name + " - " + somePlayer.gameObject.name);
		} else {
			pX = Random.Range(-1, 2);
			pY = Random.Range(-1, 2);
			checkCollisions((int)xp + pX, (int)yp + pY);
		}
	}

	private Actor checkForPlayer()
	{
		//TODO: Previous version was utter poop.
		int startX = (int)transform.position.x - viewRange;
		int endX = (int)transform.position.x + viewRange;

		int startY = (int)transform.position.y - viewRange;
		int endY = (int)transform.position.y + viewRange;

		for(int y = startY; y <= endY; y++) {
			for(int x = startX; x <= endX; x++) {
				if(x >= 0 && y >= 0) {
					if(NPCController.npcMap[x,y] is Player){
						return NPCController.npcMap[x,y] as Actor;
					}
				}
			}
		}
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
					SetPosition(moveToX, moveToY);
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
        
        
    }

    public override void OnDamage(float dmg)
    {
        base.OnDamage(dmg);
        Color col = new Color(1f, 1f, 0f); //yellow for damage you deal as the player
        SpawnDamageText((int)dmg, col);
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
