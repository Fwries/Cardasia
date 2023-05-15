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

    private float prevMouseXPos;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameBehav.CurrentPlayerTurn != GameBehav.Player) { return; }
        if (GameBehav.Player.BackCharacter == null) { return; }
        if (GameBehav.Player.BackCharacter.IsDead == true) { return; }

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

            if (DragOffsetXPos <= -1)
            {
                DragOffsetXPos = 0;
                StartPos = GetMousePos();
                OnStartDrag = true;

                Shift(false);
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

            if (DragOffsetXPos >= 1)
            {
                DragOffsetXPos = 0;
                StartPos = GetMousePos();
                OnStartDrag = true;

                Shift(true);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (GameBehav.Player.ActiveCharacter[i].IsDead == true)
            {
                if (i == 0)
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

                    GameBehav.Player.UpdateActive();
                    ResetCharPos();
                }
                else if (i == 1)
                {
                    if (GameBehav.Player.ActiveCharacter[0].IsDead == true || GameBehav.Player.ActiveCharacter[2].IsDead == true) { return; }

                    Shift(true);
                    Shift(true);
                }
                else if (i == 2)
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

                    GameBehav.Player.UpdateActive();
                    ResetCharPos();
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameBehav.CurrentPlayerTurn != GameBehav.Player) { return; }
        if (GameBehav.Player.BackCharacter == null) { return; }
        if (GameBehav.Player.BackCharacter.IsDead == true) { return; }

        StartPos = GetMousePos();
        OnStartDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameBehav.CurrentPlayerTurn != GameBehav.Player) { return; }
        if (GameBehav.Player.BackCharacter == null) { return; }
        if (GameBehav.Player.BackCharacter.IsDead == true) { return; }

        DragOffsetXPos = GetMousePos().x - StartPos.x;

        if (prevMouseXPos > GetMousePos().x)
        {
            GameBehav.Player.ActiveCharacter[1].CharDisplay.SetCurrAnim(GameBehav.Player.ActiveCharacter[1].Character.Walk_Left_Anim);
            GameBehav.Player.ActiveCharacter[0].CharDisplay.SetCurrAnim(GameBehav.Player.ActiveCharacter[0].Character.Walk_Down_Anim);
            GameBehav.Player.ActiveCharacter[2].CharDisplay.SetCurrAnim(GameBehav.Player.ActiveCharacter[2].Character.Walk_Left_Anim);
            GameBehav.Player.BackCharacter.CharDisplay.SetCurrAnim(GameBehav.Player.BackCharacter.Character.Walk_Up_Anim);
        }
        else if (prevMouseXPos < GetMousePos().x)
        {
            GameBehav.Player.ActiveCharacter[1].CharDisplay.SetCurrAnim(GameBehav.Player.ActiveCharacter[1].Character.Walk_Right_Anim);
            GameBehav.Player.ActiveCharacter[0].CharDisplay.SetCurrAnim(GameBehav.Player.ActiveCharacter[0].Character.Walk_Right_Anim);
            GameBehav.Player.ActiveCharacter[2].CharDisplay.SetCurrAnim(GameBehav.Player.ActiveCharacter[2].Character.Walk_Down_Anim);
            GameBehav.Player.BackCharacter.CharDisplay.SetCurrAnim(GameBehav.Player.BackCharacter.Character.Walk_Up_Anim);
        }

        prevMouseXPos = GetMousePos().x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameBehav.CurrentPlayerTurn != GameBehav.Player) { return; }
        if (GameBehav.Player.BackCharacter == null) { return; }
        if (GameBehav.Player.BackCharacter.IsDead == true) { return; }

        StartPos = new Vector3(0, 0, 0);
        prevMouseXPos = DragOffsetXPos = 0;
        OnStartDrag = false;
        ResetCharPos();

        GameBehav.Player.ActiveCharacter[1].CharDisplay.SetCurrAnim(GameBehav.Player.ActiveCharacter[1].Character.Idle_Up_Anim);
        GameBehav.Player.ActiveCharacter[0].CharDisplay.SetCurrAnim(GameBehav.Player.ActiveCharacter[0].Character.Idle_Up_Anim);
        GameBehav.Player.ActiveCharacter[2].CharDisplay.SetCurrAnim(GameBehav.Player.ActiveCharacter[2].Character.Idle_Up_Anim);
        GameBehav.Player.BackCharacter.CharDisplay.SetCurrAnim(GameBehav.Player.BackCharacter.Character.Idle_Up_Anim);
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

    public void Shift(bool right)
    {
        if (GameBehav.Player.BackCharacter == null) { return; }
        if (GameBehav.Player.BackCharacter.IsDead == true) { return; }

        if (right)
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
        }
        else
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
        }

        GameBehav.Player.UpdateActive();
        ResetCharPos();
    }
}
