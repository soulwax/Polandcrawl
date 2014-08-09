using UnityEngine;
using System.Collections;

public class TileRenderer : MonoBehaviour
{
	public void RenderFOV(GameObject gObject, int playerX, int playerY, int radius, TDMap map, int width, int height, Color[][] tiles, int tileResolution)
	{
		int texWidth = width * tileResolution;
		int texHeight = height * tileResolution;
		Texture2D texture = new Texture2D(texWidth, texHeight);

		int[,] fov = CalcFOV(playerX, playerY, radius, map, width, height);

		Color alphaColor = Color.white;
		alphaColor.a = 0;

		Color[] p = tiles[1];
		Color[] b = tiles[0];

		for(int y = 0; y < height; y++) {
			for(int x = 0; x < width; x++) {
				if(x == playerX && y == playerY) {
					texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
				} else if(fov[x, y] == 1) {
					texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
				} else if(fov[x, y] == 2) {
					texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
				} else if(fov[x, y] == 3) {
					texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
				} else {
					texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, b);
				}
			}
		}

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		
		MeshRenderer mesh_renderer = gObject.GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
	}

	public void BuildDungeonTexture(GameObject gObject, TDMap map, Color[][] tiles, int width, int height, int tileResolution)
	{		
		int texWidth = width * tileResolution;
		int texHeight = height * tileResolution;
		Texture2D texture = new Texture2D(texWidth, texHeight);

		
		for(int y = 0; y < height; y++) {
			for(int x = 0; x < width; x++) {
				Color[] p = tiles[map.GetTileAt(x,y)];
				texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
			}	
		}
		
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		
		MeshRenderer mesh_renderer = gObject.GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
	}

	public void InitialisePlayer(GameObject gObject, Color[][] tiles, int width, int height, int tileResolution, int playerX, int playerY)
	{
		int texWidth = width * tileResolution;
		int texHeight = height * tileResolution;
		Texture2D texture = new Texture2D(texWidth, texHeight);

		Color alphaColor = Color.white;
		alphaColor.a = 0;

		for(int y = 0; y < height; y++) {
			for(int x = 0; x < width; x++) {
				if(x == playerX && y == playerY) {
					Color[] p = tiles[0];
					texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
				} else {
					Color[] s = tiles[1];
					texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, s);
				}
			}
		}

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		
		MeshRenderer mesh_renderer = gObject.GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
	}

	public void BuildMesh(GameObject gObject, int width, int height, float tileSize) 
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

	public int[,] CalcFOV(int playerX, int playerY, int radius, TDMap map, int width, int height)
	{
		int[,] fovMap  = new int[width, height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				
				// Calc black locations.
				if(x >= playerX + radius)
					fovMap[x, y] = '0';
				else if(x <= playerX - radius)
					fovMap[x, y] = '0';
				else if(y >= playerY + radius)
					fovMap[x, y] = '0';
				else if(y <= playerY - radius)
					fovMap[x, y] = '0';
				else
					fovMap[x, y] = map.GetTileAt(x, y);
			}
		}
		
		return fovMap;
	}
}
