using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterDisplay : MonoBehaviour
{
    public SC_Character Character;

    private List<Sprite> CurrAnim;
    private int CurrFrame;
    private float AnimTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        SetCurrAnim(Character.Idle_Down_Anim);
    }

    // Update is called once per frame
    void Update()
    {
        AnimTime += Time.deltaTime;
        if (AnimTime >= 0.12f)
        {
            CurrFrame++;
            if (CurrFrame >= CurrAnim.Count) { CurrFrame = 0; }
            this.gameObject.GetComponent<Image>().sprite = CurrAnim[CurrFrame];
            AnimTime = 0;
        }
    }

    public void SetCurrAnim(Sprite[] NewAnim)
    {
        if (CurrAnim != null)
        {
            for (int i = 0; i <= NewAnim.Length; i++)
            {
                if (i == NewAnim.Length) { return; }
                if (NewAnim[i] != CurrAnim[i]) { break; }
            }
        }

        List<Sprite> _CurrAnim = new List<Sprite>();
        for (int i = 0; i < NewAnim.Length; i++)
        {
            _CurrAnim.Add(NewAnim[i]);
        }
        CurrAnim = _CurrAnim;
        CurrFrame = Random.Range(0, NewAnim.Length);
    }
}
