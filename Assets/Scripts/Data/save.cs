using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class save : MonoBehaviour
{
	public bool SaveStart;

	public string nameStr;
	public SC_Map Map;
	public int xPos, yPos;
	
	public CharacterData[] PartyCharacterData;

	public SC_Character TempChar;

	void Start()
	{
		if (SaveStart) 
		{
			CreateNewCharacterData(TempChar, 5);
			SaveFile();
		}

		LoadFile();
	}

	public void SaveFile()
	{
		string destination = Application.persistentDataPath + "/save.dat";
		FileStream file;

		if (File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		CharacterMovement CharMovement = GetComponent<CharacterMovement>();
		if (CharMovement != null && PartyCharacterData.Length > 0)
        {
			PartyCharacterData[0].SetCurrAnim(CharMovement.Character, CharMovement.CurrAnim);
		}

		GameData data = new GameData(nameStr, Map, xPos, yPos, PartyCharacterData);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, data);
		file.Close();
		
		Debug.Log("Saved");
	}

	public void LoadFile()
	{
		string destination = Application.persistentDataPath + "/save.dat";
		FileStream file;

		if (File.Exists(destination)) file = File.OpenRead(destination);
		else
		{
			Debug.LogError("File not found");
			return;
		}

		BinaryFormatter bf = new BinaryFormatter();
		GameData data = (GameData)bf.Deserialize(file);
		file.Close();

		nameStr = data.name;
		Map = ScriptableObject.CreateInstance<SC_Map>();
		JsonUtility.FromJsonOverwrite(data.MapJson, Map);
		xPos = data.x;
		yPos = data.y;

		PartyCharacterData = data.PartyCharacterData;

		CharacterMovement CharMovement = GetComponent<CharacterMovement>();
		if (CharMovement != null)
        {
			CharMovement.TeleportPlayer(xPos, yPos);
			GameObject.Find("Map").GetComponent<MapBehaviour>().ChangeMap(Map);
			if (PartyCharacterData.Length > 0)
            {
				CharMovement.CurrAnim = PartyCharacterData[0].GetCurrAnim();
			}
		}
	}

	public void CreateCharacterData(SC_Character _Character, int _Level)
    {
		CharacterData[] temp = PartyCharacterData;
		PartyCharacterData = new CharacterData[PartyCharacterData.Length + 1];
		for (int i = 0; i < temp.Length; i++)
        {
			PartyCharacterData[i] = temp[i];
        }
		PartyCharacterData[PartyCharacterData.Length - 1] = new CharacterData(_Character, _Level);
	}

	public void CreateCharacterData(SC_Character _Character, int _Health, int _Level, int _Exp, int _Bullet, Sprite[] _CurrAnim)
	{
		CharacterData[] temp = PartyCharacterData;
		PartyCharacterData = new CharacterData[PartyCharacterData.Length + 1];
		for (int i = 0; i < temp.Length; i++)
		{
			PartyCharacterData[i] = temp[i];
		}
		PartyCharacterData[PartyCharacterData.Length - 1] = new CharacterData(_Character, _Health, _Level, _Exp, _Bullet, _CurrAnim);
	}
	public void CreateNewCharacterData(SC_Character _Character, int _Level)
	{
		CharacterData[] temp = PartyCharacterData;
		PartyCharacterData = new CharacterData[PartyCharacterData.Length + 1];
		for (int i = 0; i < temp.Length; i++)
		{
			PartyCharacterData[i] = temp[i];
		}
		PartyCharacterData[PartyCharacterData.Length - 1] = new CharacterData(_Character, _Level);
		PartyCharacterData[PartyCharacterData.Length - 1].Health = _Character.Health;
	}
}
