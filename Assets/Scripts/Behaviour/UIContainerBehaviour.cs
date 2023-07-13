using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIContainerBehaviour : MonoBehaviour, IEventSystemHandler, IDropHandler
{
    public GameObject ContainerContent;
    public List<SC_Card> CardContainer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyAllCards()
    {
        CardContainer = new List<SC_Card>();
        foreach (Transform child in ContainerContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddCards(List<SC_Card> CardList)
    {
        for (int i = 0; i < CardList.Count; i++)
        {
            GameObject Card = Instantiate(Resources.Load("CardUI", typeof(GameObject))) as GameObject;
            Card.transform.SetParent(ContainerContent.transform);
            Card.GetComponent<CardDisplay>().Currentcard = CardList[i];
            CardContainer.Add(Card.GetComponent<CardDisplay>().Currentcard);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        DragDropUI DragDropBehav = eventData.pointerDrag.GetComponent<DragDropUI>();
        if (DragDropBehav == null) { return; }
        if (!DragDropBehav.Consumable) { return; }
        if (DragDropBehav.EmptyCard.transform == transform) { return; }
        
        //Debug.Log("Dropped");

        GameObject Card = Instantiate(Resources.Load("CardUI", typeof(GameObject))) as GameObject;
        Card.transform.SetParent(ContainerContent.transform);
        Card.GetComponent<CardDisplay>().Currentcard = eventData.pointerDrag.GetComponent<CardDisplay>().Currentcard;
        CardContainer.Add(Card.GetComponent<CardDisplay>().Currentcard);

        Destroy(eventData.pointerDrag.GetComponent<DragDropUI>().EmptyCard);
        Destroy(eventData.pointerDrag);
    }
}