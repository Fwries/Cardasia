using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transition", menuName = "Cardasia/Transition")]
public class SC_Transition : ScriptableObject
{
    public string[] TextureName;
    public Texture[] TransitionTexture;
}
