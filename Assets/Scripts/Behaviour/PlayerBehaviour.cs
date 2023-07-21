using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject[] CharObj;
    public CharacterBehaviour[] CharacterTape;
    public RotationBehaviour RotationBehav;

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
        int ActiveCharacter = 3;
        if (ActiveCharacter > Opponent.CharacterTape.Length) { ActiveCharacter = Opponent.CharacterTape.Length; }
        if (ActiveCharacter == 0) { return null; }

        if (TargetType == 1 /*Card.Currentcard.Target.Enemy*/)
        {
            int LowestHealthChar = Opponent.CharacterTape[0].Health, LowestChar = 0;

            for (int i = 0; i < 3; i++)
            {
                if (i >= Opponent.CharacterTape.Length) { break; }
                if (Opponent.CharacterTape[i].Health < LowestHealthChar && Opponent.CharacterTape[i].Health > 0)
                {
                    LowestHealthChar = Opponent.CharacterTape[i].Health;
                    LowestChar = i;
                }
            }
            return Opponent.CharacterTape[LowestChar];
        }
        else if (TargetType == 2 /*Card.Currentcard.Target.Ally*/)
        {
            int LowestHealthChar = CharacterTape[0].Health, LowestChar = 0;

            for (int i = 0; i < 3; i++)
            {
                if (i >= CharacterTape.Length) { break; }
                if (CharacterTape[i].Health < LowestHealthChar && CharacterTape[i].Health > 0)
                {
                    LowestHealthChar = CharacterTape[i].Health;
                    LowestChar = i;
                }
            }
            return CharacterTape[LowestChar];
        }
        else if (TargetType == 3 /*Card.Currentcard.Target.Centre*/)
        {

        }
        else if (TargetType == 4 /*Card.Currentcard.Target.RandomEnemy*/)
        {
            return Opponent.CharacterTape[Random.Range(0, ActiveCharacter)];
        }
        return GetTarget(4, Opponent);
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
