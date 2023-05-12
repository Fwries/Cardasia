using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public SC_Character Character;

    public List<GameObject> HandCards;
    public List<GameObject> Deck;

    [HideInInspector] public int MaxHealth;
    public int Health;

    [HideInInspector] public int MaxBoth;
    [HideInInspector] public int MaxStamina;
    [HideInInspector] public int MaxMana;

    public int Both;
    public int Stamina;
    public int Mana;

    public bool[] ShockMana = { false, false, false, false, false };

    public int Level = 1;
    public bool IsActive;

    public int Freeze;
    public int Shock;
    public bool Burn;
    public bool Trip;

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth = Character.Health;
        Both = MaxBoth = Character.MaxBoth;
        Stamina = MaxStamina = Character.MaxStamina;
        Mana = MaxMana = Character.MaxMana;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shuffle(int randomIndex, int i)
    {
        SC_Card currentcard = Deck[i].GetComponent<CardDisplay>().Currentcard;
        Deck[i].GetComponent<CardDisplay>().Currentcard = Deck[randomIndex].GetComponent<CardDisplay>().Currentcard;
        Deck[randomIndex].GetComponent<CardDisplay>().Currentcard = currentcard;
    }

    public void Draw(int DrawNum)
    {
        if (Trip) { return; }
        for (int index1 = 0; index1 < DrawNum; index1++)
        {
            if (Deck.Count > 0)
            {
                int index2 = Deck.Count - 1;
                HandCards.Add(Deck[index2]);
                HandCards[HandCards.Count + 1].GetComponent<CardBehaviour>().Zones = 1;
                AdjustHand();
                Deck.RemoveAt(index2);
            }
            else if (Deck.Count <= 0)
            {

            }
        }
    }

    public void AdjustHand()
    {

    }
}
