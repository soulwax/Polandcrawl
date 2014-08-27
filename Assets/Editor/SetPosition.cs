using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Enemy))] 
public class SetPosition : Editor 
{
	override public void  OnInspectorGUI () 
	{
		Enemy colliderCreator = (Enemy)target;
		if(GUILayout.Button("Refresh Map Position")) {
			colliderCreator.setNPCMapPosition(); // how do i call this?
		}
		DrawDefaultInspector();
	}
}
