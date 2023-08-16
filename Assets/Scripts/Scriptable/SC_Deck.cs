using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Cardasia/Deck")]
public class SC_Deck : ScriptableObject
{
    public List<SC_Card> List;
}
