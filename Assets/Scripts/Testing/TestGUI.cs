using UnityEngine;
using System.Collections;

public class TestGUI : MonoBehaviour 
{
	public Player player;

	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width - (Screen.width * 0.25f) + 5, 5, 200, 20), ("Player Position: x-" + player.positionX + " y-" + player.positionY));
		GUI.Label(new Rect(Screen.width - (Screen.width * 0.25f) + 5, 25, 200, 20), "Player Health: NUMBER");
		GUI.Label(new Rect(Screen.width - (Screen.width * 0.25f) + 5, 45, 200, 20), "Player Mana: NUMBER");
	}
}
