using UnityEngine;
using System;

public class StairsDown : Tile {

	public StairsDown(int id) : base(id){
		walkable = true;
	}

	public override void RenderTile(int xp, int yp){
		view.DrawOnTexture(xp * resolution, yp * resolution, levelTexture, spriteSheet[3], resolution);
	}
}