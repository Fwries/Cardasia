using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Cardasia/Character")]
public class SC_Character : ScriptableObject
{
    [SerializeField] public string CharName;

    [SerializeField] public int Health;
    [SerializeField] public int Defence;
    [SerializeField] public int Attack;

    [SerializeField] public int MaxBoth;
    [SerializeField] public int MaxStamina;
    [SerializeField] public int MaxMana;

    [SerializeField] public int MaxBullet;

    [SerializeField] public SC_Deck DefaultDeck;

    [SerializeField] public Sprite[] Idle_Down_Anim;
    [SerializeField] public Sprite[] Idle_Up_Anim;
    [SerializeField] public Sprite[] Idle_Left_Anim;
    [SerializeField] public Sprite[] Idle_Right_Anim;

    [SerializeField] public Sprite[] Walk_Down_Anim;
    [SerializeField] public Sprite[] Walk_Up_Anim;
    [SerializeField] public Sprite[] Walk_Left_Anim;
    [SerializeField] public Sprite[] Walk_Right_Anim;

    [SerializeField] public Sprite[] Dead_Sprite;

    [SerializeField] public int EXPGain;
}
