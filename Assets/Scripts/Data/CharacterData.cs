using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
	public string CharJson;
	public int Health;
	public int Level;
	public int Exp;
	public int Bullet;

	public int CurrAnim;

	public CharacterData(SC_Character _Character, int _Level)
    {
		CharJson = JsonUtility.ToJson(_Character);
		Level = _Level;
	}

	public CharacterData(SC_Character _Character, int _Health, int _Level, int _Exp, int _Bullet, Sprite[] _CurrAnim)
	{
		CharJson = JsonUtility.ToJson(_Character);
		Health = _Health;
		Level = _Level;
		Exp = _Exp;
		Bullet = _Bullet;

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

	public SC_Character GetCharacter()
    {
		SC_Character scCharacter = ScriptableObject.CreateInstance<SC_Character>();
		JsonUtility.FromJsonOverwrite(CharJson, scCharacter);
		return scCharacter;
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
}
