using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Template", menuName = "Cardasia/Template")]
public class SC_Template : ScriptableObject
{
    public Sprite TypeConsumable;
    public Sprite TypeStamina;
    public Sprite TypeMana;
    public Sprite TypeBoth;

    public Sprite Frozen;
}
