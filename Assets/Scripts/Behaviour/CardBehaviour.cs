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
            case "HB":
                target.Health -= 70;
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
