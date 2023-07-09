using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationBehaviourUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Camera cam;
    public PartyCharacterBehaviour PartyBehav;
    public GameObject Character;

    public Vector2 CentrePos;
    public Vector2 LeftPos;
    public Vector2 RightPos;

    private Vector2 CharacterPos;

    public Vector2 LeftBackPos;
    public Vector2 RightBackPos;

    public Vector2 TempCentrePos;
    public Vector2 TempLeftPos;
    public Vector2 TempRightPos;

    public Vector2 TempLeftBackPos;
    public Vector2 TempRightBackPos;

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
        CharacterPos = new Vector2(Character.transform.position.x, Character.transform.position.y);

        TempCentrePos = CentrePos + CharacterPos;
        TempLeftPos = LeftPos + CharacterPos;
        TempRightPos = RightPos + CharacterPos;

        TempLeftBackPos = LeftBackPos + CharacterPos;
        TempRightBackPos = RightBackPos + CharacterPos;

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
                    PartyBehav.RotationCharacter[PartyBehav.CharacterTape.Length - 1].transform.position = TempRightBackPos;
                }
                OnStartDrag = false;
            }

            switch (PartyBehav.CharacterTape.Length)
            {
                case 2:
                    PartyBehav.RotationCharacter[0].transform.position = TempLeftPos + DistNormalize(TempLeftPos, TempLeftBackPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = TempCentrePos + DistNormalize(TempCentrePos, TempLeftPos) * -DragOffsetXPos * m_Speed;
                    break;
                case 3:
                    PartyBehav.RotationCharacter[0].transform.position = TempLeftPos + DistNormalize(TempLeftPos, TempLeftBackPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = TempCentrePos + DistNormalize(TempCentrePos, TempLeftPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[2].transform.position = TempRightPos + DistNormalize(TempRightPos, TempCentrePos) * -DragOffsetXPos * m_Speed;
                    break;
                case 4:
                    PartyBehav.RotationCharacter[0].transform.position = TempLeftPos + DistNormalize(TempLeftPos, TempLeftBackPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = TempCentrePos + DistNormalize(TempCentrePos, TempLeftPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[2].transform.position = TempRightPos + DistNormalize(TempRightPos, TempCentrePos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[3].transform.position = TempRightBackPos + DistNormalize(TempRightBackPos, TempRightPos) * -DragOffsetXPos * m_Speed;
                    break;
                case 5:
                    PartyBehav.RotationCharacter[0].transform.position = TempLeftPos + DistNormalize(TempLeftPos, TempLeftBackPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = TempCentrePos + DistNormalize(TempCentrePos, TempLeftPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[2].transform.position = TempRightPos + DistNormalize(TempRightPos, TempCentrePos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[3].transform.position = TempRightBackPos + DistNormalize(TempRightBackPos, TempRightPos) * -DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[4].transform.position = TempLeftBackPos + DistNormalize(TempLeftBackPos, TempRightBackPos) * -DragOffsetXPos * m_Speed;
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
                    PartyBehav.RotationCharacter[PartyBehav.CharacterTape.Length - 1].transform.position = TempLeftBackPos;
                }
                OnStartDrag = false;
            }

            switch (PartyBehav.CharacterTape.Length)
            {
                case 2:
                    PartyBehav.RotationCharacter[0].transform.position = TempLeftPos + DistNormalize(TempLeftPos, TempCentrePos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = TempCentrePos + DistNormalize(TempCentrePos, TempRightPos) * DragOffsetXPos * m_Speed;
                    break;
                case 3:
                    PartyBehav.RotationCharacter[0].transform.position = TempLeftPos + DistNormalize(TempLeftPos, TempCentrePos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = TempCentrePos + DistNormalize(TempCentrePos, TempRightPos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[2].transform.position = TempRightPos + DistNormalize(TempRightPos, TempRightBackPos) * DragOffsetXPos * m_Speed;
                    break;
                case 4:
                    PartyBehav.RotationCharacter[0].transform.position = TempLeftPos + DistNormalize(TempLeftPos, TempCentrePos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = TempCentrePos + DistNormalize(TempCentrePos, TempRightPos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[2].transform.position = TempRightPos + DistNormalize(TempRightPos, TempRightBackPos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[3].transform.position = TempLeftBackPos + DistNormalize(TempLeftBackPos, TempLeftPos) * DragOffsetXPos * m_Speed;
                    break;
                case 5:
                    PartyBehav.RotationCharacter[0].transform.position = TempLeftPos + DistNormalize(TempLeftPos, TempCentrePos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[1].transform.position = TempCentrePos + DistNormalize(TempCentrePos, TempRightPos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[2].transform.position = TempRightPos + DistNormalize(TempRightPos, TempRightBackPos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[3].transform.position = TempRightBackPos + DistNormalize(TempRightBackPos, TempLeftBackPos) * DragOffsetXPos * m_Speed;
                    PartyBehav.RotationCharacter[4].transform.position = TempLeftBackPos + DistNormalize(TempLeftBackPos, TempLeftPos) * DragOffsetXPos * m_Speed;
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
            PartyBehav.RotationCharacter[0].transform.position = TempCentrePos;
            return;
        }

        for (int i = 0; i < PartyBehav.CharacterTape.Length; i++)
        {
            switch (i)
            {
                case 0:
                    PartyBehav.RotationCharacter[0].transform.position = TempLeftPos;
                    break;
                case 1:
                    PartyBehav.RotationCharacter[1].transform.position = TempCentrePos;
                    break;
                case 2:
                    PartyBehav.RotationCharacter[2].transform.position = TempRightPos;
                    break;
                case 3:
                    PartyBehav.RotationCharacter[3].transform.position = TempRightBackPos;
                    break;
                case 4:
                    PartyBehav.RotationCharacter[4].transform.position = TempLeftBackPos;
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
        UICharacterDisplay[] TempTape = PartyBehav.CharacterTape;
        PartyBehav.CharacterTape = new UICharacterDisplay[PartyBehav.CharacterTape.Length];

        UICharacterDisplay[] UITempTape = PartyBehav.UICharacter;
        PartyBehav.UICharacter = new UICharacterDisplay[5];

        UICharacterDisplay[] RotTempTape = PartyBehav.RotationCharacter;
        PartyBehav.RotationCharacter = new UICharacterDisplay[5];

        if (right)
        {
            for (int i = 0; i < PartyBehav.CharacterTape.Length; i++)
            {
                if (i - 1 >= 0)
                {
                    PartyBehav.CharacterTape[i] = TempTape[i - 1];
                    PartyBehav.UICharacter[i] = UITempTape[i - 1];
                    PartyBehav.RotationCharacter[i] = RotTempTape[i - 1];
                }
                else
                {
                    PartyBehav.CharacterTape[i] = TempTape[PartyBehav.CharacterTape.Length - 1];
                    PartyBehav.UICharacter[i] = UITempTape[PartyBehav.CharacterTape.Length - 1];
                    PartyBehav.RotationCharacter[i] = RotTempTape[PartyBehav.CharacterTape.Length - 1];
                }
            }
        }
        else
        {
            for (int i = 0; i < PartyBehav.CharacterTape.Length; i++)
            {
                if (i + 1 < PartyBehav.CharacterTape.Length)
                {
                    PartyBehav.CharacterTape[i] = TempTape[i + 1];
                    PartyBehav.UICharacter[i] = UITempTape[i + 1];
                    PartyBehav.RotationCharacter[i] = RotTempTape[i + 1];
                }
                else
                {
                    PartyBehav.CharacterTape[i] = TempTape[0];
                    PartyBehav.UICharacter[i] = UITempTape[0];
                    PartyBehav.RotationCharacter[i] = RotTempTape[0];
                }
            }
        }

        for (int i = PartyBehav.CharacterTape.Length; i < 5; i++)
        {
            PartyBehav.UICharacter[i] = UITempTape[i];
            PartyBehav.RotationCharacter[i] = RotTempTape[i];
        }

        ResetCharPos();
    }
}
