using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameView))]
public class TileMarker: MonoBehaviour {

    GameView _tileMap;

    public Color boxColor = new Color(0f,1f,0f);

    private Vector3 markerPos;
    public Transform boxParticle;
	
	void Start() {
        _tileMap = GetComponent<GameView>();
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		
		if(collider.Raycast (ray, out hitInfo, Mathf.Infinity)) {
			Vector3 coordinates = transform.worldToLocalMatrix.MultiplyPoint3x4(hitInfo.point);
			int x = Mathf.FloorToInt(coordinates.x / _tileMap.tileSize);
			int y = Mathf.FloorToInt(coordinates.y / _tileMap.tileSize);		
            markerPos = new Vector3(x * _tileMap.tileSize, y * _tileMap.tileSize + _tileMap.levelHeight, 1.8f);
            boxParticle.localPosition = markerPos;
        } 
	}

    public Vector3 MarkerPosition
    {
        get { return markerPos; }
    }
}