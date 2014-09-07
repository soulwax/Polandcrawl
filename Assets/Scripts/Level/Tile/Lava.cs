using UnityEngine;
using System;

public class Lava : Tile {
	
	public Lava(int id) : base(id){
	}

	public override void RenderTile(int xp, int yp){
		view.DrawOnTexture(xp * resolution, yp * resolution, levelTexture, spriteSheet[2], resolution);
	}
}