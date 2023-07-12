using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

	void Awake()
    {
		if (SaveStart)
		{
			CreateNewCharacterData(TempChar, 5);
			SaveFile("save");
			SaveFile("battle");
			SaveStart = false;
		}

		DontDestroyOnLoad(this.gameObject);
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

		CharacterMovement CharMovement = GetComponent<CharacterMovement>();
		if (CharMovement != null && PartyCharacterData.Length > 0)
        {
			PartyCharacterData[0].SetCurrAnim(CharMovement.Character, CharMovement.CurrAnim);
		}

		GameData data = new GameData(nameStr, Map, xPos, yPos, PartyCharacterData);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, data);
		file.Close();

		//Debug.Log("Saved " + SaveFileName);
	}

	public void LoadFile(string SaveFileName)
	{
		string destination = Application.persistentDataPath + "/" + SaveFileName + ".dat";
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

		PartyCharacterData = new CharacterData[data.PartyCharacterData.Length];
		PartyCharacterData = data.PartyCharacterData;

		//Debug.Log("Loaded " + SaveFileName);
	}

	public void CreateCharacterData(SC_Character _Character, int _Level)
    {
		CharacterData[] temp = PartyCharacterData;
		PartyCharacterData = new CharacterData[PartyCharacterData.Length + 1];
		for (int i = 0; i < temp.Length; i++)
        {
			PartyCharacterData[i] = temp[i];
        }
		PartyCharacterData[PartyCharacterData.Length - 1] = new CharacterData(_Character, _Level, PartyCharacterData.Length - 1);
	}

	public void CreateCharacterData(SC_Character _Character, int _Health, int _Level, int _Exp, int _Bullet, Sprite[] _CurrAnim)
	{
		CharacterData[] temp = PartyCharacterData;
		PartyCharacterData = new CharacterData[PartyCharacterData.Length + 1];
		for (int i = 0; i < temp.Length; i++)
		{
			PartyCharacterData[i] = temp[i];
		}
		PartyCharacterData[PartyCharacterData.Length - 1] = new CharacterData(_Character, _Health, _Level, _Exp, _Bullet, _CurrAnim, PartyCharacterData.Length - 1);
	}
	public void CreateNewCharacterData(SC_Character _Character, int _Level)
	{
		CharacterData[] temp = PartyCharacterData;
		PartyCharacterData = new CharacterData[PartyCharacterData.Length + 1];
		for (int i = 0; i < temp.Length; i++)
		{
			PartyCharacterData[i] = temp[i];
		}
		PartyCharacterData[PartyCharacterData.Length - 1] = new CharacterData(_Character, _Level, PartyCharacterData.Length - 1);
		PartyCharacterData[PartyCharacterData.Length - 1].Health = _Character.Health;
	}

	public void Start(string save)
    {
		ChangeScene("RPGScene", save);
	}
	public void ChangeScene(string SceneName, string save)
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
		LoadFile(save);
		if (SceneName == "RPGScene")
        {
			StartCoroutine(LoadingRPGChar());
		}
	}

	public void BattleUpdate(GameBehaviour GameBehav)
    {
		PartyCharacterData = new CharacterData[GameBehav.Player.CharacterTape.Length];
		for (int i = 0; i < PartyCharacterData.Length; i++)
        {
			PartyCharacterData[i] = new CharacterData(GameBehav.Player.GetOrigCharacterTape(i), i);
		}
	}
	private IEnumerator LoadingRPGChar()
    {
		while (true)
        {
			GameObject RPGChar = GameObject.Find("Character_RPG");
			if (RPGChar != null)
			{
				CharacterMovement CharMove = RPGChar.GetComponent<CharacterMovement>();
				CharMove.SaveData = this;
				CharMove.TeleportPlayer(xPos, yPos);

				MapBehaviour MapBehav = GameObject.Find("Map").GetComponent<MapBehaviour>();
				MapBehav.SaveData = this;
				MapBehav.ChangeMap(Map);

				if (PartyCharacterData.Length > 0)
				{
					CharMove.CurrAnim = PartyCharacterData[0].GetCurrAnim();
				}
				break;
			}
			else { yield return null; }
		}
    }
}
