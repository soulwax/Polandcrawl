using UnityEngine;
using System.Collections.Generic;

public class GameView : MonoBehaviour 
{
	#region Delegates & Events
	public delegate void ProcessTurn();
	public static event ProcessTurn processTurn;
	#endregion

	#region Variables
	public float cycleRate = 0.5f;
	private float nextCycle = 0.0f;

	public int levelWidth = 100;
	public int levelHeight = 50;

	public float tileSize = 1.0f;
	public int tileResolution = 16;

	public Texture2D dungeonTileset;

	public static int[,] dungeonMap;

	public NPCController npcController;
	public ItemController itemController;

	private int
		playerX,
		playerY,
		pX,
		pY;

	public PlayerTest player;
	public int currentLevel = 1;

    private DungeonVariables variables;
    private int rooms;

	#endregion

	// Use this for initialization
	void Start () 
	{
        GameObject dv = GameObject.Find("DungeonVariables");
        if(dv != null) variables = dv.GetComponent<DungeonVariables>();

        if (variables != null) 
        { 
            Debug.Log("Dungeon Variables found");
            if (variables.size == 1)
            {
                levelWidth = 50;
                levelHeight = 50;
                rooms = 5;
            } 
            else if(variables.size == 2)
            {
                levelWidth = 80;
                levelHeight = 80;
                rooms = 10;
            }

            else if(variables.size == 3)
            {
                levelWidth = 120;
                levelHeight = 120;
                rooms = 15;
            }
        }
        else
        {
            //use standard variables
            levelWidth = 100;
            levelHeight = 50;
            rooms = 10;
        }

       
		// Build mesh and move to correct view point.
		buildMesh(this.gameObject, levelWidth, levelHeight, tileSize);
		transform.position = new Vector3(transform.position.x, transform.position.y + levelHeight, transform.position.z);

		// Generate dungeon map and apply textue.
		dungeonMap = new int[levelWidth, levelHeight];
		buildDungeonTexture(this.gameObject, ChopUpTiles(dungeonTileset, tileResolution), levelWidth, levelHeight, rooms, tileResolution, dungeonMap);


		// Initialise the player and generate list of viable position on the map.
		bool initPlayer = false;
		List<Vector2> viableLocations = new List<Vector2>();

		for(int y = 0; y < levelHeight; y++) {
			for(int x = 0; x < levelWidth; x++) {
				if(dungeonMap[x,y] == 1){
					if(!initPlayer) {
						player.setPosition(x, y);
						initPlayer = true;
					}
					viableLocations.Add(new Vector2(x, y));
				}
			}
		}
		// Initialise Controllers
		npcController.populateEnemies(viableLocations, currentLevel, levelWidth, levelHeight);
		itemController.populateItems(viableLocations, levelWidth, levelHeight);

		/*ConvertToString cTS = new ConvertToString();
		Debug.Log(cTS.Covert(NPCController.npcMap, levelWidth, levelHeight));
		Debug.Log(cTS.Covert(ItemController.itemMap, levelWidth, levelHeight));*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		pY = 0;
		pX = 0;
		
		// Get shitty inputs
		if(Input.GetKey("w") && Time.time > nextCycle){
			pY=1;
			nextCycle = Time.time + cycleRate;
		}
		if(Input.GetKey("s") && Time.time > nextCycle) {
			pY=-1;
			nextCycle = Time.time + cycleRate;
		}
		if(Input.GetKey("d") && Time.time > nextCycle) {
			pX=1;
			nextCycle = Time.time + cycleRate;
		}
		if(Input.GetKey("a") && Time.time > nextCycle) {
			pX=-1;
			nextCycle = Time.time + cycleRate;
		}

		if(Input.GetKey(".") && Time.time > nextCycle) {
			processTurn();
			nextCycle = Time.time + cycleRate;
			return;
		}
		
		if (pX==0 && pY==0) return; // No movement


		//Check collisions
		checkCollisions(player.playerX + pX, player.playerY + pY);
	}

	private void checkCollisions(int moveToX, int moveToY)
	{
		if(NPCController.npcMap[moveToX, moveToY] == 0) {
			if(ItemController.itemMap[moveToX, moveToY] == 0) {
				if(dungeonMap[moveToX, moveToY] == 1) {
					player.setPosition(moveToX, moveToY);
				} else {
					Debug.Log("Hit Wall!");
					return;
				}
			} else {
				//TODO: Pickup item ect.
				Debug.Log("Hit Item!");
			}
		} else {
			//TODO: Combat :D:D
			Debug.Log("Hit Enemy!");
		}

		processTurn();
	}

	private void buildMesh(GameObject gObject, int width, int height, float tileSize) 
	{
		
		int numTiles = width * height;
		int numTris = numTiles * 2;
		
		int vwidth = width + 1;
		int vheight = height + 1;
		int numVerts = vwidth * vheight;
		
		//generate the mesh data
		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[numTris * 3];
		
		int x, y;
		for (y = 0; y < vheight; y++) {
			for (x = 0; x < vwidth; x++) {
				vertices[y * vwidth + x] = new Vector3(x*tileSize,-y*tileSize,0);	
				normals[y * vwidth + x] = new Vector3(0,0,-1);
				uv[y * vwidth + x] = new Vector2((float)x / width, 1f-(float)y / height);
			}
		}
		
		for (y = 0; y < height; y++) {
			for (x = 0; x < width; x++) {
				int squareIndex = y * width + x;
				int triOffset = squareIndex * 6;
				
				triangles[triOffset + 0] = y * vwidth + x + 0;
				triangles[triOffset + 2] = y * vwidth + x + vwidth + 0;
				triangles[triOffset + 1] = y * vwidth + x + vwidth + 1;
				
				triangles[triOffset + 3] = y * vwidth + x + 0;
				triangles[triOffset + 5] = y * vwidth + x + vwidth + 1;
				triangles[triOffset + 4] = y * vwidth + x + 1;
			}
		}
		
		//Create a new Mesh and populate with the data
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		//Assign our mesh to our filter/renderer/collider
		MeshFilter mesh_filter = gObject.GetComponent<MeshFilter> ();
		//MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		MeshCollider mesh_collider = gObject.GetComponent<MeshCollider>();
		
		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
	}

	private int[,] buildDungeonTexture(GameObject gObject, Color[][] tiles, int width, int height, int rooms, int tileResolution, int[,] dungeonMap)
	{		
        TDMap map;
        if(variables != null) map = new TDMap(width, height, rooms, variables.type);
        else map = new TDMap(width, height, rooms);

		int texWidth = width * tileResolution;
		int texHeight = height * tileResolution;
		Texture2D texture = new Texture2D(texWidth, texHeight);
		
		for(int y = 0; y < height; y++) {
			for(int x = 0; x < width; x++) {
				dungeonMap[x, y] = map.GetTileAt(x, y);
				Color[] p = tiles[dungeonMap[x, y]];
				texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
			}	
		}
		
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		
		MeshRenderer mesh_renderer = gObject.GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;

		return dungeonMap;
	}

	public Color[][] ChopUpTiles(Texture2D tileTexture, int tileResolution){
		int numTilesPerRow = tileTexture.width / tileResolution;
		int numRows = tileTexture.height / tileResolution;
		
		Color[][] tiles = new Color[numTilesPerRow*numRows][];
		
		for(int y = 0; y < numRows; y++) {
			for(int x = 0; x < numTilesPerRow; x++) {
				tiles[y * numTilesPerRow + x] = tileTexture.GetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution);
			}	
		}
		
		return tiles;
	}
}
