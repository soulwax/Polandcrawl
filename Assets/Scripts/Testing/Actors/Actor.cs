using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
	#region Variables
	public int positionX, positionY;

	public int health;
	public int mana;
	public int damage;
	#endregion

	void Awake()
	{
		positionX = (int)transform.position.x; //Store location as an int
		positionY = (int)transform.position.y;
	}

	public void setPosition(int x, int y) //Set location, used by the player
	{
		positionX = x;
		positionY = y;
		transform.position = new Vector3(x, y, transform.position.z); //Move to location
	}

	public void setPosition(int originX, int originY, int newX, int newY) //Set location, used by NPCs
	{
		NPCController.npcMap[originX, originY] = null; //Empty old location
		NPCController.npcMap[newX, newY] = this; //Store enemy in the new location
		positionX = newX; //Save location
		positionY = newY;
		transform.position = new Vector3(newX, newY, transform.position.z); //Move to location
	}

	public Vector2 getPosition()
	{
		return new Vector2(positionX, positionY);
	}

	public void OnAttack(int x, int y)
	{	
		Actor enemyTemp = NPCController.npcMap[x, y];
		enemyTemp.OnDamage(damage);
	}

	public void OnDamage(int damage)
	{
		health -= damage;

		if(health <= 0) {
			Destroy(this.gameObject);
		}
	}
}
