using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public string name;

	public string MapJson;
	public int x, y;

	public CharacterData[] PartyCharacterData;

	public GameData(string _name, SC_Map Map, int _x, int _y, CharacterData[] _PartyCharacterData)
	{
		name = _name;
		MapJson = JsonUtility.ToJson(Map);
		x = _x;
		y = _y;

		PartyCharacterData = _PartyCharacterData;
	}
}
