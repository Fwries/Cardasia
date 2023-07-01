using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MonoBehaviour
{
    public GameObject TilePrefab;
    public SC_Tile[] Tileset;

    public int SpawnX, SpawnY;

    [HideInInspector] public int[,] TileLayer;
    
    [HideInInspector] public bool[,] SolidTileMap;
    [HideInInspector] public bool[,] InteractableTileMap;
    [HideInInspector] public bool[,] EventTileMap;
    [HideInInspector] public bool[,] SolidEventTileMap;

    // Start is called before the first frame update
    void Start()
    {
        ChangeMap();
        SpawnMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMap()
    {
        int[,] NewMap = {
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
            { 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 2},
            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2},
            { 1, 1, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 2},
            { 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 2},
            { 1, 1, 1, 0, 0, 1, 3, 1, 5, 1, 4, 1, 0, 0, 0, 0, 1, 2},
            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2},
            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
            { 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 2},
            { 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 2},
            { 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 2},
            { 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 2},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2},
        };
        
        TileLayer = new int[NewMap.GetLength(0), NewMap.GetLength(1)];
        
        for (int y = 0; y < NewMap.GetLength(0); y++)
        {
            for (int x = 0; x < NewMap.GetLength(1); x++)
            {
                TileLayer[NewMap.GetLength(0) -1 -y, x] = NewMap[y, x];
            }
        }

        SolidTileMap        = new bool[TileLayer.GetLength(0), TileLayer.GetLength(1)];
        InteractableTileMap = new bool[TileLayer.GetLength(0), TileLayer.GetLength(1)];
        EventTileMap        = new bool[TileLayer.GetLength(0), TileLayer.GetLength(1)];
        SolidEventTileMap   = new bool[TileLayer.GetLength(0), TileLayer.GetLength(1)];

        for (int y = 0; y < TileLayer.GetLength(0); y++)
        {
            for (int x = 0; x < TileLayer.GetLength(1); x++)
            {
                SolidTileMap[y, x] = Tileset[TileLayer[y, x]].Solid;
                InteractableTileMap[y, x] = Tileset[TileLayer[y, x]].Interactable;
                EventTileMap[y, x] = Tileset[TileLayer[y, x]].Event;
                SolidEventTileMap[y, x] = Tileset[TileLayer[y, x]].SolidEvent;
            }
        }
    }

    void SpawnMap()
    {
        for (int y = 0; y < TileLayer.GetLength(0); y++)
        {
            for (int x = 0; x < TileLayer.GetLength(1); x++)
            {
                if (TileLayer[y, x] != 0 && TileLayer[y, x] != 1)
                {
                    if (Tileset[TileLayer[y, x]].TileImage != null)
                    {
                        GameObject TempTile = Instantiate(TilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                        TempTile.GetComponent<SpriteRenderer>().sprite = Tileset[TileLayer[y, x]].TileImage;
                        TempTile.transform.SetParent(this.transform);
                        TempTile.name = x + "x" + y + "y" + "Bottom";
                    }

                    if (Tileset[TileLayer[y, x]].TileTopImage.Length > 0)
                    {
                        float z = -0.1f;
                        if (Tileset[TileLayer[y, x]].AbovePlayer) { z = -1.1f; }

                        GameObject TempTileTop = Instantiate(TilePrefab, new Vector3(x, y, z), Quaternion.identity);
                        TempTileTop.GetComponent<SpriteRenderer>().sprite = Tileset[TileLayer[y, x]].TileTopImage[0];
                        TempTileTop.transform.SetParent(this.transform);
                        TempTileTop.name = x + "x" + y + "y" + "Top";

                        if (Tileset[TileLayer[y, x]].IsAnim)
                        {
                            TileAnim newScript = TempTileTop.AddComponent<TileAnim>();
                            newScript.Init(Tileset[TileLayer[y, x]]);
                        }
                    }
                }
            }
        }
    }
}
