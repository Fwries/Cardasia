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
        CharacterBehav.PlayerBehav.CardPlayed = true;

        int MaxActive = 3;
        if (target.PlayerBehav.CharacterTape.Length < 3)
        {
            MaxActive = target.PlayerBehav.CharacterTape.Length;
        }

        if (!IsGenerated)
        {
            //CharacterBehav
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
                return;
            case "Bayonet":
                CharacterBehav.DealDamage(20, 1, target);
                if (CharacterBehav.PlayerBehav.CardPlayed == false) { CharacterBehav.Draw(1); }
                return;
            case "Campfire":
                for (int i = 0; i < MaxActive; i++)
                {
                    CharacterBehav.PlayerBehav.CharacterTape[i].FullRestore();
                }
                CharacterBehav.PlayerBehav.LoseATurn = true;
                GameBehav.EndTurn(CharacterBehav.IsEnemy);
                return;
            case "Cryo Chamber":
                target.FullRestore();
                target.Freeze = 5;
                target.Trip = true;
                return;
            case "Daggers & Roses":
                CharacterBehav.DealDamage(20, 1, target);
                CharacterBehav.Draw(1);
                target.Trip = true;
                return;
            case "Drill":
                target.Health -= 70;
                return;
            case "Extra Time":
                CharacterBehav.GainMana("Stamina", 1);
                return;
            case "Fairy Wand":
                CharacterBehav.AddCard(GameBehav.RandomCardPool.Deck[Random.Range(0, GameBehav.RandomCardPool.Deck.Count)]);
                return;
            case "Guns & Roses":
                if (CharacterBehav.Bullet < 1) { return; }
                CharacterBehav.Bullet--;
                CharacterBehav.DealDamage(70, 1, target);
                target.Trip = true;
                return;
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
                return;
            case "Overdrive":
                CharacterBehav.GainMana("Both", 3);
                return;
            case "Punch":
                CharacterBehav.DealDamage(20, 1, target);
                return;
            case "Barrel Reload": // Reload Gun Barrel
                CharacterBehav.Reload(6);
                return;
            case "Rose":
                target.Trip = true;
                return;
            case "Run":
                CharacterBehav.Draw(1);
                if (CharacterBehav.PlayerBehav.CardPlayed == false) { CharacterBehav.Draw(1); }
                return;
            case "Saber Slash":
                CharacterBehav.DealDamage(70, 2, target);
                return;
            case "Sprint":
                CharacterBehav.Draw(2);
                if (CharacterBehav.PlayerBehav.CardPlayed == false) { CharacterBehav.Draw(1); }
                return;
            case "Targeting":
                target.Ward = true;
                return;

            // Consumable Cards

            case "Bullet":
                CharacterBehav.Reload(1);
                return;
            case "Bullets":
                CharacterBehav.Reload(3);
                return;
            case "Flash Grenade":
                for (int i = 0; i < MaxActive; i++)
                {
                    GameBehav.Opponent.CharacterTape[i].Shock += 1;
                }
                return;
            case "Health Capsule":
                target.RestoreHealth(150);
                return;
            case "Health Potion": // Heart Bottle
                target.RestoreHealth(70);
                return;
            case "Heavy Bullets":
                CharacterBehav.Reload(3);
                CharacterBehav.CritChance = 2;
                return;
            case "Medicines":
                target.RestoreHealth(50);
                target.Burn = false;
                target.Trip = false;
                return;
            case "Miracle Medicine":
                target.RestoreHealth(300);
                target.ClearStatus("Freeze");
                target.ClearStatus("Shock");
                target.Burn = false;
                target.Trip = false;
                return;
            case "Overdose":
                target.RestoreHealth(-20);
                target.ClearStatus("Random");
                target.ClearStatus("Random");
                target.ClearStatus("Random");
                return;
            case "Pill":
                target.ClearStatus("Random");
                return;
            case "Stun Grenade":
                target.Shock = 2;
                return;
            case "Supersonic Bullet":
                CharacterBehav.Reload(1);
                CharacterBehav.Draw(1);
                return;
            case "Syringe":
                target.RestoreHealth(70);
                return;
        }

        Debug.Log(Currentcard.CardName + " does not exist");
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
