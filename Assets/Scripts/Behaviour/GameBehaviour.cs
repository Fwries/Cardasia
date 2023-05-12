using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    public PlayerBehaviour CurrentPlayerTurn;
    public PlayerBehaviour Player;
    public PlayerBehaviour Opponent;

    public bool EnemyAI;
    public SC_Template Template;

    public CharacterBehaviour Selected;

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
        if (Selected == null) { return; }

        DisplayCharacterText.text = "Lv. " + Selected.Level + " " + Selected.Character.CardName;

        Pointer = 0;
        for (int i = 0; i < Selected.Both; i++)
        {
            ManaIcon[Pointer].SetActive(true);
            ManaIcon[Pointer].GetComponent<Image>().sprite = Template.TypeConsumable;
            Pointer++;
        }
        for (int i = 0; i < Selected.Stamina; i++)
        {
            ManaIcon[Pointer].SetActive(true);
            ManaIcon[Pointer].GetComponent<Image>().sprite = Template.TypeStamina;
            Pointer++;
        }
        for (int i = 0; i < Selected.Mana; i++)
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
