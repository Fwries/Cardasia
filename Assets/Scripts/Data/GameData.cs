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
	public string InventoryJson;

	public GameData(string _name, SC_Map Map, int _x, int _y, CharacterData[] _PartyCharacterData, List<SC_Card> _Inventory)
	{
		name = _name;
		MapJson = JsonUtility.ToJson(Map);
		x = _x;
		y = _y;

		PartyCharacterData = _PartyCharacterData;

		SC_Deck scInventory = ScriptableObject.CreateInstance<SC_Deck>();
		scInventory.Deck = _Inventory;
		InventoryJson = JsonUtility.ToJson(scInventory);
	}

	public List<SC_Card> GetInventory()
    {
		SC_Deck scInventory = ScriptableObject.CreateInstance<SC_Deck>();
		JsonUtility.FromJsonOverwrite(InventoryJson, scInventory);

		List<SC_Card> Inventory = new List<SC_Card>();
		for (int i = 0; i < scInventory.Deck.Count; i++)
		{
			Inventory.Add(scInventory.Deck[i]);
		}
		return Inventory;
	}
}
