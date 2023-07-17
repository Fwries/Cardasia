using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    public CharacterBehaviour CharacterBehav;
    public SC_Card Currentcard;

    public bool IsGenerated;
    public int CardCost;
    public bool Frozen;

    public float m_Speed = 1.7f;

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
        //Debug.Log(Currentcard.CardName);
        if (target == null) { return; }

        GameBehaviour GameBehav = CharacterBehav.GameBehav;

        int MaxActive = 3;
        if (target.PlayerBehav.CharacterTape.Length < 3)
        {
            MaxActive = target.PlayerBehav.CharacterTape.Length;
        }

        if (!IsGenerated && Currentcard.CardType == 0)
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
            // Card Skills

            case "Backstab":
                if (CharacterBehav.PlayerBehav.CardPlayed == false)
                {
                    CharacterBehav.DealDamage(20, 2, target);
                    CharacterBehav.Draw(1);
                }
                else
                {
                    CharacterBehav.DealDamage(20, 1, target);
                }
                break;
            case "Bayonet":
                CharacterBehav.DealDamage(20, 1, target);
                if (CharacterBehav.PlayerBehav.CardPlayed == false) { CharacterBehav.Draw(1); }
                break;
            case "Campfire":
                for (int i = 0; i < MaxActive; i++)
                {
                    CharacterBehav.PlayerBehav.CharacterTape[i].FullRestore();
                }
                CharacterBehav.PlayerBehav.LoseATurn = true;
                GameBehav.EndTurn(CharacterBehav.IsEnemy);
                break;
            case "Cryo Chamber":
                target.FullRestore();
                target.Freeze = 5;
                target.Trip = true;
                break;
            case "Daggers & Roses":
                CharacterBehav.DealDamage(20, 1, target);
                CharacterBehav.Draw(1);
                target.Trip = true;
                break;
            case "Drill":
                target.Health -= 70;
                break;
            case "Extra Time":
                CharacterBehav.GainMana("Stamina", 1);
                break;
            case "Fairy Wand":
                CharacterBehav.AddCard(GameBehav.RandomCardPool.Deck[Random.Range(0, GameBehav.RandomCardPool.Deck.Count)]);
                break;
            case "Guns & Roses":
                if (CharacterBehav.Bullet < 1) { break; }
                CharacterBehav.Bullet--;
                CharacterBehav.DealDamage(70, 1, target);
                target.Trip = true;
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
            case "Punch":
                CharacterBehav.DealDamage(20, 1, target);
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
                CharacterBehav.DealDamage(70, 2, target);
                break;
            case "Sprint":
                CharacterBehav.Draw(2);
                if (CharacterBehav.PlayerBehav.CardPlayed == false) { CharacterBehav.Draw(1); }
                break;
            case "Targeting":
                target.Ward = true;
                break;

            // Consumable Cards

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
                target.RestoreHealth(150);
                break;
            case "Health Potion": // Heart Bottle
                target.RestoreHealth(70);
                break;
            case "Heavy Bullets":
                CharacterBehav.Reload(3);
                CharacterBehav.CritChance = 2;
                break;
            case "Medicines":
                target.RestoreHealth(50);
                target.Burn = false;
                target.Trip = false;
                break;
            case "Miracle Medicine":
                target.RestoreHealth(300);
                target.ClearStatus("Freeze");
                target.ClearStatus("Shock");
                target.Burn = false;
                target.Trip = false;
                break;
            case "Overdose":
                target.RestoreHealth(-20);
                target.ClearStatus("Random");
                target.ClearStatus("Random");
                target.ClearStatus("Random");
                break;
            case "Pill":
                target.ClearStatus("Random");
                break;
            case "Stun Grenade":
                target.Shock = 2;
                break;
            case "Supersonic Bullet":
                CharacterBehav.Reload(1);
                CharacterBehav.Draw(1);
                break;
            case "Syringe":
                target.RestoreHealth(70);
                break;
        }
        CharacterBehav.PlayerBehav.CardPlayed = true;

        //Debug.Log(Currentcard.CardName + " does not exist");
    }

    public IEnumerator PlayAnim(GameBehaviour Game, CharacterBehaviour Target)
    {
        Game.EnemyCardAnim = true;
        if (Target != null)
        {
            Vector2 StartPos = CharacterBehav.gameObject.transform.position;
            float dt = 0;
            while (dt < 0.5f)
            {
                this.gameObject.transform.position = StartPos + DistNormalize(StartPos, Target.gameObject.transform.position) * m_Speed * Vector2.Distance(StartPos, Target.gameObject.transform.position) * dt;
                dt += Time.deltaTime;
                yield return null;
            }
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
