using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Cardasia/Character")]
public class SC_Character : ScriptableObject
{
    public string CardName;

    public int Health;
    public int Strength;
    public int Armour;

    public int MaxBoth;
    public int MaxStamina;
    public int MaxMana;

    public enum Class
    {
        None,
        Fighter,
        Rogue,
        Mage,
        Archer,
        Backpack,
    };
    public Class CharacterClass;

    public enum SubClass
    {
        None,
        Fighter,
        Rogue,
        Mage,
        Archer,
        Backpack,
    };
    public SubClass CharacterSubClass;

    public Sprite[] Idle_Down_Anim;
    public Sprite[] Idle_Up_Anim;
    public Sprite[] Idle_Left_Anim;
    public Sprite[] Idle_Right_Anim;

    public Sprite[] Walk_Down_Anim;
    public Sprite[] Walk_Up_Anim;
    public Sprite[] Walk_Left_Anim;
    public Sprite[] Walk_Right_Anim;

    public Sprite[] Dead_Sprite;
}
