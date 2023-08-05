using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Camera Cam;
    [SerializeField] private MapBehaviour Map;
    [SerializeField] private DialogueBehaviour DialogueBehav;
    [SerializeField] private SpriteRenderer CharacterRenderer;
    [SerializeField] private Material TransitionMaterial;
    [SerializeField] private PartyCharacterBehaviour PartyUIBehav;
    [SerializeField] private SettingsBehaviour SettingsBehav;
    public SC_Character Character;

    [SerializeField] private GameObject[] MenuUI;

    private bool isMoving;
    private Vector3 startPos, targetPos;
    private float MoveTime = 0.22f;

    public Sprite[] CurrAnim;
    private int CurrFrame;
    private float AnimTime = 1;

    private string[] EventSc;
    private int CurrSc;
    private bool IsSc;
    private bool UI;
    private bool Delay;
    private bool Init;

    // Start is called before the first frame update
    void Start()
    {
        TransitionMaterial.SetFloat("_Cutoff", 0f);
        TransitionMaterial.SetFloat("_Fade", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Init)
        {
            if (save.Instance.nameStr.Length > 0)
            {
                Map.ChangeMap(save.Instance.Map);
                TeleportPlayer(save.Instance.xPos, save.Instance.yPos);

                if (save.Instance.PartyCharacterData.Length > 0)
                {
                    CurrAnim = save.Instance.PartyCharacterData[0].GetCurrAnim();
                }
                else
                {
                    CurrAnim = Character.Idle_Down_Anim;
                    save.Instance.CurrDirection = Vector3.down;
                }
                Init = true;
            }
            return;
        }

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

            if (Input.GetKey(KeyCode.Space) && !isMoving && EventSc == null && !UI && !Delay)
                Interact();

            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X))&& !isMoving && EventSc == null)
            {
                UI = !UI;
                MenuUI[0].SetActive(!MenuUI[0].activeSelf);
                
                if (!MenuUI[0].activeSelf)
                {
                    CloseMenu();
                }
                else
                {
                    AudioManager.Instance.PlaySFX("MenuIn");
                }
            }

            //if (Input.GetKeyDown(KeyCode.P) && !isMoving && EventSc == null)
            //{
            //    save.Instance.CreateNewCharacterData(Character, 1);
            //}
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
        SettingsBehav.ResetSettings();
        PartyUIBehav.SaveAll();

        for (int i = 0; i < MenuUI.Length; i++)
        {
            MenuUI[i].SetActive(false);
        }
        AudioManager.Instance.PlaySFX("MenuOut");
    }

    public void TeleportPlayer(int X, int Y)
    {
        transform.position = new Vector3(X, Y, -1);
        Cam.transform.position = new Vector3(transform.position.x, transform.position.y, Cam.transform.position.z);

        save.Instance.xPos = X;
        save.Instance.yPos = Y;
    }

    private IEnumerator MovePlayer(Vector3 direction, bool IsComand)
    {
        startPos = transform.position;
        targetPos = startPos + direction;
        save.Instance.CurrDirection = direction;

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

                save.Instance.xPos = (int)targetPos.x;
                save.Instance.yPos = (int)targetPos.y;

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
        targetPos = transform.position + save.Instance.CurrDirection;

        if (targetPos.x > -1 && targetPos.x < Map.InteractableTileMap.GetLength(1) &&
            targetPos.y > -1 && targetPos.y < Map.InteractableTileMap.GetLength(0))
        {
            if (!Map.Tileset[Map.TileLayer[(int)targetPos.y, (int)targetPos.x]].Interactable) { return; }
            EventSc = Map.Tileset[Map.TileLayer[(int)targetPos.y, (int)targetPos.x]].Script;
        }
    }

    private void Command(string strg)
    {
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
                    IsSc = false; CurrSc++;
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
        else if (strg[1] == 'D' && strg[2] == 'L')
        {
            if (strg[3] == 'E' && strg[4] == 'n' && strg[5] == 'd')
            {
                DialogueBehav.Reset();
                StartCoroutine(Delayfor(0.25f));
                DialogueBehav.gameObject.SetActive(false);
                IsSc = false; CurrSc++;
            }
            else if (strg[3] == 'O' && strg[4] == 'p' && strg[5] == 't' && strg[6] == 'a' && strg[7] == 'i' && strg[8] == 'n')
            {
                bool XIsFound = false;
                string sX = "", sY = "";

                for (int i = 10; i <= strg.Length; i++)
                {
                    if (i == strg.Length && XIsFound)
                    {
                        if (!DialogueBehav.gameObject.activeSelf) { DialogueBehav.gameObject.SetActive(true); }

                        SC_Tile Tile = Map.GetTile(Convert.ToInt32(sX), Convert.ToInt32(sY));
                        DialogueBehav.DialogueString = save.Instance.Obtain(Tile.ObtainList);
                        StartCoroutine(DialogueBehav.StartDialogue(true));
                        return;
                    }

                    if (strg[i] != ' ' && !XIsFound) { sX += strg[i]; }
                    else if (!XIsFound) { XIsFound = true; i++; }

                    if (strg[i] != ' ' && XIsFound) { sY += strg[i]; }
                }
            }
            else
            {
                if (!DialogueBehav.gameObject.activeSelf) { DialogueBehav.gameObject.SetActive(true); }

                string Dialogue = "";
                for (int i = 4; i < strg.Length; i++)
                {
                    Dialogue += strg[i];
                }
                DialogueBehav.Reset();
                DialogueBehav.DialogueString[0] = Dialogue;
                StartCoroutine(DialogueBehav.StartDialogue(false));
            }
        }
        else if (strg[1] == 'F' && strg[2] == 'c')
        {
            if (strg[4] == 'U') { CurrAnim = Character.Idle_Up_Anim; }
            else if (strg[4] == 'D') { CurrAnim = Character.Idle_Down_Anim; }
            else if (strg[4] == 'L') { CurrAnim = Character.Idle_Left_Anim; }
            else if (strg[4] == 'R') { CurrAnim = Character.Idle_Right_Anim; }
            IsSc = false; CurrSc++;
        }
        else if (strg[1] == 'H' && strg[2] == 'e' && strg[3] == 'a' && strg[4] == 'l')
        {
            PlaySFX("Heal");
            StartCoroutine(Heal());
        }
        else if (strg[1] == 'G' && strg[2] == 'r' && strg[3] == 'a' && strg[4] == 's' && strg[5] == 's')
        {
            string Chance = "";
            for (int i = 7; i < strg.Length; i++)
            {
                Chance += strg[i];
            }

            if (UnityEngine.Random.Range(0, 101) <= Convert.ToInt32(Chance))
            {
                StartCoroutine(Battle((int)transform.position.x, (int)transform.position.y));
                AudioManager.Instance.PlayMusic("Danger");
            }
            else
            {
                IsSc = false; CurrSc++;
            }
        }
        else if (strg[1] == 'B' && strg[2] == 'a' && strg[3] == 't' && strg[4] == 't' && strg[5] == 'l' && strg[6] == 'e')
        {
            bool XIsFound = false;
            string sX = "", sY = "";

            for (int i = 8; i <= strg.Length; i++)
            {
                if (i == strg.Length && XIsFound)
                {
                    int x = Convert.ToInt32(sX);
                    int y = Convert.ToInt32(sY);

                    StartCoroutine(Battle(x, y));
                    save.Instance.CantRun = true;
                    AudioManager.Instance.PlayMusic("Danger");
                    return;
                }

                if (strg[i] != ' ' && !XIsFound) { sX += strg[i]; }
                else if (!XIsFound) { XIsFound = true; i++; }

                if (strg[i] != ' ' && XIsFound) { sY += strg[i]; }
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

        else if (strg[1] == 'C' && strg[2] == 'h' && strg[3] == 'a' && strg[4] == 'n' && strg[5] == 'g' && strg[6] == 'e' && strg[7] == 'M' && strg[8] == 'a' && strg[9] == 'p')
        {
            string TileNo = "";

            for (int i = 11; i <= strg.Length; i++)
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
                            GameObject.Find(sX + "x" + sY + "yTop").GetComponent<TileAnim>().SetCurrFrame(FirstOrLast);
                            IsSc = false; CurrSc++;
                            return;
                        }

                        if (strg[i] != ' ' && !XIsFound) { sX += strg[i]; }
                        else if (!XIsFound) { XIsFound = true; i++; }

                        if (strg[i] != ' ' && XIsFound) { sY += strg[i]; }
                    }
                }
                else if (strg[9] == 'A' && strg[10] == 'l' && strg[11] == 'l')
                {
                    string index = "";
                    for (int i = 12; i <= strg.Length; i++)
                    {
                        if (i == strg.Length)
                        {
                            Map.AnimAll(Convert.ToInt32(index));
                            return;
                        }
                        index += strg[i];
                    }
                }
                else
                {
                    for (int i = 10; i <= strg.Length; i++)
                    {
                        if (i == strg.Length && XIsFound)
                        {
                            GameObject.Find(sX + "x" + sY + "yTop").GetComponent<TileAnim>().Anim = true;
                            return;
                        }

                        if (strg[i] != ' ' && !XIsFound) { sX += strg[i]; }
                        else if (!XIsFound) { XIsFound = true; i++; }

                        if (strg[i] != ' ' && XIsFound) { sY += strg[i]; }
                    }
                }
            }
            else if (strg[5] == 'C' && strg[6] == 'h' && strg[7] == 'a' && strg[8] == 'n' && strg[9] == 'g' && strg[10] == 'e' && strg[11] == 'S' && strg[12] == 't' && strg[13] == 'a' && strg[14] == 't' && strg[15] == 'e')
            {
                if (strg[16] == 'A' && strg[17] == 'l' && strg[18] == 'l')
                {
                    for (int i = 20; i <= strg.Length; i++)
                    {
                        if (i == strg.Length && XIsFound)
                        {
                            Map.ChangeAllTileState(Convert.ToInt32(sX), Convert.ToInt32(sY));
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
                    for (int i = 17; i <= strg.Length; i++)
                    {
                        if (i == strg.Length && XIsFound)
                        {
                            Map.ChangeTileState(Convert.ToInt32(sX), Convert.ToInt32(sY));
                            IsSc = false; CurrSc++;
                            return;
                        }

                        if (strg[i] != ' ' && !XIsFound) { sX += strg[i]; }
                        else if (!XIsFound) { XIsFound = true; i++; }

                        if (strg[i] != ' ' && XIsFound) { sY += strg[i]; }
                    }
                }
            }
        }
        else if (strg[1] == 'P' && strg[2] == 'l' && strg[3] == 'a' && strg[4] == 'y' && strg[5] == 'S' && strg[6] == 'o' && strg[7] == 'u' && strg[8] == 'n' && strg[9] == 'd')
        {
            string Audio = "";
            for (int i = 11; i < strg.Length; i++)
            {
                Audio += strg[i];
            }
            AudioManager.Instance.PlaySFX(Audio);
            IsSc = false; CurrSc++;
        }

        else { Debug.Log("Command not found: " + strg); IsSc = false; CurrSc++; }
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

    private IEnumerator Battle(int x, int y)
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
        save.Instance.EnemyList = Map.Tileset[Map.TileLayer[y, x]].EnemyList;
        
        save.Instance.SaveFile("battle");
        save.Instance.ChangeScene("BattleScene", "battle");
    }

    private IEnumerator Heal()
    {
        for (int i = 0; i < save.Instance.PartyCharacterData.Length; i++)
        {
            save.Instance.PartyCharacterData[i].RestoreMaxHealth();
        }
        
        float elapsedTime = 0;
        while (true)
        {
            if (elapsedTime >= 0.4f)
            {
                elapsedTime = 0;
                break;
            }
            else { elapsedTime += Time.deltaTime; yield return null; }
        }

        IsSc = false; CurrSc++;
    }

    private IEnumerator Delayfor(float DelayTime)
    {
        float elapsedTime = 0;
        Delay = true;
        while (true)
        {
            if (elapsedTime >= DelayTime)
            {
                Delay = false;
                elapsedTime = 0;
                break;
            }
            else { elapsedTime += Time.deltaTime; yield return null; }
        }
    }

    public void SaveFile(string SaveFileName)
    {
        Debug.Log("Saved");
        save.Instance.SaveFile(SaveFileName);
    }

    public void ChangeScene(string SceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
        AudioManager.Instance.musicSource.Stop();
    }

    public void PlaySFX(string SoundName)
    {
        AudioManager.Instance.PlaySFX(SoundName);
    }
}
