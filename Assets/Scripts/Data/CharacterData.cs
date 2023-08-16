using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
	public string FilePath;
	public int Health;
	public int Level;
	public int Exp;
	public int Bullet;

	public string DeckJson;
	public string ItemDeckJson;
	public int CurrAnim;

	public CharacterData(SC_Character _Character, int _Level, List<SC_Card> _ItemDeck)
    {
		FilePath = "Scriptables/" + _Character.FilePath + _Character.name;
		Level = _Level;
		Health = _Character.Health;

		//DeckJson = JsonUtility.ToJson(_Character.SkillsDeck);

		SC_Deck ItemDeck = ScriptableObject.CreateInstance<SC_Deck>();
		ItemDeck.List = _ItemDeck;
		ItemDeckJson = JsonUtility.ToJson(ItemDeck);
	}

	public CharacterData(SC_Character _Character, int _Health, int _Level, int _Exp, int _Bullet, Sprite[] _CurrAnim, SC_Deck ItemDeck)
	{
		FilePath = "Scriptables/" + _Character.FilePath + _Character.name;
		Health = _Health;
		Level = _Level;
		Exp = _Exp;
		Bullet = _Bullet;
		
		//DeckJson = JsonUtility.ToJson(_Character.SkillsDeck);
		ItemDeckJson = JsonUtility.ToJson(ItemDeck);

		SetCurrAnim(_Character, _CurrAnim);
	}

	public CharacterData(CharacterBehaviour CharacterBehav)
    {
		SC_Character _Character = CharacterBehav.Character;
		FilePath = "Scriptables/" + _Character.FilePath + _Character.name;

		Level = CharacterBehav.Level;
		Health = CharacterBehav.Health;
		Exp = CharacterBehav.Exp;
		Bullet = CharacterBehav.Bullet;

		//DeckJson = JsonUtility.ToJson(CharacterBehav.scDeck);
		ItemSetDeck(CharacterBehav.ItemDeck);

		CurrAnim = CharacterBehav.CurrAnim;
	}

	public SC_Character GetCharacter()
    {
		return Resources.Load<SC_Character>(FilePath);
	}

	public SC_Deck GetDeck()
    {
		SC_Deck scDeck = ScriptableObject.CreateInstance<SC_Deck>();
		JsonUtility.FromJsonOverwrite(DeckJson, scDeck);
		return scDeck;
	}
	
	public SC_Deck ItemGetDeck()
	{
		SC_Deck scItemDeck = ScriptableObject.CreateInstance<SC_Deck>();
		JsonUtility.FromJsonOverwrite(ItemDeckJson, scItemDeck);
		return scItemDeck;
	}
	public List<SC_Card> ItemGetList()
	{
		SC_Deck scItemDeck = ScriptableObject.CreateInstance<SC_Deck>();
		JsonUtility.FromJsonOverwrite(ItemDeckJson, scItemDeck);

		List<SC_Card> ItemDeck = new List<SC_Card>();
		for (int i = 0; i < scItemDeck.List.Count; i++)
        {
			ItemDeck.Add(scItemDeck.List[i]);
		}
		return ItemDeck;
	}
	public void ItemSetDeck(List<SC_Card> _ItemDeck)
    {
		SC_Deck ItemDeck = ScriptableObject.CreateInstance<SC_Deck>();
		ItemDeck.List = _ItemDeck;
		ItemDeckJson = JsonUtility.ToJson(ItemDeck);
	}

	public Sprite[] GetCurrAnim()
    {
		switch (CurrAnim)
		{
			case 0:
				return GetCharacter().Idle_Down_Anim;
			case 1:
				return GetCharacter().Idle_Up_Anim;
			case 2:
				return GetCharacter().Idle_Left_Anim;
			case 3:
				return GetCharacter().Idle_Right_Anim;

			case 4:
				return GetCharacter().Walk_Down_Anim;
			case 5:
				return GetCharacter().Walk_Up_Anim;
			case 6:
				return GetCharacter().Walk_Left_Anim;
			case 7:
				return GetCharacter().Walk_Right_Anim;

			case 8:
				return GetCharacter().Dead_Sprite;
		}
		return null;
	}
	public Sprite[] GetCurrAnim(Vector3 dir)
    {
		if (dir.x == -1)
        {
			return GetCharacter().Idle_Left_Anim;
		}
		else if (dir.x == 1)
        {
			return GetCharacter().Idle_Right_Anim;
		}
		else if (dir.y == -1)
		{
			return GetCharacter().Idle_Down_Anim;
		}
		else if (dir.y == 1)
		{
			return GetCharacter().Idle_Up_Anim;
		}
		return null;
	}

	public void SetCurrAnim(SC_Character _Character, Sprite[] _CurrAnim)
    {
		if (_Character.Idle_Down_Anim == _CurrAnim)
		{
			CurrAnim = 0;
		}
		else if (_Character.Idle_Up_Anim == _CurrAnim)
		{
			CurrAnim = 1;
		}
		else if (_Character.Idle_Left_Anim == _CurrAnim)
		{
			CurrAnim = 2;
		}
		else if (_Character.Idle_Right_Anim == _CurrAnim)
		{
			CurrAnim = 3;
		}

		else if (_Character.Walk_Down_Anim == _CurrAnim)
		{
			CurrAnim = 4;
		}
		else if (_Character.Walk_Up_Anim == _CurrAnim)
		{
			CurrAnim = 5;
		}
		else if (_Character.Walk_Left_Anim == _CurrAnim)
		{
			CurrAnim = 6;
		}
		else if (_Character.Walk_Right_Anim == _CurrAnim)
		{
			CurrAnim = 7;
		}

		else if (_Character.Dead_Sprite == _CurrAnim)
		{
			CurrAnim = 8;
		}
	}

	public void RestoreHealth(int HPRestore)
    {
		if (Health + HPRestore < GetCharacter().Health + 20 * Level)
        {
			Health += HPRestore;
		}
		else
        {
			RestoreMaxHealth();
		}
    }
	public void RestoreMaxHealth()
	{
		Health = GetCharacter().Health + 20 * Level;
	}
}
