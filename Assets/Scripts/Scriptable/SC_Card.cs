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
        Stamina = 1,
        Mana = 2,
        Both = 3,
    };
    public Type CardType;

    public enum Rariety
    {
        None,
        Common = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4,
    };
    public Rariety CardRariety;

    public bool DoesTarget;
    public enum Target
    {
        None,
        Enemy = 1,
        Ally = 2,
        Centre = 3,
        RandomEnemy = 4
    };
    public Target CardTarget;

    public List<string> CardClass;
    public string CardTrait;
    public string CardSkill;
    public int CardCost;
}
