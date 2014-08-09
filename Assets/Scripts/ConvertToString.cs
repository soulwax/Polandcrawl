using UnityEngine;
using System.Collections;
using System.Text;

public class ConvertToString
{
	public StringBuilder Covert(TDMap map, int size_x, int size_y)
	{
		StringBuilder mapString = new StringBuilder();

		for(int y = size_y - 1; y >= 0; y--) {
			for(int x = 0; x < size_x; x++) {
				if(map.GetTileAt(x, y) == 0) {
					mapString.Append('0');
				} else if(map.GetTileAt(x, y) == 1) {
					mapString.Append('1');
				} else if(map.GetTileAt(x, y) == 2) {
					mapString.Append('2');
				} else {
					mapString.Append('3');
				}
			}
			mapString.AppendLine();
		}

		return mapString;
	}
}
