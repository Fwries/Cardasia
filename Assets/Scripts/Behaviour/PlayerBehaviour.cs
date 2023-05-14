using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public List<CharacterBehaviour> Character;
    public CharacterBehaviour[] ActiveCharacter = { null, null, null};
    public CharacterBehaviour BackCharacter;

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

    void UpdateActive()
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
}
