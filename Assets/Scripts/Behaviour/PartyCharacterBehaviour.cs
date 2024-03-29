using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PartyCharacterBehaviour : MonoBehaviour
{
    public RotationBehaviourUI RotationUI;

    public UICharacterDisplay[] UICharacter;
    public UICharacterDisplay[] RotationCharacter;
    public UICharacterDisplay[] CharacterTape;

    public UIContainerBehaviour DeckContainer;
    public UIContainerBehaviour InventoryContainer;

    public SC_Deck scDeck;
    public SC_Deck SkillDeck;
    public SC_Deck ItemDeck;
    public List<SC_Card> Inventory;

    public int CurrCharacter;

    public GameObject BlackDrop;
    public CardDisplay Card;
    public Text CardText;

    private bool HasInit;
    private bool HasChanged = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CharacterTape.Length < save.Instance.PartyCharacterData.Length || CharacterTape.Length > save.Instance.PartyCharacterData.Length) { HasChanged = true; }

        if (HasChanged)
        {
            RotationUI.Init();
            UpdateUI();
            HasChanged = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIElement())
            {
                BlackDrop.SetActive(false);
                if (CharacterTape[CurrCharacter].Weapon == null)
                {
                    Card.gameObject.SetActive(false);
                }
            }
        }
    }

    public void UpdateUI()
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
                UICharacter[i].SetCharacter();

                CharacterTape[i] = UICharacter[i];

                RotationCharacter[i].CharUI.SetActive(true);
                RotationCharacter[i].CharData = save.Instance.PartyCharacterData[i];
                RotationCharacter[i].SetCharacter();
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

    public void Init()
    {
        UpdateUI();
        Inventory = save.Instance.Inventory;
        UpdateInventory();
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
        scDeck = ScriptableObject.CreateInstance<SC_Deck>();
        scDeck.List = save.Instance.PartyCharacterData[pos].GetCharacter().DefaultDeck;
        SkillDeck = save.Instance.PartyCharacterData[pos].GetDeck();
        ItemDeck = save.Instance.PartyCharacterData[pos].ItemGetDeck();

        for (int i = 0; i < scDeck.List.Count; i++)
        {
            GameObject Card = Instantiate(Resources.Load("CardUI", typeof(GameObject))) as GameObject;
            Card.transform.SetParent(DeckContainer.ContainerContent.transform);
            Card.GetComponent<CardDisplay>().Currentcard = scDeck.List[i];
        }

        //DeckContainer.AddCards(SkillDeck.List);
        DeckContainer.AddCards(ItemDeck.List);

        save.Instance.Inventory = InventoryContainer.CardContainer;
    }

    public void UpdateInventory()
    {
        if (HasInit)
        {
            save.Instance.Inventory = InventoryContainer.CardContainer;
            InventoryContainer.DestroyAllCards();
        }

        // TODO Update Skill Cards OR Inventory Cards

        InventoryContainer.AddCards(save.Instance.Skill);
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
        
        // TODO Save Skill Cards OR Inventory Cards

        save.Instance.PartyCharacterData[CurrCharacter].ItemSetDeck(DeckContainer.CardContainer);
        save.Instance.Inventory = InventoryContainer.CardContainer;
    }

    private bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Card");

        foreach (GameObject element in gameObjects)
        {
            CanvasGroup canvasGroup = element.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                continue;

            if (canvasGroup.blocksRaycasts && canvasGroup.interactable && canvasGroup.alpha > 0)
            {
                RectTransform rectTransform = element.GetComponent<RectTransform>();
                Vector3 originalScale = rectTransform.localScale;

                // Temporarily set the scale to 1 for tap detection
                rectTransform.localScale = Vector3.one;

                if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, eventData.pressEventCamera))
                {
                    // Restore the original scale after tap detection
                    rectTransform.localScale = originalScale;
                    return true;
                }

                // Restore the original scale after tap detection
                rectTransform.localScale = originalScale;
            }
        }

        return false;
    }
}
