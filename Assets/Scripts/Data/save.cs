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
	private bool DebugMode;

	public string nameStr;
	public SC_Map Map;
	public int xPos, yPos;
	
	public CharacterData[] PartyCharacterData;
	public List<SC_Card> Inventory;

	public SC_Character TempChar;
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
        //LoadFile("save");
        //LoadSettings();
    }

	void Start()
	{

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

		GameData data = new GameData(nameStr, Map, xPos, yPos, PartyCharacterData, Inventory);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, data);
		file.Close();

		if (DebugMode) { Debug.Log("Saved " + SaveFileName); }
	}

	public void LoadFile(string SaveFileName)
	{
		string destination = Application.persistentDataPath + "/" + SaveFileName + ".dat";
		FileStream file;

		if (File.Exists(destination)) file = File.OpenRead(destination);
		else
		{
			Debug.Log("File not found");
			
			GameObject ContinueButton = GameObject.Find("Continue");
			if (ContinueButton != null)
            {
				ContinueButton.GetComponent<Button>().interactable = false;
			}
			
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

		PartyCharacterData = new CharacterData[data.PartyCharacterData.Length];
		PartyCharacterData = data.PartyCharacterData;

		Inventory = data.GetInventory();

		if (DebugMode) { Debug.Log("Loaded " + SaveFileName); }
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
		PartyCharacterData[PartyCharacterData.Length - 1] = new CharacterData(_Character, _Level, PartyCharacterData.Length - 1, null);
	}
	public void CreateCharacterData(SC_Character _Character, int _Health, int _Level, int _Exp, int _Bullet, Sprite[] _CurrAnim, SC_Deck ItemDeck)
	{
		CharacterData[] temp = PartyCharacterData;
		PartyCharacterData = new CharacterData[PartyCharacterData.Length + 1];
		for (int i = 0; i < temp.Length; i++)
		{
			PartyCharacterData[i] = temp[i];
		}
		PartyCharacterData[PartyCharacterData.Length - 1] = new CharacterData(_Character, _Health, _Level, _Exp, _Bullet, _CurrAnim, PartyCharacterData.Length - 1, ItemDeck);
	}
	public void CreateNewCharacterData(SC_Character _Character, int _Level)
	{
		CharacterData[] temp = PartyCharacterData;
		PartyCharacterData = new CharacterData[PartyCharacterData.Length + 1];
		
		for (int i = 0; i < temp.Length; i++)
		{
			PartyCharacterData[i] = temp[i];
		}

		PartyCharacterData[PartyCharacterData.Length - 1] = new CharacterData(_Character, _Level, PartyCharacterData.Length - 1, null);
		PartyCharacterData[PartyCharacterData.Length - 1].Health = _Character.Health + 20 * _Level;
	}

	public void ChangeScene(string SceneName, string save)
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
		LoadFile(save);
	}

	public void BattleUpdate(GameBehaviour GameBehav)
    {
		PartyCharacterData = new CharacterData[GameBehav.Player.CharacterTape.Length];
		for (int i = 0; i < PartyCharacterData.Length; i++)
        {
			PartyCharacterData[i] = new CharacterData(GameBehav.Player.GetOrigCharacterTape(i), i);
		}
	}
}
