using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
	// Used for all character type classes, players, enemies ect.
	public int positionX, positionY;

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
}
