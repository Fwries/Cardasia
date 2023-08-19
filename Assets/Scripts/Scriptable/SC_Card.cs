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
        Armour = 3
    };
    public Type CardType;

    public enum ManaType
    {
        None = 0,
        Stamina = 1,
        Mana = 2,
        Both = 3,
    };
    public ManaType CardManaType;

    public enum Rariety
    {
        None,
        Common = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4,
    };
    public Rariety CardRariety;

    public SC_Character.Class CardClass;

    public bool DoesTarget;
    public enum Target
    {
        None,
        Agroo = 1,
        Ally = 2,
        Centre = 3,
        LowestEnemy = 4,
        RandomEnemy = 5
    };
    public Target CardTarget;

    public string CardTrait;
    public string CardSkill;
    
    public int CardCost;
    public int GoldCost;

    public SC_Deck CardList;
}
