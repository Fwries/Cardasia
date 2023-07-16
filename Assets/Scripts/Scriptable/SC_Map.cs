using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "Cardasia/Map")]
public class SC_Map : ScriptableObject
{
    [SerializeField] public GameObject MapPrefab;
    [SerializeField] public string CSVFileName;
    [SerializeField] public string MusicTheme;
    [SerializeField] public SC_Tile[] Tileset;
}
