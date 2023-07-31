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

    [HideInInspector] public Vector2 CentrePos;
    [HideInInspector] public Vector2 LeftPos;
    [HideInInspector] public Vector2 RightPos;

    [HideInInspector] public Vector2 LeftBackPos;
    [HideInInspector] public Vector2 RightBackPos;

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
        if (IsEnemy) { return; }
        if (GameBehav.CurrentPlayerTurn != PlayerBehav) { return; }
        if (PlayerBehav.CharacterTape.Length == 1) { return; }
        if (PlayerBehav.CardPlayed) { return; }

        if (DragOffsetXPos < 0)
        {
            if (OnStartDrag == true)
            {
                if (PlayerBehav.CharacterTape.Length == 3)
                {
                    PlayerBehav.CharacterTape[PlayerBehav.CharacterTape.Length - 1].transform.position = RightBackPos;
                }
                OnStartDrag = false;
            }

            switch (PlayerBehav.CharacterTape.Length)
            {
                case 2:
                    PlayerBehav.CharacterTape[0].transform.position = LeftPos + DistNormalize(LeftPos, LeftBackPos) * -DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[1].transform.position = CentrePos + DistNormalize(CentrePos, LeftPos) * -DragOffsetXPos * m_Speed;
                    break;
                case 3:
                    PlayerBehav.CharacterTape[0].transform.position = LeftPos + DistNormalize(LeftPos, LeftBackPos) * -DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[1].transform.position = CentrePos + DistNormalize(CentrePos, LeftPos) * -DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[2].transform.position = RightPos + DistNormalize(RightPos, CentrePos) * -DragOffsetXPos * m_Speed;
                    break;
                case 4:
                    PlayerBehav.CharacterTape[0].transform.position = LeftPos + DistNormalize(LeftPos, LeftBackPos) * -DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[1].transform.position = CentrePos + DistNormalize(CentrePos, LeftPos) * -DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[2].transform.position = RightPos + DistNormalize(RightPos, CentrePos) * -DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[3].transform.position = RightBackPos + DistNormalize(RightBackPos, RightPos) * -DragOffsetXPos * m_Speed;
                    break;
                case 5:
                    PlayerBehav.CharacterTape[0].transform.position = LeftPos + DistNormalize(LeftPos, LeftBackPos) * -DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[1].transform.position = CentrePos + DistNormalize(CentrePos, LeftPos) * -DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[2].transform.position = RightPos + DistNormalize(RightPos, CentrePos) * -DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[3].transform.position = RightBackPos + DistNormalize(RightBackPos, RightPos) * -DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[4].transform.position = LeftBackPos + DistNormalize(LeftBackPos, RightBackPos) * -DragOffsetXPos * m_Speed;
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
                if (PlayerBehav.CharacterTape.Length < 5)
                {
                    PlayerBehav.CharacterTape[PlayerBehav.CharacterTape.Length - 1].transform.position = LeftBackPos;
                }
                OnStartDrag = false;
            }

            switch (PlayerBehav.CharacterTape.Length)
            {
                case 2:
                    PlayerBehav.CharacterTape[0].transform.position = LeftPos + DistNormalize(LeftPos, CentrePos) * DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[1].transform.position = CentrePos + DistNormalize(CentrePos, RightPos) * DragOffsetXPos * m_Speed;
                    break;
                case 3:
                    PlayerBehav.CharacterTape[0].transform.position = LeftPos + DistNormalize(LeftPos, CentrePos) * DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[1].transform.position = CentrePos + DistNormalize(CentrePos, RightPos) * DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[2].transform.position = RightPos + DistNormalize(RightPos, RightBackPos) * DragOffsetXPos * m_Speed;
                    break;
                case 4:
                    PlayerBehav.CharacterTape[0].transform.position = LeftPos + DistNormalize(LeftPos, CentrePos) * DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[1].transform.position = CentrePos + DistNormalize(CentrePos, RightPos) * DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[2].transform.position = RightPos + DistNormalize(RightPos, RightBackPos) * DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[3].transform.position = LeftBackPos + DistNormalize(LeftBackPos, LeftPos) * DragOffsetXPos * m_Speed;
                    break;
                case 5:
                    PlayerBehav.CharacterTape[0].transform.position = LeftPos + DistNormalize(LeftPos, CentrePos) * DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[1].transform.position = CentrePos + DistNormalize(CentrePos, RightPos) * DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[2].transform.position = RightPos + DistNormalize(RightPos, RightBackPos) * DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[3].transform.position = RightBackPos + DistNormalize(RightBackPos, LeftBackPos) * DragOffsetXPos * m_Speed;
                    PlayerBehav.CharacterTape[4].transform.position = LeftBackPos + DistNormalize(LeftBackPos, LeftPos) * DragOffsetXPos * m_Speed;
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
        if (IsEnemy) { return; }
        if (GameBehav.CurrentPlayerTurn != PlayerBehav) { return; }
        if (PlayerBehav.CardPlayed) { return; }

        StartPos = GetMousePos();
        OnStartDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsEnemy) { return; }
        if (GameBehav.CurrentPlayerTurn != PlayerBehav) { return; }
        if (PlayerBehav.CharacterTape.Length == 1) { return; }
        if (PlayerBehav.CardPlayed) { return; }

        DragOffsetXPos = GetMousePos().x - StartPos.x;

        if (prevMouseXPos > GetMousePos().x)
        {
            switch (PlayerBehav.CharacterTape.Length)
            {
                case 2:
                    PlayerBehav.CharacterTape[0].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[0].Character.Walk_Down_Anim);
                    PlayerBehav.CharacterTape[1].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[1].Character.Walk_Left_Anim);
                    break;
                case 3:
                    PlayerBehav.CharacterTape[0].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[0].Character.Walk_Down_Anim);
                    PlayerBehav.CharacterTape[1].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[1].Character.Walk_Left_Anim);
                    PlayerBehav.CharacterTape[2].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[2].Character.Walk_Left_Anim);
                    break;
                case 4:
                    PlayerBehav.CharacterTape[0].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[0].Character.Walk_Down_Anim);
                    PlayerBehav.CharacterTape[1].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[1].Character.Walk_Left_Anim);
                    PlayerBehav.CharacterTape[2].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[2].Character.Walk_Left_Anim);
                    PlayerBehav.CharacterTape[3].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[3].Character.Walk_Up_Anim);
                    break;
                case 5:
                    PlayerBehav.CharacterTape[0].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[0].Character.Walk_Down_Anim);
                    PlayerBehav.CharacterTape[1].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[1].Character.Walk_Left_Anim);
                    PlayerBehav.CharacterTape[2].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[2].Character.Walk_Left_Anim);
                    PlayerBehav.CharacterTape[3].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[3].Character.Walk_Up_Anim);
                    PlayerBehav.CharacterTape[4].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[4].Character.Walk_Right_Anim);
                    break;
            }
        }
        else if (prevMouseXPos < GetMousePos().x)
        {
            switch (PlayerBehav.CharacterTape.Length)
            {
                case 2:
                    PlayerBehav.CharacterTape[0].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[0].Character.Walk_Right_Anim);
                    PlayerBehav.CharacterTape[1].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[1].Character.Walk_Right_Anim);
                    break;
                case 3:
                    PlayerBehav.CharacterTape[0].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[0].Character.Walk_Right_Anim);
                    PlayerBehav.CharacterTape[1].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[1].Character.Walk_Right_Anim);
                    PlayerBehav.CharacterTape[2].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[2].Character.Walk_Down_Anim);
                    break;
                case 4:
                    PlayerBehav.CharacterTape[0].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[0].Character.Walk_Right_Anim);
                    PlayerBehav.CharacterTape[1].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[1].Character.Walk_Right_Anim);
                    PlayerBehav.CharacterTape[2].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[2].Character.Walk_Down_Anim);
                    PlayerBehav.CharacterTape[3].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[3].Character.Walk_Up_Anim);
                    break;
                case 5:
                    PlayerBehav.CharacterTape[0].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[0].Character.Walk_Right_Anim);
                    PlayerBehav.CharacterTape[1].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[1].Character.Walk_Right_Anim);
                    PlayerBehav.CharacterTape[2].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[2].Character.Walk_Down_Anim);
                    PlayerBehav.CharacterTape[3].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[3].Character.Walk_Left_Anim);
                    PlayerBehav.CharacterTape[4].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[4].Character.Walk_Up_Anim);
                    break;
            }
        }

        prevMouseXPos = GetMousePos().x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsEnemy) { return; }
        if (GameBehav.CurrentPlayerTurn != PlayerBehav) { return; }
        if (PlayerBehav.CharacterTape.Length == 1) { return; }
        if (PlayerBehav.CardPlayed) { return; }

        StartPos = new Vector3(0, 0, 0);
        prevMouseXPos = DragOffsetXPos = 0;
        OnStartDrag = false;
        ResetCharPos();

        for (int i = 0; i < PlayerBehav.CharacterTape.Length; i++)
        {
            PlayerBehav.CharacterTape[i].CharDisplay.SetCurrAnim(PlayerBehav.CharacterTape[i].Character.Idle_Up_Anim);
        }
    }

    private Vector3 GetMousePos()
    {
        Vector3 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        worldPoint.z = 0.0f;
        return worldPoint;
    }

    public void UpdateDeadCharacters()
    {
        for (int i = 0; i < PlayerBehav.CharacterTape.Length; i++)
        {
            if (PlayerBehav.CharacterTape[i].IsDead == true)
            {
                CharacterBehaviour[] Temp = new CharacterBehaviour[PlayerBehav.CharacterTape.Length - 1];
                int k = 0;
                for (int j = 0; j < PlayerBehav.CharacterTape.Length; j++)
                {
                    if (j != i)
                    {
                        Temp[k] = PlayerBehav.CharacterTape[j];
                        k++;
                    }
                }

                PlayerBehav.CharacterTape = Temp;
            }
        }

        if (PlayerBehav.CharacterTape.Length == 0)
        {
            GameBehav.CheckGameOver();
            return;
        }
        ResetCharPos();
    }

    public void ResetCharPos()
    {
        for (int i = 0; i < PlayerBehav.CharacterTape.Length; i++)
        {
            switch (i)
            {
                case 0:
                    PlayerBehav.CharacterTape[0].transform.position = LeftPos;
                    if (PlayerBehav.CharacterTape.Length == 1)
                    {
                        PlayerBehav.CharacterTape[0].transform.position = CentrePos;
                    }
                    break;
                case 1:
                    PlayerBehav.CharacterTape[1].transform.position = CentrePos;
                    break;
                case 2:
                    PlayerBehav.CharacterTape[2].transform.position = RightPos;
                    break;
                case 3:
                    PlayerBehav.CharacterTape[3].transform.position = RightBackPos;
                    break;
                case 4:
                    PlayerBehav.CharacterTape[4].transform.position = LeftBackPos;
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
        CharacterBehaviour[] TempTape = PlayerBehav.CharacterTape;
        PlayerBehav.CharacterTape = new CharacterBehaviour[PlayerBehav.CharacterTape.Length];

        if (right)
        {
            for (int i = 0; i < PlayerBehav.CharacterTape.Length; i++)
            {
                if (i - 1 >= 0)
                {
                    PlayerBehav.CharacterTape[i] = TempTape[i - 1];
                }
                else
                {
                    PlayerBehav.CharacterTape[i] = TempTape[PlayerBehav.CharacterTape.Length - 1];
                }
            }
        }
        else
        {
            for (int i = 0; i < PlayerBehav.CharacterTape.Length; i++)
            {
                if (i + 1 < PlayerBehav.CharacterTape.Length)
                {
                    PlayerBehav.CharacterTape[i] = TempTape[i + 1];
                }
                else
                {
                    PlayerBehav.CharacterTape[i] = TempTape[0];
                }
            }
        }

        PlayerBehav.UpdateActive();
        if (PlayerBehav == GameBehav.Player) { GameBehav.Select(PlayerBehav.CharacterTape[1]); }
        ResetCharPos();
    }
}
