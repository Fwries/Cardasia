using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterBehaviour : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDropHandler
{
    public SC_Character Character;
    public SC_Card Weapon;
    [HideInInspector] public GameBehaviour GameBehav;
    [HideInInspector] public PlayerBehaviour PlayerBehav;
    [HideInInspector] public CharacterDisplay CharDisplay;

    public List<GameObject> HandCards;
    public SC_Deck scDeck;
    public List<SC_Card> ItemDeck;
    public List<SC_Card> Deck;

    public int Level = 1;
    [HideInInspector] public int MaxHealth;
    public int Health = 1;
    [HideInInspector] public int MaxExp;
    public int Exp;

    [HideInInspector] public int MaxBoth;
    [HideInInspector] public int MaxStamina;
    [HideInInspector] public int MaxMana;

    public int Both;
    public int Stamina;
    public int Mana;

    public int DEF;
    public int ATK;

    public int DEFModif;
    public int ATKModif;

    [HideInInspector] public int MaxBullet;
    public int Bullet;

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

    public int Claw;

    public bool IsEnemy;
    public int OrigPos;
    public int CurrAnim;

    public float duration = 0.2f;
    public int strength;

    public void Init(CharacterData CharData)
    {
        Character = CharData.GetCharacter();
        OrigPos = CharData.OrigPos;
        CurrAnim = CharData.CurrAnim;

        Level = CharData.Level;

        MaxHealth = Character.Health + 20 * Level;
        Health = CharData.Health;
        
        Exp = CharData.Exp; MaxExp = 80 + 20 * Level;

        Both = MaxBoth = Character.MaxBoth;
        Stamina = MaxStamina = Character.MaxStamina;
        Mana = MaxMana = Character.MaxMana;

        DEF = Character.Defence + 5 * Level;
        ATK = Character.Attack + 10 * Level;
        Bullet = CharData.Bullet;
        MaxBullet = Character.MaxBullet;

        scDeck = Character.DefaultDeck;
        ItemDeck = CharData.ItemGetList();

        Deck = new List<SC_Card>();
        for (int i = 0; i < scDeck.Deck.Count; i++)
        {
            Deck.Add(scDeck.Deck[i]);
        }
        for (int i = 0; i < ItemDeck.Count; i++)
        {
            Deck.Add(ItemDeck[i]);
        }
        Shuffle();
    }

    public void Init(SC_Character _Character, int _Level)
    {
        Character = _Character;
        OrigPos = -1;
        CurrAnim = 0;

        Level = _Level;
        Health = MaxHealth = Character.Health + 20 * Level;
        Exp = 0; MaxExp = MaxExp = 100 + 20 * Level;

        Both = MaxBoth = Character.MaxBoth;
        Stamina = MaxStamina = Character.MaxStamina;
        Mana = MaxMana = Character.MaxMana;

        DEF = Character.Defence + 5 * Level;
        ATK = Character.Attack + 10 * Level;
        MaxBullet = Character.MaxBullet;

        scDeck = Character.DefaultDeck;

        Deck = new List<SC_Card>();
        for (int i = 0; i < scDeck.Deck.Count; i++)
        {
            Deck.Add(scDeck.Deck[i]);
        }
        Shuffle();
    }

    void Awake()
    {
        GameBehav = GameObject.Find("Stats").GetComponent<GameBehaviour>();
        CharDisplay = GetComponent<CharacterDisplay>();
        HandCards = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead) { return; }
        if (Health <= 0) 
        {
            IsDead = true;
            HandObject.SetActive(false);
            GameBehav.Delay = false;
            GameBehav.Select(GameBehav.Player.CharacterTape[0]);

            if (IsEnemy)
            {
                StartCoroutine(GameBehav.GiveEXP(GameBehav.Player, this));
            }
            else
            {
                PlayerBehav.RotationBehav.UpdateDeadCharacters();
                gameObject.SetActive(false);
            }
        }
        if (Exp >= MaxExp)
        {
            Exp -= MaxExp;
            LevelUp();
        }
    }

    public void Shuffle()
    {
        //AudioManager.Instance.PlaySFX("shuffle");
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
        if (Trip) { Popup("Tripped", Color.black); return; }
        for (int index1 = 0; index1 < DrawNum; index1++)
        {
            if (HandCards == null || HandCards.Count == 5) { return; }
            if (Deck.Count > 0)
            {
                int index2 = Deck.Count - 1;

                GameObject Card = Instantiate(Resources.Load("Card", typeof(GameObject))) as GameObject;
                Card.transform.SetParent(HandObject.transform);
                Card.GetComponent<CardBehaviour>().Currentcard = Card.GetComponent<CardDisplay>().Currentcard = Deck[index2];
                Card.GetComponent<CardBehaviour>().CharacterBehav = this;

                AudioManager.Instance.PlaySFX("draw");
                HandCards.Add(Card);
                AdjustHand();
                Deck.RemoveAt(index2);
            }
            else if (Deck.Count <= 0)
            {
                AddCard(Resources.Load<SC_Card>("Scriptables/Cards/Skill/Struggle"));
            }
        }
        AdjustHand();
    }

    public void AddCard(SC_Card SCCard)
    {
        AudioManager.Instance.PlaySFX("draw");
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
        GameBehav.GameDis.CardDisplay.SetActive(false);
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
        if (Card.Currentcard.CardType == SC_Card.Type.Consumable)
            return true;
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

    public void DealDamage(SC_Card Card, int DMG, int CritType, bool Pierce, CharacterBehaviour Target)
    {
        int CritMultiplier = 1, IsConsumable = 1;
        int IsPierce = 1, bonus = 0;
        GameBehav.Delay = true;

        if (CritChance > CritType && CritType != 0) { CritType = CritChance; }
        if (Pierce) { IsPierce = 0; }
        if (Card.CardTrait == "<Claw>") { bonus += Claw; }
        if (Card.CardType == SC_Card.Type.Consumable) { IsConsumable = 0; }

        switch (CritType)
        {
            case 0: // No Crit
                AudioManager.Instance.PlaySFX("Impact");
                break;
            case 1: // Base Crit
                if (Random.Range(0, 100) <= 12) { CritMultiplier = 2; }
                AudioManager.Instance.PlaySFX("Impact");
                break;
            case 2: // High Crit
                if (Random.Range(0, 2) == 0) { CritMultiplier = 2; }
                AudioManager.Instance.PlaySFX("Crit");
                break;
            case 3: // Gurantee Crit
                CritMultiplier = 2;
                AudioManager.Instance.PlaySFX("Crit");
                break;
        }

        if (Target == null)
        {
            Target = PlayerBehav.GetTarget(4, GameBehav.GetOpponent(PlayerBehav));
        }
        Target.DealtDamage((DMG * CritMultiplier + ATK * IsConsumable + bonus) - IsPierce * Target.DEF, CritMultiplier);
    }

    public void DealtDamage(int DMG)
    {
        if (DMG < 0) { DMG = 0; }
        AudioManager.Instance.PlaySFX("Impact");
        Popup("" + DMG, Color.red);
        Shake(DMG);
    }

    public void DealtDamage(int DMG, int Crit)
    {
        Color TextColor = Color.red;

        if (Crit == 2) { TextColor = Color.yellow; }
        if (DMG < 0) { DMG = 0; TextColor = Color.red; }

        Popup("" + DMG, TextColor);
        Shake(DMG);
    }

    public void Shake(int _strength)
    {
        GameBehav.Delay = true;
        strength = _strength * 10;
        StartCoroutine(Shaking(_strength));
    }

    private IEnumerator Shaking(int DMG)
    {
        Vector3 StartPos = this.transform.position;
        Vector3 ShakePos;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            ShakePos = Random.insideUnitSphere * strength / 100;
            transform.position += ShakePos;
            yield return null;
        }

        transform.position = StartPos;
        StartCoroutine(HealthDecreasePerFrame(DMG));
    }

    public IEnumerator HealthDecreasePerFrame(int DMG)
    {
        GameBehav.Delay = true;

        int i = 0;
        float elapsedTime = 0;
        float Speed = 1f / DMG;

        while (i < DMG)
        {
            if (elapsedTime >= Speed)
            {
                Health--; i++;
                elapsedTime = 0;
            }
            else { elapsedTime += Time.deltaTime; yield return null; }
        }
        GameBehav.Delay = false;
    }

    public void RestoreHealth(int HealthRestored)
    {
        if (HealthRestored + Health > MaxHealth)
        {
            StartCoroutine(HealthRestoredPerFrame(MaxHealth - Health));
        }
        else
        {
            StartCoroutine(HealthRestoredPerFrame(HealthRestored));
        }
    }

    public IEnumerator HealthRestoredPerFrame(int HealthRestored)
    {
        GameBehav.Delay = true;

        int i = 0;
        float elapsedTime = 0;
        float Speed = 1f / HealthRestored;
        AudioManager.Instance.PlaySFX("Heal");

        while (i < HealthRestored)
        {
            if (elapsedTime >= Speed)
            {
                Health++; i++;
                elapsedTime = 0;
            }
            else { elapsedTime += Time.deltaTime; yield return null; }
        }
        GameBehav.Delay = false;
    }

    public void FullRestore()
    {
        if (IsDead) { return; }
        StartCoroutine(HealthRestoredPerFrame(MaxHealth - Health));
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

    public void LevelUp()
    {
        Level++;
        AudioManager.Instance.PlaySFX("LevelUp");
        Popup("Level Up!" + "\n" + "+10 ATK +5 DEF", Color.green);
        
        Health = MaxHealth = Character.Health + 20 * Level;
        DEF = Character.Defence + 5 * Level;
        ATK = Character.Attack + 10 * Level;

        Both = MaxBoth = Character.MaxBoth;
        Stamina = MaxStamina = Character.MaxStamina;

        MaxExp = 80 + 20 * Level;
    }

    public void Popup(string text, Color textColor)
    {
        GameObject DMGPopup = Instantiate(Resources.Load("DmgPopup"), this.transform.position, Quaternion.identity, this.transform) as GameObject;
        DamagePopup PopupBehav = DMGPopup.GetComponent<DamagePopup>();
        PopupBehav.Init(text, textColor);
    }
}
