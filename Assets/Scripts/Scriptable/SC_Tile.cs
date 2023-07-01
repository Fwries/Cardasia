using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Cardasia/Tile")]
public class SC_Tile : ScriptableObject
{
    public Sprite TileImage;
    public Sprite[] TileTopImage;
    
    public bool Solid;
    public bool AbovePlayer;

    public bool Interactable;
    public bool Event;
    public bool SolidEvent;
    public string[] Script;

    public bool IsAnim;
    public bool ConstantAnim;
}
