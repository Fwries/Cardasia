using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public string name;

	public string MapName;
	public int x, y;
	public int xDirection, yDirection;

	public CharacterData[] PartyCharacterData;
	public InteractableData[] InteractableList;
	public string InventoryJson;

	public GameData(string _name, string _MapName, Vector3 _CurrDirection, int _x, int _y, CharacterData[] _PartyCharacterData, List<SC_Card> _Inventory, InteractableData[] _InteractableList)
	{
		name = _name;
		MapName = _MapName;
		x = _x;
		y = _y;
		xDirection = (int)_CurrDirection.x;
		yDirection = (int)_CurrDirection.y;

		PartyCharacterData = _PartyCharacterData;
		InteractableList = _InteractableList;

		SC_Deck scInventory = ScriptableObject.CreateInstance<SC_Deck>();
		scInventory.List = _Inventory;
		InventoryJson = JsonUtility.ToJson(scInventory);
	}

	public List<SC_Card> GetInventory()
    {
		SC_Deck scInventory = ScriptableObject.CreateInstance<SC_Deck>();
		JsonUtility.FromJsonOverwrite(InventoryJson, scInventory);

		List<SC_Card> Inventory = new List<SC_Card>();
		for (int i = 0; i < scInventory.List.Count; i++)
		{
			Inventory.Add(scInventory.List[i]);
		}
		return Inventory;
	}
}
