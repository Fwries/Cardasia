using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    public PlayerBehaviour CurrentPlayerTurn;
    public PlayerBehaviour Player;
    public PlayerBehaviour Opponent;
    public save SaveFile;

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

    [HideInInspector] public CharacterBehaviour Selected;

    void Awake()
    {
        SaveFile.LoadFile();

        Player.CharacterTape = new CharacterBehaviour[SaveFile.PartyCharacterData.Length];
        for (int i = 0; i < 5; i++)
        {
            if (i < SaveFile.PartyCharacterData.Length)
            {
                Player.CharacterTape[i] = Player.CharObj[i].AddComponent<CharacterBehaviour>();
                Player.CharacterTape[i].HandObject = HandObjects[i];
                Player.CharacterTape[i].Init(SaveFile.PartyCharacterData[i]);
                Player.CharacterTape[i].PlayerBehav = Player;
                Player.CharacterTape[i].GetComponent<CharacterDisplay>().SetBehaviour(Player.CharacterTape[i]);
            }
            else
            {
                Player.CharObj[i].gameObject.SetActive(false);
            }
        }
        Player.UpdateActive();

        Opponent.CharacterTape = new CharacterBehaviour[5];
        for (int i = 0; i < 5; i++)
        {
            if (i < 5)
            {
                Opponent.CharacterTape[i] = Opponent.CharObj[i].AddComponent<CharacterBehaviour>();
                Opponent.CharacterTape[i].HandObject = HandObjects[i + 5];
                Opponent.CharacterTape[i].Init(SaveFile.TempChar);
                Opponent.CharacterTape[i].PlayerBehav = Opponent;
                Opponent.CharacterTape[i].IsEnemy = true;
                Opponent.CharacterTape[i].GetComponent<CharacterDisplay>().SetBehaviour(Opponent.CharacterTape[i]);
            }
            else
            {
                Opponent.CharObj[i].gameObject.SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Transition());

        for (int i = 0; i < Player.CharacterTape.Length; i++)
        {
            Player.CharacterTape[i].Draw(3);
        }
        for (int i = 0; i < Opponent.CharacterTape.Length; i++)
        {
            Opponent.CharacterTape[i].Draw(3);
        }

        Select(Player.CharacterTape[0]);
    }

    // Update is called once per frame
    void Update()
    {
        // AI Turn
        if (EnemyAI && CurrentPlayerTurn == Opponent && !EnemyAIThinking)
        {
            StartCoroutine(EnemyTurnAI());
        }
        //int AmtCheck = 0;
        //for (int i = 0; i < Player.CharacterTape.Length; i++)
        //{
        //    if (Player.CharacterTape[i].IsDead) { AmtCheck++; }
        //}
        //if (AmtCheck == 4) { UnityEngine.SceneManagement.SceneManager.LoadScene("RPGScene"); }

        //AmtCheck = 0;
        //for (int i = 0; i < Opponent.CharacterTape.Length; i++)
        //{
        //    if (Player.CharacterTape[i].IsDead) { AmtCheck++; }
        //}
        //if (AmtCheck == 4) { UnityEngine.SceneManagement.SceneManager.LoadScene("RPGScene"); }
    }

    public void EndTurn()
    {
        if (/*Condition to check whether its current player turn*/ true)
        {
            // End of Turn Effects
            for (int i = 0; i < CurrentPlayerTurn.CharacterTape.Length; i++)
            {
                if (CurrentPlayerTurn.CharacterTape[i].Burn)
                {
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
                    int RandInt = Random.Range(0, CurrentPlayerTurn.CharacterTape[i].HandCards.Count);
                    if (CurrentPlayerTurn.CharacterTape[i].HandCards[RandInt].GetComponent<CardBehaviour>().Frozen == false)
                    {
                        CurrentPlayerTurn.CharacterTape[i].HandCards[RandInt].GetComponent<CardBehaviour>().Frozen = true;
                        CurrentPlayerTurn.CharacterTape[i].HandCards[RandInt].GetComponent<CardDisplay>().FrozenDisplay.SetActive(true);
                        freeze++;
                    }
                }
                CurrentPlayerTurn.CharacterTape[i].Freeze = 0;

                if (CurrentPlayerTurn.CharacterTape[i].Shock > TotalMana) { CurrentPlayerTurn.CharacterTape[i].Shock = TotalMana; }
                while (shock < CurrentPlayerTurn.CharacterTape[i].Shock)
                {
                    int RandInt = Random.Range(0, TotalMana);
                    if (CurrentPlayerTurn.CharacterTape[i].ShockMana[TotalMana] == false)
                    {
                        CurrentPlayerTurn.CharacterTape[i].ShockMana[TotalMana] = true;
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
        for (int CharIdx = 0; CharIdx < 3; CharIdx++)
        {
            if (Opponent.CharacterTape[CharIdx].Health > 0)
            {
                for (int CardCostIdx = Opponent.CharacterTape[CharIdx].Both + Opponent.CharacterTape[CharIdx].Stamina + Opponent.CharacterTape[CharIdx].Mana; CardCostIdx >= 0; CardCostIdx--)
                {
                    for (int CardIdx = 0; CardIdx < Opponent.CharacterTape[CharIdx].HandCards.Count; CardIdx++)
                    {
                        CardBehaviour Card = Opponent.CharacterTape[CharIdx].HandCards[CardIdx].GetComponent<CardBehaviour>();
                        if (Card.CardCost == CardCostIdx && Card.Frozen == false)
                        {
                            CharacterBehaviour Target = null;
                            if (Card.Currentcard.DoesTarget)
                            {
                                Target = Opponent.GetTarget((int)Card.Currentcard.CardTarget, Player);
                            }

                            if (Opponent.CharacterTape[CharIdx].CanBePlayed(Card, true))
                            {
                                if (Target != null)
                                {
                                    StartCoroutine(Card.PlayAnim(this, Target));
                                    while (EnemyCardAnim) { yield return null; }
                                }
                                else if (!Card.Currentcard.DoesTarget)
                                {
                                    while (EnemyCardAnim) { yield return null; }
                                }
                            }
                        }
                    }
                }
            }
        }
        EndTurn();
        EnemyAIThinking = false;
    }

    public IEnumerator Transition()
    {
        while (true)
        {
            if (TopObj.transform.position.y >= 1350 && BottomObj.transform.position.y >= -270)
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
    }
}
