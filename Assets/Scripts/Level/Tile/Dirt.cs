using UnityEngine;
using System;

public class Dirt : Tile {
	public Dirt(int id) : base(id) {
	}

	public override void RenderTile(int xp, int yp){
		view.DrawOnTexture(xp * resolution, yp * resolution, levelTexture, spriteSheet[1], resolution);
	}
}