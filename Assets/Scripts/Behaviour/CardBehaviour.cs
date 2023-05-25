using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    public CharacterBehaviour CharacterBehav;
    public SC_Card Currentcard;

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
        switch (Currentcard.CardIdx)
        {
            case 1: // Saber Slash
                target.DealDamage(70, 1);
                break;
            case 2: // Cryo Chamber
                target.Health = target.MaxHealth;
                target.Freeze = 5;
                target.Trip = true;
                break;
            case 3: // Targeting
                target.Ward = true;
                break;
            case 4: // Extra Time
                if (target.Stamina < 5) { target.Stamina += 1; }
                break;
            case 5: // Backstab
                target.DealDamage(20, 1);
                if (CharacterBehav.PlayerBehav.CardPlayed == false) 
                { 
                    target.DealDamage(20, 2);
                    CharacterBehav.Draw(1);
                }
                else
                {
                    target.DealDamage(20, 1);
                }
                break;
            case 6: // Bayonet
                target.DealDamage(20, 1);
                if (CharacterBehav.PlayerBehav.CardPlayed == false) { CharacterBehav.Draw(1); }
                break;
            case 7: // Dagger & Roses
                target.DealDamage(20, 1);
                CharacterBehav.Draw(1);
                target.Trip = true;
                break;
        }

        CharacterBehav.PlayerBehav.CardPlayed = true;
    }

    public IEnumerator PlayAnim(GameBehaviour Game, CharacterBehaviour Target)
    {
        Vector2 StartPos = CharacterBehav.gameObject.transform.position;
        Game.EnemyCardAnim = true;
        float dt = 0;
        while (dt < 0.5f)
        {
            this.gameObject.transform.position = StartPos + DistNormalize(StartPos, Target.gameObject.transform.position) * m_Speed * Vector2.Distance(StartPos, Target.gameObject.transform.position) * dt;
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
