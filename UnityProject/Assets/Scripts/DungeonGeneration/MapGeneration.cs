using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    public Tile backgroundTile;
    public Tile wallTile;

    public Tilemap backgroundLayer;
    public Tilemap middlegroundLayer;
    public Tilemap lightLayer;
    public Tilemap foregroundLayer;
    
    public string pathPrefix = "/Scripts/DungeonGeneration/Maps/";
    public string fileName = "test";
    private uint width;
    private uint height;
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
        Dictionary<KeyValuePair<uint, uint>, string> map = new Dictionary<KeyValuePair<uint, uint>, string>();
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
                    if(cellType == "")
                    {
                        cellType = "TBC";
                    }
                    map.Add(new KeyValuePair<uint, uint>(rowCounter, columnCounter), cellType);

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

    private void Parse(uint width, uint height, Dictionary<KeyValuePair<uint, uint>, string> map)
    {
        // read the array by square of 3 by 3.
        KeyValuePair<uint, uint> topLeft = new KeyValuePair<uint, uint>(0, 0);
        KeyValuePair<uint, uint> topCenter = new KeyValuePair<uint, uint>(1, 0);
        KeyValuePair<uint, uint> topRight = new KeyValuePair<uint, uint>(2, 0);

        KeyValuePair<uint, uint> midleLeft = new KeyValuePair<uint, uint>(0, 1);
        KeyValuePair<uint, uint> center = new KeyValuePair<uint, uint>(1, 1);
        KeyValuePair<uint, uint> midleRight = new KeyValuePair<uint, uint>(2, 1);

        KeyValuePair<uint, uint> bottomLeft = new KeyValuePair<uint, uint>(0, 2);
        KeyValuePair<uint, uint> bottomCenter = new KeyValuePair<uint, uint>(1, 2);
        KeyValuePair<uint, uint> bottomRight = new KeyValuePair<uint, uint>(2, 2);

        List < KeyValuePair<uint, uint> > positions = new List<KeyValuePair<uint, uint>>();
        positions.Add(topLeft);
        positions.Add(topCenter);
        positions.Add(topRight);
        positions.Add(midleLeft);
        positions.Add(midleRight);
        positions.Add(bottomLeft);
        positions.Add(bottomCenter);
        positions.Add(bottomRight);

        for (int x = - 1; x < width; ++x)
        {
            for (int y = -1; y < height; ++y)
            {
                Dictionary<KeyValuePair<uint, uint>, string> subSquare = new Dictionary<KeyValuePair<uint, uint>, string>();

                for (uint i = 0; i < 3; ++i)
                {
                    for (uint j = 0; j < 3; ++j)
                    {
                        if (x + i < 0 || y + j < 0 || x + i >= width || y + j >= height)
                        {
                            subSquare.Add(new KeyValuePair<uint, uint>(i, j), "!"); //Doesn't exists in the map
                            continue;
                        }

                        KeyValuePair<uint, uint> mapKeyPair = new KeyValuePair<uint, uint>((uint)x + i, (uint)y + j);
                        subSquare.Add(new KeyValuePair<uint, uint>(i, j), map[mapKeyPair]);
                    }
                }


                if(subSquare[center] == "F" 
                    || subSquare[center] == "DR" 
                    || subSquare[center] == "DB" 
                    || subSquare[center] == "DL"
                    || subSquare[center] == "DT"
                    || subSquare[center] == "DSB"
                    || subSquare[center] == "DPT"
                    || subSquare[center] == "DPB"
                    || subSquare[center] == "DPR"
                    || subSquare[center] == "DPL"
                    || subSquare[center] == "DSL"
                    || subSquare[center] == "DSR"
                    || subSquare[center] == "SDD"
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
                for (uint i = 0; i < 3; ++i)
                {
                    for (uint j = 0; j < 3; ++j)
                    {
                        KeyValuePair<uint, uint> squareKeyPair = new KeyValuePair<uint, uint>(i, j);
                        if (subSquare[squareKeyPair] == "!")
                        {
                            continue;
                        }

                        KeyValuePair<uint, uint> mapKeyPair = new KeyValuePair<uint, uint>((uint)x + i, (uint)y + j);
                        map[mapKeyPair] = subSquare[squareKeyPair];
                    }
                }
            }
        }

        GenerateMap(width, height, map);

    }
    private void GenerateMap(uint width, uint height, Dictionary<KeyValuePair<uint, uint>, string> map)
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                KeyValuePair<uint, uint> mapKeyPair = new KeyValuePair<uint, uint>((uint)x, (uint)y);
                var cellType = map[mapKeyPair];
                Debug.Log(cellType);

                if (cellType == "F"
                    || cellType == "DR"
                    || cellType == "DB"
                    || cellType == "DL"
                    || cellType == "DT"
                    || cellType == "DSB"
                    || cellType == "DPT"
                    || cellType == "DPR"
                    || cellType == "DPB"
                    || cellType == "DPL"
                    || cellType == "DSL"
                    || cellType == "DSR"
                    || cellType == "SDD")
                {
                    backgroundLayer.SetTile(new Vector3Int(x, y, 0), backgroundTile);
                }
                else if (cellType == "WALL")
                {
                    middlegroundLayer.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }
    }
}
