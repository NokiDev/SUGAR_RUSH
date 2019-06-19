using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    /* 
     F      FLOOR
     DOOR
     SECRET_DOOR
     HERSE
     SD     Begining entrance
     SDD    Entrance Up Stairs
     SU     Begining exit
     SUU    Exit Down Stairs
     */


    public Tile backgroundTile;
    public Tile wallTile;
    public Tile doorTile;

    public GameObject lightCubePrefab;
    public Tilemap backgroundLayer;
    public Tilemap middlegroundLayer;
    public GameObject lightLayer;
    public Tilemap foregroundLayer;
    
    public string pathPrefix = "/Scripts/DungeonGeneration/Maps/";
    public string fileName = "test";
    private uint width;
    private uint height;

    public delegate void Loader(Vector3 startPosition);
    public Loader onLoaded;
    // Start is called before the first frame update
    void Start()
    {
        LoadMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMap()
    {
        StreamReader inp_stm = new StreamReader(Application.dataPath + pathPrefix + fileName + ".txt");

        uint columnCounter = 0;
        uint rowCounter = 0;
        Dictionary<KeyValuePair<int, int>, string> map = new Dictionary<KeyValuePair<int, int>, string>();
        //Load the array in the heap.
        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            rowCounter = 0;
            string cellType = "";
            foreach (char c in inp_ln)
            {
                // Go to next row when encountering a tab.
                if(c == '\t')
                {
                    // Pre treatment
                    if(cellType == "")
                    {
                        cellType = "TBC";
                    }
                    else if(cellType == "DR" || cellType == "DB" || cellType == "DL" || cellType == "DT") // Stqnds for Top, bottom, left and right.
                    {
                        cellType = "DOOR";
                    }
                    else if(cellType == "DPR" || cellType == "DPB" || cellType == "DPL" || cellType == "DPT")
                    {
                        cellType = "HERSE";
                    }
                    else if (cellType == "DSR" || cellType == "DSB" || cellType == "DSL" || cellType == "DST")
                    {
                        cellType = "SECRET_DOOR";
                    }
                    map.Add(new KeyValuePair<int, int>((int)rowCounter, (int)columnCounter), cellType);

                    cellType = "";
                    rowCounter += 1;
                }
                else //we build the item type;
                {
                    cellType += c;
                }
            }
            columnCounter += 1; 
        }
        inp_stm.Close();

        width = rowCounter;
        height = columnCounter;

        Parse(width, height, map);
    }

    private void Parse(uint width, uint height, Dictionary<KeyValuePair<int, int>, string> map)
    {        // read the array by square of 3 by 3.
        KeyValuePair<int, int> topLeft = new KeyValuePair<int, int>(0, 0);
        KeyValuePair<int, int> topCenter = new KeyValuePair<int, int>(1, 0);
        KeyValuePair<int, int> topRight = new KeyValuePair<int, int>(2, 0);

        KeyValuePair<int, int> midleLeft = new KeyValuePair<int, int>(0, 1);
        KeyValuePair<int, int> center = new KeyValuePair<int, int>(1, 1);
        KeyValuePair<int, int> midleRight = new KeyValuePair<int, int>(2, 1);

        KeyValuePair<int, int> bottomLeft = new KeyValuePair<int, int>(0, 2);
        KeyValuePair<int, int> bottomCenter = new KeyValuePair<int, int>(1, 2);
        KeyValuePair<int, int> bottomRight = new KeyValuePair<int, int>(2, 2);

        List < KeyValuePair<int, int> > positions = new List<KeyValuePair<int, int>>();
        positions.Add(topLeft);
        positions.Add(topCenter);
        positions.Add(topRight);
        positions.Add(midleLeft);
        positions.Add(midleRight);
        positions.Add(bottomLeft);
        positions.Add(bottomCenter);
        positions.Add(bottomRight);

        for (int x = - 1; x < width ; ++x)
        {
            for (int y = -1; y < height; ++y)
            {
                Dictionary<KeyValuePair<int, int>, string> subSquare = new Dictionary<KeyValuePair<int, int>, string>();

                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        if (x + i < 0 || y + j < 0 || x + i >= width || y + j >= height)
                        {
                            subSquare.Add(new KeyValuePair<int, int>(i, j), "TBC"); //Doesn't exists in the map
                            continue;
                        }

                        KeyValuePair<int, int> mapKeyPair = new KeyValuePair<int, int>(x + i, y + j);
                        subSquare.Add(new KeyValuePair<int, int>(i, j), map[mapKeyPair]);
                    }
                }


                if(subSquare[center] == "F" 
                    || subSquare[center] == "DOOR" 
                    || subSquare[center] == "HERSE" 
                    || subSquare[center] == "SECRET_DOOR"
                    || subSquare[center] == "SD"
                    || subSquare[center] == "SDD"
                    || subSquare[center] == "SU"
                    || subSquare[center] == "SUU"
                    ) // we are on a path or in a room
                {
                    //determines walls around.
                    foreach (var pos in positions)
                    {
                        if (subSquare[pos] == "TBC")
                        {
                            subSquare[pos] = "WALL";
                        }
                    }
                    if(x == 0 || y == 0 || x >= width -2 || y >= height -2)
                    {
                        subSquare[center] = "WALL"; // avoid to have boundary floor.
                    }
                }
                else if(subSquare[center] == "TBC")
                {
                    //determines if the current is a wall.
                    foreach (var pos in positions)
                    {
                        if (subSquare[pos] == "F")
                        {
                            subSquare[center] = "WALL";
                            break;
                        }
                    }
                }

                // write back
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        KeyValuePair<int, int> squareKeyPair = new KeyValuePair<int, int>(i, j);
                        if (subSquare[squareKeyPair] == "!")
                        {
                            continue;
                        }

                        KeyValuePair<int, int> mapKeyPair = new KeyValuePair<int, int>(x + i, y + j);
                        map[mapKeyPair] = subSquare[squareKeyPair];
                    }
                }
            }
        }

        GenerateMap(width, height, map);

    }
    private void GenerateMap(uint width, uint height, Dictionary<KeyValuePair<int, int>, string> map)
    {
        Vector3 startPosition = Vector3.zero;
        foreach(var item in map)
        {
            var cellType = item.Value;
            if (cellType == "F"
                || cellType == "SD"
                || cellType == "SDD"
                || cellType == "SUU"
                )
            {
                backgroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), backgroundTile);
            }
            else if (cellType == "SU")
            {
                backgroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), backgroundTile);
                startPosition = new Vector3(item.Key.Key + 0.5f, -item.Key.Value + 0.5f, 0);
            }
            else if (cellType == "WALL")
            {
                middlegroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), wallTile);
                var lightCube = Instantiate(lightCubePrefab, lightLayer.transform);
                lightCube.transform.position = new Vector3(item.Key.Key + 0.5f, -item.Key.Value + 0.5f, lightCube.transform.position.z);
            }
            else if(cellType == "DOOR"
                || cellType == "HERSE"
                || cellType == "SECRET_DOOR")
            {
                backgroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), backgroundTile);
                middlegroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), doorTile);
            }
        }

        onLoaded?.Invoke(startPosition);
    }
}
