using UnityEngine;
using System.Collections;

public class DungeonGenerator : MonoBehaviour {

	public const float PIX_TO_UNIT = 100; //how many pixels fill one unity internal length

	public bool verbose = true;

	public GameObject basicWall;
	public GameObject basicFloor;

	public int roomAmount = 5;
	public float averageRoomSize = 5f;

	private float floorTileWidth;
	private float floorTileHeight;

	private float wallTileWidth;
	private float wallTileHeight;

	private bool failed = false;

	void Awake() {
		if (basicFloor != null) {
			SpriteRenderer renderer = basicFloor.GetComponent<SpriteRenderer> ();
			if (renderer != null) {
				floorTileWidth = renderer.sprite.textureRect.width / PIX_TO_UNIT;
				floorTileHeight = renderer.sprite.textureRect.height / PIX_TO_UNIT;

				if(floorTileWidth != 0 && floorTileHeight != 0 && verbose) {
					Debug.Log("Floor texture size successfully retreived.");
				}

				wallTileWidth = renderer.sprite.textureRect.width / PIX_TO_UNIT;
				wallTileHeight = renderer.sprite.textureRect.height / PIX_TO_UNIT;

				if(wallTileWidth != 0 && wallTileHeight != 0 && verbose) {
					Debug.Log("Wall texture size successfully retreived.");
				}

			} else {
				if(verbose) {
					Debug.Log("Error retreiving tile information.");
				}
				failed = true;
			}
		}
	}
	
	void Start () {
		if (!failed) {
			GenerateDungeon ();
		}
	}
	
	void Update () {
	
	}

	private void GenerateDungeon() {
		GenerateRooms ();
		GenerateJunctions ();
		FinalizeDungeon ();
	}

	private void GenerateRooms(){
		Rect[] rooms = new Rect[roomAmount];
		for(int i = 0; i < rooms.Length; i++) {
			//room generation comes here
		}
	}

	private void GenerateJunctions(){

	}

	private void FinalizeDungeon(){

	}
}
