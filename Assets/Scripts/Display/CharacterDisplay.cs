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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
