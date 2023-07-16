using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCharacterBehaviour : MonoBehaviour
{
    public RotationBehaviourUI RotationUI;

    public UICharacterDisplay[] UICharacter;
    public UICharacterDisplay[] RotationCharacter;
    public UICharacterDisplay[] CharacterTape;

    public UIContainerBehaviour DeckContainer;
    public UIContainerBehaviour InventoryContainer;

    public SC_Deck scDeck;
    public SC_Deck ItemDeck;
    public List<SC_Card> Inventory;

    public int CurrCharacter;

    private bool HasInit;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        bool HasChanged = false;
        if (CharacterTape.Length < save.Instance.PartyCharacterData.Length || CharacterTape.Length > save.Instance.PartyCharacterData.Length) { HasChanged = true; }

        CharacterTape = new UICharacterDisplay[save.Instance.PartyCharacterData.Length];

        for (int i = 0; i <= save.Instance.PartyCharacterData.Length; i++)
        {
            if (i == save.Instance.PartyCharacterData.Length)
            {
                for (int i2 = i; i2 < 5; i2++)
                {
                    UICharacter[i2].CharUI.SetActive(false);
                    UICharacter[i2].CharData = null;

                    RotationCharacter[i2].CharUI.SetActive(false);
                    RotationCharacter[i2].CharData = null;
                }
            }
            else if (save.Instance.PartyCharacterData[i] != null)
            {
                UICharacter[i].CharUI.SetActive(true);
                UICharacter[i].CharData = save.Instance.PartyCharacterData[i];

                CharacterTape[i] = UICharacter[i];
                RotationCharacter[i].CharUI.SetActive(true);
                RotationCharacter[i].CharData = save.Instance.PartyCharacterData[i];
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
        CharacterTape = new UICharacterDisplay[save.Instance.PartyCharacterData.Length];

        for (int i = 0; i <= save.Instance.PartyCharacterData.Length; i++)
        {
            if (i == save.Instance.PartyCharacterData.Length)
            {
                for (int i2 = i; i2 < 5; i2++)
                {
                    UICharacter[i2].CharUI.SetActive(false);
                    UICharacter[i2].CharData = null;

                    RotationCharacter[i2].CharUI.SetActive(false);
                    RotationCharacter[i2].CharData = null;
                }
            }
            else if (save.Instance.PartyCharacterData[i] != null)
            {
                UICharacter[i].CharUI.SetActive(true);
                UICharacter[i].CharData = save.Instance.PartyCharacterData[i];

                CharacterTape[i] = UICharacter[i];
                RotationCharacter[i].CharUI.SetActive(true);
                RotationCharacter[i].CharData = save.Instance.PartyCharacterData[i];
            }
            else
            {
                UICharacter[i].CharUI.SetActive(false);
                UICharacter[i].CharData = null;

                RotationCharacter[i].CharUI.SetActive(false);
                RotationCharacter[i].CharData = null;
            }
        }
        Inventory = save.Instance.Inventory;

        RotationUI.Init();
        SetCurrentDeck(0);

        HasInit = true;
    }

    public void SetCurrentDeck(int pos)
    {
        if (HasInit)
        {
            save.Instance.PartyCharacterData[CurrCharacter].ItemSetDeck(DeckContainer.CardContainer);
            DeckContainer.DestroyAllCards();
        }

        CurrCharacter = pos;
        scDeck = save.Instance.PartyCharacterData[pos].GetDeck();
        ItemDeck = save.Instance.PartyCharacterData[pos].ItemGetDeck();

        for (int i = 0; i < scDeck.Deck.Count; i++)
        {
            GameObject Card = Instantiate(Resources.Load("CardUI", typeof(GameObject))) as GameObject;
            Card.transform.SetParent(DeckContainer.ContainerContent.transform);
            Card.GetComponent<CardDisplay>().Currentcard = scDeck.Deck[i];
        }

        DeckContainer.AddCards(ItemDeck.Deck);

        UpdateInventory();
    }

    public void UpdateInventory()
    {
        if (HasInit)
        {
            save.Instance.Inventory = InventoryContainer.CardContainer;
            InventoryContainer.DestroyAllCards();
        }

        InventoryContainer.AddCards(save.Instance.Inventory);
    }

    //public void Sort()
    //{
    //    SC_Card temp = null;

    //    for (int i = 0; i < Inventory.Count; i++)
    //    {
    //        for (int j = 0; j < Inventory.Count - 1; j++)
    //        {
    //            if (Inventory.CardName[j] > Inventory.CardName[j + 1])
    //            {
    //                temp = Inventory[j + 1];
    //                Inventory[j + 1] = Inventory[j];
    //                Inventory[j] = temp;
    //            }
    //        }
    //    }
    //}

    public void SaveAll()
    {
        if (!gameObject.activeSelf) { return; }
        save.Instance.PartyCharacterData[CurrCharacter].ItemSetDeck(DeckContainer.CardContainer);
        save.Instance.Inventory = InventoryContainer.CardContainer;
    }
}
