using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class TileMap : MonoBehaviour {

	public int size_x = 100;
	public int size_y = 50;
	public float tileSize = 1.0f;
	public float scatterRate = 1.0f;

	public Texture2D terrainTiles;
	public int tileResolution = 16;

	private int numTilesPerRow;
	private int numRows;

	// Use this for initialization
	void Start() {
		numTilesPerRow = terrainTiles.width / tileResolution;
		numRows = terrainTiles.height / tileResolution;

		BuildMesh();
	}

	Color[][] ChopUpTiles(){
		Color[][] tiles = new Color[numTilesPerRow*numRows][];

		for(int y = 0; y < numRows; y++){
			for(int x = 0; x < numTilesPerRow; x++){
				tiles[y * numTilesPerRow + x] = terrainTiles.GetPixels(x*tileResolution,y*tileResolution,tileResolution,tileResolution);
			}
		}
		return tiles;
	}

	void BuildTexture() {


		int texWidth = size_x * tileResolution;
		int texHeight = size_y * tileResolution;
		Texture2D texture = new Texture2D(texWidth,texHeight);

		Color[][] tiles = ChopUpTiles();

		for(int y = 0; y < size_y; y++){
			for(int x = 0; x < size_x; x++){
				Color[] p = tiles[Random.Range (0,4)];
				texture.SetPixels(x*tileResolution,y*tileResolution,tileResolution,tileResolution, p);
			}
		}
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();

		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
	}

	//Generates a mesh for the tilemap
	//TODO: continuing Groundwork
	public void BuildMesh() {

		int numTiles = size_x * size_y;
		int numTris = numTiles * 2;

		int vsize_x = size_x + 1;
		int vsize_y = size_y + 1;
		int numVerts = vsize_x * vsize_y;

		//generate the mesh data
		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];

		int[] triangles = new int[numTris * 3];

		int x, y;
		for (y = 0; y < vsize_y; y++) {
			for (x = 0; x < vsize_x; x++) {
				float rnd = Random.Range(-scatterRate,scatterRate);
				vertices[y * vsize_x + x] = new Vector3(x*tileSize,y*tileSize,rnd);	
				normals[y * vsize_x + x] = new Vector3(rnd,-rnd,-1);
				uv[y * vsize_x + x] = new Vector2((float)x / size_x, (float)y / size_y);
			}
		}

		for (y = 0; y < size_y; y++) {
			for (x = 0; x < size_x; x++) {
				int squareIndex = y * size_x + x;
				int triOffset = squareIndex * 6;

				triangles[triOffset + 0] = y * vsize_x + x +           0;
				triangles[triOffset + 1] = y * vsize_x + x + vsize_x + 0;
				triangles[triOffset + 2] = y * vsize_x + x + vsize_x + 1;

				triangles[triOffset + 3] = y * vsize_x + x +           0;
				triangles[triOffset + 4] = y * vsize_x + x + vsize_x + 1;
				triangles[triOffset + 5] = y * vsize_x + x +           1;
			}
		}

		//Create a new Mesh and populate with the data
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

		//Assign our mesh to our filter/renderer/collider
		MeshFilter mesh_filter = GetComponent<MeshFilter> ();
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();

		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;

		BuildTexture();
	}
}
