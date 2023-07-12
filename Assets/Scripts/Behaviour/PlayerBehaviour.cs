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
        for (int i = 0; i < CharacterTape.Length; i++)
        {
            if (i < 3)
            {
                CharacterTape[i].IsActive = true;
            }
            else
            {
                CharacterTape[i].IsActive = false;
            }
        }
    }

    public CharacterBehaviour GetTarget(int TargetType, PlayerBehaviour Opponent)
    {
        //Debug.Log(TargetType);

        int ActiveCharacter = 3;
        if (ActiveCharacter > Opponent.CharacterTape.Length) { ActiveCharacter = Opponent.CharacterTape.Length; }
        if (ActiveCharacter == 0) { return null; }

        if (TargetType == 1 /*Card.Currentcard.Target.Enemy*/)
        {
            int LowestHealthChar = 0, LowestChar = -1;

            for (int i = 0; i < ActiveCharacter; i++)
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
            int LowestHealthChar = 0, LowestChar = -1;

            for (int i = 0; i < ActiveCharacter; i++)
            {
                if (CharacterTape[i] != null && ((LowestHealthChar == 0 && CharacterTape[i].Health > 0) ||
                    (CharacterTape[i].Health < LowestHealthChar && CharacterTape[i].Health > 0)))
                {
                    LowestHealthChar = CharacterTape[i].Health;
                    LowestChar = i;
                }
            }
            if (LowestChar == -1) { return null; }
            return CharacterTape[LowestChar];
        }
        else if (TargetType == 3 /*Card.Currentcard.Target.Centre*/)
        {

        }
        else if (TargetType == 4 /*Card.Currentcard.Target.RandomEnemy*/)
        {
            return Opponent.CharacterTape[Random.Range(0, ActiveCharacter)];
        }
        return GetTarget(1, Opponent);
    }

    public CharacterBehaviour GetOrigCharacterTape(int i)
    {
        for (int j = 0; j < CharacterTape.Length; j++)
        {
            if (CharacterTape[j].OrigPos == i)
            {
                return CharacterTape[j];
            }
        }
        return null;
    }
}
