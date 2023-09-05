using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cardasia/Card")]
public class SC_Card : ScriptableObject
{
    public Sprite CardArt;
    public string CardName;

    public enum Type
    {
        Consumable = 0,
        Skill = 1,
        
        Weapon = 2,
        Ability = 3
    };
    [SerializeField, HideInInspector] public Type CardType;

    public enum ManaType
    {
        None = 0,
        Stamina = 1,
        Mana = 2,
        Both = 3,
    };
    [SerializeField, HideInInspector] public ManaType CardManaType;

    public enum Rariety
    {
        None,
        Common = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4,
    };
    [SerializeField, HideInInspector] public Rariety CardRariety;

    [SerializeField, HideInInspector] public SC_Character.Class CardClass;

    [SerializeField, HideInInspector] public bool DoesTarget;
    public enum Target
    {
        None,
        Agroo = 1,
        Ally = 2,
        Centre = 3,
        LowestEnemy = 4,
        RandomEnemy = 5
    };
    [SerializeField, HideInInspector] public Target CardTarget;

    [SerializeField, HideInInspector] public string CardTrait;
    [SerializeField, HideInInspector] public string CardSkill;

    // Weapon, Ability
    [SerializeField, HideInInspector] public bool IsDragable = false;
    [SerializeField, HideInInspector] public string[] StrongAgainst;
    [SerializeField, HideInInspector] public int CardAtk;
    [SerializeField, HideInInspector] public int CardHp;

    [SerializeField, HideInInspector] public int CardCost = 0;
    [SerializeField, HideInInspector] public int GoldCost = 0;

    // Consumable, Skill
    [SerializeField, HideInInspector] public SC_Deck CardList;
}
