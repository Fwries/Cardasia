using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic("Opening");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Start(string saveName)
    {
        save.Instance.ChangeScene("RPGScene", saveName);
    }
    public void NewGame(string saveName)
    {
        save.Instance.CreateNewCharacterData(save.Instance.TempChar, 5);
        save.Instance.Inventory = save.Instance.TempItemDeck;
        save.Instance.SaveFile("save");
        save.Instance.SaveFile("battle");
        save.Instance.ChangeScene("RPGScene", saveName);
    }
}
