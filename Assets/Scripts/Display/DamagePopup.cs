using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    public Text PopupText;
    private float disappearTimer;

    public void Init(string Popup, Color TextColor)
    {
        PopupText.text = Popup;
        PopupText.color = TextColor;
        disappearTimer = 1f;
    }

    public void Update()
    {
        float MoveYSpeed = 150f, DisappearSpeed = 1f, increaseScaleAmount = 1f;
        
        transform.position += new Vector3(0f, MoveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            Color textColor = PopupText.color;
            textColor.a -= DisappearSpeed * Time.deltaTime;
            PopupText.color = textColor;

            if (PopupText.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
