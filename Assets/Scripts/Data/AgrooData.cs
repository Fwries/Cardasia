using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AgrooData
{
    public CharacterBehaviour AgrooTarget;
    public float Agroo = 0;

    public AgrooData(CharacterBehaviour _AgrooTarget, float _Agroo)
    {
        AgrooTarget = _AgrooTarget;
        Agroo = _Agroo;
    }
}
