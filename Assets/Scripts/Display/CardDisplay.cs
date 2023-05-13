using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public SC_Card Currentcard;
    public SC_Card Empty;
    private SC_Card PrevCard;

    public SC_Template Template;
    public Image CardArt;
    public GameObject CostIcon;
    public GameObject CostText;

    public CardBehaviour cardBehav;
    [HideInInspector] public int PositionIndex;

    // Start is called before the first frame update
    void Start()
    {
        cardBehav.Currentcard = Currentcard;
        cardBehav.CardCost = Currentcard.CardCost;
    }

    // Update is called once per frame
    void Update()
    {
        if (PrevCard != Currentcard)
        {
            CardArt.sprite = Currentcard.CardArt;
            CostText.GetComponent<Text>().text = "" + cardBehav.CardCost;

            switch (Currentcard.CardType)
            {
                case SC_Card.Type.Consumable:
                    CostIcon.SetActive(false);
                    break;
                case SC_Card.Type.Stamina:
                    CostIcon.GetComponent<Image>().sprite = Template.TypeStamina;
                    CostIcon.SetActive(true);
                    break;
                case SC_Card.Type.Mana:
                    CostIcon.GetComponent<Image>().sprite = Template.TypeMana;
                    CostIcon.SetActive(true);
                    break;
                case SC_Card.Type.Both:
                    CostIcon.GetComponent<Image>().sprite = Template.TypeBoth;
                    CostIcon.SetActive(true);
                    break;
            }

            PrevCard = Currentcard;
        }
    }
}
