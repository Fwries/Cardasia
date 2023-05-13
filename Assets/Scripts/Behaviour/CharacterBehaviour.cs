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

    [HideInInspector] public int MaxBoth;
    [HideInInspector] public int MaxStamina;
    [HideInInspector] public int MaxMana;

    public int Both;
    public int Stamina;
    public int Mana;

    public bool[] ShockMana = { false, false, false, false, false };

    [HideInInspector] public bool IsActive;
    [HideInInspector] public bool IsDead;
    [HideInInspector] public float LeftMost;
    [HideInInspector] public GameObject HandObject;

    public int Freeze;
    public int Shock;
    public bool Burn;
    public bool Trip;

    void Awake()
    {
        GameBehav = GameObject.Find("Stats").GetComponent<GameBehaviour>();
        PlayerBehav = GameBehav.Player;

        if (PlayerBehav == GameBehav.Player) { CharDisplay.SetActive(true); }

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
            this.gameObject.GetComponent<Image>().sprite = Character.Dead_Sprite[Random.Range(0, Character.Dead_Sprite.Length)];
            IsDead = true; 
        }

        if (Input.GetKeyDown("r"))
        {
            Shuffle();
        }
        if (Input.GetKeyDown("t"))
        {
            Draw(1);
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
                Card.GetComponent<CardDisplay>().Currentcard = Deck[index2];
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
        if (IsDead) { return; }
        GameBehav.Select(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (IsDead) { return; }

        CardBehaviour Card = eventData.pointerDrag.GetComponent<CardBehaviour>();
        CharacterBehaviour Character = Card.CharacterBehav;

        if (Card.CardCost <= Character.Both + Character.Stamina + Character.Mana)
        {
            if (Card.Currentcard.CardType == SC_Card.Type.Stamina)
            {
                if (Card.CardCost <= Character.Stamina)
                {
                    Character.Stamina -= Card.CardCost;
                }
                else if (Card.CardCost <= Character.Stamina + Character.Both)
                {
                    int LeftOverCost = Card.CardCost - Character.Stamina;
                    Character.Stamina = 0;
                    Character.Both -= LeftOverCost;
                }
                else { return; }
            }
            else if (Card.Currentcard.CardType == SC_Card.Type.Mana)
            {
                if (Card.CardCost <= Character.Mana)
                {
                    Character.Mana -= Card.CardCost;
                }
                else if (Card.CardCost <= Character.Mana + Character.Both)
                {
                    int LeftOverCost = Card.CardCost - Character.Mana;
                    Character.Mana = 0;
                    Character.Both -= LeftOverCost;
                }
                else { return; }
            }
            else if (Card.Currentcard.CardType == SC_Card.Type.Both)
            {
                if (Card.CardCost <= Character.Mana)
                {
                    Character.Mana -= Card.CardCost;
                }
                else if (Card.CardCost <= Character.Mana + Character.Stamina)
                {
                    int LeftOverCost = Card.CardCost - Character.Mana;
                    Character.Mana = 0;
                    Character.Stamina -= LeftOverCost;
                }
                else { return; }
            }
        }
        else { return; }

        eventData.pointerDrag.GetComponent<CardBehaviour>().Play(this);
        Character.HandCards.Remove(eventData.pointerDrag);

        Character.AdjustHand();
        Destroy(eventData.pointerDrag);
    }
}
