using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "New Character", menuName = "Cardasia/Character")]
public class SC_Character : ScriptableObject
{
    public string CharName;
    public string FilePath = "Character/";

    public int Health;
    public int Defence;
    public int Attack;

    public int MaxBoth;
    public int MaxStamina;
    public int MaxMana;

    public enum Class
    {
        None,
    };
    public Class CharacterClass;

    public int MaxBullet;
    public int MaxSkillsDeck = 5;
    public int MaxDeckSize = 30;

    public List<SC_Card> DefaultDeck;
    public List<SC_Card> SkillDeck;

    public SC_Card Weapon;
    public SC_Card Ability;

    public Sprite[] Idle_Down_Anim;
    public Sprite[] Idle_Up_Anim;
    public Sprite[] Idle_Left_Anim;
    public Sprite[] Idle_Right_Anim;

    public Sprite[] Walk_Down_Anim;
    public Sprite[] Walk_Up_Anim;
    public Sprite[] Walk_Left_Anim;
    public Sprite[] Walk_Right_Anim;

    public Sprite[] Dead_Sprite;

    public int EXPGain;
    public int GoldGain;
}
