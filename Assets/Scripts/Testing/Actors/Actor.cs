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
		positionX = (int)transform.position.x;
		positionY = (int)transform.position.y;
	}

	public void setPosition(int x, int y)
	{
		positionX = x;
		positionY = y;
		transform.position = new Vector3(x, y, transform.position.z);
	}

	public void setPosition(int originX, int originY, int newX, int newY)
	{
		NPCController.npcMap[originX, originY] = null;
		NPCController.npcMap[newX, newY] = this;
		positionX = newX;
		positionY = newY;
		transform.position = new Vector3(newX, newY, transform.position.z);
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
