using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(TileMap))]
public class TileMapInspector : Editor {

	//just a random variable
	float v = 54.0f;

	public override void OnInspectorGUI(){
		DrawDefaultInspector();

		//Slider doesn't do anything yet
		EditorGUILayout.BeginVertical();
		v = EditorGUILayout.Slider (v, 0, 2.0f);
		EditorGUILayout.EndVertical();

		if(GUILayout.Button ("Regenerate")) {
			 TileMap tileMap = (TileMap) target;
			 tileMap.BuildMesh();
		}
	}	
}
