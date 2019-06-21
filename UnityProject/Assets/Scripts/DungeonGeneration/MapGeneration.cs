using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    

    public Vector2 leftCorner;
    public int width;
    public int height;
    List<Vector2> doorEntrances = new List<Vector2>();

    public Room(Vector2 leftCorner, int width, int height)
    {
        this.leftCorner = leftCorner;
        this.width = width;
        this.height = height;
    }

    public void AddDoorEntrance(Vector2 doorEntrance)
    {
        doorEntrances.Add(doorEntrance);
    }

    

    public List<Vector2> GetDoorEntrance()
    {
        return doorEntrances;
    }


}


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
    private Dictionary<Vector2, Room> rooms = new Dictionary<Vector2, Room>();

    [Range(0, 30)]
    public float randGlobalModifier;

    [Range(0, 2)]
    public float randEnemyModifier;
    public uint maxEnemyByRooms;
    [Range(0, 3)]
    public float randTrashModifier;
    public uint maxTrashByRoom;

    public GameObject astarGO;
    public Tile backgroundTile;
    public Tile wallTile;
    public Tile doorTile;
    public Tile trashTile;
    public Tile vomiTile;
    public Tile exitTile;
    public GameObject exitPrefab;
    public GameObject enemyPrefab;
    public GameObject lightCubePrefab;
    public Tilemap backgroundLayer;
    public Tilemap collidegroundLayer;
    public Tilemap middlegroundLayer;
    public GameObject lightLayer;
    public Tilemap foregroundLayer;
    
    public string pathPrefix = "/Scripts/DungeonGeneration/Maps/";
    public string fileName = "test";
    private uint width;
    private uint height;

    public delegate void Loader(Vector3 startPosition, Vector3 endPosition);
    public Loader onLoaded;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
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
        List < KeyValuePair<int, int> > diag = new List<KeyValuePair<int, int>>();
        List < KeyValuePair<int, int> > cross = new List<KeyValuePair<int, int>>();
        positions.Add(topLeft);
        positions.Add(topCenter);
        positions.Add(topRight);
        positions.Add(midleLeft);
        positions.Add(midleRight);
        positions.Add(bottomLeft);
        positions.Add(bottomCenter);
        positions.Add(bottomRight);

        for (int x = - 1; x < width +1 ; ++x)
        {
            for (int y = -1; y < height +1; ++y)
            {
                Dictionary<KeyValuePair<int, int>, string> subSquare = new Dictionary<KeyValuePair<int, int>, string>();

                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        if (!map.ContainsKey(new KeyValuePair<int, int>(x +i,y+ j)))
                        {
                            subSquare.Add(new KeyValuePair<int, int>(i, j), "!"); //Doesn't exists in the map
                            continue;
                        }

                        KeyValuePair<int, int> mapKeyPair = new KeyValuePair<int, int>(x + i, y + j);
                        subSquare.Add(new KeyValuePair<int, int>(i, j), map[mapKeyPair]);
                    }
                }


                if(subSquare[center] == "F"
                    || subSquare[center] == "DR" || subSquare[center] == "DB" || subSquare[center] == "DL" || subSquare[center] == "DT"
                    || subSquare[center] == "DPR" || subSquare[center] == "DPB" || subSquare[center] == "DPL" || subSquare[center] == "DPT"
                    || subSquare[center] == "DSR" || subSquare[center] == "DSB" || subSquare[center] == "DSL" || subSquare[center] == "DST"
                    || subSquare[center] == "SD"
                    || subSquare[center] == "SDD"
                    || subSquare[center] == "SU"
                    || subSquare[center] == "SUU"
                    ) // we are on a path or in a room
                {
                    //determines walls around.
                    foreach (var pos in positions)
                    {
                        if (subSquare[pos] == "TBC" || subSquare[pos] == "!")
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

        // A room is near
        // Detect room
        foreach (var item in map)
        {
            int x = item.Key.Key;
            int y = item.Key.Value;
                KeyValuePair<int, int> mapKeyPair = new KeyValuePair<int, int>(x, y);
                var cellType = map[mapKeyPair];

                if (cellType == "DR" || cellType == "DB" || cellType == "DL" || cellType == "DT"
                    || cellType == "DPR" || cellType == "DPB" || cellType == "DPL" || cellType == "DPT"
                    || cellType == "DSR" || cellType == "DSB" || cellType == "DSL" || cellType == "DST"
                )
                {
                    Vector2 roomCorner = Vector2.zero;
                    Vector2 doorEntrance = Vector2.zero;
                    int roomWidth = 0;
                    int roomHeight = 0;

                    if (cellType.Contains("L"))
                    {
                        // check room at posx -1
                        var analyzedCell = "F";
                        roomWidth = 0;
                        while (analyzedCell == "F")
                        {
                            ++roomWidth;
                            KeyValuePair<int, int> keyPair = new KeyValuePair<int, int>(x - roomWidth, y);
                            analyzedCell = map[keyPair];
                        }
                        //
                        // Pos - 1
                        roomWidth--;
                        var tmpHeight = 0;
                        analyzedCell = "F";
                        while (analyzedCell == "F")
                        {
                            ++tmpHeight;
                            KeyValuePair<int, int> keyPair = new KeyValuePair<int, int>(x - roomWidth, y + tmpHeight);
                            analyzedCell = map[keyPair];
                        }
                        tmpHeight--;

                        roomHeight = 0;
                        analyzedCell = "F";
                        while (analyzedCell == "F")
                        {
                            roomHeight--;
                            KeyValuePair<int, int> keyPair = new KeyValuePair<int, int>(x - roomWidth, y + roomHeight + tmpHeight);
                            analyzedCell = map[keyPair];
                        }
                        roomHeight++;
                        roomCorner = new Vector2(x - roomWidth, y + tmpHeight + roomHeight);
                        doorEntrance = new Vector2(x - 1, y);
                        //roomWidth += 1;
                        roomHeight = -(roomHeight - 1);
                    }
                    if (cellType.Contains("R"))
                    {
                        var analyzedCell = "F";
                        roomWidth = 0;
                        while (analyzedCell == "F")
                        {
                            ++roomWidth;
                            KeyValuePair<int, int> keyPair = new KeyValuePair<int, int>(x + 1 + roomWidth, y);
                            analyzedCell = map[keyPair];
                        }
                        roomWidth--;
                        var tmpHeight = 0;
                        analyzedCell = "F";
                        while (analyzedCell == "F")
                        {
                            ++tmpHeight;
                            KeyValuePair<int, int> keyPair = new KeyValuePair<int, int>(x + 1 + roomWidth, y + tmpHeight);
                            analyzedCell = map[keyPair];
                        }
                        tmpHeight--;

                        roomHeight = 0;
                        analyzedCell = "F";
                        while(analyzedCell == "F")
                        {
                            roomHeight--;
                            KeyValuePair<int, int> keyPair = new KeyValuePair<int, int>(x + 1 + roomWidth, y + tmpHeight + roomHeight);
                            analyzedCell = map[keyPair];
                        }
                        roomHeight++;

                        roomCorner = new Vector2(x + 1, y + tmpHeight + roomHeight);
                        doorEntrance = new Vector2(x + 1, y);

                        roomWidth += 1;
                        roomHeight = -(roomHeight - 1);


                        // check room at posx + 1
                    }
                    if (cellType.Contains("B"))
                    {
                        // check room at posx -1
                        var analyzedCell = "F";
                        roomHeight = 0;
                        while (analyzedCell == "F")
                        {
                            ++roomHeight;
                            KeyValuePair<int, int> keyPair = new KeyValuePair<int, int>(x, y +  1 + roomHeight);
                            analyzedCell = map[keyPair];
                        }
                        roomHeight--;
                        var tmpWidth = 0;
                        analyzedCell = "F";
                        while (analyzedCell == "F")
                        {
                            ++tmpWidth;
                            KeyValuePair<int, int> keyPair = new KeyValuePair<int, int>(x + tmpWidth, y + 1 + roomHeight);
                            analyzedCell = map[keyPair];
                        }
                        tmpWidth--;

                        roomWidth = 0;
                        analyzedCell = "F";
                        while (analyzedCell == "F")
                        {
                            roomWidth--;
                            KeyValuePair<int, int> keyPair = new KeyValuePair<int, int>(x + tmpWidth + roomWidth, y + 1 + roomHeight);
                            analyzedCell = map[keyPair];
                        }
                        roomWidth++;
                        
                        roomCorner = new Vector2(x + tmpWidth + roomWidth, y + 1);
                        doorEntrance = new Vector2(x, y + 1);
                        roomWidth = -(roomWidth - 1);
                        roomHeight++;

                        //check room at posy -1
                    }
                    if (cellType.Contains("T"))
                    {
                        // check room at posx -1
                        var analyzedCell = "F";
                        roomHeight = 0;
                        while (analyzedCell == "F")
                        {
                            ++roomHeight;
                            KeyValuePair<int, int> keyPair = new KeyValuePair<int, int>(x, y - roomHeight);
                            analyzedCell = map[keyPair];
                        }
                        roomHeight--;
                        var tmpWidth = 0;
                        analyzedCell = "F";
                        while (analyzedCell == "F")
                        {
                            ++tmpWidth;
                            KeyValuePair<int, int> keyPair = new KeyValuePair<int, int>(x + tmpWidth, y - roomHeight);
                            analyzedCell = map[keyPair];
                        }
                        tmpWidth--;

                        roomWidth = 0;
                        analyzedCell = "F";
                        while (analyzedCell == "F")
                        {
                            roomWidth--;
                            KeyValuePair<int, int> keyPair = new KeyValuePair<int, int>(x + tmpWidth + roomWidth, y - roomHeight);
                            analyzedCell = map[keyPair];
                        }

                        roomWidth++;
                        
                        roomCorner = new Vector2(x + tmpWidth + roomWidth, y - roomHeight);
                        doorEntrance = new Vector2(x, y - 1);

                        roomWidth = -(roomWidth - 1);
                    }

                    if (! rooms.ContainsKey(roomCorner))
                    {
                        rooms.Add(roomCorner, new Room(roomCorner, roomWidth, roomHeight));
                    }

                    var room = rooms[roomCorner];
                    room.AddDoorEntrance(doorEntrance);

                }
            
        }



        GenerateMap(width, height, map);
       
    }


    private void GenerateMap(uint width, uint height, Dictionary<KeyValuePair<int, int>, string> map)
    {
        // TODO Configure Astar
        
        


        Vector3 startPosition = Vector3.zero;
        Vector3 endPosition = Vector3.zero;
        foreach (var item in map)
        {
            var cellType = item.Value;
            if (cellType == "F"
                || cellType == "SDD"
                || cellType == "SUU"
                )
            {
                backgroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), backgroundTile);
            }
            else if (cellType == "SU")
            {
                backgroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), backgroundTile);
                middlegroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), exitTile);
                startPosition = new Vector3(item.Key.Key + 0.5f, -item.Key.Value + 0.5f, 0);
            }
            else if(cellType == "SD")
            {
                endPosition = new Vector3(item.Key.Key + 0.5f, -item.Key.Value + 0.5f, 0);
                backgroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), backgroundTile);
                middlegroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), exitTile);
                var exitGO = Instantiate(exitPrefab, new Vector3(item.Key.Key + 0.5f, -item.Key.Value + 0.5f, 0), Quaternion.identity, collidegroundLayer.transform);

            }
            else if (cellType == "WALL")
            {
                collidegroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), wallTile);
                var lightCube = Instantiate(lightCubePrefab, lightLayer.transform);
                lightCube.transform.position = new Vector3(item.Key.Key + 0.5f, -item.Key.Value + 0.5f, lightCube.transform.position.z);
            }
            else if(cellType == "DR" || cellType == "DB" || cellType == "DL" || cellType == "DT"
                    || cellType == "DPR" || cellType == "DPB" || cellType == "DPL" || cellType == "DPT"
                    || cellType == "DSR" || cellType == "DSB" || cellType == "DSL" || cellType == "DST")
            {
                backgroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), backgroundTile);
                collidegroundLayer.SetTile(new Vector3Int(item.Key.Key, -item.Key.Value, 0), doorTile);
            }
        }

        foreach(var room in rooms)
        {
            int rWidth = room.Value.width;
            int rHeight = room.Value.height;
            Vector2 rCorner = room.Key;
            var entrances = room.Value.GetDoorEntrance();

            List<Vector2> borders = new List<Vector2>();
            List<Vector2> inner = new List<Vector2>();

            borders.Add(new Vector2(rCorner.x, rCorner.y));
            borders.Add(new Vector2(rCorner.x, rCorner.y + rHeight - 1));
            borders.Add(new Vector2(rCorner.x + rWidth -1, rCorner.y));
            borders.Add(new Vector2(rCorner.x + rWidth -1, rCorner.y + rHeight -1));

            for (float x = rCorner.x + 1; x < rCorner.x + rWidth -1; ++x)
            {
                borders.Add(new Vector2(x, rCorner.y));
                borders.Add(new Vector2(x, rCorner.y + rHeight-1));
                for (float y = rCorner.y + 1; y < rCorner.y + rHeight - 1; ++y)
                {
                    borders.Add(new Vector2(rCorner.x, y));
                    borders.Add(new Vector2(rCorner.x + rWidth -1, y));
                    
                    inner.Add(new Vector2(x, y));
                }
            }
            //Be sure entrance won't get blocked;
            foreach(var entrance in entrances)
            {
                borders.Remove(entrance);
            }

            int area = (rWidth * rHeight) / (int)randGlobalModifier;

            var rand = new System.Random();
            var randTrash = rand.Next(1, (int)(area*randTrashModifier));
            // Trash are only set next to a border;
           
            if (randTrash > maxTrashByRoom)
                randTrash = (int)maxTrashByRoom;

            for (int trashIndex = 0; trashIndex < randTrash; ++trashIndex)
            {
                var positionIndex = rand.Next(0, borders.Count -1);
                collidegroundLayer.SetTile(new Vector3Int((int)(borders[positionIndex].x), (int)(-borders[positionIndex].y), 0), trashTile);
               // var lightCube = Instantiate(lightCubePrefab, lightLayer.transform);
               // lightCube.transform.position = new Vector3((borders[positionIndex].x) + 0.5f, -(borders[positionIndex].y) + 0.5f, lightCube.transform.position.z);
                borders.RemoveAt(positionIndex);
            }

            // Be sure, there is actually an inner in the room (e.g for 2*2 room)
            if(inner.Count > 0)
            {
                var randEnemy = rand.Next(1,  (int)(area * randEnemyModifier));
                if (randEnemy > maxEnemyByRooms)
                    randEnemy = (int)maxEnemyByRooms;
                for(int enemyIndex = 0; enemyIndex < randEnemy; ++enemyIndex)
                {
                    var rand2 = new System.Random();
                    var positionIndex = rand2.Next(0, inner.Count -1);
                    var rotation = rand2.Next(0, 3);
                    Quaternion rot = Quaternion.identity;
                    switch(rotation)
                    {
                        case 0:
                            rot = Quaternion.Euler(0f, 0f, -90f);
                            break;
                        case 1:
                            rot = Quaternion.Euler(0f, 0f, -180f);
                            break;
                        case 2:
                            rot = Quaternion.Euler(0f, 0f, -270f);
                            break;
                        case 3:
                            Quaternion.Euler(0f, 0f, 0f);
                            break;
                    }
                    var enemy = Instantiate(enemyPrefab, new Vector3(inner[positionIndex].x + 0.5f, -inner[positionIndex].y + 0.5f, 0), rot, collidegroundLayer.transform);
                    
                }
            }

            var randvomi = rand.Next(0, 1);
            for (int vomiIndex = 0; vomiIndex < randvomi; ++vomiIndex)
            {
                var positionIndex = rand.Next(0, entrances.Count - 1);
                collidegroundLayer.SetTile(new Vector3Int((int)(entrances[positionIndex].x), (int)(-entrances[positionIndex].y), 0), vomiTile);
            }
        }


        onLoaded?.Invoke(startPosition, endPosition);
        
    }
}
