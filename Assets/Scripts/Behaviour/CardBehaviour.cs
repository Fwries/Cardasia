using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour
{
    public CharacterBehaviour CharacterBehav;
    public SC_Card Currentcard;

    public bool IsGenerated;
    public int CardCost;
    public bool Frozen;

    public float m_Speed = 1.7f;
    private int RepeatError = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play(CharacterBehaviour target)
    {
        if (target == null) { return; }

        GameBehaviour GameBehav = CharacterBehav.GameBehav;

        int MaxActive = 3;
        if (target.PlayerBehav.CharacterTape.Length < 3)
        {
            MaxActive = target.PlayerBehav.CharacterTape.Length;
        }

        if (!IsGenerated && Currentcard.CardType == SC_Card.Type.Consumable)
        {
            for (int i = 0; i < CharacterBehav.ItemDeck.Count; i++)
            {
                if (Currentcard.CardName == CharacterBehav.ItemDeck[i].CardName)
                {
                    CharacterBehav.ItemDeck.RemoveAt(i);
                    break;
                }
            }
        }

        switch (Currentcard.CardName)
        {
            #region CardSkills

            case "All For One":
                int Atk = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (i < CharacterBehav.PlayerBehav.CharacterTape.Length && CharacterBehav != CharacterBehav.PlayerBehav.CharacterTape[i])
                    {
                        Atk += CharacterBehav.PlayerBehav.CharacterTape[i].ATK + CharacterBehav.PlayerBehav.CharacterTape[i].ATKModif;
                    }
                }
                CharacterBehav.DealDamage(Currentcard, Atk, 1, true, target);
                break;
            case "Backstab":
                if (CharacterBehav.PlayerBehav.CardPlayed == false)
                {
                    CharacterBehav.DealDamage(Currentcard, 20, 2, false, target);
                    CharacterBehav.Draw(1);
                }
                else
                {
                    CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                }
                break;
            case "Bayonet":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                if (CharacterBehav.PlayerBehav.CardPlayed == false) { CharacterBehav.Draw(1); }
                break;
            case "Campfire":
                for (int i = 0; i < MaxActive; i++)
                {
                    FullRestore(CharacterBehav.PlayerBehav.CharacterTape[i]);
                }
                CharacterBehav.PlayerBehav.LoseATurn = true;
                GameBehav.EndTurn(CharacterBehav.IsEnemy);
                break;
            case "Cryo Chamber":
                FullRestore(target);
                target.Freeze = 5;
                target.Trip = true;
                break;
            case "Claw":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                break;
            case "Daggers & Roses":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                CharacterBehav.Draw(1);
                target.Trip = true;
                break;
            case "Drill":
                CharacterBehav.DealDamage(Currentcard, 70, 1, true, target);
                break;
            case "Extra Time":
                target.GainMana("Stamina", 1);
                break;
            case "Fairy Wand":
                CharacterBehav.AddCard(GetRandomCard(false));
                break;
            case "Guns & Roses":
                if (CharacterBehav.Bullet < 1) { break; }
                CharacterBehav.Bullet--;
                CharacterBehav.DealDamage(Currentcard, 70, 1, false, target);
                target.Trip = true;
                break;
            case "Healing":
                Heal(target, 70);
                break;
            case "Nuclear Bomb":
                for (int i = 0; i < 3; i++)
                {
                    if (i < GameBehav.Player.CharacterTape.Length)
                    {
                        GameBehav.Player.CharacterTape[i].DealtDamage(999);
                    }
                    if (i < GameBehav.Opponent.CharacterTape.Length)
                    {
                        GameBehav.Opponent.CharacterTape[i].DealtDamage(999);
                    }
                }
                break;
            case "Overdrive":
                CharacterBehav.GainMana("Both", 3);
                break;
            case "Power Up":
                CharacterBehav.ATKModif += 20;
                break;
            case "Punch":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                break;
            case "Barrel Reload": // Reload Gun Barrel
                CharacterBehav.Reload(6);
                break;
            case "Rose":
                target.Trip = true;
                break;
            case "Run":
                CharacterBehav.Draw(1);
                if (CharacterBehav.PlayerBehav.CardPlayed == false) { CharacterBehav.Draw(1); }
                break;
            case "Saber Slash":
                CharacterBehav.DealDamage(Currentcard, 70, 2, false, target);
                break;
            case "Struggle":
                CharacterBehav.Stamina = 0;
                CharacterBehav.DealtDamage(20);
                CharacterBehav.DealDamage(Currentcard, 20, 0, false, target);
                break;
            case "Sprint":
                CharacterBehav.Draw(2);
                if (CharacterBehav.PlayerBehav.CardPlayed == false) { CharacterBehav.Draw(1); }
                break;
            case "Sword Clash":
                CharacterBehav.DealDamage(Currentcard, CharacterBehav.ATK - target.ATK, 0, true, target);
                break;
            case "Targeting":
                target.Ward = true;
                break;
            case "Thunder Blade":
                CharacterBehav.DealDamage(Currentcard, 70, 1, false, target);
                target.Shock += 1;
                break;
            case "Quiver":
                CharacterBehav.AddCard(GetRandomCard(true));
                CharacterBehav.AddCard(GetRandomCard(true));
                break;

            case "Clear Body":
                CharacterBehav.RestoreHealth(100);
                CharacterBehav.Draw(1);
                break;
            case "Fly":
                CharacterBehav.Draw(2);
                break;
            case "Grasping Claws":
                CharacterBehav.DealDamage(Currentcard, 30, 1, false, target);
                target.Trip = true;
                break;
            case "Midnight Claw":
                for (int i = 0; i < 3; i++)
                {
                    if (i < CharacterBehav.PlayerBehav.CharacterTape.Length)
                    {
                        CharacterBehav.PlayerBehav.CharacterTape[i].Claw += 20;
                    }
                }
                break;
            case "Sonic Screech":
                for (int i = 0; i < 3; i++)
                {
                    if (i < target.PlayerBehav.CharacterTape.Length)
                    {
                        target.PlayerBehav.CharacterTape[i].Trip = true;
                        CharacterBehav.DealDamage(Currentcard, 0, 1, false, target.PlayerBehav.CharacterTape[i]);
                    }
                }
                break;

            #endregion CardSkills

            #region Consumables

            case "Bullet":
                CharacterBehav.Reload(1);
                break;
            case "Bullets":
                CharacterBehav.Reload(3);
                break;
            case "Flash Grenade":
                for (int i = 0; i < MaxActive; i++)
                {
                    GameBehav.Opponent.CharacterTape[i].Shock += 1;
                }
                break;
            case "Health Capsule":
                Heal(target, 150);
                break;
            case "Health Potion": // Heart Bottle
                Heal(target, 70);
                break;
            case "Heavy Bullets":
                CharacterBehav.Reload(3);
                CharacterBehav.CritChance = 2;
                break;
            case "Medicines":
                Heal(target, 50);
                target.Burn = false;
                target.Trip = false;
                break;
            case "Miracle Medicine":
                Heal(target, 300);
                target.ClearStatus("Freeze");
                target.ClearStatus("Shock");
                target.Burn = false;
                target.Trip = false;
                break;
            case "Overdose":
                Heal(target, -20);
                target.ClearStatus("Random");
                target.ClearStatus("Random");
                target.ClearStatus("Random");
                target.ClearStatus("Random");
                break;
            case "Pill":
                target.ClearStatus("Random");
                break;
            case "Stun Grenade":
                target.Shock += 2;
                break;
            case "Supersonic Bullet":
                CharacterBehav.Reload(1);
                CharacterBehav.Draw(1);
                break;
            case "Syringe":
                Heal(target, 70);
                break;

            case "Arrow":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                break;
            case "Chemical Arrow":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                switch (Random.Range(0, 4))
                {
                    case 0:
                        target.Freeze += 1;
                        break;
                    case 1:
                        target.Shock += 1;
                        break;
                    case 2:
                        target.Burn = true;
                        break;
                    case 3:
                        target.Trip = true;
                        break;
                }
                break;
            case "Charged Arrow":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                target.Shock += 1;
                break;
            case "Barbed Arrow":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                break;
            case "Interleaved Arrows":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                target.Trip = true;
                break;
            case "Frozen Arrow":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                target.Freeze += 1;
                break;
            case "Spiral Arrow":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                target.PlayerBehav.RotationBehav.Shift(true);
                break;
            case "Fast Arrow":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                CharacterBehav.Draw(2);
                break;
            case "Winged Arrow":
                CharacterBehav.DealDamage(Currentcard, 20, 2, false, target);
                CharacterBehav.Draw(2);
                break;
            case "Slicing Arrow":
                CharacterBehav.DealDamage(Currentcard, 20, 1, true, target);
                break;
            case "Arrow Cluster":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                break;
            case "Broken Arrow":
                if (Random.Range(0, 2) == 0)
                {
                    CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                }
                break;
            case "Branch Arrow":
                CharacterBehav.DealDamage(Currentcard, 20, 1, false, target);
                break;
            case "Crosshair Arrow":
                CharacterBehav.DealDamage(Currentcard, 20, 2, false, target);
                break;
            case "Supersonic Arrow":
                CharacterBehav.DealDamage(Currentcard, 40, 1, true, target);
                break;

            #endregion Consumables

            #region NOTcoded

            #endregion NOTcoded

            default:
                Debug.LogError(Currentcard.CardName + " does not exist");
                break;
        }
        CharacterBehav.PlayerBehav.CardPlayed = true;
        if (CharacterBehav.PlayerBehav == GameBehav.Player) { GameBehav.RunObj.GetComponent<Button>().interactable = false; }
    }

    public void Heal(CharacterBehaviour target, int HealAmt)
    {
        CharacterBehav.AddAgroo(CharacterBehav.GameBehav.GetOpponent(CharacterBehav.PlayerBehav), HealAmt * 1.5f);
        target.RestoreHealth(HealAmt);
    }

    public void FullRestore(CharacterBehaviour target)
    {
        CharacterBehav.AddAgroo(CharacterBehav.GameBehav.GetOpponent(CharacterBehav.PlayerBehav), (target.MaxHealth - target.Health) * 1.5f);
        target.FullRestore();
    }

    public SC_Card GetRandomCard(bool SortByRariety)
    {
        if (SortByRariety)
        {
            SC_Card ReturnCard;
            int Rand = Random.Range(1, 101);

            if (Rand <= 50)
            {
                ReturnCard = GetRandomCard(SC_Card.Rariety.Common);
            }
            else if (Rand <= 70)
            {
                ReturnCard = GetRandomCard(SC_Card.Rariety.Rare);
            }
            else if (Rand <= 98)
            {
                ReturnCard = GetRandomCard(SC_Card.Rariety.Epic);
            }
            else
            {
                ReturnCard = GetRandomCard(SC_Card.Rariety.Legendary);
            }

            if (RepeatError >= 150) { return null; }
            else if (ReturnCard == null)
            {
                RepeatError++;
                Debug.Log(Currentcard + "'s List has an no Cards of this Rariety, " + RepeatError + " Reroll...");
                return GetRandomCard(true);
            }
            RepeatError = 0;
            return ReturnCard;
        }
        else
        {
            return Currentcard.CardList.List[Random.Range(0, Currentcard.CardList.List.Count)];
        }
    }

    public SC_Card GetRandomCard(SC_Card.Rariety rariety)
    {
        List<SC_Card> CardList = new List<SC_Card>();
        for (int i = 0; i < Currentcard.CardList.List.Count; i++)
        {
            if (Currentcard.CardList.List[i].CardRariety == rariety)
            {
                CardList.Add(Currentcard.CardList.List[i]);
            }
        }

        if (CardList.Count == 0) { return null; }
        return CardList[Random.Range(0, CardList.Count)];
    }

    public IEnumerator PlayAnim(GameBehaviour Game, CharacterBehaviour Target)
    {
        Game.EnemyCardAnim = true;
        Game.GameDis.SetCardDisplay(Currentcard);

        Vector2 StartPos = this.gameObject.transform.position = CharacterBehav.gameObject.transform.position;
        float dt = 0;

        while (dt < 0.5f)
        {
            if (Currentcard.DoesTarget)
            {
                this.gameObject.transform.position = StartPos + DistNormalize(StartPos, Target.gameObject.transform.position) * m_Speed * Vector2.Distance(StartPos, Target.gameObject.transform.position) * dt;
            }
            dt += Time.deltaTime;
            yield return null;
        }

        Play(Target);
        CharacterBehav.HandCards.Remove(this.gameObject);
        CharacterBehav.AdjustHand();
        Destroy(this.gameObject);

        Game.EnemyCardAnim = false;
    }

    private Vector2 DistNormalize(Vector2 A, Vector2 B)
    {
        Vector2 C = B - A;
        C.Normalize();
        return C;
    }
}
