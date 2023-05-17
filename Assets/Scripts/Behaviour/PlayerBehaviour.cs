using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public List<CharacterBehaviour> Character;
    public CharacterBehaviour[] ActiveCharacter = { null, null, null};
    public CharacterBehaviour BackCharacter;

    [HideInInspector] public bool CardPlayed;
    public bool IsAI;

    // Start is called before the first frame update
    void Start()
    {
        switch (Character.Count)
        {
            case 1:
                ActiveCharacter[1] = Character[0];
                break;
            case 2:
                ActiveCharacter[0] = Character[1];
                ActiveCharacter[1] = Character[0];
                break;
            case 3:
                ActiveCharacter[2] = Character[2];
                ActiveCharacter[0] = Character[1];
                ActiveCharacter[1] = Character[0];
                break;
            case 4:
                BackCharacter = Character[3];
                ActiveCharacter[2] = Character[2];
                ActiveCharacter[0] = Character[1];
                ActiveCharacter[1] = Character[0];
                break;
        }
        UpdateActive();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateActive()
    {
        for (int iC = 0; iC < Character.Count; iC++)
        {
            for (int iA = 0; iA < ActiveCharacter.Length; iA++)
            {
                if (Character[iC] == ActiveCharacter[iA])
                {
                    Character[iC].IsActive = true;
                    break;
                }
                else
                {
                    Character[iC].IsActive = false;
                }
            }
        }
    }

    public CharacterBehaviour GetTarget(int TargetType, PlayerBehaviour Opponent)
    {
        if (TargetType == 1 /*Card.Currentcard.Target.Enemy*/)
        {
            int LowestHealthChar = 0, LowestChar = -1;
            for (int i = 0; i < 3; i++)
            {
                if (Opponent.ActiveCharacter[i] != null && ((LowestHealthChar == 0 && Opponent.ActiveCharacter[i].Health > 0) ||
                    (Opponent.ActiveCharacter[i].Health < LowestHealthChar && Opponent.ActiveCharacter[i].Health > 0)))
                {
                    LowestHealthChar = Opponent.ActiveCharacter[i].Health;
                    LowestChar = i;
                }
            }
            if (LowestChar == -1) { return null; }
            return Opponent.ActiveCharacter[LowestChar];
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
                CharacterBehaviour Target = Opponent.ActiveCharacter[Random.Range(0, 3)];
                if (Target != null) { return Target; }
            }
        }
        return null;
    }
}
