using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationBehaviourUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Camera cam;
    public PartyCharacterBehaviour PartyBehav;

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

    public void Init()
    {
        ResetCharPos();

        for (int i = 0; i < PartyBehav.CharacterTape.Length; i++)
        {
            PartyBehav.RotationCharacter[i].SetCurrAnim(PartyBehav.RotationCharacter[i].Character.Idle_Up_Anim);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PartyBehav.CharacterTape.Length == 1) { return; }

        if (DragOffsetXPos < 0)
        {
            if (OnStartDrag == true)
            {
                if (PartyBehav.CharacterTape.Length == 3)
                {
                    PartyBehav.RotationCharacter[PartyBehav.CharacterTape.Length - 1].transform.position = RightBackPos;
                }
                OnStartDrag = false;
            }

            switch (PartyBehav.CharacterTape.Length)
            {
                case 2:
                    PartyBehav.RotationCharacter[0].transform.position = LeftPos + DistNormalize(LeftPos, LeftBackPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = CentrePos + DistNormalize(CentrePos, LeftPos) * -DragOffsetXPos * m_Speed;
                    break;
                case 3:
                    PartyBehav.RotationCharacter[0].transform.position = LeftPos + DistNormalize(LeftPos, LeftBackPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = CentrePos + DistNormalize(CentrePos, LeftPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[2].transform.position = RightPos + DistNormalize(RightPos, CentrePos) * -DragOffsetXPos * m_Speed;
                    break;
                case 4:
                    PartyBehav.RotationCharacter[0].transform.position = LeftPos + DistNormalize(LeftPos, LeftBackPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = CentrePos + DistNormalize(CentrePos, LeftPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[2].transform.position = RightPos + DistNormalize(RightPos, CentrePos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[3].transform.position = RightBackPos + DistNormalize(RightBackPos, RightPos) * -DragOffsetXPos * m_Speed;
                    break;
                case 5:
                    PartyBehav.RotationCharacter[0].transform.position = LeftPos + DistNormalize(LeftPos, LeftBackPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = CentrePos + DistNormalize(CentrePos, LeftPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[2].transform.position = RightPos + DistNormalize(RightPos, CentrePos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[3].transform.position = RightBackPos + DistNormalize(RightBackPos, RightPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[4].transform.position = LeftBackPos + DistNormalize(LeftBackPos, RightBackPos) * -DragOffsetXPos * m_Speed;
                    break;
            }

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
                if (PartyBehav.CharacterTape.Length < 5)
                {
                    PartyBehav.RotationCharacter[PartyBehav.CharacterTape.Length - 1].transform.position = LeftBackPos;
                }
                OnStartDrag = false;
            }

            switch (PartyBehav.CharacterTape.Length)
            {
                case 2:
                    PartyBehav.RotationCharacter[0].transform.position = LeftPos + DistNormalize(LeftPos, CentrePos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = CentrePos + DistNormalize(CentrePos, RightPos) * DragOffsetXPos * m_Speed;
                    break;
                case 3:
                    PartyBehav.RotationCharacter[0].transform.position = LeftPos + DistNormalize(LeftPos, CentrePos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = CentrePos + DistNormalize(CentrePos, RightPos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[2].transform.position = RightPos + DistNormalize(RightPos, RightBackPos) * DragOffsetXPos * m_Speed;
                    break;
                case 4:
                    PartyBehav.RotationCharacter[0].transform.position = LeftPos + DistNormalize(LeftPos, CentrePos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = CentrePos + DistNormalize(CentrePos, RightPos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[2].transform.position = RightPos + DistNormalize(RightPos, RightBackPos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[3].transform.position = LeftBackPos + DistNormalize(LeftBackPos, LeftPos) * DragOffsetXPos * m_Speed;
                    break;
                case 5:
                    PartyBehav.RotationCharacter[0].transform.position = LeftPos + DistNormalize(LeftPos, CentrePos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = CentrePos + DistNormalize(CentrePos, RightPos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[2].transform.position = RightPos + DistNormalize(RightPos, RightBackPos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[3].transform.position = RightBackPos + DistNormalize(RightBackPos, LeftBackPos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[4].transform.position = LeftBackPos + DistNormalize(LeftBackPos, LeftPos) * DragOffsetXPos * m_Speed;
                    break;
            }

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
        StartPos = GetMousePos();
        OnStartDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (PartyBehav.CharacterTape.Length == 1) { return; }

        DragOffsetXPos = GetMousePos().x - StartPos.x;

        if (prevMouseXPos > GetMousePos().x)
        {
            switch (PartyBehav.CharacterTape.Length)
            {
                case 2:
                    PartyBehav.RotationCharacter[0].SetCurrAnim(PartyBehav.RotationCharacter[0].Character.Walk_Down_Anim);
                    PartyBehav.RotationCharacter[1].SetCurrAnim(PartyBehav.RotationCharacter[1].Character.Walk_Left_Anim);
                    break;
                case 3:
                    PartyBehav.RotationCharacter[0].SetCurrAnim(PartyBehav.RotationCharacter[0].Character.Walk_Down_Anim);
                    PartyBehav.RotationCharacter[1].SetCurrAnim(PartyBehav.RotationCharacter[1].Character.Walk_Left_Anim);
                    PartyBehav.RotationCharacter[2].SetCurrAnim(PartyBehav.RotationCharacter[2].Character.Walk_Left_Anim);
                    break;
                case 4:
                    PartyBehav.RotationCharacter[0].SetCurrAnim(PartyBehav.RotationCharacter[0].Character.Walk_Down_Anim);
                    PartyBehav.RotationCharacter[1].SetCurrAnim(PartyBehav.RotationCharacter[1].Character.Walk_Left_Anim);
                    PartyBehav.RotationCharacter[2].SetCurrAnim(PartyBehav.RotationCharacter[2].Character.Walk_Left_Anim);
                    PartyBehav.RotationCharacter[3].SetCurrAnim(PartyBehav.RotationCharacter[3].Character.Walk_Up_Anim);
                    break;
                case 5:
                    PartyBehav.RotationCharacter[0].SetCurrAnim(PartyBehav.RotationCharacter[0].Character.Walk_Down_Anim);
                    PartyBehav.RotationCharacter[1].SetCurrAnim(PartyBehav.RotationCharacter[1].Character.Walk_Left_Anim);
                    PartyBehav.RotationCharacter[2].SetCurrAnim(PartyBehav.RotationCharacter[2].Character.Walk_Left_Anim);
                    PartyBehav.RotationCharacter[3].SetCurrAnim(PartyBehav.RotationCharacter[3].Character.Walk_Up_Anim);
                    PartyBehav.RotationCharacter[4].SetCurrAnim(PartyBehav.RotationCharacter[4].Character.Walk_Right_Anim);
                    break;
            }
        }
        else if (prevMouseXPos < GetMousePos().x)
        {
            switch (PartyBehav.CharacterTape.Length)
            {
                case 2:
                    PartyBehav.RotationCharacter[0].SetCurrAnim(PartyBehav.RotationCharacter[0].Character.Walk_Right_Anim);
                    PartyBehav.RotationCharacter[1].SetCurrAnim(PartyBehav.RotationCharacter[1].Character.Walk_Right_Anim);
                    break;
                case 3:
                    PartyBehav.RotationCharacter[0].SetCurrAnim(PartyBehav.RotationCharacter[0].Character.Walk_Right_Anim);
                    PartyBehav.RotationCharacter[1].SetCurrAnim(PartyBehav.RotationCharacter[1].Character.Walk_Right_Anim);
                    PartyBehav.RotationCharacter[2].SetCurrAnim(PartyBehav.RotationCharacter[2].Character.Walk_Down_Anim);
                    break;
                case 4:
                    PartyBehav.RotationCharacter[0].SetCurrAnim(PartyBehav.RotationCharacter[0].Character.Walk_Right_Anim);
                    PartyBehav.RotationCharacter[1].SetCurrAnim(PartyBehav.RotationCharacter[1].Character.Walk_Right_Anim);
                    PartyBehav.RotationCharacter[2].SetCurrAnim(PartyBehav.RotationCharacter[2].Character.Walk_Down_Anim);
                    PartyBehav.RotationCharacter[3].SetCurrAnim(PartyBehav.RotationCharacter[3].Character.Walk_Up_Anim);
                    break;
                case 5:
                    PartyBehav.RotationCharacter[0].SetCurrAnim(PartyBehav.RotationCharacter[0].Character.Walk_Right_Anim);
                    PartyBehav.RotationCharacter[1].SetCurrAnim(PartyBehav.RotationCharacter[1].Character.Walk_Right_Anim);
                    PartyBehav.RotationCharacter[2].SetCurrAnim(PartyBehav.RotationCharacter[2].Character.Walk_Down_Anim);
                    PartyBehav.RotationCharacter[3].SetCurrAnim(PartyBehav.RotationCharacter[3].Character.Walk_Left_Anim);
                    PartyBehav.RotationCharacter[4].SetCurrAnim(PartyBehav.RotationCharacter[4].Character.Walk_Up_Anim);
                    break;
            }
        }

        prevMouseXPos = GetMousePos().x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (PartyBehav.CharacterTape.Length == 1) { return; }

        StartPos = new Vector3(0, 0, 0);
        prevMouseXPos = DragOffsetXPos = 0;
        OnStartDrag = false;
        ResetCharPos();

        for (int i = 0; i < PartyBehav.CharacterTape.Length; i++)
        {
            PartyBehav.RotationCharacter[i].SetCurrAnim(PartyBehav.RotationCharacter[i].Character.Idle_Up_Anim);
        }
    }

    private Vector3 GetMousePos()
    {
        Vector3 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        worldPoint.z = 0.0f;
        return worldPoint;
    }

    public void ResetCharPos()
    {
        if (PartyBehav.CharacterTape.Length == 1)
        {
            PartyBehav.RotationCharacter[0].transform.position = CentrePos;
            return;
        }

        for (int i = 0; i < PartyBehav.CharacterTape.Length; i++)
        {
            switch (i)
            {
                case 0:
                    PartyBehav.RotationCharacter[0].transform.position = LeftPos;
                    break;
                case 1:
                    PartyBehav.RotationCharacter[1].transform.position = CentrePos;
                    break;
                case 2:
                    PartyBehav.RotationCharacter[2].transform.position = RightPos;
                    break;
                case 3:
                    PartyBehav.RotationCharacter[3].transform.position = RightBackPos;
                    break;
                case 4:
                    PartyBehav.RotationCharacter[4].transform.position = LeftBackPos;
                    break;
            }
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
        //CharacterBehaviour[] TempTape = PartyBehav.RotationCharacter;
        //PartyBehav.RotationCharacter = new CharacterBehaviour[PartyBehav.CharacterTape.Length];

        //if (right)
        //{
        //    for (int i = 0; i < PartyBehav.CharacterTape.Length; i++)
        //    {
        //        if (i - 1 >= 0)
        //        {
        //            PartyBehav.RotationCharacter[i] = TempTape[i - 1];
        //        }
        //        else
        //        {
        //            PartyBehav.RotationCharacter[i] = TempTape[PartyBehav.CharacterTape.Length - 1];
        //        }
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < PartyBehav.CharacterTape.Length; i++)
        //    {
        //        if (i + 1 < PartyBehav.CharacterTape.Length)
        //        {
        //            PartyBehav.RotationCharacter[i] = TempTape[i + 1];
        //        }
        //        else
        //        {
        //            PartyBehav.RotationCharacter[i] = TempTape[0];
        //        }
        //    }
        //}
        //ResetCharPos();
    }
}
