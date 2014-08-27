/**
 * Field of Vision calculations
 * TODO: Both enemy and player usable.
 * TODO: It is super simple but atleast it works.
 */
using UnityEngine;
using System.Collections;

public class FieldOfVision
{
	public char[,] CalcFOV(int playerX, int playerY, int radius, char[,] map, int width, int height)
	{
		char[,] fovMap  = new char[width, height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {

				// Calc black locations.
				if(x >= playerX + radius)
					fovMap[x, y] = 'b';
				else if(x <= playerX - radius)
					fovMap[x, y] = 'b';
				else if(y >= playerY + radius)
					fovMap[x, y] = 'b';
				else if(y <= playerY - radius)
					fovMap[x, y] = 'b';
				else
					fovMap[x, y] = map[x, y];
			}
		}

		return fovMap;
	}
}
