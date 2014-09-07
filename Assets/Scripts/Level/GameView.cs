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
    public int rooms;

	public float tileSize = 1.0f;
	public int tileResolution = 16;

	public Texture2D dungeonTileset;
	public Texture2D dungeonTilesetFlipped;

	public static byte[,] dungeonMap;

	public NPCController npcController;
	public ItemController itemController;

	private int
		playerX,
		playerY;

	public Player player;
	public int currentLevel = 1;

    private DungeonVariables variables;
    public Texture2D levelTexture;
    public Color[][] spriteSheet;
    public Color[][] spriteSheetFlipped;
    
    public static byte[] tiles;
    public static int numRows;
    public static int numTilesPerRow;
    //private byte[] data;

	#endregion

	// Use this for initialization
	void Awake () 
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
                rooms = 10;
            } 
            else if(variables.size == 2)
            {
                levelWidth = 80;
                levelHeight = 80;
                rooms = 25;
            }

            else if(variables.size == 3)
            {
                levelWidth = 120;
                levelHeight = 120;
                rooms = 35;
            }
        }
        else //handling so far only includes using the standard variables
        {
        }

		int texWidth = levelWidth * tileResolution;
		int texHeight = levelHeight * tileResolution;
		levelTexture = new Texture2D(texWidth, texHeight);

		// Build mesh and move to correct view point.
		buildMesh(this.gameObject, levelWidth, levelHeight, tileSize);
		transform.position = new Vector3(transform.position.x, transform.position.y + levelHeight, transform.position.z);

		// Generate dungeon map and apply textue.
		dungeonMap = new byte[levelWidth, levelHeight];
		spriteSheet = ChopUpTiles(dungeonTileset, tileResolution);
		spriteSheetFlipped = ChopUpTiles(dungeonTilesetFlipped, tileResolution);
		BuildLevel(levelWidth, levelHeight, tileResolution);


		// Initialise the player and generate list of viable position on the map.
		bool initPlayer = false;
		List<Vector2> viableLocations = new List<Vector2>();

		for(int y = 0; y < levelHeight; y++) {
			for(int x = 0; x < levelWidth; x++) {
				if(dungeonMap[x,y] == 1){
					if(!initPlayer) {
						player.setPositionDirectly(x, y);
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

	private void BuildLevel(int width, int height, float tileResolution) {
        
        /*TDMap map;
        if(variables != null) map = new TDMap(width, height, rooms, variables.type);
        else map = new TDMap(width, height, rooms);*/     	

        int depth = 2;
       	byte[][] map = LevelGenerator.CreateAndValidateUndergroundMap(width, height, depth);
       	tiles = map[0];

		
		ApplyZeroLayer();
		ApplyFirstLayer();
		ApplySecondLayer();
		FinalizeLevelTexture(levelTexture);
	}

	public Color[][] ChopUpTiles(Texture2D tileTexture, int tileResolution){
		numTilesPerRow = tileTexture.width / tileResolution;
		numRows = tileTexture.height / tileResolution;
		
		Color[][] ctiles = new Color[numTilesPerRow*numRows][];
		
		for(int y = 0; y < numRows; y++) {
			for(int x = 0; x < numTilesPerRow; x++) {
				ctiles[y * numTilesPerRow + x] = tileTexture.GetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution);
			}	
		}
		
		return ctiles;
	}

    public void NextCycle()
    {
        nextCycle = Time.time + cycleRate;
        processTurn();
    }

    public float GetNextCycle()
    {
        return nextCycle;
    }

    public bool IsNextCycle(){
    	return Time.time > nextCycle;
    }

    public Tile GetTileSprite(int id){
    	return Tile.tiles[(byte)id];
    }

    public byte GetTile(int x, int y) {
    	if(x < 0 || x >= levelWidth) return 255;
    	if(y < 0 || y >= levelHeight) return 255;
    	return tiles[x+y*levelWidth];
    }

    public void DrawOnTexture(int x, int y, Texture2D texture, Color[] sprite, int tileResolution){

    	//texture.SetPixels(x, y, tileResolution, tileResolution, sprite);
    	for(int yy = y, _y = 0; yy < y + tileResolution; yy++, _y++) {
    		for(int xx = x, _x = 0; xx < x + tileResolution; xx++, _x++) {
    			Color c = sprite[_x+_y*tileResolution];
    			if(c.a == 0) continue;	
    			texture.SetPixel(xx, yy, c); 			
    		}		   		
    	}
    }

    public void ApplyZeroLayer(){
    	for(int y = levelHeight-1; y >= 0; y--) {
			for(int x = levelWidth-1; x >= 0; x--) {						
				DrawOnTexture(x*tileResolution, y*tileResolution, levelTexture, spriteSheet[1], tileResolution);
			}	
		}
    }


    public void ApplyFirstLayer(){
    	for(int y = levelHeight-1; y >= 0; y--) {
			for(int x = levelWidth-1; x >= 0; x--) {
				int i = x+y*levelWidth;
				dungeonMap[x,y] = tiles[i];
				if(tiles[i]==Tile.rock.id) continue;						
				Tile tile = GetTileSprite(dungeonMap[x,y]);			
				tile.RenderTile(x, y);
			}	
		}
    }

    public void ApplySecondLayer(){
    	for(int y = levelHeight-1; y >= 0; y--) {
			for(int x = levelWidth-1; x >= 0; x--) {
				int i = x+y*levelWidth;
				if(tiles[i]!=Tile.rock.id) continue;
				Tile tile = GetTileSprite(dungeonMap[x,y]);
				tile.RenderTile(x, y);
			}	
		}
    }

    public void FinalizeLevelTexture(Texture2D texture) {
    	texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
    }
}
