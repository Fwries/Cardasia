using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [HideInInspector] public CharacterBehaviour Selected;

    void Awake()
    {
        for (int i = 0; i < Player.Character.Count; i++)
        {
            Player.Character[i].PlayerBehav = Player;
        }
        for (int i = 0; i < Opponent.Character.Count; i++)
        {
            Opponent.Character[i].PlayerBehav = Opponent;
            Opponent.Character[i].IsEnemy = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Player.Character.Count; i++)
        {
            Player.Character[i].HandObject = HandObjects[i];
            Player.Character[i].Draw(3);
        }
        for (int i = 0; i < Opponent.Character.Count; i++)
        {
            Opponent.Character[i].HandObject = HandObjects[i+4];
            Opponent.Character[i].Draw(3);
        }

        Select(Player.Character[0]);
    }

    // Update is called once per frame
    void Update()
    {
        // AI Turn
        if (EnemyAI && CurrentPlayerTurn == Opponent && !EnemyAIThinking)
        {
            StartCoroutine(EnemyTurnAI());
        }
    }

    public void EndTurn()
    {
        if (/*Condition to check whether its current player turn*/ true)
        {
            // End of Turn Effects
            for (int i = 0; i < CurrentPlayerTurn.Character.Count; i++)
            {
                if (CurrentPlayerTurn.Character[i].Burn)
                {
                    CurrentPlayerTurn.Character[i].Health -= (CurrentPlayerTurn.Character[i].MaxHealth / 16);
                    if (Random.Range(0, 2) == 0) { CurrentPlayerTurn.Character[i].Burn = false; }
                }

                if (CurrentPlayerTurn.Character[i].Freeze == 0)
                {
                    for (int freeze = 0; freeze < CurrentPlayerTurn.Character[i].HandCards.Count; freeze++)
                    {
                        CurrentPlayerTurn.Character[i].HandCards[freeze].GetComponent<CardBehaviour>().Frozen = false;
                        CurrentPlayerTurn.Character[i].HandCards[freeze].GetComponent<CardDisplay>().FrozenDisplay.SetActive(false);
                    }
                }

                CurrentPlayerTurn.Character[i].Shock = 0;
                for (int shock = 0; shock < CurrentPlayerTurn.Character[i].ShockMana.Length; shock++)
                {
                    CurrentPlayerTurn.Character[i].ShockMana[shock] = false;
                }

                CurrentPlayerTurn.Character[i].CritChance = 1;
                CurrentPlayerTurn.Character[i].Trip = false;
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
            for (int i = 0; i < CurrentPlayerTurn.Character.Count; i++)
            {
                int freeze = 0, shock = 0;
                int TotalMana = CurrentPlayerTurn.Character[i].MaxBoth + CurrentPlayerTurn.Character[i].MaxStamina + CurrentPlayerTurn.Character[i].MaxMana;
                int TempBoth = CurrentPlayerTurn.Character[i].MaxBoth, TempStamina = CurrentPlayerTurn.Character[i].MaxStamina, TempMana = CurrentPlayerTurn.Character[i].MaxMana;

                if (CurrentPlayerTurn.Character[i].Freeze > CurrentPlayerTurn.Character[i].HandCards.Count) { CurrentPlayerTurn.Character[i].Freeze = CurrentPlayerTurn.Character[i].HandCards.Count; }
                while (freeze < CurrentPlayerTurn.Character[i].Freeze)
                {
                    int RandInt = Random.Range(0, CurrentPlayerTurn.Character[i].HandCards.Count);
                    if (CurrentPlayerTurn.Character[i].HandCards[RandInt].GetComponent<CardBehaviour>().Frozen == false)
                    {
                        CurrentPlayerTurn.Character[i].HandCards[RandInt].GetComponent<CardBehaviour>().Frozen = true;
                        CurrentPlayerTurn.Character[i].HandCards[RandInt].GetComponent<CardDisplay>().FrozenDisplay.SetActive(true);
                        freeze++;
                    }
                }
                CurrentPlayerTurn.Character[i].Freeze = 0;

                if (CurrentPlayerTurn.Character[i].Shock > TotalMana) { CurrentPlayerTurn.Character[i].Shock = TotalMana; }
                while (shock < CurrentPlayerTurn.Character[i].Shock)
                {
                    int RandInt = Random.Range(0, TotalMana);
                    if (CurrentPlayerTurn.Character[i].ShockMana[TotalMana] == false)
                    {
                        CurrentPlayerTurn.Character[i].ShockMana[TotalMana] = true;
                        shock++;
                    }
                }
                for (shock = 0; shock < 5; shock++)
                {
                    if (CurrentPlayerTurn.Character[i].ShockMana[shock] == true)
                    {
                        if (shock < CurrentPlayerTurn.Character[i].MaxBoth) { TempBoth--; }
                        else if (shock < CurrentPlayerTurn.Character[i].MaxBoth + CurrentPlayerTurn.Character[i].MaxStamina) { TempStamina--; }
                        else { TempMana--; }
                    }
                }

                CurrentPlayerTurn.Character[i].Both = TempBoth;
                CurrentPlayerTurn.Character[i].Stamina = TempStamina;
                CurrentPlayerTurn.Character[i].Mana = TempMana;
                CurrentPlayerTurn.Character[i].Ward = false;

                if (CurrentPlayerTurn.Character[i].IsActive)
                {
                    CurrentPlayerTurn.Character[i].Draw(1);
                }
            }
            CurrentPlayerTurn.CardPlayed = false;
        }
    }

    public void Select(CharacterBehaviour _Selected)
    {
        Selected = _Selected;
        for (int i = 0; i < Player.Character.Count; i++)
        {
            if (Selected != Player.Character[i])
            {
                Player.Character[i].HandObject.transform.position = new Vector3(960, -265, 0);
            }
            else
            {
                Player.Character[i].HandObject.transform.position = new Vector3(960, 135, 0);
                DeadHandObj.SetActive(Player.Character[i].IsDead);
            }
        }
    }

    public IEnumerator EnemyTurnAI()
    {
        EnemyAIThinking = true;
        for (int CharIdx = 0; CharIdx < Opponent.ActiveCharacter.Length; CharIdx++)
        {
            if (Opponent.ActiveCharacter[CharIdx].Health > 0)
            {
                for (int CardCostIdx = Opponent.ActiveCharacter[CharIdx].Both + Opponent.ActiveCharacter[CharIdx].Stamina + Opponent.ActiveCharacter[CharIdx].Mana; CardCostIdx >= 0; CardCostIdx--)
                {
                    for (int CardIdx = 0; CardIdx < Opponent.ActiveCharacter[CharIdx].HandCards.Count; CardIdx++)
                    {
                        CardBehaviour Card = Opponent.ActiveCharacter[CharIdx].HandCards[CardIdx].GetComponent<CardBehaviour>();
                        if (Card.CardCost == CardCostIdx && Card.Frozen == false)
                        {
                            CharacterBehaviour Target = null;
                            if (Card.Currentcard.DoesTarget)
                            {
                                Target = Opponent.GetTarget((int)Card.Currentcard.CardTarget, Player);
                            }

                            if (Opponent.ActiveCharacter[CharIdx].CanBePlayed(Card, true))
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
}
