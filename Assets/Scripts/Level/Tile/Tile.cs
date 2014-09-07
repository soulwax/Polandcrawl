using UnityEngine;
using System.Collections;
using System;

public class Tile {
	public static Tile[] tiles = new Tile[256];

	public static Tile water = new Water(0);
	public static Tile dirt = new Dirt(1);
	public static Tile lava = new Lava(2);
	public static Tile stairsDown = new StairsDown(3);
	public static Tile rock = new Rock(4);
	public static Tile ironOre = new IronOre(5);

	public readonly byte id;
	public Color[][] spriteSheet;
	public Color[][] spriteSheetFlipped;
	public GameView view;
	public int resolution;
	public Texture2D levelTexture;

	public Tile(int id) {
		this.id = (byte) id;
		if(tiles[id] != null) Debug.LogError("Duplicate Tile IDs!");
		tiles[id] = this;
		view = GameObject.FindWithTag("GameView").GetComponent<GameView>();
		spriteSheet = view.spriteSheet;
		levelTexture = view.levelTexture;
		resolution = view.tileResolution;
	}

	public virtual void RenderTile(int xp, int yp){
	}
}
