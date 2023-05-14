using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationBehaviour : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameBehaviour GameBehav;
    private Camera cam;

    public Vector2 CentrePos;
    public Vector2 LeftPos;
    public Vector2 RightPos;

    public Vector2 LeftBackPos;
    public Vector2 RightBackPos;

    public int m_Speed;

    private bool OnStartDrag;
    private Vector3 StartPos;
    public float DragOffsetXPos;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (DragOffsetXPos < 0)
        {
            if (OnStartDrag == true)
            {
                GameBehav.Player.BackCharacter.transform.position = RightBackPos;
                OnStartDrag = false;
            }

            GameBehav.Player.ActiveCharacter[1].transform.position = CentrePos + DistNormalize(CentrePos, LeftPos) * -DragOffsetXPos * m_Speed;
            GameBehav.Player.ActiveCharacter[0].transform.position = LeftPos + DistNormalize(LeftPos, LeftBackPos) * -DragOffsetXPos * m_Speed;
            GameBehav.Player.ActiveCharacter[2].transform.position = RightPos + DistNormalize(RightPos, CentrePos) * -DragOffsetXPos * m_Speed;
            GameBehav.Player.BackCharacter.transform.position = RightBackPos + DistNormalize(RightBackPos, RightPos) * -DragOffsetXPos * m_Speed;

            if (DragOffsetXPos <= -1f)
            {
                CharacterBehaviour Temp0, Temp1, Temp2, Temp3;
                Temp0 = GameBehav.Player.ActiveCharacter[0];
                Temp1 = GameBehav.Player.ActiveCharacter[1];
                Temp2 = GameBehav.Player.ActiveCharacter[2];
                Temp3 = GameBehav.Player.BackCharacter;

                GameBehav.Player.ActiveCharacter[0] = Temp1;
                GameBehav.Player.ActiveCharacter[1] = Temp2;
                GameBehav.Player.ActiveCharacter[2] = Temp3;
                GameBehav.Player.BackCharacter = Temp0;

                DragOffsetXPos = 0;
                StartPos = GetMousePos();
                OnStartDrag = true;

                GameBehav.Player.UpdateActive();
                ResetCharPos();
            }
        }
        if (DragOffsetXPos > 0)
        {
            if (OnStartDrag == true)
            {
                GameBehav.Player.BackCharacter.transform.position = LeftBackPos;
                OnStartDrag = false;
            }

            GameBehav.Player.ActiveCharacter[1].transform.position = CentrePos + DistNormalize(CentrePos, RightPos) * DragOffsetXPos * m_Speed;
            GameBehav.Player.ActiveCharacter[0].transform.position = LeftPos + DistNormalize(LeftPos, CentrePos) * DragOffsetXPos * m_Speed;
            GameBehav.Player.ActiveCharacter[2].transform.position = RightPos + DistNormalize(RightPos, RightBackPos) * DragOffsetXPos * m_Speed;
            GameBehav.Player.BackCharacter.transform.position = LeftBackPos + DistNormalize(LeftBackPos, LeftPos) * DragOffsetXPos * m_Speed;

            if (DragOffsetXPos >= 1f)
            {
                CharacterBehaviour Temp0, Temp1, Temp2, Temp3;
                Temp0 = GameBehav.Player.ActiveCharacter[0];
                Temp1 = GameBehav.Player.ActiveCharacter[1];
                Temp2 = GameBehav.Player.ActiveCharacter[2];
                Temp3 = GameBehav.Player.BackCharacter;

                GameBehav.Player.ActiveCharacter[0] = Temp3;
                GameBehav.Player.ActiveCharacter[1] = Temp0;
                GameBehav.Player.ActiveCharacter[2] = Temp1;
                GameBehav.Player.BackCharacter = Temp2;

                DragOffsetXPos = 0;
                StartPos = GetMousePos();
                OnStartDrag = true;

                GameBehav.Player.UpdateActive();
                ResetCharPos();
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
            GameBehav.Player.ActiveCharacter[1].transform.position = CentrePos;
        }
        if (GameBehav.Player.ActiveCharacter[0] != null)
        {
            GameBehav.Player.ActiveCharacter[0].transform.position = LeftPos;
        }
        if (GameBehav.Player.ActiveCharacter[2] != null)
        {
            GameBehav.Player.ActiveCharacter[2].transform.position = RightPos;
        }
        if (GameBehav.Player.BackCharacter != null)
        {
            GameBehav.Player.BackCharacter.transform.position = LeftBackPos;
        }
    }

    private Vector2 DistNormalize(Vector2 A, Vector2 B)
    {
        Vector2 C = B - A;
        C.Normalize();
        return C;
    }
}
