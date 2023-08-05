using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterDisplay : MonoBehaviour
{
    public SC_Character Character;
    public SC_Card Weapon;
    public CharacterData CharData;

    public GameObject CharUI;
    public Text DisplayCharacterText;
    public Text HealthText;
    public Slider HealthSlider;

    private bool IsDead;

    private List<Sprite> CurrAnim;
    private int CurrFrame;
    private float AnimTime = 1;

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            if (CurrAnim != null)
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
        }

        Character = CharData.GetCharacter();

        DisplayCharacterText.text = "Lv. " + CharData.Level + " " + Character.CharName;

        HealthText.text = CharData.Health + " / " + (Character.Health + 20 * CharData.Level);
        HealthSlider.maxValue = Character.Health + 20 * CharData.Level;
        HealthSlider.value = CharData.Health;

        if (CharData.Health <= 0 && !IsDead)
        {
            this.gameObject.GetComponent<Image>().sprite = Character.Dead_Sprite[0];
            IsDead = true;
        }
        else if (CharData.Health > 0) { IsDead = false; }
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

    public void SetCharacter()
    {
        Character = CharData.GetCharacter();
        SetCurrAnim(Character.Idle_Down_Anim);
    }
}
