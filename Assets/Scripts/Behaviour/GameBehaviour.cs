using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    public PlayerBehaviour CurrentPlayerTurn;
    public PlayerBehaviour Player;
    public PlayerBehaviour Opponent;

    public bool EnemyAI;
    public int TurnNo;

    public CharacterBehaviour Selected;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void EndTurn()
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

                CurrentPlayerTurn.Character[i].Freeze = 0;
                for (int freeze = 0; freeze < CurrentPlayerTurn.Character[i].HandCards.Count; freeze++)
                {
                    CurrentPlayerTurn.Character[i].HandCards[freeze].GetComponent<CardBehaviour>().Frozen = false;
                }

                CurrentPlayerTurn.Character[i].Shock = 0;
                for (int shock = 0; shock < CurrentPlayerTurn.Character[i].ShockMana.Length; shock++)
                {
                    CurrentPlayerTurn.Character[i].ShockMana[shock] = false;
                }

                CurrentPlayerTurn.Character[i].Trip = false;
            }

            TurnNo++;
            if (CurrentPlayerTurn == Player)
            {
                //ButtonPrint.text = "Opponent's Turn";
                CurrentPlayerTurn = Opponent;
            }
            else if (CurrentPlayerTurn == Opponent)
            {
                //ButtonPrint.text = "End Turn";
                CurrentPlayerTurn = Player;
            }

            // Start of Turn Effects
            for (int i = 0; i < CurrentPlayerTurn.Character.Count; i++)
            {
                int freeze = 0, shock = 0;
                int TotalMana = CurrentPlayerTurn.Character[i].MaxBoth + CurrentPlayerTurn.Character[i].MaxStamina + CurrentPlayerTurn.Character[i].MaxMana;
                int TempBoth = CurrentPlayerTurn.Character[i].MaxBoth, TempStamina = CurrentPlayerTurn.Character[i].MaxStamina, TempMana = CurrentPlayerTurn.Character[i].MaxMana;

                if (CurrentPlayerTurn.Character[i].IsActive)
                {
                    if (CurrentPlayerTurn.Character[i].Freeze > CurrentPlayerTurn.Character[i].HandCards.Count) { CurrentPlayerTurn.Character[i].Freeze = CurrentPlayerTurn.Character[i].HandCards.Count; }
                    while (freeze < CurrentPlayerTurn.Character[i].Freeze)
                    {
                        int RandInt = Random.Range(0, CurrentPlayerTurn.Character[i].HandCards.Count);
                        if (CurrentPlayerTurn.Character[i].HandCards[RandInt].GetComponent<CardBehaviour>().Frozen == false)
                        {
                            CurrentPlayerTurn.Character[i].HandCards[RandInt].GetComponent<CardBehaviour>().Frozen = true;
                            freeze++;
                        }
                    }

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
                        if (CurrentPlayerTurn.Character[i].ShockMana[TotalMana] == true)
                        {
                            if (shock < CurrentPlayerTurn.Character[i].MaxBoth) { TempBoth--; }
                            else if (shock < CurrentPlayerTurn.Character[i].MaxBoth + CurrentPlayerTurn.Character[i].MaxStamina) { TempStamina--; }
                            else { TempMana--; }
                        }
                    }

                    CurrentPlayerTurn.Character[i].Both = TempBoth;
                    CurrentPlayerTurn.Character[i].Stamina = TempStamina;
                    CurrentPlayerTurn.Character[i].Mana = TempMana;

                    if (CurrentPlayerTurn.Character[i].IsActive)
                    {
                        CurrentPlayerTurn.Character[i].Draw(1);
                    }
                }
            }
        }
    }
}
