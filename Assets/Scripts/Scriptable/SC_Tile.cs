using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Cardasia/Tile")]
public class SC_Tile : ScriptableObject
{
    [SerializeField] public Sprite TileImage;
    [SerializeField] public Sprite[] TileTopImage;

    [SerializeField] public bool Solid;
    [SerializeField] public bool AbovePlayer;

    [SerializeField] public bool Interactable;
    [SerializeField] public bool Event;
    [SerializeField] public bool SolidEvent;
    [SerializeField] public string[] Script;

    [SerializeField] public bool IsAnim;
    [SerializeField] public bool ConstantAnim;

    [SerializeField] public SC_Map ChangeMap;
    [SerializeField] public SC_EnemyList EnemyList;
}
