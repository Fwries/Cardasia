using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnim : MonoBehaviour
{
    public bool Anim;
    private bool IsAnim;
    private bool ConstantAnim;

    private Sprite[] CurrAnim;

    private SpriteRenderer Renderer;
    private int CurrFrame = 0;
    private float AnimTime = 1;

    public void Init(SC_Tile SCTile)
    {
        Renderer = GetComponent<SpriteRenderer>();
        CurrAnim = SCTile.TileTopImage;
        ConstantAnim = SCTile.ConstantAnim;
    }

    // Update is called once per frame
    void Update()
    {
        if (Anim && !IsAnim) { StartCoroutine(StartAnim()); }
        else if (!ConstantAnim) { return; }

        AnimTime += Time.deltaTime;
        if (AnimTime >= 0.24f)
        {
            CurrFrame++;
            if (CurrFrame >= CurrAnim.Length) { CurrFrame = 0; }
            Renderer.sprite = CurrAnim[CurrFrame];
            AnimTime = 0;
        }
    }

    private IEnumerator StartAnim()
    {
        IsAnim = true;
        bool Reverse = false;

        if (CurrFrame == CurrAnim.Length - 1)
        {
            Reverse = true;
        }

        while (true)
        {
            AnimTime += Time.deltaTime;
            if (AnimTime >= 0.12f)
            {
                if (!Reverse)
                {
                    CurrFrame++;
                    if (CurrFrame >= CurrAnim.Length) { CurrFrame = CurrAnim.Length - 1; break; }
                    Renderer.sprite = CurrAnim[CurrFrame];
                    AnimTime = 0;
                }
                else
                {
                    CurrFrame--;
                    if (CurrFrame < 0) { CurrFrame = 0; break; }
                    Renderer.sprite = CurrAnim[CurrFrame];
                    AnimTime = 0;
                }
            }
            yield return null;
        }
        IsAnim = Anim = false;
        GameObject.Find("Character_RPG").GetComponent<CharacterMovement>().Continue();
    }

    public void SetCurrFrame(bool FirstOrLast)
    {
        if (FirstOrLast)
        {
            CurrFrame = 0;
            Renderer.sprite = CurrAnim[CurrFrame];
        }
        else
        {
            CurrFrame = CurrAnim.Length - 1;
            Renderer.sprite = CurrAnim[CurrFrame];
        }
    }
}
