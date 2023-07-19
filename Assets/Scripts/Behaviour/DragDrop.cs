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
    private GameDisplay DisplayBehav;
    private CardBehaviour Cardbehav;

    private void Awake()
    {
        cam = Camera.main;
        canvasGroup = GetComponent<CanvasGroup>();
        Cardbehav = this.GetComponent<CardBehaviour>();
        
        GameBehav = GameObject.Find("Stats").GetComponent<GameBehaviour>();
        DisplayBehav = GameObject.Find("Stats").GetComponent<GameDisplay>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameBehav.CurrentPlayerTurn != Cardbehav.CharacterBehav.PlayerBehav) { return; }
        if (Cardbehav.Frozen) { return; }
        if (Cardbehav.CharacterBehav.Health <= 0) { return; }

        canvasGroup.blocksRaycasts = false;
        IsDragging = true;
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameBehav.CurrentPlayerTurn != Cardbehav.CharacterBehav.PlayerBehav) { return; }
        if (Cardbehav.Frozen) { return; }
        if (Cardbehav.CharacterBehav.Health <= 0) { return; }

        transform.position = Input.mousePosition - GetMousePos();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameBehav.CurrentPlayerTurn != Cardbehav.CharacterBehav.PlayerBehav) { return; }
        if (Cardbehav.Frozen) { return; }
        if (Cardbehav.CharacterBehav.Health <= 0) { return; }

        AudioManager.Instance.PlaySFX("Error");

        transform.position = new Vector3((Cardbehav.CharacterBehav.HandCards.Count - 1) * -100 + Cardbehav.CharacterBehav.HandObject.transform.position.x + gameObject.GetComponent<CardDisplay>().PositionIndex * 200,
                Cardbehav.CharacterBehav.HandObject.transform.position.y, 0.0f);
        canvasGroup.blocksRaycasts = true;
        IsDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DisplayBehav.SetCardDisplay(Cardbehav.Currentcard);
    }

    private Vector3 GetMousePos()
    {
        Vector3 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        worldPoint.z = 0.0f;
        return worldPoint;
    }
}
