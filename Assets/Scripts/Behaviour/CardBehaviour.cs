using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    public CharacterBehaviour CharacterBehav;
    public SC_Card Currentcard;

    public int CardCost;
    public bool Frozen;

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
}
