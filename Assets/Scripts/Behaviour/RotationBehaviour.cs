using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationBehaviour : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameBehaviour GameBehav;
    private Camera cam;

    public PlayerBehaviour PlayerBehav;
    public bool IsEnemy;

    public Vector2 CentrePos;
    public Vector2 LeftPos;
    public Vector2 RightPos;

    public Vector2 LeftBackPos;
    public Vector2 RightBackPos;

    public float m_Speed;
    public float DragAmt;

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
        if (PlayerBehav.BackCharacter == null) { return; }
        if (PlayerBehav.BackCharacter.IsDead == true) { return; }

        for (int i = 0; i < 3; i++)
        {
            if (PlayerBehav.ActiveCharacter[i].IsDead == true)
            {
                if (i == 0)
                {
                    CharacterBehaviour Temp0, Temp1, Temp2, Temp3;
                    Temp0 = PlayerBehav.ActiveCharacter[0];
                    Temp1 = PlayerBehav.ActiveCharacter[1];
                    Temp2 = PlayerBehav.ActiveCharacter[2];
                    Temp3 = PlayerBehav.BackCharacter;

                    PlayerBehav.ActiveCharacter[0] = Temp1;
                    PlayerBehav.ActiveCharacter[1] = Temp2;
                    PlayerBehav.ActiveCharacter[2] = Temp3;
                    PlayerBehav.BackCharacter = Temp0;

                    PlayerBehav.UpdateActive();
                    ResetCharPos();
                }
                else if (i == 1)
                {
                    if (PlayerBehav.ActiveCharacter[0].IsDead == true || PlayerBehav.ActiveCharacter[2].IsDead == true) { return; }

                    Shift(true);
                    Shift(true);
                }
                else if (i == 2)
                {
                    CharacterBehaviour Temp0, Temp1, Temp2, Temp3;
                    Temp0 = PlayerBehav.ActiveCharacter[0];
                    Temp1 = PlayerBehav.ActiveCharacter[1];
                    Temp2 = PlayerBehav.ActiveCharacter[2];
                    Temp3 = PlayerBehav.BackCharacter;

                    PlayerBehav.ActiveCharacter[0] = Temp3;
                    PlayerBehav.ActiveCharacter[1] = Temp0;
                    PlayerBehav.ActiveCharacter[2] = Temp1;
                    PlayerBehav.BackCharacter = Temp2;

                    PlayerBehav.UpdateActive();
                    ResetCharPos();
                }
            }
        }

        if (GameBehav.CurrentPlayerTurn != PlayerBehav) { return; }

        if (DragOffsetXPos < 0)
        {
            if (OnStartDrag == true)
            {
                PlayerBehav.BackCharacter.transform.position = RightBackPos;
                OnStartDrag = false;
            }

            PlayerBehav.ActiveCharacter[1].transform.position = CentrePos + DistNormalize(CentrePos, LeftPos) * -DragOffsetXPos * m_Speed;
            PlayerBehav.ActiveCharacter[0].transform.position = LeftPos + DistNormalize(LeftPos, LeftBackPos) * -DragOffsetXPos * m_Speed;
            PlayerBehav.ActiveCharacter[2].transform.position = RightPos + DistNormalize(RightPos, CentrePos) * -DragOffsetXPos * m_Speed;
            PlayerBehav.BackCharacter.transform.position = RightBackPos + DistNormalize(RightBackPos, RightPos) * -DragOffsetXPos * m_Speed;

            if (DragOffsetXPos <= -DragAmt)
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
                PlayerBehav.BackCharacter.transform.position = LeftBackPos;
                OnStartDrag = false;
            }

            PlayerBehav.ActiveCharacter[1].transform.position = CentrePos + DistNormalize(CentrePos, RightPos) * DragOffsetXPos * m_Speed;
            PlayerBehav.ActiveCharacter[0].transform.position = LeftPos + DistNormalize(LeftPos, CentrePos) * DragOffsetXPos * m_Speed;
            PlayerBehav.ActiveCharacter[2].transform.position = RightPos + DistNormalize(RightPos, RightBackPos) * DragOffsetXPos * m_Speed;
            PlayerBehav.BackCharacter.transform.position = LeftBackPos + DistNormalize(LeftBackPos, LeftPos) * DragOffsetXPos * m_Speed;

            if (DragOffsetXPos >= DragAmt)
            {
                DragOffsetXPos = 0;
                StartPos = GetMousePos();
                OnStartDrag = true;

                Shift(true);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsEnemy) { return; }
        if (GameBehav.CurrentPlayerTurn != PlayerBehav) { return; }
        if (PlayerBehav.BackCharacter == null) { return; }
        if (PlayerBehav.BackCharacter.IsDead == true) { return; }

        StartPos = GetMousePos();
        OnStartDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsEnemy) { return; }
        if (GameBehav.CurrentPlayerTurn != PlayerBehav) { return; }
        if (PlayerBehav.BackCharacter == null) { return; }
        if (PlayerBehav.BackCharacter.IsDead == true) { return; }

        DragOffsetXPos = GetMousePos().x - StartPos.x;

        if (prevMouseXPos > GetMousePos().x)
        {
            PlayerBehav.ActiveCharacter[1].CharDisplay.SetCurrAnim(PlayerBehav.ActiveCharacter[1].Character.Walk_Left_Anim);
            PlayerBehav.ActiveCharacter[0].CharDisplay.SetCurrAnim(PlayerBehav.ActiveCharacter[0].Character.Walk_Down_Anim);
            PlayerBehav.ActiveCharacter[2].CharDisplay.SetCurrAnim(PlayerBehav.ActiveCharacter[2].Character.Walk_Left_Anim);
            PlayerBehav.BackCharacter.CharDisplay.SetCurrAnim(PlayerBehav.BackCharacter.Character.Walk_Up_Anim);
        }
        else if (prevMouseXPos < GetMousePos().x)
        {
            PlayerBehav.ActiveCharacter[1].CharDisplay.SetCurrAnim(PlayerBehav.ActiveCharacter[1].Character.Walk_Right_Anim);
            PlayerBehav.ActiveCharacter[0].CharDisplay.SetCurrAnim(PlayerBehav.ActiveCharacter[0].Character.Walk_Right_Anim);
            PlayerBehav.ActiveCharacter[2].CharDisplay.SetCurrAnim(PlayerBehav.ActiveCharacter[2].Character.Walk_Down_Anim);
            PlayerBehav.BackCharacter.CharDisplay.SetCurrAnim(PlayerBehav.BackCharacter.Character.Walk_Up_Anim);
        }

        prevMouseXPos = GetMousePos().x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsEnemy) { return; }
        if (GameBehav.CurrentPlayerTurn != PlayerBehav) { return; }
        if (PlayerBehav.BackCharacter == null) { return; }
        if (PlayerBehav.BackCharacter.IsDead == true) { return; }

        StartPos = new Vector3(0, 0, 0);
        prevMouseXPos = DragOffsetXPos = 0;
        OnStartDrag = false;
        ResetCharPos();

        PlayerBehav.ActiveCharacter[1].CharDisplay.SetCurrAnim(PlayerBehav.ActiveCharacter[1].Character.Idle_Up_Anim);
        PlayerBehav.ActiveCharacter[0].CharDisplay.SetCurrAnim(PlayerBehav.ActiveCharacter[0].Character.Idle_Up_Anim);
        PlayerBehav.ActiveCharacter[2].CharDisplay.SetCurrAnim(PlayerBehav.ActiveCharacter[2].Character.Idle_Up_Anim);
        PlayerBehav.BackCharacter.CharDisplay.SetCurrAnim(PlayerBehav.BackCharacter.Character.Idle_Up_Anim);
    }

    private Vector3 GetMousePos()
    {
        Vector3 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        worldPoint.z = 0.0f;
        return worldPoint;
    }

    public void ResetCharPos()
    {
        if (PlayerBehav.ActiveCharacter[1] != null)
        {
            PlayerBehav.ActiveCharacter[1].transform.position = CentrePos;
        }
        if (PlayerBehav.ActiveCharacter[0] != null)
        {
            PlayerBehav.ActiveCharacter[0].transform.position = LeftPos;
        }
        if (PlayerBehav.ActiveCharacter[2] != null)
        {
            PlayerBehav.ActiveCharacter[2].transform.position = RightPos;
        }
        if (PlayerBehav.BackCharacter != null)
        {
            PlayerBehav.BackCharacter.transform.position = LeftBackPos;
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
        if (PlayerBehav.BackCharacter == null) { return; }
        if (PlayerBehav.BackCharacter.IsDead == true) { return; }

        if (right)
        {
            CharacterBehaviour Temp0, Temp1, Temp2, Temp3;
            Temp0 = PlayerBehav.ActiveCharacter[0];
            Temp1 = PlayerBehav.ActiveCharacter[1];
            Temp2 = PlayerBehav.ActiveCharacter[2];
            Temp3 = PlayerBehav.BackCharacter;

            PlayerBehav.ActiveCharacter[0] = Temp3;
            PlayerBehav.ActiveCharacter[1] = Temp0;
            PlayerBehav.ActiveCharacter[2] = Temp1;
            PlayerBehav.BackCharacter = Temp2;
        }
        else
        {
            CharacterBehaviour Temp0, Temp1, Temp2, Temp3;
            Temp0 = PlayerBehav.ActiveCharacter[0];
            Temp1 = PlayerBehav.ActiveCharacter[1];
            Temp2 = PlayerBehav.ActiveCharacter[2];
            Temp3 = PlayerBehav.BackCharacter;

            PlayerBehav.ActiveCharacter[0] = Temp1;
            PlayerBehav.ActiveCharacter[1] = Temp2;
            PlayerBehav.ActiveCharacter[2] = Temp3;
            PlayerBehav.BackCharacter = Temp0;
        }

        PlayerBehav.UpdateActive();
        ResetCharPos();
    }
}
