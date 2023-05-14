using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationBehaviour : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameBehaviour GameBehav;
    private Camera cam;

    public GameObject CentrePos;
    public GameObject LeftPos;
    public GameObject RightPos;

    public GameObject LeftBackPos;
    public GameObject RightBackPos;

    private bool OnStartDrag;
    private Vector3 StartPos;
    public float DragOffsetXPos;

    private void Awake()
    {
        cam = Camera.main;
        //DragOffsetXPos = -0.4f;
    }

    // Update is called once per frame
    void Update()
    {
        if (DragOffsetXPos < 0)
        {
            GameBehav.Player.ActiveCharacter[1].transform.position = new Vector3(
            CentrePos.transform.position.x + LeftPos.transform.position.x * (DragOffsetXPos * 1.14f),
            CentrePos.transform.position.y + LeftPos.transform.position.y * (DragOffsetXPos / 7), 0);

            GameBehav.Player.ActiveCharacter[0].transform.position = new Vector3(
            LeftPos.transform.position.x - LeftBackPos.transform.position.x * (DragOffsetXPos / 3),
            LeftPos.transform.position.y + LeftBackPos.transform.position.y * (DragOffsetXPos * 4), 0);

            GameBehav.Player.ActiveCharacter[2].transform.position = new Vector3(
            RightPos.transform.position.x + CentrePos.transform.position.x * (DragOffsetXPos / 1.28f),
            RightPos.transform.position.y - CentrePos.transform.position.y * (DragOffsetXPos / 7), 0);

            if (OnStartDrag == true)
            {
                GameBehav.Player.BackCharacter.transform.position = RightBackPos.transform.position;
                OnStartDrag = false;
            }

            //GameBehav.Player.BackCharacter.transform.position = new Vector3(
            //RightBackPos.transform.position.x - RightPos.transform.position.x * (DragOffsetXPos),
            //RightBackPos.transform.position.y - RightPos.transform.position.y * (DragOffsetXPos / 2), 0);
        }
        if (DragOffsetXPos > 0)
        {
            if (OnStartDrag == true)
            {
                GameBehav.Player.BackCharacter.transform.position = LeftBackPos.transform.position;
                OnStartDrag = false;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StartPos = GetMousePos();
        OnStartDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragOffsetXPos = GetMousePos().x - StartPos.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartPos = new Vector3(0, 0, 0);
        DragOffsetXPos = 0;
        OnStartDrag = false;
        ResetCharPos();
    }

    private Vector3 GetMousePos()
    {
        Vector3 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        worldPoint.z = 0.0f;
        return worldPoint;
    }

    public void ResetCharPos()
    {
        if (GameBehav.Player.ActiveCharacter[1] != null)
        {
            GameBehav.Player.ActiveCharacter[1].transform.position = new Vector3(CentrePos.transform.position.x, CentrePos.transform.position.y, 0);
        }
        if (GameBehav.Player.ActiveCharacter[0] != null)
        {
            GameBehav.Player.ActiveCharacter[0].transform.position = new Vector3(LeftPos.transform.position.x, LeftPos.transform.position.y, 0);
        }
        if (GameBehav.Player.ActiveCharacter[2] != null)
        {
            GameBehav.Player.ActiveCharacter[2].transform.position = new Vector3(RightPos.transform.position.x, RightPos.transform.position.y, 0);
        }
        if (GameBehav.Player.BackCharacter != null)
        {
            GameBehav.Player.BackCharacter.transform.position = new Vector3(LeftBackPos.transform.position.x, LeftBackPos.transform.position.y, 0);
        }
    }
}
