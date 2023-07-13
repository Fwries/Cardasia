using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCharacterBehaviour : MonoBehaviour
{
    public save SaveData;
    public RotationBehaviourUI RotationUI;

    public UICharacterDisplay[] UICharacter;
    public UICharacterDisplay[] RotationCharacter;
    public UICharacterDisplay[] CharacterTape;

    public GameObject DeckContent;
    public GameObject InventoryContent;

    public SC_Deck scDeck;
    public SC_Deck ItemDeck;

    // Start is called before the first frame update
    void Start()
    {
        SaveData = GameObject.Find("Save").GetComponent<save>();
        Init();
        SetCurrentDeck(0);
    }

    // Update is called once per frame
    void Update()
    {
        bool HasChanged = false;
        if (CharacterTape.Length < SaveData.PartyCharacterData.Length || CharacterTape.Length > SaveData.PartyCharacterData.Length) { HasChanged = true; }

        CharacterTape = new UICharacterDisplay[SaveData.PartyCharacterData.Length];

        for (int i = 0; i <= SaveData.PartyCharacterData.Length; i++)
        {
            if (i == SaveData.PartyCharacterData.Length)
            {
                for (int i2 = i; i2 < 5; i2++)
                {
                    UICharacter[i2].CharUI.SetActive(false);
                    UICharacter[i2].CharData = null;

                    RotationCharacter[i2].CharUI.SetActive(false);
                    RotationCharacter[i2].CharData = null;
                }
            }
            else if (SaveData.PartyCharacterData[i] != null)
            {
                UICharacter[i].CharUI.SetActive(true);
                UICharacter[i].CharData = SaveData.PartyCharacterData[i];

                CharacterTape[i] = UICharacter[i];
                RotationCharacter[i].CharUI.SetActive(true);
                RotationCharacter[i].CharData = SaveData.PartyCharacterData[i];
            }
            else
            {
                UICharacter[i].CharUI.SetActive(false);
                UICharacter[i].CharData = null;

                RotationCharacter[i].CharUI.SetActive(false);
                RotationCharacter[i].CharData = null;
            }
        }

        if (HasChanged)
        {
            RotationUI.Init();
        }
    }

    void Init()
    {
        CharacterTape = new UICharacterDisplay[SaveData.PartyCharacterData.Length];

        for (int i = 0; i <= SaveData.PartyCharacterData.Length; i++)
        {
            if (i == SaveData.PartyCharacterData.Length)
            {
                for (int i2 = i; i2 < 5; i2++)
                {
                    UICharacter[i2].CharUI.SetActive(false);
                    UICharacter[i2].CharData = null;

                    RotationCharacter[i2].CharUI.SetActive(false);
                    RotationCharacter[i2].CharData = null;
                }
            }
            else if (SaveData.PartyCharacterData[i] != null)
            {
                UICharacter[i].CharUI.SetActive(true);
                UICharacter[i].CharData = SaveData.PartyCharacterData[i];

                CharacterTape[i] = UICharacter[i];
                RotationCharacter[i].CharUI.SetActive(true);
                RotationCharacter[i].CharData = SaveData.PartyCharacterData[i];
            }
            else
            {
                UICharacter[i].CharUI.SetActive(false);
                UICharacter[i].CharData = null;

                RotationCharacter[i].CharUI.SetActive(false);
                RotationCharacter[i].CharData = null;
            }
        }

        RotationUI.Init();
    }

    public void SetCurrentDeck(int Number)
    {
        foreach (Transform child in DeckContent.transform)
        {
            Destroy(child.gameObject);
        }

        if (scDeck != SaveData.PartyCharacterData[Number].GetDeck()) 
        { 
            scDeck = SaveData.PartyCharacterData[Number].GetDeck();
            ItemDeck = SaveData.PartyCharacterData[Number].ItemGetDeck();

            for (int i = 0; i < scDeck.Deck.Count; i++)
            {
                GameObject Card = Instantiate(Resources.Load("CardUI", typeof(GameObject))) as GameObject;
                Card.transform.SetParent(DeckContent.transform);
                Card.GetComponent<CardDisplay>().Currentcard = scDeck.Deck[i];
            }
            for (int i = 0; i < ItemDeck.Deck.Count; i++)
            {
                GameObject Card = Instantiate(Resources.Load("CardUI", typeof(GameObject))) as GameObject;
                Card.transform.SetParent(DeckContent.transform);
                Card.GetComponent<CardDisplay>().Currentcard = ItemDeck.Deck[i];
            }
        }
    }
}
