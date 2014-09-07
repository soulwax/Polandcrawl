using UnityEngine;
using System;

public class Rock : Tile {
	public Rock(int id) : base(id) {
	}

	public override void RenderTile(int xp, int yp){	
		//view.DrawOnTexture(xp, yp, levelTexture, spriteSheetFlipped[5]);
		bool u = view.GetTile(xp, yp+1) != this.id;
		bool r = view.GetTile(xp+1, yp) != this.id;
		bool d = view.GetTile(xp, yp-1) != this.id;
		bool l = view.GetTile(xp-1, yp) != this.id;

		bool ul = view.GetTile(xp-1, yp+1) != this.id;
		bool ur = view.GetTile(xp+1, yp+1) != this.id;
		bool dl = view.GetTile(xp-1, yp-1) != this.id;
		bool dr = view.GetTile(xp+1, yp-1) != this.id;

		int rw = GameView.numTilesPerRow;
		//28, 29 || 30, 31, 32
		//15, 16 || 17, 18, 19
		//           4 , 5 , 6

		if(xp + 1 >= view.levelWidth) return;
		if(yp - 1 < 0) return;
		
		if (!u && !l) {
			if (!ul)
				view.DrawOnTexture(xp * resolution, yp * resolution, levelTexture, spriteSheet[18], resolution);
			else
				view.DrawOnTexture(xp * resolution, yp * resolution, levelTexture, spriteSheetFlipped[28], resolution);
		} else 
			view.DrawOnTexture(xp * resolution, yp * resolution, levelTexture, spriteSheetFlipped[(l ? 6 : 5) + (u ? 0 : 1) * rw], resolution);
		

		if (!u && !r) {
			if (!ur)
				view.DrawOnTexture(xp * resolution + resolution/2, yp * resolution, levelTexture, spriteSheetFlipped[18], resolution);
			else
				view.DrawOnTexture(xp * resolution + resolution/2, yp * resolution, levelTexture, spriteSheetFlipped[29], resolution);
		} else 
			view.DrawOnTexture(xp * resolution + resolution/2, yp * resolution, levelTexture, spriteSheetFlipped[(r ? 4 : 5) + (u ? 0 : 1)*rw], resolution);

		if (!d && !l) { 
			if (!dl)
				view.DrawOnTexture(xp * resolution, yp * resolution - resolution/2, levelTexture, spriteSheetFlipped[18], resolution);
			else
				view.DrawOnTexture(xp * resolution, yp * resolution - resolution/2, levelTexture, spriteSheetFlipped[15], resolution);
		} else 
			view.DrawOnTexture(xp * resolution, yp * resolution - resolution/2, levelTexture, spriteSheetFlipped[(l ? 6 : 5) + (d ? 2 : 1) * rw], resolution);
		if (!d && !r) {
			if (!dr)
				view.DrawOnTexture(xp * resolution + resolution/2, yp * resolution - resolution/2, levelTexture, spriteSheetFlipped[18], resolution);
			else
				view.DrawOnTexture(xp * resolution + resolution/2, yp * resolution - resolution/2, levelTexture, spriteSheetFlipped[16], resolution);
		} else 
			view.DrawOnTexture(xp * resolution + resolution/2, yp * resolution - resolution/2, levelTexture, spriteSheetFlipped[(r ? 4 : 5) + (d ? 2 : 1) * rw], resolution);

	}
}