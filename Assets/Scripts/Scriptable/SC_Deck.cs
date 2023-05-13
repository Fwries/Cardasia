using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Cardasia/Deck")]
public class SC_Deck : ScriptableObject
{
    public SC_Character.Class Class;
    public SC_Character.SubClass SubClass;
    public List<SC_Card> Deck;
}
