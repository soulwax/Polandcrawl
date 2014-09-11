using UnityEngine;
using System;

public class Water : Tile {
	public Water(int id) : base(id){
		swimmable = true;
	}

	public override void RenderTile(int xp, int yp) {

		bool u = view.GetTile(xp, yp+1) != this.id;
		bool r = view.GetTile(xp+1, yp) != this.id;
		bool d = view.GetTile(xp, yp-1) != this.id;
		bool l = view.GetTile(xp-1, yp) != this.id;

		//10, 11, 12
		//

		int rw = GameView.numTilesPerRow;

		if (!u && !l) {
			view.DrawOnTexture(xp*resolution, yp*resolution, levelTexture, spriteSheet[0], resolution);
		} else
			view.DrawOnTexture(xp*resolution, yp*resolution, levelTexture, spriteSheet[(l ? 10 : 11) + (u ? 2 : 1) * rw], resolution);

		if (!u && !r) {
			view.DrawOnTexture(xp*resolution+resolution/2, yp*resolution, levelTexture, spriteSheet[0], resolution);
		} else
			view.DrawOnTexture(xp*resolution+resolution/2, yp*resolution, levelTexture, spriteSheet[(r ? 12 : 11) + (u ? 2 : 1) * rw], resolution);
		if (!d && !l) {
			view.DrawOnTexture(xp*resolution, yp*resolution-resolution/2, levelTexture, spriteSheet[0], resolution);
		} else
			view.DrawOnTexture(xp*resolution, yp*resolution-resolution/2, levelTexture, spriteSheet[(l ? 10 : 11) + (d ? 0 : 1) * rw], resolution);
		if (!d && !r) {
			view.DrawOnTexture(xp*resolution+resolution/2, yp*resolution-resolution/2, levelTexture, spriteSheet[0], resolution);
		} else
			view.DrawOnTexture(xp*resolution+resolution/2, yp*resolution-resolution/2, levelTexture, spriteSheet[(r ? 12 : 11) + (d ? 0 : 1) * rw], resolution);
	}
}