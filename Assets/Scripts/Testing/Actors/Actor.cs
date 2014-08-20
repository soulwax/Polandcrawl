using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
	#region Variables
	public int positionX, positionY;
	public bool isNPC;

	public int baseHealth;
	public int baseMana;
	public int baseDamage;
	#endregion

	public NPCController enemies;

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
		if(isNPC) {
			NPCController.npcMap[originX, originY] = null;
			NPCController.npcMap[newX, newY] = this;
		}
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
		Debug.Log(enemyTemp.getPosition());
	}

	public void OnDamage(int damage)
	{

	}
}
