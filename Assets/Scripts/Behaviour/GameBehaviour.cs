using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameBehaviour : MonoBehaviour
{
    public PlayerBehaviour CurrentPlayerTurn;
    public PlayerBehaviour Player;
    public PlayerBehaviour Opponent;

    public GameDisplay GameDis;
    public List<GameObject> HandObjects;
    public GameObject DeadHandObj;

    public SC_Deck RandomCardPool;

    public bool EnemyAI;
    public bool EnemyAIThinking;
    public bool EnemyCardAnim;
    public int TurnNo;

    public GameObject TopObj;
    public GameObject BottomObj;

    public GameObject GameOverScrn;
    public GameObject WinScrn;
    public GameObject LoseScrn;

    [HideInInspector] public CharacterBehaviour Selected;

    public ScreenShake ShakeScreen;
    public bool Trans;
    public bool Delay;
    public bool GameOver;

    void Awake()
    {
        Player.CharacterTape = new CharacterBehaviour[save.Instance.PartyCharacterData.Length];
        for (int i = 0; i < 5; i++)
        {
            if (i < save.Instance.PartyCharacterData.Length)
            {
                Player.CharacterTape[i] = Player.CharObj[i].AddComponent<CharacterBehaviour>();
                Player.CharacterTape[i].HandObject = HandObjects[i];
                Player.CharacterTape[i].Init(save.Instance.PartyCharacterData[i]);
                Player.CharacterTape[i].PlayerBehav = Player;
                Player.CharacterTape[i].GetComponent<CharacterDisplay>().SetBehaviour(Player.CharacterTape[i]);
            }
            else
            {
                Player.CharObj[i].gameObject.SetActive(false);
            }
        }
        Player.UpdateActive();

        int RandAmt = RandInc(save.Instance.EnemyList.MinSpawn, save.Instance.EnemyList.MaxSpawn);
        Opponent.CharacterTape = new CharacterBehaviour[RandAmt];
        for (int i = 0; i < 5; i++)
        {
            if (i < RandAmt)
            {
                Opponent.CharacterTape[i] = Opponent.CharObj[i].AddComponent<CharacterBehaviour>();
                Opponent.CharacterTape[i].HandObject = HandObjects[i + 5];

                int RandChance = UnityEngine.Random.Range(1, 101);
                SC_EnemyList.Enemy EnemySpawn = null;
                for (int j = 0; j < save.Instance.EnemyList.EnemyList.Length; j++)
                {
                    if (save.Instance.EnemyList.EnemyList[j].SpawnChance >= RandChance)
                    {
                        EnemySpawn = save.Instance.EnemyList.EnemyList[j];
                        break;
                    }
                    else
                    {
                        RandChance -= save.Instance.EnemyList.EnemyList[j].SpawnChance;
                    }
                }

                Opponent.CharacterTape[i].Init(EnemySpawn.scEnemy, RandInc(EnemySpawn.MinLevel, EnemySpawn.MaxLevel));
                
                Opponent.CharacterTape[i].PlayerBehav = Opponent;
                Opponent.CharacterTape[i].IsEnemy = true;
                Opponent.CharacterTape[i].GetComponent<CharacterDisplay>().SetBehaviour(Opponent.CharacterTape[i]);
            }
            else
            {
                Opponent.CharObj[i].gameObject.SetActive(false);
            }
        }
        Opponent.UpdateActive();
    }

    // Start is called before the first frame update
    void Start()
    {
        Select(Player.CharacterTape[0]);
        StartCoroutine(Transition());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver) { return; }

        // AI Turn
        if (EnemyAI && CurrentPlayerTurn == Opponent && !EnemyAIThinking)
        {
            StartCoroutine(EnemyTurnAI());
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIElement())
            {
                GameDis.SetCardDisplay(null);
            }
        }
    }

    public void EndTurn(bool IsAi)
    {
        if (GameOver || Delay || Trans) { return; }
        if ((!IsAi && CurrentPlayerTurn == Player) || (IsAi && CurrentPlayerTurn == Opponent))
        {
            AudioManager.Instance.PlaySFX("Passturn");
            // End of Turn Effects
            for (int i = 0; i < CurrentPlayerTurn.CharacterTape.Length; i++)
            {
                if (CurrentPlayerTurn.CharacterTape[i].Burn)
                {
                    CurrentPlayerTurn.CharacterTape[i].Popup("Burned", Color.red);
                    CurrentPlayerTurn.CharacterTape[i].Health -= (CurrentPlayerTurn.CharacterTape[i].MaxHealth / 16);
                    if (Random.Range(0, 2) == 0) { CurrentPlayerTurn.CharacterTape[i].Burn = false; }
                }

                if (CurrentPlayerTurn.CharacterTape[i].Freeze == 0)
                {
                    for (int freeze = 0; freeze < CurrentPlayerTurn.CharacterTape[i].HandCards.Count; freeze++)
                    {
                        CurrentPlayerTurn.CharacterTape[i].HandCards[freeze].GetComponent<CardBehaviour>().Frozen = false;
                        CurrentPlayerTurn.CharacterTape[i].HandCards[freeze].GetComponent<CardDisplay>().FrozenDisplay.SetActive(false);
                    }
                }

                CurrentPlayerTurn.CharacterTape[i].Shock = 0;
                for (int shock = 0; shock < CurrentPlayerTurn.CharacterTape[i].ShockMana.Length; shock++)
                {
                    CurrentPlayerTurn.CharacterTape[i].ShockMana[shock] = false;
                }

                CurrentPlayerTurn.CharacterTape[i].CritChance = 1;
                CurrentPlayerTurn.CharacterTape[i].Trip = false;
                CurrentPlayerTurn.CharacterTape[i].Claw = 0;
            }

            TurnNo++;
            if (CurrentPlayerTurn == Player)
            {
                if (CurrentPlayerTurn.LoseATurn)
                {
                    CurrentPlayerTurn.LoseATurn = false;
                }
                else
                {
                    GameDis.ButtonPrint.text = "Opponent's Turn";
                    CurrentPlayerTurn = Opponent;
                }
            }
            else if (CurrentPlayerTurn == Opponent)
            {
                if (CurrentPlayerTurn.LoseATurn)
                {
                    CurrentPlayerTurn.LoseATurn = false;
                }
                else
                {
                    GameDis.ButtonPrint.text = "End Turn";
                    CurrentPlayerTurn = Player;
                }
            }

            // Start of Turn Effects
            for (int i = 0; i < CurrentPlayerTurn.CharacterTape.Length; i++)
            {
                int freeze = 0, shock = 0;
                int TotalMana = CurrentPlayerTurn.CharacterTape[i].MaxBoth + CurrentPlayerTurn.CharacterTape[i].MaxStamina + CurrentPlayerTurn.CharacterTape[i].MaxMana;
                int TempBoth = CurrentPlayerTurn.CharacterTape[i].MaxBoth, TempStamina = CurrentPlayerTurn.CharacterTape[i].MaxStamina, TempMana = CurrentPlayerTurn.CharacterTape[i].MaxMana;

                if (CurrentPlayerTurn.CharacterTape[i].Freeze > CurrentPlayerTurn.CharacterTape[i].HandCards.Count) { CurrentPlayerTurn.CharacterTape[i].Freeze = CurrentPlayerTurn.CharacterTape[i].HandCards.Count; }
                while (freeze < CurrentPlayerTurn.CharacterTape[i].Freeze)
                {
                    CurrentPlayerTurn.CharacterTape[i].Popup("Frozen", Color.cyan);
                    int RandInt = Random.Range(0, CurrentPlayerTurn.CharacterTape[i].HandCards.Count);
                    if (CurrentPlayerTurn.CharacterTape[i].HandCards[RandInt].GetComponent<CardBehaviour>().Frozen == false)
                    {
                        CurrentPlayerTurn.CharacterTape[i].HandCards[RandInt].GetComponent<CardBehaviour>().Frozen = true;
                        CurrentPlayerTurn.CharacterTape[i].HandCards[RandInt].GetComponent<CardDisplay>().FrozenDisplay.SetActive(true);
                        freeze++;
                    }
                }
                CurrentPlayerTurn.CharacterTape[i].Freeze = 0;

                if (CurrentPlayerTurn.CharacterTape[i].Shock > 0) { CurrentPlayerTurn.CharacterTape[i].Popup("Shocked", Color.yellow); }
                if (CurrentPlayerTurn.CharacterTape[i].Shock > TotalMana) { CurrentPlayerTurn.CharacterTape[i].Shock = TotalMana; }
                while (shock < CurrentPlayerTurn.CharacterTape[i].Shock)
                {
                    int RandInt = Random.Range(0, TotalMana);
                    if (CurrentPlayerTurn.CharacterTape[i].ShockMana[RandInt] == false)
                    {
                        CurrentPlayerTurn.CharacterTape[i].ShockMana[RandInt] = true;
                        shock++;
                    }
                }
                for (shock = 0; shock < 5; shock++)
                {
                    if (CurrentPlayerTurn.CharacterTape[i].ShockMana[shock] == true)
                    {
                        if (shock < CurrentPlayerTurn.CharacterTape[i].MaxBoth) { TempBoth--; }
                        else if (shock < CurrentPlayerTurn.CharacterTape[i].MaxBoth + CurrentPlayerTurn.CharacterTape[i].MaxStamina) { TempStamina--; }
                        else { TempMana--; }
                    }
                }

                CurrentPlayerTurn.CharacterTape[i].Both = TempBoth;
                CurrentPlayerTurn.CharacterTape[i].Stamina = TempStamina;
                CurrentPlayerTurn.CharacterTape[i].Mana = TempMana;
                CurrentPlayerTurn.CharacterTape[i].Ward = false;

                if (CurrentPlayerTurn.CharacterTape[i].IsActive)
                {
                    CurrentPlayerTurn.CharacterTape[i].Draw(1);
                }
            }
            CurrentPlayerTurn.CardPlayed = false;
        }
    }

    public void Select(CharacterBehaviour _Selected)
    {
        Selected = _Selected;
        for (int i = 0; i < Player.CharacterTape.Length; i++)
        {
            if (Selected != Player.CharacterTape[i])
            {
                Player.CharacterTape[i].HandObject.transform.position = new Vector3(960, -265, 0);
            }
            else
            {
                Player.CharacterTape[i].HandObject.transform.position = new Vector3(960, 135, 0);
                DeadHandObj.SetActive(Player.CharacterTape[i].IsDead);
            }
        }
    }

    public IEnumerator EnemyTurnAI()
    {
        EnemyAIThinking = true;

        bool shouldBreak = false;
        int MaxActive = 3;

        if (MaxActive > Opponent.CharacterTape.Length) { MaxActive = Opponent.CharacterTape.Length; }
        for (int CharIdx = 0; CharIdx < MaxActive; CharIdx++)
        {
            if (Opponent.CharacterTape[CharIdx].Health > 0)
            {
                for (int CardCostIdx = Opponent.CharacterTape[CharIdx].Both + Opponent.CharacterTape[CharIdx].Stamina + Opponent.CharacterTape[CharIdx].Mana; CardCostIdx >= 0; CardCostIdx--)
                {
                    for (int CardIdx = 0; CardIdx < Opponent.CharacterTape[CharIdx].HandCards.Count; CardIdx++)
                    {
                        CardBehaviour Card = Opponent.CharacterTape[CharIdx].HandCards[CardIdx].GetComponent<CardBehaviour>();
                        if ((Card.CardCost == CardCostIdx || Card.CardCost == 0) && Card.Frozen == false)
                        {
                            CharacterBehaviour Target = Opponent.GetTarget((int)Card.Currentcard.CardTarget, Player);
                            if (Target == null)
                            {
                                shouldBreak = true;
                                break;
                            }

                            if (Opponent.CharacterTape[CharIdx].CanBePlayed(Card, true))
                            {
                                StartCoroutine(Card.PlayAnim(this, Target));
                                while (EnemyCardAnim || Delay)
                                {
                                    yield return null;
                                }
                            }
                        }
                    }
                    if (shouldBreak)
                        break;
                }
            }
            if (shouldBreak)
                break;
        }
        while (Delay)
        {
            yield return null;
        }
        EndTurn(true);
        EnemyAIThinking = false;
    }

    public IEnumerator Transition()
    {
        TopObj.SetActive(true);
        BottomObj.SetActive(true);

        Trans = true;
        while (true)
        {
            if (TopObj.transform.position.y >= 1350 && BottomObj.transform.position.y <= -270)
            {
                TopObj.transform.position = new Vector3(TopObj.transform.position.x, 1350, TopObj.transform.position.z);
                BottomObj.transform.position = new Vector3(BottomObj.transform.position.x, -270, BottomObj.transform.position.z);
                break;
            }
            if (TopObj.transform.position.y < 1350)
            {
                TopObj.transform.position = new Vector3(TopObj.transform.position.x, TopObj.transform.position.y + (Time.deltaTime * 225), TopObj.transform.position.z);
            }
            if (BottomObj.transform.position.y > -270)
            {
                BottomObj.transform.position = new Vector3(BottomObj.transform.position.x, BottomObj.transform.position.y - (Time.deltaTime * 225), BottomObj.transform.position.z);
            }
            yield return null;
        }
        
        for (int i = 0; i < Player.CharacterTape.Length; i++)
        {
            Player.CharacterTape[i].Draw(3);
        }
        for (int i = 0; i < Opponent.CharacterTape.Length; i++)
        {
            Opponent.CharacterTape[i].Draw(3);
        }

        Trans = false;
    }

    public bool CheckGameOver()
    {
        if (Player.CharacterTape.Length == 0) 
        {
            AudioManager.Instance.PlayMusic("Game Over");
            GameOverScrn.SetActive(true);
            LoseScrn.SetActive(true);

            return true;
        }

        if (Opponent.CharacterTape.Length == 0) 
        {
            AudioManager.Instance.PlayMusic("Victory");
            GameOverScrn.SetActive(true);
            WinScrn.SetActive(true);

            return true;
        }
        return false;
    }

    public void ContinueBtn()
    {
        save.Instance.BattleUpdate(this);
        save.Instance.SaveFile("battle");
        save.Instance.ChangeScene("RPGScene", "battle");
    }

    public void QuitBtn()
    {
        save.Instance.ChangeScene("MenuScene", "save");
    }

    public void LastSaveBtn()
    {
        save.Instance.ChangeScene("RPGScene", "save");
    }

    public IEnumerator GiveEXP(PlayerBehaviour player, CharacterBehaviour CharacterBehav)
    {
        Delay = true;

        int EXP = (CharacterBehav.Character.EXPGain + 20 * CharacterBehav.Level) / player.CharacterTape.Length;
        float elapsedTime = 0;
        float Speed = 1f / EXP;

        for (int j = 0; j < player.CharacterTape.Length; j++)
        {
            if (j < 3)
            {
                int i = 0;
                Selected = player.CharacterTape[j];
                while (i < EXP)
                {
                    if (elapsedTime >= Speed)
                    {
                        Selected.Exp++; i++;
                        elapsedTime = 0;
                        if (Selected.Exp >= Selected.MaxExp)
                        {
                            Selected.Exp -= Selected.MaxExp;
                            Selected.LevelUp();
                        }
                    }
                    else { elapsedTime += Time.deltaTime; yield return null; }
                }
            }
            else
            {
                player.CharacterTape[j].Exp += EXP;
                while (player.CharacterTape[j].Exp >= player.CharacterTape[j].MaxExp)
                {
                    player.CharacterTape[j].Exp -= player.CharacterTape[j].MaxExp;
                    player.CharacterTape[j].LevelUp();
                    yield return null;
                }
            }
        }
        Delay = false;
        Opponent.RotationBehav.UpdateDeadCharacters();
        CharacterBehav.gameObject.SetActive(false);
    }

    public PlayerBehaviour GetOpponent(PlayerBehaviour PlayerBehav)
    {
        PlayerBehaviour OppBehav = Opponent;
        if (PlayerBehav == Opponent)
        {
            OppBehav = Player;
        }
        return OppBehav;
    }

    public int RandInc(int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            if (Random.Range(0,4) == 0)
            {
                return i;
            }
        }
        return end;
    }

    private bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Card");

        foreach (GameObject element in gameObjects)
        {
            CanvasGroup canvasGroup = element.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                continue;

            if (canvasGroup.blocksRaycasts && canvasGroup.interactable && canvasGroup.alpha > 0)
            {
                RectTransform rectTransform = element.GetComponent<RectTransform>();
                Vector3 originalScale = rectTransform.localScale;

                // Temporarily set the scale to 1 for tap detection
                rectTransform.localScale = Vector3.one;

                if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, eventData.pressEventCamera))
                {
                    // Restore the original scale after tap detection
                    rectTransform.localScale = originalScale;
                    return true;
                }

                // Restore the original scale after tap detection
                rectTransform.localScale = originalScale;
            }
        }

        return false;
    }
}
