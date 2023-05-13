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

    [HideInInspector] public bool IsDragging;

    private GameBehaviour GameBehav;
    private CardBehaviour Cardbehav;

    private void Awake()
    {
        cam = Camera.main;
        canvasGroup = GetComponent<CanvasGroup>();
        Cardbehav = this.GetComponent<CardBehaviour>();
        GameBehav = GameObject.Find("Stats").GetComponent<GameBehaviour>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameBehav.CurrentPlayerTurn != Cardbehav.CharacterBehav.PlayerBehav)
            return;
        canvasGroup.blocksRaycasts = false;
        IsDragging = true;
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameBehav.CurrentPlayerTurn != Cardbehav.CharacterBehav.PlayerBehav || this.GetComponent<CardBehaviour>().Frozen)
            return;
        transform.position = Input.mousePosition - GetMousePos();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameBehav.CurrentPlayerTurn != Cardbehav.CharacterBehav.PlayerBehav)
            return;

        transform.position = new Vector3((Cardbehav.CharacterBehav.HandCards.Count - 1) * -100 + Cardbehav.CharacterBehav.HandObject.transform.position.x + gameObject.GetComponent<CardDisplay>().PositionIndex * 200,
                Cardbehav.CharacterBehav.HandObject.transform.position.y, 0.0f);
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
