using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDisplay : MonoBehaviour
{
    public Button Continue;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic("Opening");
        Continue.interactable = save.Instance.CheckFileExist("save");
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
        Destroy(save.Instance.gameObject);
        save.Instance = null;
        Instantiate(Resources.Load("Save"));

        save.Instance.CreateNewCharacterData(save.Instance.TempChar, 5);
        save.Instance.Inventory = save.Instance.TempItemDeck;
        save.Instance.SaveFile("save");
        save.Instance.SaveFile("battle");
        save.Instance.ChangeScene("RPGScene", saveName);
    }
}
