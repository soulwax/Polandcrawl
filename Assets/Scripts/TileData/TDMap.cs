using UnityEngine;
using System.Collections.Generic;

public class TDMap {
	public bool verbose = false;

	protected class DRoom {
		public int xp;
		public int yp;
		public int width;
		public int height;
		public Rect rect;
		public int x_bound, y_bound;

		public DRoom(int xp, int yp, int width, int height, int x_bound, int y_bound) {
			this.xp = xp;
			this.yp = yp;
			this.width = width;
			this.height = height;
			this.x_bound = x_bound;
			this.y_bound = y_bound;
			rect = new Rect(this.xp, this.yp, this.width, this.height);
		}

		public bool collidesWith(DRoom r){
			if(this.rect.Overlaps(r.rect)) return true;
			else return false;
		}

		public void RandomizePos() {
			this.rect.x = this.xp = Random.Range(0, x_bound - this.width);
			this.rect.y = this.yp = Random.Range(0, y_bound - this.height);
		}
	}

	protected int size_x;
	protected int size_y;

	protected int[,] map_data;

	protected List<DRoom> rooms;


	/*
	 * Tile id:
	 * 0 = unknown/void 
	 * 1 = floor
	 * 2 = wall
	 * 3 = water
	 * 
	 */

	public TDMap(int size_x = 20, int size_y = 20){
		this.size_x = size_x;
		this.size_y = size_y;
		
		map_data = new int[size_x,size_y];
		
		rooms = new List<DRoom>();

		GenerateScatteredRooms(20, 5, 15);
	}

	public void GenerateScatteredRooms(int amount, int minSize, int maxSize) {

		for(int y = 0; y < size_y; y++) {
			for(int x = 0; x < size_x; x++) {
				map_data[x,y] = 3;
			}
		}
		
		for(int i = 0; i < amount; i++){
			
			DRoom r = CreateRandomizedRoom(minSize,maxSize);
			if(rooms.Count > 0) {
				int attempts = 0;
				int maxAttempts = 100;
				while(CollidesWithAnyRoom(r) && attempts < maxAttempts) {
					r.RandomizePos();
					attempts++;
					if(verbose) Debug.Log ("Randomizing! "+ attempts +"th attempt");
				}
			}
			rooms.Add(r);
			if(verbose) Debug.Log ("Room "+i+" added! xp:"+r.xp+", yp: "+ r.yp +", w: "+r.width +", h:" + r.height);
		}
		
		foreach(DRoom rr in rooms) {
			AddRoomToTileData(rr);
		}
	}
	
	public int GetTileAt(int x, int y){
		if(x >= size_x || x < 0 || y >= size_y || y < 0) {
			return 0;
		}
		return map_data[x,y];
	}

	void AddRoomToTileData(DRoom room){
		for(int y = 0; y < room.height; y++) {
			for(int x = 0; x < room.width; x++) {
				if(x == 0 || x == room.width-1 || y == 0 || y == room.height-1) {
					map_data[room.xp + x,room.yp + y] = 2;
				} else {
					map_data[room.xp + x,room.yp + y] = 1;
				}
			}
		}
	}

	DRoom CreateRandomizedRoom(int r0, int r1) {
		int rsx = Random.Range(r0,r1);
		int rsy = Random.Range(r0,r1);
		int rpx = Random.Range(0, size_x - rsx);
		int rpy = Random.Range(0, size_y - rsy);
		
		return new DRoom(rpx,rpy,rsx,rsy, this.size_x, this.size_y);
	}

	bool CollidesWithAnyRoom(DRoom r) {
		foreach(DRoom rr in rooms) {
			if(r.collidesWith(rr)){
				if(verbose) Debug.Log ("Room "+r+" collided with room "+rr);
				return true;
			}
		}
		return false;
	}
}
