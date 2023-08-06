using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopBehaviour : MonoBehaviour
{
    public UIContainerBehaviour ShopContainer;
    public UIContainerBehaviour CartContainer;

    public Button BuyBtn;
    public Text CostText;

    public CardDisplay Card;
    public Text CardNameText;
    public Text CardText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CostText.text = "Cost: " + CartContainer.Cost;

        if (save.Instance.Gold >= CartContainer.Cost && CartContainer.Cost != 0)
        {
            BuyBtn.interactable = true;
        }
        else
        {
            BuyBtn.interactable = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X))
        {
            Close();
        }
    }

    public void Buy()
    {
        save.Instance.Gold -= CartContainer.Cost;
        AudioManager.Instance.PlaySFX("Collect");
        for (int i = 0; i < CartContainer.CardContainer.Count; i++)
        {
            save.Instance.Inventory.Add(CartContainer.CardContainer[i]);
        }

        CartContainer.DestroyAllCards();
    }

    public void Close()
    {
        ShopContainer.AddCards(CartContainer.CardContainer);
        CartContainer.DestroyAllCards();

        gameObject.SetActive(false);
        AudioManager.Instance.PlaySFX("MenuOut");
        GameObject.Find("Character_RPG").GetComponent<CharacterMovement>().Continue();
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
