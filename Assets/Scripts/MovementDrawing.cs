/**
 * Some script of the internets changed to actually be useful.
 * 
 * TODO: The texture is draw upside down, so all inputs are taken inversed.
 */
using UnityEngine;
using System.Collections;

public class MovementDrawing : MonoBehaviour 
{
	// Dem varriableses
	public float cycleRate = 0.5f;
	private float nextCycle = 0.0f;

	public int tileReso = 10;

	private int width, height;

	private int pX = 0;
	private int pY = 0;
	private int playerX;
	private int playerY;

	private char[,] map;
	private Texture2D texture;

	// Const
	private const char floorMap = '.';
	private const char wallMap = '#';
	private const char playerMap = '@';

	// Use this for initialization
	void Start () 
	{
		width = (int)renderer.bounds.size.x*10 / tileReso;
		height = (int)renderer.bounds.size.y*10 / tileReso; // This is a retarded way and based on rendering size, I'm just a lazy cunt.

		map = new char[width, height];

		texture = new Texture2D(width, height);
		renderer.material.mainTexture = texture;
		renderer.material.mainTexture.filterMode = FilterMode.Point;

		/*
		 * Map being used, 10x10
		 * 
		 * . . . . . . . . . .
		 * . . . . . . . . . .
		 * . . # # # # # # . .
		 * . . # . . . . # . .
		 * . . # . . . . # . .
		 * . . # . . . . # . .
		 * . . # . . . . # . .
		 * . . # # # # # # . .
		 * . . . . . . . . . .
		 * . . . . . . . . . .
		 * 
		 */

		for(int y = 0; y < height; y++){
			for(int x = 0; x < width; x++) {
				map[x, y] = '.';
			}
		}
		#region new memes being borned, no touchings.

		map[2, 2] = '#';
		map[3, 2] = '#';
		map[4, 2] = '#';
		map[5, 2] = '#';
		map[6, 2] = '#';
		map[7, 2] = '#';

		map[2, 3] = '#';
		map[7, 3] = '#';

		map[2, 4] = '#';
		map[7, 4] = '#';

		map[2, 5] = '#';
		map[7, 5] = '#';

		map[2, 6] = '#';
		map[7, 6] = '#';

		map[2, 7] = '#';
		map[3, 7] = '#';
		map[4, 7] = '#';
		map[5, 7] = '#';
		map[6, 7] = '#';
		map[7, 7] = '#';

		#endregion


		playerX = 3;
		playerY = 3;

		Render ();
	}

	void Render ()
	{
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {

				if (x == playerX && y == playerY) {
					texture.SetPixel(x, y, Color.green);
				} else if(map[x, y] == floorMap) {
					texture.SetPixel(x, y, Color.blue);
				} else {
					texture.SetPixel(x, y, Color.red);
				}
			}
		}

		texture.Apply();
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
