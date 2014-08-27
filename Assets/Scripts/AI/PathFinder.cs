using UnityEngine;
using System.Collections.Generic;

public class PathFinder  {
    public static bool VERBOSE = false;
    private GameView view;

    private List<Vector2> result = new List<Vector2>();
	private int[] travelCosts;
    

    private int xStart;
    private int yStart;
    private int xEnd;
    private int yEnd;

    private int lwidth;
    private int lheight;

    public PathFinder(GameView view) {
        this.view = view;
        this.lwidth = view.levelWidth;
        this.lheight = view.levelHeight;

    }

    public List<Vector2> GetPath(int[] travelCosts, int xStart, int yStart, int xEnd, int yEnd) {
        this.travelCosts = travelCosts;

        this.xStart = xStart;
        this.yStart = yStart;
        this.xEnd = xEnd;
        this.yEnd = yEnd;

        return result;
    }
}
