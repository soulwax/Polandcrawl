using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TGMap))]
public class TGMapInspector : MonoBehaviour {

	TGMap _tileMap;
	public Transform selectionCube;

	void Start() {
		_tileMap = GetComponent<TGMap>();
		selectionCube.localScale *= _tileMap.tileSize;
	}

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		if(collider.Raycast (ray, out hitInfo, Mathf.Infinity)) {
			Vector3 coordinates = transform.worldToLocalMatrix.MultiplyPoint3x4(hitInfo.point);
			int x = Mathf.FloorToInt(coordinates.x / _tileMap.tileSize);
			int y = Mathf.FloorToInt(coordinates.y / _tileMap.tileSize);
			float offs = _tileMap.tileSize / 2;
			selectionCube.localPosition = new Vector3(x*_tileMap.tileSize+offs,y*_tileMap.tileSize+offs,-offs);
		} else {
		}
	}
}
