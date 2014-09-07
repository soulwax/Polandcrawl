using UnityEngine;
using System;

public class IronOre : Tile {
	public IronOre(int id) : base(id) {
	}

	public override void RenderTile(int xp, int yp){
		view.DrawOnTexture(xp * resolution, yp * resolution, levelTexture, spriteSheet[13], resolution);
	}
}