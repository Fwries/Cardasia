using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapBehaviour : MonoBehaviour
{
    public GameObject TilePrefab;
    public SC_Map SCMap;

    [HideInInspector] public SC_Tile[] Tileset;
    [HideInInspector] private int[,] Map;
    [HideInInspector] public int SpawnX = 8, SpawnY = 6;

    private GameObject MapObj;

    [HideInInspector] public int[,] TileLayer;
    
    [HideInInspector] public bool[,] SolidTileMap;
    [HideInInspector] public bool[,] InteractableTileMap;
    [HideInInspector] public bool[,] EventTileMap;
    [HideInInspector] public bool[,] SolidEventTileMap;

    private bool Locked;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMap(int TilesetTileNo)
    {
        ChangeMap(Tileset[TilesetTileNo].ChangeMap);
    }

    public void ChangeMap(SC_Map _SCMap)
    {
        save.Instance.Map = SCMap = _SCMap;
        Tileset = SCMap.Tileset;
        ReadCSVMap();

        TileLayer = new int[Map.GetLength(0), Map.GetLength(1)];

        for (int y = 0; y < Map.GetLength(0); y++)
        {
            for (int x = 0; x < Map.GetLength(1); x++)
            {
                TileLayer[Map.GetLength(0) -1 -y, x] = Map[y, x];
            }
        }

        for (int i = 0; i < save.Instance.InteractableList.Length; i++)
        {
            if (save.Instance.InteractableList[i].HasInteracted && save.Instance.InteractableList[i].MapName == SCMap.MapName)
            {
                if (save.Instance.InteractableList[i].Single)
                {
                    TileLayer[save.Instance.InteractableList[i].PosY, save.Instance.InteractableList[i].PosX] = save.Instance.InteractableList[i].State2;
                }
                else
                {
                    for (int y = 0; y < Map.GetLength(0); y++)
                    {
                        for (int x = 0; x < Map.GetLength(1); x++)
                        {
                            if (TileLayer[y, x] == save.Instance.InteractableList[i].State1)
                            {
                                TileLayer[y, x] = save.Instance.InteractableList[i].State2;
                            }
                        }
                    }
                }
            }
        }

        SolidTileMap        = new bool[TileLayer.GetLength(0), TileLayer.GetLength(1)];
        InteractableTileMap = new bool[TileLayer.GetLength(0), TileLayer.GetLength(1)];
        EventTileMap        = new bool[TileLayer.GetLength(0), TileLayer.GetLength(1)];
        SolidEventTileMap   = new bool[TileLayer.GetLength(0), TileLayer.GetLength(1)];

        SpawnMap();
    }

    public void ChangeTile(int TilesetTileNo, int x, int y)
    {
        TileLayer[y, x] = TilesetTileNo;

        GameObject Tile = GameObject.Find(x + "x" + y + "yBottom");
        GameObject TileTop = GameObject.Find(x + "x" + y + "yTop");

        if (Tile != null) { Destroy(Tile); }
        if (TileTop != null) { Destroy(TileTop); }

        SpawnTile(x, y);
    }

    public void ChangeTileState(int x, int y)
    {
        for (int i = 0; i < save.Instance.InteractableList.Length; i++)
        {
            if (save.Instance.InteractableList[i].MapName == SCMap.MapName)
            {
                if (save.Instance.InteractableList[i].PosX == x && save.Instance.InteractableList[i].PosY == y)
                {
                    save.Instance.InteractableList[i].HasInteracted = !save.Instance.InteractableList[i].HasInteracted;
                    if (save.Instance.InteractableList[i].HasInteracted)
                    {
                        ChangeTile(save.Instance.InteractableList[i].State2, x, y);
                    }
                    else
                    {
                        ChangeTile(save.Instance.InteractableList[i].State1, x, y);
                    }

                    return;
                }
            }
        }
    }

    public void ChangeAllTileState(int Index, int ConsumedIndex)
    {
        for (int i = 0; i < save.Instance.InteractableList.Length; i++)
        {
            if (save.Instance.InteractableList[i].MapName == SCMap.MapName)
            {
                if ((save.Instance.InteractableList[i].State1 == Index && save.Instance.InteractableList[i].State2 == ConsumedIndex) ||
                    (save.Instance.InteractableList[i].State1 == ConsumedIndex && save.Instance.InteractableList[i].State2 == Index))
                {
                    save.Instance.InteractableList[i].HasInteracted = !save.Instance.InteractableList[i].HasInteracted;
                    for (int y = 0; y < TileLayer.GetLength(0); y++)
                    {
                        for (int x = 0; x < TileLayer.GetLength(1); x++)
                        {
                            if (TileLayer[y, x] != Index) { continue; }

                            if (save.Instance.InteractableList[i].HasInteracted)
                            {
                                ChangeTile(save.Instance.InteractableList[i].State2, x, y);
                            }
                            else
                            {
                                ChangeTile(save.Instance.InteractableList[i].State1, x, y);
                            }
                        }
                    }
                }
            }
        }
    }

    public void AnimAll(int Index)
    {
        Locked = false;
        for (int y = 0; y < TileLayer.GetLength(0); y++)
        {
            for (int x = 0; x < TileLayer.GetLength(1); x++)
            {
                if (TileLayer[y, x] == Index)
                {
                    GameObject.Find(x + "x" + y + "yTop").GetComponent<TileAnim>().StartAnimAll();
                }
            }
        }
    }
    public void ContinueLock()
    {
        if (!Locked)
        {
            GameObject.Find("Character_RPG").GetComponent<CharacterMovement>().Continue();
            Locked = true;
        }
    }

    public SC_Tile GetTile(int x, int y)
    {
        return Tileset[TileLayer[y, x]];
    }

    void SpawnMap()
    {
        if (MapObj != null)
        {
            Destroy(MapObj);
        }
        MapObj = Instantiate(Resources.Load("Scriptables/Maps/" + SCMap.MapName + "/" + SCMap.MapName, typeof(GameObject)), this.transform) as GameObject;

        for (int y = 0; y < TileLayer.GetLength(0); y++)
        {
            for (int x = 0; x < TileLayer.GetLength(1); x++)
            {
                SpawnTile(x, y);
            }
        }

        if (SCMap.MusicTheme != "") { AudioManager.Instance.PlayMusic(SCMap.MusicTheme); }
    }

    void SpawnTile(int x, int y)
    {
        if (TileLayer[y, x] != 0 && TileLayer[y, x] != 1)
        {
            if (Tileset[TileLayer[y, x]].TileImage != null)
            {
                GameObject TempTile = Instantiate(TilePrefab, new Vector3(x, y, -0.01f), Quaternion.identity);
                TempTile.GetComponent<SpriteRenderer>().sprite = Tileset[TileLayer[y, x]].TileImage;
                TempTile.transform.SetParent(MapObj.transform);
                TempTile.name = x + "x" + y + "yBottom";
            }

            if (Tileset[TileLayer[y, x]].TileTopImage.Length > 0)
            {
                float z = -0.1f;
                if (Tileset[TileLayer[y, x]].AbovePlayer) { z = -1.1f; }

                GameObject TempTileTop = Instantiate(TilePrefab, new Vector3(x, y, z), Quaternion.identity);
                TempTileTop.GetComponent<SpriteRenderer>().sprite = Tileset[TileLayer[y, x]].TileTopImage[0];
                TempTileTop.transform.SetParent(MapObj.transform);
                TempTileTop.name = x + "x" + y + "yTop";

                if (Tileset[TileLayer[y, x]].IsAnim)
                {
                    TileAnim newScript = TempTileTop.AddComponent<TileAnim>();
                    newScript.Init(Tileset[TileLayer[y, x]]);
                }
            }
        }

        SolidTileMap[y, x] = Tileset[TileLayer[y, x]].Solid;
        InteractableTileMap[y, x] = Tileset[TileLayer[y, x]].Interactable;
        EventTileMap[y, x] = Tileset[TileLayer[y, x]].Event;
        SolidEventTileMap[y, x] = Tileset[TileLayer[y, x]].SolidEvent;
    }

    void ReadCSVMap()
    {
        string line;
        string CSVLocation = Path.Combine(Application.streamingAssetsPath, "CSV", SCMap.MapName + ".csv");
        StreamReader strReader = new StreamReader(CSVLocation);
        List<string> fileLines = new List<string>();
        using (strReader)
        {
            do
            {
                line = strReader.ReadLine();
                if (line != null)
                {
                    fileLines.Add(line);
                }
            }
            while (line != null);
            strReader.Close();
        }

        int Width = 0;

        for (int y = 0; y < fileLines.Count; y++)
        {
            string[] tileRowIds = fileLines[y].Split(',');
            if (Width == 0)
            {
                Width = tileRowIds.Length;
                Map = new int[fileLines.Count, Width];
            }

            for (int x = 0; x < Width; x++)
            {
                Map[y, x] = int.Parse(tileRowIds[x]);
            }
        }
    }
}
