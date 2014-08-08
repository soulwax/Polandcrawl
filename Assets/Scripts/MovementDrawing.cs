/**
 * Some script of the internets changed to actually be useful.
 * 
 * TODO: The texture is draw upside down, so all inputs are taken inversed.
 */
using UnityEngine;
using System.Collections;
using System;

public class MovementDrawing : MonoBehaviour 
{
	// Dem varriableses
	public float cycleRate = 0.5f;
	private float nextCycle = 0.0f;

	public int tileReso = 10;
	public int fovRadius = 3;

	private int width, height;

	private int pX = 0;
	private int pY = 0;
	private int playerX;
	private int playerY;

	private char[,] map;
	private Texture2D texture;
	private Texture2D fovTexture;
	private Texture2D playerTexture;

	public GameObject fovObject;
	public GameObject playerObject;

	private bool dungeonRendered = false;

	// Const
	private const char floorMap = '.';
	private const char wallMap = '#';
	private const char playerMap = '@';
	private const char blankMap = 'b';

	private FieldOfVision fovCalc;

	#region Map
	private const string mapString = @"
#####bbbbb
#...######
#........#
########.#
bbbbbbb#.#
bbbbbbb#.#
bbb#####.#
bbb#.....#
bbb#.....#
bbb#######
";

	#endregion

	// Use this for initialization
	void Start () 
	{
		fovCalc = new FieldOfVision();

		width = (int)renderer.bounds.size.x*10 / tileReso;
		height = (int)renderer.bounds.size.y*10 / tileReso; // This is a retarded way and based on rendering size, I'm just a lazy cunt.

		map = new char[width, height];

		texture = new Texture2D(width, height);
		renderer.material.mainTexture = texture;
		renderer.material.mainTexture.filterMode = FilterMode.Point;

		fovTexture = new Texture2D(width, height);
		fovObject.renderer.material.mainTexture = fovTexture;
		fovObject.renderer.material.mainTexture.filterMode = FilterMode.Point;

		playerTexture = new Texture2D(width, height);
		playerObject.renderer.material.mainTexture = playerTexture;
		playerObject.renderer.material.mainTexture.filterMode = FilterMode.Point;


		string[] mapChars = mapString.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

		for(int y = 0; y < height; y++){
			for(int x = 0; x < width; x++) {
				map[x, y] = mapChars[y][x];
			}
		}

		playerX = 1;
		playerY = 1;

		Render ();
	}

	void Render ()
	{
		Color alphaColor = Color.white;
		alphaColor.a = 0;

		#region Dungeon render
		if(!dungeonRendered) {
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					if(map[x, y] == floorMap) {
						texture.SetPixel(x, y, Color.blue);
					} else if(map[x, y] == wallMap){
						texture.SetPixel(x, y, Color.red);
					} else {
						texture.SetPixel(x, y, Color.black);
					}
				}
			}

			texture.Apply();

			MeshRenderer dungeon_mesh_renderer = GetComponent<MeshRenderer>();
			dungeon_mesh_renderer.sharedMaterials[0].mainTexture = texture;
			dungeonRendered = true;
		}
		#endregion

		#region Player render
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				
				if (x == playerX && y == playerY) {
					playerTexture.SetPixel(x, y, Color.green);
				} else {
					playerTexture.SetPixel(x, y, alphaColor);
				}
			}
		}

		playerTexture.Apply();

		MeshRenderer player_mesh_render = playerObject.GetComponent<MeshRenderer>();
		player_mesh_render.sharedMaterials[0].mainTexture = playerTexture;
		#endregion

		#region FOV render
		char[,] fovMap = fovCalc.CalcFOV(playerX, playerY, fovRadius, map, width, height);

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {				
				if (x == playerX && y == playerY) {
					fovTexture.SetPixel(x, y, alphaColor);
				} else if(fovMap[x, y] == floorMap) {
					fovTexture.SetPixel(x, y, alphaColor);
				} else if(fovMap[x, y] == wallMap){
					fovTexture.SetPixel(x, y, alphaColor);
				} else {
					fovTexture.SetPixel(x, y, Color.black);
				}
			}
		}

		fovTexture.Apply();

		MeshRenderer fov_mesh_renderer = fovObject.GetComponent<MeshRenderer>();
		fov_mesh_renderer.sharedMaterials[0].mainTexture = fovTexture;
		#endregion

	}

	// Update is called once per frame
	void Update () 
	{
		pY = 0;
		pX = 0;

		// Get shitty inputs
		if(Input.GetKey("w") && Time.time > nextCycle){
			pY=-1;
			nextCycle = Time.time + cycleRate;
		}
		if (Input.GetKey("s") && Time.time > nextCycle) {
			pY=1;
			nextCycle = Time.time + cycleRate;
		}
		if (Input.GetKey("d") && Time.time > nextCycle) {
			pX=-1;
			nextCycle = Time.time + cycleRate;
		}
		if (Input.GetKey("a") && Time.time > nextCycle) {
			pX=1;
			nextCycle = Time.time + cycleRate;
		}

		if (pX==0 && pY==0) return; // No movement

		if(map[playerX + pX, playerY + pY] != wallMap) {
			playerX += pX;
			playerY += pY;
			Render();
		}
	}
}
