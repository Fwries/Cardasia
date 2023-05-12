using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDiplay : MonoBehaviour
{
    public GameBehaviour GameBehav;

    public SC_Template Template;
    public Text DisplayCharacterText;

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

        DisplayCharacterText.text = "Lv. " + GameBehav.Selected.Level + " " + GameBehav.Selected.Character.CardName;

        Pointer = 0;
        for (int i = 0; i < GameBehav.Selected.Both; i++)
        {
            ManaIcon[Pointer].SetActive(true);
            ManaIcon[Pointer].GetComponent<Image>().sprite = Template.TypeConsumable;
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
}
