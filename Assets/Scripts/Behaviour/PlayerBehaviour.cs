using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject[] CharObj;
    public CharacterBehaviour[] CharacterTape;

    public bool LoseATurn;

    public bool CardPlayed;
    public bool IsAI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateActive()
    {
        //for (int iC = 0; iC < Character.Count; iC++)
        //{
        //    for (int iA = 0; iA < ActiveCharacter.Length; iA++)
        //    {
        //        if (Character[iC] == ActiveCharacter[iA])
        //        {
        //            Character[iC].IsActive = true;
        //            break;
        //        }
        //        else
        //        {
        //            Character[iC].IsActive = false;
        //        }
        //    }
        //}
    }

    public CharacterBehaviour GetTarget(int TargetType, PlayerBehaviour Opponent)
    {
        if (TargetType == 1 /*Card.Currentcard.Target.Enemy*/)
        {
            int LowestHealthChar = 0, LowestChar = -1;
            for (int i = 0; i < 3; i++)
            {
                if (Opponent.CharacterTape[i] != null && ((LowestHealthChar == 0 && Opponent.CharacterTape[i].Health > 0) ||
                    (Opponent.CharacterTape[i].Health < LowestHealthChar && Opponent.CharacterTape[i].Health > 0)))
                {
                    LowestHealthChar = Opponent.CharacterTape[i].Health;
                    LowestChar = i;
                }
            }
            if (LowestChar == -1) { return null; }
            return Opponent.CharacterTape[LowestChar];
        }
        else if (TargetType == 2 /*Card.Currentcard.Target.Ally*/)
        {

        }
        else if (TargetType == 3 /*Card.Currentcard.Target.Centre*/)
        {

        }
        else if (TargetType == 4 /*Card.Currentcard.Target.RandomEnemy*/)
        {
            while (true)
            {
                CharacterBehaviour Target = Opponent.CharacterTape[Random.Range(0, 3)];
                if (Target != null) { return Target; }
            }
        }
        return null;
    }
}
