using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public List<CharacterBehaviour> Character;
    public CharacterBehaviour[] ActiveCharacter = { null, null, null};

    // Start is called before the first frame update
    void Start()
    {
        if (Character.Count >= 3)
        {
            ActiveCharacter[1] = Character[0];
            ActiveCharacter[0] = Character[1];
            ActiveCharacter[2] = Character[2];
        }
        else if (Character.Count >= 2)
        {
            ActiveCharacter[1] = Character[0];
            ActiveCharacter[0] = Character[1];
        }
        else if (Character.Count >= 1)
        {
            ActiveCharacter[1] = Character[0];
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
