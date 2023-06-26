using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Camera Cam;
    [SerializeField] private MapBehaviour Map;
    [SerializeField] private SpriteRenderer CharacterRenderer;
    [SerializeField] private Material TransitionMaterial;
    public SC_Character Character;

    private bool isMoving;
    private Vector3 startPos, targetPos;
    private float MoveTime = 0.25f;

    private Sprite[] CurrAnim;
    private int CurrFrame;
    private float AnimTime = 1;

    private string[] EventSc;
    private int CurrSc;
    private bool IsSc;

    // Start is called before the first frame update
    void Start()
    {
        TeleportPlayer(Map.SpawnX, Map.SpawnY);
        TransitionMaterial.SetFloat("_Cutoff", 0f);
        TransitionMaterial.SetFloat("_Fade", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSc != null)
        {
            if (CurrSc < EventSc.GetLength(0) && !IsSc) 
            { 
                Command(EventSc[CurrSc]);
            }
            else if (CurrSc == EventSc.GetLength(0)) 
            { 
                EventSc = null; 
                CurrSc = 0; 
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W) && !isMoving)
                StartCoroutine(MovePlayer(Vector3.up, false));

            if (Input.GetKey(KeyCode.A) && !isMoving)
                StartCoroutine(MovePlayer(Vector3.left, false));

            if (Input.GetKey(KeyCode.S) && !isMoving)
                StartCoroutine(MovePlayer(Vector3.down, false));

            if (Input.GetKey(KeyCode.D) && !isMoving)
                StartCoroutine(MovePlayer(Vector3.right, false));
        }

        AnimTime += Time.deltaTime;
        if (AnimTime >= 0.12f)
        {
            CurrFrame++;
            if (CurrFrame >= CurrAnim.Length) { CurrFrame = 0; }
            CharacterRenderer.sprite = CurrAnim[CurrFrame];
            AnimTime = 0;
        }
    }

    private void TeleportPlayer(int X, int Y)
    {
        transform.position = new Vector3(X, Y, -1);
        Cam.transform.position = new Vector3(transform.position.x, transform.position.y, Cam.transform.position.z);
        CurrAnim = Character.Idle_Down_Anim;
    }

    private IEnumerator MovePlayer(Vector3 direction, bool IsComand)
    {
        startPos = transform.position;
        targetPos = startPos + direction;

        if (targetPos.x > -1 && targetPos.x < Map.SolidTileMap.GetLength(1) &&
            targetPos.y > -1 && targetPos.y < Map.SolidTileMap.GetLength(0))
        {
            if (Map.SolidTileMap[(int)targetPos.y, (int)targetPos.x] == false)
            {
                isMoving = true;
                float elapsedTime = 0;

                if (direction == Vector3.up) { CurrAnim = Character.Walk_Up_Anim; }
                else if (direction == Vector3.left) { CurrAnim = Character.Walk_Left_Anim; }
                else if (direction == Vector3.down) { CurrAnim = Character.Walk_Down_Anim; }
                else if (direction == Vector3.right) { CurrAnim = Character.Walk_Right_Anim; }

                while (elapsedTime < MoveTime)
                {
                    transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / MoveTime));
                    Cam.transform.position = new Vector3(transform.position.x, transform.position.y, Cam.transform.position.z);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                transform.position = targetPos;
                Cam.transform.position = new Vector3(transform.position.x, transform.position.y, Cam.transform.position.z);

                if (direction == Vector3.up) { CurrAnim = Character.Idle_Up_Anim; }
                else if (direction == Vector3.left) { CurrAnim = Character.Idle_Left_Anim; }
                else if (direction == Vector3.down) { CurrAnim = Character.Idle_Down_Anim; }
                else if (direction == Vector3.right) { CurrAnim = Character.Idle_Right_Anim; }

                isMoving = false;

                if (IsComand) { IsSc = false; CurrSc++; }
                else if (Map.EventTileMap[(int)targetPos.y, (int)targetPos.x] == true)
                {
                    EventSc = Map.Tileset[Map.TileLayer[(int)targetPos.y, (int)targetPos.x]].EventScript;
                }
            }
            else
            {
                if (direction == Vector3.up) { CurrAnim = Character.Idle_Up_Anim; }
                else if (direction == Vector3.left) { CurrAnim = Character.Idle_Left_Anim; }
                else if (direction == Vector3.down) { CurrAnim = Character.Idle_Down_Anim; }
                else if (direction == Vector3.right) { CurrAnim = Character.Idle_Right_Anim; }
            }
        }
        else
        {
            if (direction == Vector3.up) { CurrAnim = Character.Idle_Up_Anim; }
            else if (direction == Vector3.left) { CurrAnim = Character.Idle_Left_Anim; }
            else if (direction == Vector3.down) { CurrAnim = Character.Idle_Down_Anim; }
            else if (direction == Vector3.right) { CurrAnim = Character.Idle_Right_Anim; }
        }
    }

    private void Command(string strg)
    {
        //Debug.Log(strg);
        
        if (strg[0] == '/')
        {
            if (strg[1] == 'T' && strg[2] == 'p')
            {
                bool XIsFound = false;
                string sX = "", sY = "";
                IsSc = true;

                for (int i = 4; i <= strg.Length; i++)
                {
                    if (i == strg.Length && XIsFound) 
                    { 
                        TeleportPlayer(Convert.ToInt32(sX), Convert.ToInt32(sY)); 
                        IsSc = false; 
                        CurrSc++; 
                        return; 
                    }

                    if (strg[i] != ' ' && !XIsFound) { sX += strg[i]; }
                    else if (!XIsFound) { XIsFound = true; i++; }

                    if (strg[i] != ' ' && XIsFound) { sY += strg[i]; }
                }
            }
            else if (strg[1] == 'M' && strg[2] == 'v' && !isMoving)
            {
                IsSc = true;

                if (strg[4] == 'U') { StartCoroutine(MovePlayer(Vector3.up, true)); }
                else if (strg[4] == 'D') { StartCoroutine(MovePlayer(Vector3.down, true)); }
                else if (strg[4] == 'L') { StartCoroutine(MovePlayer(Vector3.left, true)); }
                else if (strg[4] == 'R') { StartCoroutine(MovePlayer(Vector3.right, true)); }
            }
            else if (strg[1] == 'G' && strg[2] == 'r' && strg[3] == 'a' && strg[4] == 's' && strg[5] == 's')
            {
                int Rand = UnityEngine.Random.Range(0, 100);
                if (Rand <= 15)
                {
                    StartCoroutine(Battle());
                }
                else
                {
                    CurrSc++;
                }
            }
            else if (strg[1] == 'F' && strg[2] == 'l' && strg[3] == 'a' && strg[4] == 's' && strg[5] == 'h')
            {
                IsSc = true;

                if (strg[7] == 'i' && strg[8] == 'n')
                {
                    StartCoroutine(Flash(true));
                }
                else if (strg[7] == 'o' && strg[8] == 'u' && strg[9] == 't')
                {
                    StartCoroutine(Flash(false));
                }
            }
        }
    }

    private IEnumerator Flash(bool flashin)
    {
        TransitionMaterial.SetColor("_Color", Color.black);
        TransitionMaterial.SetFloat("_Cutoff", 1f);

        if (flashin)
        {
            TransitionMaterial.SetFloat("_Fade", 0f);
            while (TransitionMaterial.GetFloat("_Fade") < 1)
            {
                TransitionMaterial.SetFloat("_Fade", TransitionMaterial.GetFloat("_Fade") + (Time.deltaTime * 3.5f));
                yield return null;
            }
            CurrSc++; IsSc = false;
        }
        else
        {
            TransitionMaterial.SetFloat("_Fade", 1f);
            while (TransitionMaterial.GetFloat("_Fade") > 0)
            {
                TransitionMaterial.SetFloat("_Fade", TransitionMaterial.GetFloat("_Fade") - (Time.deltaTime * 3.5f));
                yield return null;
            }
            TransitionMaterial.SetFloat("_Cutoff", 0f);
            CurrSc++; IsSc = false;
        }
    }

    private IEnumerator Battle()
    {
        isMoving = true;

        TransitionMaterial.SetColor("_Color", Color.white);
        TransitionMaterial.SetFloat("_Cutoff", 1f);
        TransitionMaterial.SetFloat("_Fade", 0f);

        for (int i = 0; i < 2; i++)
        {
            while (TransitionMaterial.GetFloat("_Fade") <= 0.97f)
            {
                TransitionMaterial.SetFloat("_Fade", TransitionMaterial.GetFloat("_Fade") + (Time.deltaTime * 3.5f));
                yield return null;
            }
            TransitionMaterial.SetFloat("_Fade", 1f);

            while (TransitionMaterial.GetFloat("_Fade") >= 0.01f)
            {
                TransitionMaterial.SetFloat("_Fade", TransitionMaterial.GetFloat("_Fade") - (Time.deltaTime * 3.5f));
                yield return null;
            }
            TransitionMaterial.SetFloat("_Fade", 0f);
        }

        TransitionMaterial.SetColor("_Color", Color.black);
        TransitionMaterial.SetFloat("_Cutoff", 0f);
        TransitionMaterial.SetFloat("_Fade", 1f);

        while (TransitionMaterial.GetFloat("_Cutoff") < 1)
        {
            TransitionMaterial.SetFloat("_Cutoff", TransitionMaterial.GetFloat("_Cutoff") + (Time.deltaTime * 0.5f));
            yield return null;
        }
        TransitionMaterial.SetFloat("_Cutoff", 1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
    }
}
