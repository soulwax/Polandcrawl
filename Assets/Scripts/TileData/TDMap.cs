public class TDMap {
	protected TDTile[] _tiles;
	protected int _width;
	protected int _height;

	public TDMap(int width, int height){
		this._width = width;
		this._height = height;

		_tiles = new TDTile[_width * _height];
	}

	public TDTile GetTile(int x, int y) {
		if(x < 0 || x >= _width || y < 0 || y >= _height) {
			return null;
		}

		return _tiles[y*_width + x];
	}
}
