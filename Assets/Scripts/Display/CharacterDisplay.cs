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

    private List<Sprite> CurrAnim;
    private int CurrFrame;
    private float AnimTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        CurrAnim = new List<Sprite>();
        if (CharBehav.PlayerBehav == CharBehav.GameBehav.Player)
        {
            for (int i = 0; i < CharBehav.Character.Idle_Up_Anim.Length; i++)
            {
                CurrAnim.Add(CharBehav.Character.Idle_Up_Anim[i]);
            }
        }
        else
        {
            for (int i = 0; i < CharBehav.Character.Idle_Down_Anim.Length; i++)
            {
                CurrAnim.Add(CharBehav.Character.Idle_Down_Anim[i]);
            }
        }
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

        if (IsPlayer) { return; }

        DisplayCharacterText.text = "Lv. " + CharBehav.Level + " " + CharBehav.Character.CardName;

        HealthText.text = CharBehav.Health + " / " + CharBehav.MaxHealth;
        HealthSlider.maxValue = CharBehav.MaxHealth;
        HealthSlider.value = CharBehav.Health;
    }

    public void SetActive(bool Active)
    {
        CharUI.SetActive(!Active);
        IsPlayer = Active;
    }
}
