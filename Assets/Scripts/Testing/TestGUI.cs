using UnityEngine;
using System.Collections;

public class TestGUI : MonoBehaviour 
{

	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width - (Screen.width * 0.25f) + 5, 5, 200, 20), "Player Position: X, Y");
		GUI.Label(new Rect(Screen.width - (Screen.width * 0.25f) + 5, 25, 200, 20), "Player Health: NUMBER");
		GUI.Label(new Rect(Screen.width - (Screen.width * 0.25f) + 5, 45, 200, 20), "Player Mana: NUMBER");
	}
}
