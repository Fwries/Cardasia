using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDisplay : MonoBehaviour
{
    public GameBehaviour GameBehav;

    public GameObject CardDisplay;
    public CardDisplay CardUI;
    public Text CardName;
    public Text CardTrait;
    public Text CardSkill;

    public SC_Template Template;
    public Text ButtonPrint;
    public Text DisplayCharacterText;

    public Text HealthText;
    public Slider HealthSlider;
    public Text EXPText;
    public Slider EXPSlider;

    public Text CharacterStats;
    public Text CharacterDeckCount;

    public List<GameObject> ManaIcon;
    private int Pointer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameBehav.Selected == null) { return; }

        DisplayCharacterText.text = "Lv. " + GameBehav.Selected.Level + " " + GameBehav.Selected.Character.CharName;

        HealthText.text = GameBehav.Selected.Health + " / " + GameBehav.Selected.MaxHealth;
        HealthSlider.maxValue = GameBehav.Selected.MaxHealth;
        HealthSlider.value = GameBehav.Selected.Health;

        EXPText.text = GameBehav.Selected.Exp + " / " + GameBehav.Selected.MaxExp;
        EXPSlider.maxValue = GameBehav.Selected.MaxExp;
        EXPSlider.value = GameBehav.Selected.Exp;

        CharacterStats.text = "HP: " + GameBehav.Selected.Health + "\n";

        if (GameBehav.Selected.ATKModif > 0) { CharacterStats.text += "ATK: " + GameBehav.Selected.ATK + " (+" + GameBehav.Selected.ATKModif + ")" + "\n"; }
        else { CharacterStats.text += "ATK: " + GameBehav.Selected.ATK + "\n"; }
        if (GameBehav.Selected.DEFModif > 0) { CharacterStats.text += "DEF: " + GameBehav.Selected.DEF + " (+" + GameBehav.Selected.DEFModif + ")" + "\n"; }
        else { CharacterStats.text += "DEF: " + GameBehav.Selected.DEF + "\n"; }

        CharacterDeckCount.text = "" + GameBehav.Selected.Deck.Count;

        if (GameBehav.Selected.MaxBullet > 0)
        {
            CharacterStats.text += "BLT: (" + GameBehav.Selected.Bullet + "/" + GameBehav.Selected.MaxBullet + ")" + "\n";
        }
        if (GameBehav.Selected.Claw > 0)
        {
            CharacterStats.text += "CLAW: " + GameBehav.Selected.Claw + "\n";
        }

        Pointer = 0;
        for (int i = 0; i < GameBehav.Selected.Both; i++)
        {
            ManaIcon[Pointer].SetActive(true);
            ManaIcon[Pointer].GetComponent<Image>().sprite = Template.TypeBoth;
            Pointer++;
        }
        for (int i = 0; i < GameBehav.Selected.Stamina; i++)
        {
            ManaIcon[Pointer].SetActive(true);
            ManaIcon[Pointer].GetComponent<Image>().sprite = Template.TypeStamina;
            Pointer++;
        }
        for (int i = 0; i < GameBehav.Selected.Mana; i++)
        {
            ManaIcon[Pointer].SetActive(true);
            ManaIcon[Pointer].GetComponent<Image>().sprite = Template.TypeMana;
            Pointer++;
        }
        for (int i = Pointer; i < 5; i++)
        {
            ManaIcon[Pointer].SetActive(false);
            Pointer++;
        }
    }

    public void SetCardDisplay(SC_Card CurrCard)
    {
        if (CurrCard == null)
        {
            CardDisplay.SetActive(false);
            return;
        }

        if (!CardDisplay.activeSelf)
        {
            CardDisplay.SetActive(true);
        }

        CardUI.Currentcard = CurrCard;

        CardName.text = CurrCard.CardName;
        CardTrait.text = CurrCard.CardTrait;
        CardSkill.text = CurrCard.CardSkill;
    }
}
