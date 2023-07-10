using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Camera Cam;
    [SerializeField] private save SaveData;
    [SerializeField] private MapBehaviour Map;
    [SerializeField] private SpriteRenderer CharacterRenderer;
    [SerializeField] private Material TransitionMaterial;
    public SC_Character Character;

    [SerializeField] private GameObject[] MenuUI;

    private bool isMoving;
    private Vector3 startPos, targetPos;
    private float MoveTime = 0.25f;

    private Vector3 CurrDirection;
    public Sprite[] CurrAnim;
    private int CurrFrame;
    private float AnimTime = 1;

    private string[] EventSc;
    private int CurrSc;
    private bool IsSc;
    private bool UI;

    // Start is called before the first frame update
    void Start()
    {
        CurrAnim = Character.Idle_Down_Anim;
        CurrDirection = Vector3.down;
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
            if (Input.GetKey(KeyCode.W) && !isMoving && EventSc == null && !UI)
                StartCoroutine(MovePlayer(Vector3.up, false));

            if (Input.GetKey(KeyCode.A) && !isMoving && EventSc == null && !UI)
                StartCoroutine(MovePlayer(Vector3.left, false));

            if (Input.GetKey(KeyCode.S) && !isMoving && EventSc == null && !UI)
                StartCoroutine(MovePlayer(Vector3.down, false));

            if (Input.GetKey(KeyCode.D) && !isMoving && EventSc == null && !UI)
                StartCoroutine(MovePlayer(Vector3.right, false));

            if (Input.GetKey(KeyCode.Space) && !isMoving && EventSc == null && !UI)
                Interact();

            if (Input.GetKeyDown(KeyCode.X) && !isMoving && EventSc == null)
            {
                UI = !UI;
                MenuUI[0].SetActive(!MenuUI[0].activeSelf);
                
                if (!MenuUI[0].activeSelf)
                {
                    CloseMenu();
                }
            }

            if (Input.GetKeyDown(KeyCode.Z) && !isMoving && EventSc == null)
            {
                SaveData.SaveFile();
            }

            if (Input.GetKeyDown(KeyCode.P) && !isMoving && EventSc == null)
            {
                SaveData.CreateNewCharacterData(Character, 1);
            }
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

    public void Continue() { IsSc = false; CurrSc++; }

    public void CloseMenu()
    {
        UI = false;
        for (int i = 0; i < MenuUI.Length; i++)
        {
            MenuUI[i].SetActive(false);
        }
    }

    public void TeleportPlayer(int X, int Y)
    {
        transform.position = new Vector3(X, Y, -1);
        Cam.transform.position = new Vector3(transform.position.x, transform.position.y, Cam.transform.position.z);

        SaveData.xPos = X;
        SaveData.yPos = Y;
    }

    private IEnumerator MovePlayer(Vector3 direction, bool IsComand)
    {
        startPos = transform.position;
        targetPos = startPos + direction;
        CurrDirection = direction;

        if (targetPos.x > -1 && targetPos.x < Map.SolidTileMap.GetLength(1) &&
            targetPos.y > -1 && targetPos.y < Map.SolidTileMap.GetLength(0))
        {
            if (Map.SolidEventTileMap[(int)targetPos.y, (int)targetPos.x] == true && !IsComand)
            {
                if (direction == Vector3.up) { CurrAnim = Character.Idle_Up_Anim; }
                else if (direction == Vector3.left) { CurrAnim = Character.Idle_Left_Anim; }
                else if (direction == Vector3.down) { CurrAnim = Character.Idle_Down_Anim; }
                else if (direction == Vector3.right) { CurrAnim = Character.Idle_Right_Anim; }

                EventSc = Map.Tileset[Map.TileLayer[(int)targetPos.y, (int)targetPos.x]].Script;
            }
            else if (Map.SolidTileMap[(int)targetPos.y, (int)targetPos.x] == false)
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

                SaveData.xPos = (int)targetPos.x;
                SaveData.yPos = (int)targetPos.y;

                if (direction == Vector3.up) { CurrAnim = Character.Idle_Up_Anim; }
                else if (direction == Vector3.left) { CurrAnim = Character.Idle_Left_Anim; }
                else if (direction == Vector3.down) { CurrAnim = Character.Idle_Down_Anim; }
                else if (direction == Vector3.right) { CurrAnim = Character.Idle_Right_Anim; }

                isMoving = false;

                if (IsComand) { IsSc = false; CurrSc++; }
                else if (Map.EventTileMap[(int)targetPos.y, (int)targetPos.x] == true)
                {
                    EventSc = Map.Tileset[Map.TileLayer[(int)targetPos.y, (int)targetPos.x]].Script;
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

    private void Interact()
    {
        targetPos = transform.position + CurrDirection;

        if (targetPos.x > -1 && targetPos.x < Map.InteractableTileMap.GetLength(1) &&
            targetPos.y > -1 && targetPos.y < Map.InteractableTileMap.GetLength(0))
        {

        }
    }

    private void Command(string strg)
    {
        //Debug.Log(strg);
        
        if (strg[0] != '/') { return; }
        IsSc = true;

        if (strg[1] == 'T' && strg[2] == 'p')
        {
            bool XIsFound = false;
            string sX = "", sY = "";

            for (int i = 4; i <= strg.Length; i++)
            {
                if (i == strg.Length && XIsFound)
                {
                    int x, y;
                    
                    if (sX == "X")
                    {
                        x = (int)transform.position.x;
                    }
                    else
                    {
                        x = Convert.ToInt32(sX);
                    }

                    if (sY == "Y")
                    {
                        y = (int)transform.position.y;
                    }
                    else
                    {
                        y = Convert.ToInt32(sY);
                    }

                    TeleportPlayer(x, y);
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
            if (strg[4] == 'U') { StartCoroutine(MovePlayer(Vector3.up, true)); }
            else if (strg[4] == 'D') { StartCoroutine(MovePlayer(Vector3.down, true)); }
            else if (strg[4] == 'L') { StartCoroutine(MovePlayer(Vector3.left, true)); }
            else if (strg[4] == 'R') { StartCoroutine(MovePlayer(Vector3.right, true)); }
        }
        else if (strg[1] == 'F' && strg[2] == 'c')
        {
            if (strg[4] == 'U') { CurrAnim = Character.Idle_Up_Anim; }
            else if (strg[4] == 'D') { CurrAnim = Character.Idle_Down_Anim; }
            else if (strg[4] == 'L') { CurrAnim = Character.Idle_Left_Anim; }
            else if (strg[4] == 'R') { CurrAnim = Character.Idle_Right_Anim; }
            IsSc = false; CurrSc++;
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
                IsSc = false; CurrSc++;
            }
        }
        else if (strg[1] == 'F' && strg[2] == 'l' && strg[3] == 'a' && strg[4] == 's' && strg[5] == 'h')
        {
            if (strg[7] == 'i' && strg[8] == 'n')
            {
                StartCoroutine(Flash(true));
            }
            else if (strg[7] == 'o' && strg[8] == 'u' && strg[9] == 't')
            {
                StartCoroutine(Flash(false));
            }
        }

        else if (strg[1] == 'O' && strg[2] == 'b' && strg[3] == 'j')
        {
            bool XIsFound = false;
            string sX = "", sY = "";

            if (strg[5] == 'A' && strg[6] == 'n' && strg[7] == 'i' && strg[8] == 'm')
            {
                if (strg[9] == 'S' && strg[10] == 'e' && strg[11] == 't')
                {
                    bool FirstOrLast = true;

                    if (strg[13] == 'S' && strg[14] == 'r' && strg[15] == 't')
                    {
                        FirstOrLast = true;
                    }
                    else if (strg[13] == 'E' && strg[14] == 'n' && strg[15] == 'd')
                    {
                        FirstOrLast = false;
                    }
                    else
                    {
                        Debug.Log("/Obj Anim Error");
                        return;
                    }

                    for (int i = 17; i <= strg.Length; i++)
                    {
                        if (i == strg.Length && XIsFound)
                        {
                            GameObject.Find(sX + "x" + sY + "y" + "Top").GetComponent<TileAnim>().SetCurrFrame(FirstOrLast);
                            IsSc = false; CurrSc++;
                            return;
                        }

                        if (strg[i] != ' ' && !XIsFound) { sX += strg[i]; }
                        else if (!XIsFound) { XIsFound = true; i++; }

                        if (strg[i] != ' ' && XIsFound) { sY += strg[i]; }
                    }
                }
                else
                {
                    for (int i = 10; i <= strg.Length; i++)
                    {
                        if (i == strg.Length && XIsFound)
                        {
                            GameObject.Find(sX + "x" + sY + "y" + "Top").GetComponent<TileAnim>().Anim = true;
                            return;
                        }

                        if (strg[i] != ' ' && !XIsFound) { sX += strg[i]; }
                        else if (!XIsFound) { XIsFound = true; i++; }

                        if (strg[i] != ' ' && XIsFound) { sY += strg[i]; }
                    }
                }
            }
            else if (strg[5] == 'C' && strg[6] == 'h' && strg[7] == 'a' && strg[8] == 'n' && strg[9] == 'g' && strg[10] == 'e' && strg[11] == 'M' && strg[12] == 'a' && strg[13] == 'p')
            {
                string TileNo = "";

                for (int i = 15; i <= strg.Length; i++)
                {
                    if (i == strg.Length)
                    {
                        Map.ChangeMap(Convert.ToInt32(TileNo));
                        IsSc = false;
                        CurrSc++;
                        return;
                    }
                    else if (strg[i] != ' ') { TileNo += strg[i]; }
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
