using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "Cardasia/Map")]
public class SC_Map : ScriptableObject
{
    public GameObject MapPrefab;
    public string CSVFileName;
    public SC_Tile[] Tileset;
    public int SpawnX, SpawnY;
}
