using UnityEngine;
using System.Collections;

public class GameManager : TileRenderer
{
	#region Variables
	public int levelWidth = 100;
	public int levelHeight = 50;

	public Texture2D dungeonTileset;
	public Texture2D playerTileset;
	public Texture2D fovTileset;

	public float tileSize = 1.0f;
	public int tileResolution = 16;

	public TDMap currentLevel;

	public GameObject 
		dungeonObject,
		playerObject,
		fovObject;

	public int fovRadius = 4;

	private int
		playerX,
		playerY,
		pX = 0,
		pY = 0;

	public float cycleRate = 0.5f;
	private float nextCycle = 0.0f;

	private Color[][] dungeonTiles;
	private Color[][] playerTiles;
	private Color[][] fovTiles;
	#endregion

	// Use this for initialization
	void Start () 
	{
		currentLevel = new TDMap(levelWidth, levelHeight);
		dungeonTiles = ChopUpTiles(dungeonTileset, tileResolution);
		playerTiles = ChopUpTiles(playerTileset, tileResolution);
		fovTiles = ChopUpTiles(fovTileset, tileResolution);

		GenerateLevel();
	}
	
	// Update is called once per frame
	void Update () 
	{
		playerInput();
	}

	private void playerInput()
	{
		pY = 0;
		pX = 0;
		
		// Get shitty inputs
		if(Input.GetKey("w") && Time.time > nextCycle){
			pY=1;
			nextCycle = Time.time + cycleRate;
		}
		if (Input.GetKey("s") && Time.time > nextCycle) {
			pY=-1;
			nextCycle = Time.time + cycleRate;
		}
		if (Input.GetKey("d") && Time.time > nextCycle) {
			pX=1;
			nextCycle = Time.time + cycleRate;
		}
		if (Input.GetKey("a") && Time.time > nextCycle) {
			pX=-1;
			nextCycle = Time.time + cycleRate;
		}
		
		if (pX==0 && pY==0) return; // No movement
		
		if(currentLevel.GetTileAt(playerX + pX, playerY + pY) == 1) {
			playerX += pX;
			playerY += pY;
			RenderPlayer(playerObject, playerTiles, levelWidth, levelHeight, tileResolution, playerX, playerY);
			RenderFOV(fovObject, playerX, playerY, fovRadius, currentLevel, levelWidth, levelHeight, fovTiles, tileResolution);
		}
	}

	private void GenerateLevel()
	{
		BuildMesh(dungeonObject, levelWidth, levelHeight, tileSize);
		BuildMesh(playerObject, levelWidth, levelHeight, tileSize);
		BuildMesh(fovObject, levelWidth, levelHeight, tileSize);

		BuildDungeonTexture(dungeonObject, currentLevel, dungeonTiles, levelWidth, levelHeight, tileResolution);

		for(int y = 0; y < levelHeight; y++) {
			for(int x = 0; x < levelWidth; x++) {
				if(currentLevel.GetTileAt(x,y) == 1) {
					RenderPlayer(playerObject, playerTiles, levelWidth, levelHeight, tileResolution, x, y);
					RenderFOV(fovObject, x, y, fovRadius, currentLevel, levelWidth, levelHeight, fovTiles, tileResolution);
					playerX = x;
					playerY = y;
					break;
				}
			}
		}

	}
}
