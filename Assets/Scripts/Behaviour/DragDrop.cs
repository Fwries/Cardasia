using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IInitializePotentialDragHandler
{
    private CanvasGroup canvasGroup;
    private Vector3 dragOffset;
    private Camera cam;
    private GameObject Display;

    private int Zones;
    [HideInInspector] public bool IsDragging;

    private FieldBehaviour FieldBehav;
    private CardBehaviour Cardbehav;

    private void Awake()
    {
        cam = Camera.main;
        canvasGroup = GetComponent<CanvasGroup>();
        Cardbehav = this.GetComponent<CardBehaviour>();
        FieldBehav = GameObject.Find("Canvas").GetComponent<FieldBehaviour>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //if (FieldBehav.CurrentPlayerTurn != Cardbehav.playerBehav)
        //    return;
        canvasGroup.blocksRaycasts = false;
        IsDragging = true;
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if (FieldBehav.CurrentPlayerTurn != Cardbehav.playerBehav || !this.GetComponent<CardBehaviour>().IsDraggable || Zones != 1)
        //    return;
        transform.position = Input.mousePosition - GetMousePos();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //if (FieldBehav.CurrentPlayerTurn != Cardbehav.playerBehav)
        //    return;

        //if (Zones == 1)
        //    transform.position = new Vector3((Cardbehav.playerBehav.HandCards.Count - 1) * -75 + Cardbehav.playerBehav.HandObject.transform.position.x + gameObject.GetComponent<CardDisplay>().PositionIndex * 150,
        //        Cardbehav.playerBehav.HandObject.transform.position.y, 0.0f);
        canvasGroup.blocksRaycasts = true;
        IsDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Zones = GetComponent<CardBehaviour>().Zones;
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
