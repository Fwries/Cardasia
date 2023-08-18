using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject[] CharObj;
    public CharacterBehaviour[] CharacterTape;
    public CharacterBehaviour[] DeadTape;
    public RotationBehaviour RotationBehav;

    public bool LoseATurn;
    public int AmtDead;

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

    public void Init()
    {
        RotationBehav.CentrePos = CharObj[0].transform.position;
        RotationBehav.LeftPos = CharObj[1].transform.position;
        RotationBehav.RightPos = CharObj[2].transform.position;
        RotationBehav.LeftBackPos = CharObj[3].transform.position;
        RotationBehav.RightBackPos = CharObj[4].transform.position;
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

    public CharacterBehaviour GetTarget(CharacterBehaviour User, int TargetType, PlayerBehaviour Opponent)
    {
        int ActiveCharacter = 3;
        if (ActiveCharacter > Opponent.CharacterTape.Length) { ActiveCharacter = Opponent.CharacterTape.Length; }
        if (ActiveCharacter == 0) { return null; }

        if (TargetType == (int)SC_Card.Target.Agroo)
        {
            List<AgrooData> AgrooList = new List<AgrooData>();
            for (int i = 0; i < ActiveCharacter; i++)
            {
                for (int j = 0; j < User.AgrooList.Count; j++)
                {
                    if (Opponent.CharacterTape[i] == User.AgrooList[j].AgrooTarget)
                    {
                        AgrooList.Add(new AgrooData(User.AgrooList[j].AgrooTarget, User.AgrooList[j].Agroo));
                    }
                }
            }

            if (AgrooList.Count == 0) { return GetTarget(User, (int)SC_Card.Target.LowestEnemy, Opponent); }

            float HighestAgroo = AgrooList[0].Agroo;
            int HighestAgrooChar = 0, SameAgrooValue = 0;

            for (int i = 0; i < AgrooList.Count; i++)
            {
                if (AgrooList[i].Agroo > AgrooList[HighestAgrooChar].Agroo)
                {
                    HighestAgroo = AgrooList[i].Agroo;
                    HighestAgrooChar = i;
                }
                else if (AgrooList[i].Agroo == AgrooList[HighestAgrooChar].Agroo) { SameAgrooValue++; }
            }

            if (ActiveCharacter == SameAgrooValue) { return GetTarget(User, (int)SC_Card.Target.LowestEnemy, Opponent); }
            else { return Opponent.CharacterTape[HighestAgrooChar]; }
        }
        else if (TargetType == (int)SC_Card.Target.Ally)
        {
            int LowestHealthChar = CharacterTape[0].Health, LowestChar = 0;

            for (int i = 0; i < ActiveCharacter; i++)
            {
                if (CharacterTape[i].Health < LowestHealthChar && CharacterTape[i].Health > 0)
                {
                    LowestHealthChar = CharacterTape[i].Health;
                    LowestChar = i;
                }
            }
            return CharacterTape[LowestChar];
        }
        else if (TargetType == (int)SC_Card.Target.Centre)
        {
            if (ActiveCharacter == 1) { return Opponent.CharacterTape[1]; }
            else { return Opponent.CharacterTape[0]; }
        }
        else if (TargetType == (int)SC_Card.Target.LowestEnemy)
        {
            int LowestHealthChar = Opponent.CharacterTape[0].Health, LowestChar = 0;
            for (int i = 0; i < ActiveCharacter; i++)
            {
                if (Opponent.CharacterTape[i].Health < LowestHealthChar && Opponent.CharacterTape[i].Health > 0)
                {
                    LowestHealthChar = Opponent.CharacterTape[i].Health;
                    LowestChar = i;
                }
            }
            return Opponent.CharacterTape[LowestChar];
        }
        else if (TargetType == (int)SC_Card.Target.RandomEnemy)
        {
            return Opponent.CharacterTape[Random.Range(0, ActiveCharacter)];
        }
        return GetTarget(User, (int)SC_Card.Target.RandomEnemy, Opponent);
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
        for (int k = 0; k < DeadTape.Length; k++)
        {
            if (DeadTape[k].OrigPos == i)
            {
                return DeadTape[k];
            }
        }
        return null;
    }
}
