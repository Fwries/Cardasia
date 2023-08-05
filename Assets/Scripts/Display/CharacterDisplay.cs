using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{
    public CharacterBehaviour CharBehav;

    public GameObject CharUI;
    public Text DisplayCharacterText;
    public Text HealthText;
    public Slider HealthSlider;

    private bool IsPlayer;
    private bool IsDead;

    private List<Sprite> CurrAnim;
    private int CurrFrame;
    private float AnimTime = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead) { return; }
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

        if (IsPlayer) { return; }

        DisplayCharacterText.text = "Lv. " + CharBehav.Level + " " + CharBehav.Character.CharName;

        HealthText.text = CharBehav.Health + " / " + CharBehav.MaxHealth;
        HealthSlider.maxValue = CharBehav.MaxHealth;
        HealthSlider.value = CharBehav.Health;

        if (CharBehav.Health <= 0)
        {
            this.gameObject.GetComponent<Image>().sprite = CharBehav.Character.Dead_Sprite[Random.Range(0, CharBehav.Character.Dead_Sprite.Length)];
            IsDead = true;
        }
    }

    public void SetActive(bool Active)
    {
        CharUI.SetActive(!Active);
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

    public void SetBehaviour(CharacterBehaviour _CharBehav)
    {
        CharBehav = _CharBehav;

        if (!CharBehav.IsEnemy)
        {
            SetCurrAnim(CharBehav.Character.Idle_Up_Anim);
        }
        else
        {
            SetCurrAnim(CharBehav.Character.Idle_Down_Anim);
            CharUI.SetActive(true);
        }
    }
}
