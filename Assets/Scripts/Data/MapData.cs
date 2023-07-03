using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
	public SC_Map Map;
	public int x, y;

	public MapData(SC_Map _Map, int _x, int _y)
	{
		Map = _Map;
		x = _x; y = _y;
	}
}
