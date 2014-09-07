using UnityEngine;
using System;

public class Water : Tile {
	public Water(int id) : base(id){
	}

	public override void RenderTile(int xp, int yp){
		view.DrawOnTexture(xp * resolution, yp * resolution, levelTexture, spriteSheet[0], resolution);
	}
}