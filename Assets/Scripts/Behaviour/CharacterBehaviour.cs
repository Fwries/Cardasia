using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public SC_Character Character;

    public List<GameObject> HandCards;
    public List<GameObject> Deck;

    [HideInInspector] public int MaxBoth;
    [HideInInspector] public int MaxStamina;
    [HideInInspector] public int MaxMana;

    public int Both;
    public int Stamina;
    public int Mana;

    public int Level = 1;
    public bool IsActive;

    public int Freeze;
    public int Shock;
    public bool Burn;
    public bool Trip;

    // Start is called before the first frame update
    void Start()
    {
        Both = MaxBoth = Character.MaxBoth;
        Stamina = MaxStamina = Character.MaxStamina;
        Mana = MaxMana = Character.MaxMana;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
