using UnityEngine;
using System.Collections;

public class GameManager : TileRenderer
{
	public int levelWidth = 100;
	public int levelHeight = 50;

	public Texture2D dungeonTileset;
	public Texture2D playerTileset;

	public float tileSize = 1.0f;
	public int tileResolution = 16;

	public TDMap currentLevel;

	public GameObject 
		dungeonObject,
		playerObject,
		fovObject;

	public int fovRadius = 4;

	// Use this for initialization
	void Start () 
	{
		currentLevel = new TDMap(levelWidth, levelHeight);
		GenerateLevel();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	private void GenerateLevel()
	{
		Color[][] dungeonTiles = ChopUpTiles(dungeonTileset, tileResolution);
		Color[][] playerTiles = ChopUpTiles(playerTileset, tileResolution);
		Color[][] fovTiles = ChopUpTiles(playerTileset, tileResolution);

		BuildMesh(dungeonObject, levelWidth, levelHeight, tileSize);
		BuildMesh(playerObject, levelWidth, levelHeight, tileSize);
		BuildMesh(fovObject, levelWidth, levelHeight, tileSize);

		BuildDungeonTexture(dungeonObject, currentLevel, dungeonTiles, levelWidth, levelHeight, tileResolution);

		for(int y = 0; y < levelHeight; y++) {
			for(int x = 0; x < levelWidth; x++) {
				if(currentLevel.GetTileAt(x,y) == 1) {
					InitialisePlayer(playerObject, playerTiles, levelWidth, levelHeight, tileResolution, x, y);
					RenderFOV(fovObject, x, y, fovRadius, currentLevel, levelWidth, levelHeight, fovTiles, tileResolution);
					break;
				}
			}
		}

	}
}
