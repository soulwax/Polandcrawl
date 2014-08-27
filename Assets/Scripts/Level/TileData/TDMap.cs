using UnityEngine;
using System.Collections.Generic;

/*
 * Tile id:
 * 0 = unknown/void 
 * 1 = floor
 * 2 = wall
 * 3 = water
 * 
 */

public class TDMap
{
    #region variables
    //For debugging
	public bool verbose = false;

    protected int size_x;
    protected int size_y;

    protected int[,] map_data;

    protected List<DRoom> rooms;
    #endregion

    #region room class
    protected class DRoom
    {
        public float xp, yp;
        public float width, height;
        public Rect rect, innerPadding, outerPadding;
        public int x_bound, y_bound;
        public bool _connected = false;

        public DRoom(float xp, float yp, int width, int height, int x_bound, int y_bound)
        {
            this.xp = xp;
            this.yp = yp;
            this.width = width;
            this.height = height;
            this.x_bound = x_bound;
            this.y_bound = y_bound;
            rect = new Rect(this.xp, this.yp, this.width, this.height);
            innerPadding = new Rect(this.xp + 1, this.yp + 1, this.width - 1, this.height - 1);
            outerPadding = new Rect(this.xp - 1, this.yp - 1, this.width + 1, this.height + 1);
        }


        //Collision functions are for checking colliders and collisions
        //The different overrides can either deal with single rooms or lists of rooms
        //Alternatively they can also return the rooms the room in question collides with.
        //Mainly used for the dungeon creation process before adding the rooms to the room data.
        public bool CollidesWith(DRoom r)
        {
            if (this.rect.Overlaps(r.rect)) return true;
            else return false;
        }

        public bool CollidesWith(List<DRoom> rooms)
        {
            foreach (DRoom r in rooms)
            {
                if (r.Equals(this)) continue;
                else if (r.CollidesWith(this)) return true;
            }
            return false;
        }

        public DRoom GetCollider(List<DRoom> rooms)
        {
            foreach (DRoom r in rooms)
            {
                if (r.Equals(this)) continue;
                else if (r.CollidesWith(this)) return r;
            }
            return null;
        }

        public bool CollidesWith(List<DRoom> rooms, DRoom except)
        {
            foreach (DRoom r in rooms)
            {
                if (r.Equals(this) || r.Equals(except)) continue;
                else if (r.CollidesWith(this)) return true;
            }
            return false;
        }

        public bool NeighboursRoom(DRoom r)
        {
            if (this.rect.Overlaps(r.outerPadding) && !this.outerPadding.Overlaps(r.innerPadding)) return true;

            return false;
        }

        public void RandomizePos()
        {
            this.innerPadding.x = this.outerPadding.x = this.rect.x = this.xp = Random.Range(0, x_bound - this.width);
            this.innerPadding.y = this.outerPadding.y = this.rect.y = this.yp = Random.Range(0, y_bound - this.height);
        }

        //Move functions are mainly used for the dungeon generation process, when the rooms need to change
        //their positions before they get 'baked' into the map data grid. 
        //Beware! Moving rooms after they've been added to the tile data will cause errors and dungeon
        //patterns that don't make any sense.
        //returns true if a move step was not possible (e.g. when the map border was reached)
        public bool Move(Vector2 dir)
        {

            if (this.xp + dir.x >= 0 && this.xp + dir.x + width < x_bound)
                this.innerPadding.x = this.outerPadding.x = this.rect.x = this.xp += dir.x;
            else return true;

            if (this.yp + dir.y >= 0 && this.yp + dir.y + height < y_bound)
                this.innerPadding.y = this.outerPadding.y = this.rect.y = this.yp += dir.y;
            else return true;

            return false;
        }

        
        public bool Move(float xd, float yd)
        {
            return (Move(new Vector2(xd, yd)));
        }


        public float center_x
        {
            get { return xp + width / 2; }
        }

        public float center_y
        {
            get { return yp + height / 2; }
        }

        public Vector2 center
        {
            get { return new Vector2(center_x, center_y); }
        }

        public bool connected
        {
            get { return this._connected; }
            set { this._connected = value; }
        }

        public float GetDistance(DRoom r)
        {
            float xd = r.center_x - this.center_x;
            float yd = r.center_y - this.center_y;
            return Mathf.Sqrt(xd * xd + yd * yd);
        }

        //returns an array of rooms sorted according to the distance to this instance
        public DRoom[] GetSortedRooms(List<DRoom> rooms)
        {
            int n = rooms.Count;
            DRoom[] result = new DRoom[n];
            result = rooms.ToArray();

            bool swapped;
            do{
                swapped = false;
                for (int i=0; i<n-1; ++i){
                    if (GetDistance(result[i]) > GetDistance(result[i+1])) {

                        DRoom tempr1 = result[i];
                        result[i] = result[i + 1];
                        result[i + 1] = tempr1;

                        swapped = true;
                    } 
                } 
                n -= 1;
            } while (swapped == true);

            return result;
        }

        //uses the room sorting algorithm to find the closest unconnected room
        public DRoom GetClosestUnconnectedRoom(List<DRoom> rooms)
        {
            DRoom[] hierarchy = GetSortedRooms(rooms);
            for (int i = 0; i < hierarchy.Length; i++)
            {
                if (!hierarchy[i].connected) return hierarchy[i];
            }
            return null;
        }
    }

    #endregion


    #region constructors
    //Obsolete default constructor
	public TDMap(int size_x = 20, int size_y = 20){
		this.size_x = size_x;
		this.size_y = size_y;
		
		map_data = new int[size_x,size_y];
		
		rooms = new List<DRoom>();

		GenerateScatteredRooms(10, 5, 15);
	}

    //use this contructor, if needed create a default constructor for that
    public TDMap(int size_x, int size_y, int roomAmount, DungeonVariables.Type type)
    {
        // TODO: Complete member initialization
        this.size_x = size_x;
        this.size_y = size_y;

        map_data = new int[size_x, size_y];

        rooms = new List<DRoom>();

        if (type == DungeonVariables.Type.Scattered)
        {
            GenerateScatteredRooms(roomAmount, 5, 15);
        }
        else if (type == DungeonVariables.Type.Coherent)
        {
            GenerateCoherentRooms(roomAmount, 5, 15);
        }
    }

    //alternative script independent constructor 
    public TDMap(int size_x, int size_y, int roomAmount)
    {
        // TODO: Complete member initialization
        this.size_x = size_x;
        this.size_y = size_y;

        map_data = new int[size_x, size_y];

        rooms = new List<DRoom>();

        GenerateScatteredRooms(roomAmount, 5, 15);
    }

    #endregion


    //This function does basically the same as the GenerateScatteredRooms function
    //though it moves the rooms a bit closer to the center for more connections 
    //between single rooms - which in return means less corridors.
    private void GenerateCoherentRooms(int roomAmount, int minSize, int maxSize)
    {
        Vector2 center = new Vector2(size_x / 2, size_y / 2);

        DRoom r;
        for (int y = 0; y < size_y; y++)
        {
            for (int x = 0; x < size_x; x++)
            {
                map_data[x, y] = 3;
            }
        }

        for (int i = 0; i < roomAmount; i++)
        {

            r = CreateRandomizedRoom(minSize, maxSize);
            if (rooms.Count > 0)
            {
                int attempts = 0;
                int maxAttempts = 100;
                while (r.CollidesWith(rooms) && attempts < maxAttempts)
                {
                    r.RandomizePos();
                    attempts++;
                    if (verbose) Debug.Log("Randomizing! " + attempts + "th attempt");
                }
            }
            rooms.Add(r);
        }

        for (int i = 0; i < roomAmount; i++)
        {
            DRoom rr = rooms[i];

            float xr = rr.center_x - center.x;
            float yr = rr.center_y - center.y;


            float xdir = -xr / (center.x - rr.width / 2);
            float ydir = -yr / (center.y - rr.height / 2);

            int maxAttempts = 10000;
            int attempts = 0;
            while (!rr.CollidesWith(rooms))
            {


                if (rr.Move(new Vector2(xdir, ydir)))
                {
                    //TODO: do something if move not possible
                    //placeholder - just in case
                }

                attempts++;
                if (attempts >= maxAttempts) break;
            }
            AddRoomToTileData(rr);
        }

        ConnectRooms(rooms);   
    }


    

    //Main generating function to create a dungeon with more spread out rooms
	public void GenerateScatteredRooms(int amount, int minSize, int maxSize) {
		DRoom r;
		for(int y = 0; y < size_y; y++) {
			for(int x = 0; x < size_x; x++) {
				map_data[x,y] = 3;
			}
		}
		
		for(int i = 0; i < amount; i++){
			
			r = CreateRandomizedRoom(minSize,maxSize);
			if(rooms.Count > 0) {
				int attempts = 0;
				int maxAttempts = 100;
				while(r.CollidesWith(rooms) && attempts < maxAttempts) {
					r.RandomizePos();
					attempts++;
					if(verbose) Debug.Log ("Randomizing! "+ attempts +"th attempt");
				}
			}
			rooms.Add(r);
			if(verbose) Debug.Log ("Room "+i+" added! xp:"+r.xp+", yp: "+ r.yp +", w: "+r.width +", h:" + r.height);
		}

        AddAllRoomsToTileData();

        ConnectRooms(rooms);
	}

    //This thing connects a list of rooms, it's enough to call this funciton once after
    //the rooms were placed at their final position and added to the tile data array.
    private void ConnectRooms(List<DRoom> _rooms)
    {
        DRoom r1 = _rooms[0];
        DRoom r2;
        for (int i = 0; i < _rooms.Count - 1; i++)
        {
            r1.connected = true;
            r2 = r1.GetClosestUnconnectedRoom(_rooms);
            BuildCorridor(r1, r2);
            r1 = r2;
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
					map_data[(int)(room.xp + x),(int)(room.yp + y)] = 2;
				} else {
                    map_data[(int)(room.xp + x), (int)(room.yp + y)] = 1;
				}
			}
		}
	}

    void AddAllRoomsToTileData()
    {
        foreach (DRoom d in rooms)
        {
            AddRoomToTileData(d);
        }
    }

	DRoom CreateRandomizedRoom(int r0, int r1) {
		int rsx = Random.Range(r0,r1);
		int rsy = Random.Range(r0,r1);
		int rpx = Random.Range(0, size_x - rsx);
		int rpy = Random.Range(0, size_y - rsy);
		
		return new DRoom(rpx,rpy,rsx,rsy, this.size_x, this.size_y);
	}

	private void BuildCorridor(DRoom r1, DRoom r2) {
		int x = (int) r1.center_x;
        int y = (int) r1.center_y;

		while(x != (int) r2.center_x) {
			map_data[x,y] = 1;
			x += x < r2.center_x ? 1 : -1;
		}

		while (y != (int) r2.center_y) {
			map_data[x,y] = 1;
			y += y < r2.center_y ? 1 : -1;
		}
	}

}
