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

	// Use this for initialization
	void Start () {
		BuildMesh ();
	}

	//Generates a mesh for the tilemap
	//TODO: continuing Groundwork
	public void BuildMesh () {

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
				vertices[y * vsize_x + x] = new Vector3(x*tileSize,y*tileSize,0);	
				normals[y * vsize_x + x] = Vector3.up;
				uv[y * vsize_x + x] = new Vector2((float)x / vsize_x, (float)y / vsize_y);
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
	}
}
