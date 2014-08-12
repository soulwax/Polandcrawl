using UnityEngine;
using System.Collections;

public class PlayerTest : MonoBehaviour 
{
	public int playerX, playerY;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void setPosition(int x, int y)
	{
		playerX = x;
		playerY = y;
		transform.position = new Vector3(x, y, transform.position.z);
	}
}
