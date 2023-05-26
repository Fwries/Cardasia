using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterBehaviour : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDropHandler
{
    public SC_Character Character;
    [HideInInspector] public GameBehaviour GameBehav;
    [HideInInspector] public PlayerBehaviour PlayerBehav;
    public CharacterDisplay CharDisplay;

    public List<GameObject> HandCards;
    public SC_Deck scDeck;
    public List<SC_Card> Deck;

    public int Level = 1;
    [HideInInspector] public int MaxHealth;
    public int Health;
    [HideInInspector] public int MaxExp;
    public int Exp;
    [HideInInspector] public int MaxBullet;
    public int Bullet;

    [HideInInspector] public int MaxBoth;
    [HideInInspector] public int MaxStamina;
    [HideInInspector] public int MaxMana;

    public int Both;
    public int Stamina;
    public int Mana;

    [HideInInspector] public bool[] ShockMana = { false, false, false, false, false };

    [HideInInspector] public bool IsActive;
    [HideInInspector] public bool IsDead;
    [HideInInspector] public float LeftMost;
    [HideInInspector] public GameObject HandObject;

    public int CritChance = 1;

    public int Freeze;
    public int Shock;
    public bool Burn;
    public bool Trip;
    public bool Ward;

    public bool IsEnemy;

    void Awake()
    {
        GameBehav = GameObject.Find("Stats").GetComponent<GameBehaviour>();

        Health = MaxHealth = Character.Health;
        Exp = 0; MaxExp = 100;

        Both = MaxBoth = Character.MaxBoth;
        Stamina = MaxStamina = Character.MaxStamina;
        Mana = MaxMana = Character.MaxMana;

        Deck = new List<SC_Card>();
        for (int i = 0; i < scDeck.Deck.Count; i++)
        {
            Deck.Add(scDeck.Deck[i]);
        }
        Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead) { return; }
        if (Health <= 0) 
        {
            IsDead = true; 
        }
        if (Exp >= MaxExp)
        {
            Exp -= MaxExp;
            Level++;

            Health = MaxHealth = Character.Health;
            Both = MaxBoth = Character.MaxBoth;
            Stamina = MaxStamina = Character.MaxStamina;
            Mana = MaxMana = Character.MaxMana;
        }
    }

    public void Shuffle()
    {
        for (int i = 0; i < Deck.Count; ++i)
        {
            int Rand = Random.Range(i, Deck.Count);
            CardSwap(Rand, i);
        }
    }

    public void CardSwap(int randomIndex, int i)
    {
        SC_Card currentcard = Deck[i];
        Deck[i] = Deck[randomIndex];
        Deck[randomIndex] = currentcard;
    }

    public void Draw(int DrawNum)
    {
        if (Trip) { return; }
        for (int index1 = 0; index1 < DrawNum; index1++)
        {
            if (HandCards.Count == 5) { return; }
            if (Deck.Count > 0)
            {
                int index2 = Deck.Count - 1;

                GameObject Card = Instantiate(Resources.Load("Card", typeof(GameObject))) as GameObject;
                Card.transform.SetParent(HandObject.transform);
                Card.GetComponent<CardBehaviour>().Currentcard = Card.GetComponent<CardDisplay>().Currentcard = Deck[index2];
                Card.GetComponent<CardBehaviour>().CharacterBehav = this;

                HandCards.Add(Card);
                AdjustHand();
                Deck.RemoveAt(index2);
            }
            else if (Deck.Count <= 0)
            {
                Debug.Log("Deckout");
            }
        }
        AdjustHand();
    }

    public void AddCard(SC_Card SCCard)
    {
        if (Trip) { return; }

        GameObject Card = Instantiate(Resources.Load("Card", typeof(GameObject))) as GameObject;
        Card.transform.SetParent(HandObject.transform);
        Card.GetComponent<CardBehaviour>().Currentcard = Card.GetComponent<CardDisplay>().Currentcard = SCCard;
        Card.GetComponent<CardBehaviour>().CharacterBehav = this;

        HandCards.Add(Card);
        AdjustHand();
    }

    public void AdjustHand()
    {
        LeftMost = HandObject.transform.position.x + (HandCards.Count - 1) * -100.0f;
        for (int i = 0; i < HandCards.Count; i++)
        {
            if (!HandCards[i].GetComponent<DragDrop>().IsDragging)
                HandCards[i].transform.position = new Vector3(LeftMost + (i * 200), HandObject.transform.position.y, HandObject.transform.position.z);
            HandCards[i].GetComponent<CardDisplay>().PositionIndex = i;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsEnemy) { return; }
        GameBehav.Select(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (IsDead) { return; }
        if (eventData.pointerDrag.GetComponent<CardBehaviour>() == null) { return; }
        if (eventData.pointerDrag.GetComponent<CardBehaviour>().Frozen) { return; }

        CardBehaviour Card = eventData.pointerDrag.GetComponent<CardBehaviour>();
        CharacterBehaviour Character = Card.CharacterBehav;

        if (!Character.CanBePlayed(Card, true)) { return; }

        eventData.pointerDrag.GetComponent<CardBehaviour>().Play(this);
        Character.HandCards.Remove(eventData.pointerDrag);

        Character.AdjustHand();
        Destroy(eventData.pointerDrag);
    }

    public bool CanBePlayed(CardBehaviour Card, bool deduct)
    {
        if (Card.CardCost <= Both + Stamina + Mana)
        {
            if (Card.Currentcard.CardType == SC_Card.Type.Stamina)
            {
                if (Card.CardCost <= Stamina)
                {
                    if (deduct) 
                    {
                        Stamina -= Card.CardCost;
                    }
                    return true;
                }
                else if (Card.CardCost <= Stamina + Both)
                {
                    if (deduct)
                    {
                        int LeftOverCost = Card.CardCost - Stamina;
                        Stamina = 0;
                        Both -= LeftOverCost;
                    }
                    return true;
                }
                else { return false; }
            }
            else if (Card.Currentcard.CardType == SC_Card.Type.Mana)
            {
                if (Card.CardCost <= Mana)
                {
                    if (deduct)
                    {
                        Mana -= Card.CardCost;
                    }
                    return true;
                }
                else if (Card.CardCost <= Mana + Both)
                {
                    if (deduct)
                    {
                        int LeftOverCost = Card.CardCost - Mana;
                        Mana = 0;
                        Both -= LeftOverCost;
                    }
                    return true;
                }
                else { return false; }
            }
            else if (Card.Currentcard.CardType == SC_Card.Type.Both)
            {
                if (Card.CardCost <= Mana)
                {
                    if (deduct)
                    {
                        Mana -= Card.CardCost;
                    }
                    return true;
                }
                else if (Card.CardCost <= Mana + Stamina)
                {
                    if (deduct)
                    {
                        int LeftOverCost = Card.CardCost - Mana;
                        Mana = 0;
                        Stamina -= LeftOverCost;
                    }
                    return true;
                }
                else if (Card.CardCost <= Mana + Stamina + Both)
                {
                    if (deduct)
                    {
                        int LeftOverCost = Card.CardCost - Mana;
                        Mana = 0;
                        LeftOverCost -= Stamina;
                        Stamina = 0;
                        Both -= LeftOverCost;
                    }
                    return true;
                }
                else { return false; }
            }
        }
        return false;
    }

    public void DealDamage(int DMG, int CritType, CharacterBehaviour Target)
    {
        int CritMultiplier = 1;
        if (CritChance > CritType && CritType != 0) { CritType = CritChance; }
        switch (CritType)
        {
            case 0: // No Crit
                break;
            case 1: // Base Crit
                if (Random.Range(0, 4) == 0) { CritMultiplier = 2; }
                break;
            case 2: // High Crit
                if (Random.Range(0, 2) == 0) { CritMultiplier = 2; }
                break;
            case 3: // Gurantee Crit
                CritMultiplier = 2;
                break;
        }
        Target.DealtDamage(DMG * CritMultiplier);
    }

    public void DealtDamage(int DMG)
    {
        if (IsDead) { return; }
        Health -= DMG;
    }

    public void RestoreHealth(int HealthRestored)
    {
        if (IsDead) { return; }

        if (HealthRestored + Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        else
        {
            Health += HealthRestored;
        }
    }
    
    public void FullRestore()
    {
        if (IsDead) { return; }
        Health = MaxHealth;
    }

    public void GainMana(string ManaType, int Amt)
    {
        for (int i = 0; i < Amt; i++)
        {
            if (Both + Stamina + Mana < 5)
            {
                switch (ManaType)
                {
                    case "Consumable":
                        break;
                    case "Stamina":
                        Stamina += 1;
                        break;
                    case "Mana":
                        Mana += 1;
                        break;
                    case "Both":
                        Both += 1;
                        break;
                }
            }
        }
    }

    public void Reload(int Amt)
    {
        if (Amt + Bullet > MaxBullet)
        {
            Bullet = MaxBullet;
        }
        else
        {
            Bullet += Amt;
        }
    }

    public void ClearStatus(string Status)
    {
        string[] StatusType = { "Freeze", "Shock", "Burn", "Trip" };

        if (Status == "Random")
        {
            Status = StatusType[Random.Range(0, 4)];
        }

        switch (Status)
        {
            case "Freeze":
                Freeze = 0;
                for (int freeze = 0; freeze < HandCards.Count; freeze++)
                {
                    HandCards[freeze].GetComponent<CardBehaviour>().Frozen = false;
                    HandCards[freeze].GetComponent<CardDisplay>().FrozenDisplay.SetActive(false);
                }
                break;
            case "Shock":
                Shock = 0;
                for (int shock = 0; shock < ShockMana.Length; shock++)
                {
                    ShockMana[shock] = false;
                }
                break;
            case "Burn":
                Burn = false;
                break;
            case "Trip":
                Trip = false;
                break;
        }
    }
}
