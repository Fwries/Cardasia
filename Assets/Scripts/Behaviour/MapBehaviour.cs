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
            { 4, 1, 1, 3, 3, 3, 3, 1, 1, 5, 2, 2, 2, 2}, 
            { 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 2, 2, 2, 2},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2},
            { 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 2, 2, 2, 2}
        };
        
        TileLayer = new int[NewMap.GetLength(0), NewMap.GetLength(1)];
        
        for (int y = 0; y < NewMap.GetLength(0); y++)
        {
            for (int x = 0; x < NewMap.GetLength(1); x++)
            {
                TileLayer[NewMap.GetLength(0) -1 -y, x] = NewMap[y, x];
            }
        }

        SolidTileMap = new bool[TileLayer.GetLength(0), TileLayer.GetLength(1)];
        InteractableTileMap = new bool[TileLayer.GetLength(0), TileLayer.GetLength(1)];
        EventTileMap = new bool[TileLayer.GetLength(0), TileLayer.GetLength(1)];
        
        for (int y = 0; y < TileLayer.GetLength(0); y++)
        {
            for (int x = 0; x < TileLayer.GetLength(1); x++)
            {
                if (TileLayer[y, x] != 0)
                {
                    SolidTileMap[y, x] = Tileset[TileLayer[y, x]].Solid;
                    InteractableTileMap[y, x] = Tileset[TileLayer[y, x]].Interactable;
                    EventTileMap[y, x] = Tileset[TileLayer[y, x]].Event;
                }
                else
                {
                    SolidTileMap[y, x] = true;
                }
            }
        }
    }

    void SpawnMap()
    {
        for (int y = 0; y < TileLayer.GetLength(0); y++)
        {
            for (int x = 0; x < TileLayer.GetLength(1); x++)
            {
                if (TileLayer[y, x] != 0)
                {
                    GameObject TempTile = Instantiate(TilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    TempTile.GetComponent<SpriteRenderer>().sprite = Tileset[TileLayer[y, x]].TileImage;
                    TempTile.transform.SetParent(this.transform);

                    if (Tileset[TileLayer[y, x]].TileTopImage != null)
                    {
                        float z = -0.1f;
                        if (Tileset[TileLayer[y, x]].AbovePlayer) { z = -1.1f; }

                        GameObject TempTileTop = Instantiate(TilePrefab, new Vector3(x, y, z), Quaternion.identity);
                        TempTileTop.GetComponent<SpriteRenderer>().sprite = Tileset[TileLayer[y, x]].TileTopImage;
                        TempTileTop.transform.SetParent(this.transform);
                    }
                }
            }
        }
    }
}
