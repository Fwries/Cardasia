using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropUI : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IInitializePotentialDragHandler
{
    private CanvasGroup canvasGroup;
    private GameObject PartyUI;
    private Vector3 dragOffset;
    private Camera cam;
    public GameObject EmptyCard;

    [HideInInspector] public bool IsDragging;
    public CardDisplay cardDisplay;
    public UIContainerBehaviour currContainer;

    void Awake()
    {
        cam = Camera.main;
        PartyUI = GameObject.Find("PartyUI");
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!Consumable) { canvasGroup.blocksRaycasts = false; }
    }

    public void RemoveFromDeck()
    {
        for (int i = 0; i < currContainer.CardContainer.Count; i++)
        {
            if (currContainer.CardContainer[i] == cardDisplay.Currentcard)
            {
                currContainer.CardContainer.RemoveAt(i);
                break;
            }    
        }
        Destroy(EmptyCard);
        Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (cardDisplay.Currentcard.CardType != 0) { return; }

        EmptyCard.transform.SetParent(transform.parent);
        transform.SetParent(PartyUI.transform);

        canvasGroup.blocksRaycasts = false;
        IsDragging = true;
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (cardDisplay.Currentcard.CardType != 0) { return; }

        eventData.useDragThreshold = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (cardDisplay.Currentcard.CardType != 0) { return; }

        transform.position = Input.mousePosition - GetMousePos();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (cardDisplay.Currentcard.CardType != 0) { return; }

        transform.SetParent(EmptyCard.transform.parent);
        EmptyCard.transform.SetParent(transform);

        canvasGroup.blocksRaycasts = true;
        IsDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Display.GetComponent<CardDisplay>().Currentcard = this.GetComponent<CardDisplay>().Currentcard;
        //Display.GetComponent<CardDisplay>().cardBehav = this.GetComponent<CardDisplay>().cardBehav;
    }

    private Vector3 GetMousePos()
    {
        Vector3 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        worldPoint.z = 0.0f;
        return worldPoint;
    }
}
