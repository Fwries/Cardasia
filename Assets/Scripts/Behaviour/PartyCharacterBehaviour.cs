using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCharacterBehaviour : MonoBehaviour
{
    public save saveFile;
    public RotationBehaviourUI RotationUI;

    public UICharacterDisplay[] UICharacter;
    public UICharacterDisplay[] RotationCharacter;
    public UICharacterDisplay[] CharacterTape;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterTape = new UICharacterDisplay[saveFile.PartyCharacterData.Length];

        for (int i = 0; i <= saveFile.PartyCharacterData.Length; i++)
        {
            if (i == saveFile.PartyCharacterData.Length)
            {
                for (int i2 = i; i2 < 5; i2++)
                {
                    UICharacter[i2].CharUI.SetActive(false);
                    UICharacter[i2].CharData = null;

                    RotationCharacter[i2].CharUI.SetActive(false);
                    RotationCharacter[i2].CharData = null;
                }
            }
            else if (saveFile.PartyCharacterData[i] != null)
            {
                UICharacter[i].CharUI.SetActive(true);
                UICharacter[i].CharData = saveFile.PartyCharacterData[i];

                CharacterTape[i] = UICharacter[i];
                RotationCharacter[i].CharUI.SetActive(true);
                RotationCharacter[i].CharData = saveFile.PartyCharacterData[i];
            }
            else
            {
                UICharacter[i].CharUI.SetActive(false);
                UICharacter[i].CharData = null;

                RotationCharacter[i].CharUI.SetActive(false);
                RotationCharacter[i].CharData = null;
            }
        }
    }

    void Init()
    {
        CharacterTape = new UICharacterDisplay[saveFile.PartyCharacterData.Length];

        for (int i = 0; i <= saveFile.PartyCharacterData.Length; i++)
        {
            if (i == saveFile.PartyCharacterData.Length)
            {
                for (int i2 = i; i2 < 5; i2++)
                {
                    UICharacter[i2].CharUI.SetActive(false);
                    UICharacter[i2].CharData = null;

                    RotationCharacter[i2].CharUI.SetActive(false);
                    RotationCharacter[i2].CharData = null;
                }
            }
            else if (saveFile.PartyCharacterData[i] != null)
            {
                UICharacter[i].CharUI.SetActive(true);
                UICharacter[i].CharData = saveFile.PartyCharacterData[i];

                CharacterTape[i] = UICharacter[i];
                RotationCharacter[i].CharUI.SetActive(true);
                RotationCharacter[i].CharData = saveFile.PartyCharacterData[i];
            }
            else
            {
                UICharacter[i].CharUI.SetActive(false);
                UICharacter[i].CharData = null;

                RotationCharacter[i].CharUI.SetActive(false);
                RotationCharacter[i].CharData = null;
            }
        }

        RotationUI.Init();
    }
}
