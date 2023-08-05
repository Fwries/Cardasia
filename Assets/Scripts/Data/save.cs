using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class save : MonoBehaviour
{
	public static save Instance;
	public InteractableData[] InteractableList;

	public string nameStr;
	public SC_Map Map;
	public int xPos, yPos;
	public Vector3 CurrDirection;

	public int Gold;
	public bool CantRun;
	
	public CharacterData[] PartyCharacterData;
	public List<SC_Card> Inventory;

	public SC_Character[] TempChar;
	public List<SC_Card> TempItemDeck;

	public SC_EnemyList EnemyList;

	public bool MusicMute;
	public bool SFXMute;
	public float MusicVolume;
	public float SFXVolume;

	void Awake()
    {
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}

	void Start()
    {
		LoadFile("save");
		LoadSettings();
	}

	public void SaveFile(string SaveFileName)
	{
		string destination = Application.persistentDataPath + "/" + SaveFileName + ".dat";
		FileStream file;

		if (File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		GameObject RPGChar = GameObject.Find("Character_RPG");
		if (RPGChar != null && PartyCharacterData.Length > 0)
		{
			CharacterMovement CharMove = RPGChar.GetComponent<CharacterMovement>();
			PartyCharacterData[0].SetCurrAnim(CharMove.Character, CharMove.CurrAnim);
		}

		GameData data = new GameData("Hero", Map.MapName, CurrDirection, xPos, yPos, PartyCharacterData, Inventory, InteractableList);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, data);
		file.Close();
	}

	public void LoadFile(string SaveFileName)
	{
		string destination = Application.persistentDataPath + "/" + SaveFileName + ".dat";
		FileStream file;

		if (File.Exists(destination)) file = File.OpenRead(destination);
		else
		{
			Debug.Log("File not found");			
			return;
		}

		BinaryFormatter bf = new BinaryFormatter();
		GameData data = (GameData)bf.Deserialize(file);
		file.Close();

		nameStr = data.name;
		Map = Resources.Load<SC_Map>("Scriptables/Maps/" + data.MapName + "/" + data.MapName);

		xPos = data.x;
		yPos = data.y;
		CurrDirection = new Vector3(data.xDirection, data.yDirection, 0);

		PartyCharacterData = new CharacterData[data.PartyCharacterData.Length];
		PartyCharacterData = data.PartyCharacterData;

		InteractableList = new InteractableData[data.InteractableList.Length];
		InteractableList = data.InteractableList;

		Inventory = data.GetInventory();
	}

	public bool CheckFileExist(string SaveFileName)
    {
		string destination = Application.persistentDataPath + "/" + SaveFileName + ".dat";
		FileStream file;

		if (File.Exists(destination)) file = File.OpenWrite(destination);
		else
		{
			Debug.Log("File not found");
			return false;
		}
		file.Close();
		return true;
	}

	public void SaveSettings()
    {
		string destination = Application.persistentDataPath + "/Settings.dat";
		FileStream file;

		if (File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		SettingsData data = new SettingsData(AudioManager.Instance.musicSource.mute, AudioManager.Instance.sfxSource.mute,
											AudioManager.Instance.musicSource.volume, AudioManager.Instance.sfxSource.volume);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

	public void LoadSettings()
    {
		string destination = Application.persistentDataPath + "/Settings.dat";
		FileStream file;

		if (File.Exists(destination)) file = File.OpenRead(destination);
		else
		{
			SaveSettings();
			LoadSettings();
			return;
		}

        BinaryFormatter bf = new BinaryFormatter();
        SettingsData data = (SettingsData)bf.Deserialize(file);
        file.Close();

        AudioManager.Instance.musicSource.mute = data.MusicMute;
        AudioManager.Instance.sfxSource.mute = data.SFXMute;
        AudioManager.Instance.musicSource.volume = data.MusicVolume;
        AudioManager.Instance.sfxSource.volume = data.SFXVolume;
    }

	public void CreateCharacterData(SC_Character _Character, int _Level)
    {
		CharacterData[] temp = PartyCharacterData;
		PartyCharacterData = new CharacterData[PartyCharacterData.Length + 1];
		for (int i = 0; i < temp.Length; i++)
        {
			PartyCharacterData[i] = temp[i];
        }
		PartyCharacterData[PartyCharacterData.Length - 1] = new CharacterData(_Character, _Level, null);
	}
	public void CreateCharacterData(SC_Character _Character, int _Health, int _Level, int _Exp, int _Bullet, Sprite[] _CurrAnim, SC_Deck ItemDeck)
	{
		CharacterData[] temp = PartyCharacterData;
		PartyCharacterData = new CharacterData[PartyCharacterData.Length + 1];
		for (int i = 0; i < temp.Length; i++)
		{
			PartyCharacterData[i] = temp[i];
		}
		PartyCharacterData[PartyCharacterData.Length - 1] = new CharacterData(_Character, _Health, _Level, _Exp, _Bullet, _CurrAnim, ItemDeck);
	}
	public void CreateNewCharacterData(SC_Character _Character, int _Level)
	{
		CharacterData[] temp = PartyCharacterData;
		PartyCharacterData = new CharacterData[PartyCharacterData.Length + 1];
		
		for (int i = 0; i < temp.Length; i++)
		{
			PartyCharacterData[i] = temp[i];
		}

		PartyCharacterData[PartyCharacterData.Length - 1] = new CharacterData(_Character, _Level, null);
		PartyCharacterData[PartyCharacterData.Length - 1].Health = _Character.Health + 20 * _Level;
	}

	public string[] Obtain(string[] Item)
    {
		string[] returnString = new string[Item.Length];
		returnString[0] = "List is Empty";

		for (int index = 0; index < Item.Length; index++)
        {
			string ItemName = "";
			for (int i = 4; i < Item[index].Length; i++)
			{
				ItemName += Item[index][i];
			}

			if (Item[index][0] == 'G' && Item[index][1] == 'l' && Item[index][2] == 'd')
            {
				returnString[index] = nameStr + " found " + ItemName + " Gold!";
			}
			else if (Item[index][0] == 'I' && Item[index][1] == 't' && Item[index][2] == 'm')
			{
				//Inventory.Add(Resources.Load<SC_Card>("Scriptables/Cards/Skill/" + ItemName));
				//returnString[index] = nameStr + " found " + ItemName + "!";
			}
			else if (Item[index][0] == 'C' && Item[index][1] == 'o' && Item[index][2] == 'n')
			{
				Inventory.Add(Resources.Load<SC_Card>("Scriptables/Cards/Consumable/" + ItemName));
				returnString[index] = nameStr + " found " + ItemName + "!";
			}
			else if (Item[index][0] == 'S' && Item[index][1] == 'k' && Item[index][2] == 'l')
			{
				Inventory.Add(Resources.Load<SC_Card>("Scriptables/Cards/Skill/" + ItemName));
				returnString[index] = nameStr + " found " + ItemName + "!";
			}
			else if (Item[index][0] == 'W' && Item[index][1] == 'p' && Item[index][2] == 'm')
			{
				//Inventory.Add(Resources.Load<SC_Card>("Scriptables/Cards/Skill/" + ItemName));
				//returnString[index] = nameStr + " found " + ItemName + "!";
			}
		}

		return returnString;
    }

	public void ChangeScene(string SceneName, string save)
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
		LoadFile(save);
	}

	public void BattleUpdate(GameBehaviour GameBehav)
    {
		PartyCharacterData = new CharacterData[PartyCharacterData.Length];
		for (int i = 0; i < PartyCharacterData.Length; i++)
        {
			PartyCharacterData[i] = new CharacterData(GameBehav.Player.GetOrigCharacterTape(i));
		}
	}
}
