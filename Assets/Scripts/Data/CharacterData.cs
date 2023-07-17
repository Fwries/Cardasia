using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
	public int OrigPos;
	public string CharJson;
	public int Health;
	public int Level;
	public int Exp;
	public int Bullet;

	public string DeckJson;
	public string ItemDeckJson;
	public int CurrAnim;

	public CharacterData(SC_Character _Character, int _Level, int _OrigPos, List<SC_Card> _ItemDeck)
    {
		OrigPos = _OrigPos;
		CharJson = JsonUtility.ToJson(_Character);
		Level = _Level;
		Health = _Character.Health;

		DeckJson = JsonUtility.ToJson(_Character.DefaultDeck);

		SC_Deck ItemDeck = ScriptableObject.CreateInstance<SC_Deck>();
		ItemDeck.Deck = _ItemDeck;
		ItemDeckJson = JsonUtility.ToJson(ItemDeck);
	}

	public CharacterData(SC_Character _Character, int _Health, int _Level, int _Exp, int _Bullet, Sprite[] _CurrAnim, int _OrigPos, SC_Deck ItemDeck)
	{
		OrigPos = _OrigPos;
		CharJson = JsonUtility.ToJson(_Character);
		Health = _Health;
		Level = _Level;
		Exp = _Exp;
		Bullet = _Bullet;
		
		DeckJson = JsonUtility.ToJson(_Character.DefaultDeck);
		ItemDeckJson = JsonUtility.ToJson(ItemDeck);

		SetCurrAnim(_Character, _CurrAnim);
	}

	public CharacterData(CharacterBehaviour CharacterBehav, int _OrigPos)
    {
		OrigPos = _OrigPos;
		CharJson = JsonUtility.ToJson(CharacterBehav.Character);

		Level = CharacterBehav.Level;
		Health = CharacterBehav.Health;
		Exp = CharacterBehav.Exp;
		Bullet = CharacterBehav.Bullet;

		DeckJson = JsonUtility.ToJson(CharacterBehav.scDeck);
		ItemSetDeck(CharacterBehav.ItemDeck);

		CurrAnim = CharacterBehav.CurrAnim;
	}

	public SC_Character GetCharacter()
    {
		SC_Character scCharacter = ScriptableObject.CreateInstance<SC_Character>();
		JsonUtility.FromJsonOverwrite(CharJson, scCharacter);
		return scCharacter;
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
		for (int i = 0; i < scItemDeck.Deck.Count; i++)
        {
			ItemDeck.Add(scItemDeck.Deck[i]);
		}
		return ItemDeck;
	}
	public void ItemSetDeck(List<SC_Card> _ItemDeck)
    {
		SC_Deck ItemDeck = ScriptableObject.CreateInstance<SC_Deck>();
		ItemDeck.Deck = _ItemDeck;
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
}
