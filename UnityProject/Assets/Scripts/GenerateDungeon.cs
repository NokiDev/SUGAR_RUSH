using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Cell
{
    public enum CellType
    {
        //NOTHING = 0x00000000, null is the new nothing
        BLOCKED = 0x00000001,
        ROOM = 0x00000002,
        CORRIDOR = 0x00000004,
        WALL = 0x00000003,
        ENTRANCE = 0x00000020,
        PERIMETER = 0x00000010,
        DOOR = 0x00000005,
        LOCKED = 0x00000006,
        ROOM_ID = 0x0000FFC0
    };

    private Vector2Int position;
    private CellType type;
    public Cell(int x, int y, CellType type)
    {
        position = new Vector2Int(x, y);
        this.type = type;
    }

    public bool IsBlocking()
    {
        return type == CellType.BLOCKED; // TODO handle several cases.
    }

    public Vector2Int GetPosition()
    {
        return position;
    }

}



/// <summary>
/// Room is a rectangle surounded by walls and filled with normal block. 
/// 
/// 1 1 1 1 1 1 1
/// 1 0 0 0 0 0 1
/// 1 0 0 0 0 0 1
/// 1 0 0 0 0 0 1
/// 1 0 0 0 0 0 1
/// 1 0 0 0 0 0 1
/// 1 1 1 1 1 1 1
///
/// outerborders are englobing walls
/// innerborders doesn't
/// size is the total size including walls
/// 
/// </summary>
public class Room
{
    private Vector2Int size;
    private Vector2Int position;
    private int id;
    
    List<Cell> entranceCells = new List<Cell>();
    List<Cell> cells = new List<Cell>();

    public Room(int x, int y, int width, int height)
    {
        position = new Vector2Int(x, y);
        size = new Vector2Int(width, height);
        Debug.Log("Create Room at " + position + " with size " + size);
        for(int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                if((i == 0 || j == 0) || i == width -1 || j == height -1) {
                    cells.Add(new Cell(x + i, y + j, Cell.CellType.BLOCKED));
                }
                else
                {
                    cells.Add(new Cell(x + i, y + j, Cell.CellType.ROOM));
                }
            }
        }
    }

    void BreakWalls()
    {
        /*
          { "north", r1 },
            { "south", r2 },
            { "west", c1 },
            { "east", c2 },
         */
        var north_border = position.y;
        var south_border = position.y + size.y;

        int flumph = (int)Math.Sqrt(size.x * size.y);
        //int n_opens = flumph + currentRand.Next(flumph);
    }

    void Connect(Corridor corridor)
    {

    }

    public List<Cell> GetCells()
    {
        return cells;
    }

    public Vector2Int GetPosition()
    {
        return position;
    }

    public Vector2Int GetSize()
    {
        return size;
    }
    public int GetArea()
    {
        return size.x * size.y;
    }
}

public class Corridor
{

}

public class Dungeon
{
    enum Layout { BOX }
    private Layout layout = Layout.BOX;
    public Vector2Int dungeonSize;
    private Cell[][] cells;
    List<Room> rooms = new List<Room>();
    List<Corridor> corridors = new List<Corridor>();

    public Dungeon(int width = 10, int height = 10)
    {
        dungeonSize = new Vector2Int(width, height);
        Debug.Log("Created Dungeon, with size " + dungeonSize);
        cells = new Cell[dungeonSize.x][];

        for (int i = 0; i < dungeonSize.x; ++i)
        {
            cells[i] = new Cell[dungeonSize.y];
            for (int j = 0; j < dungeonSize.y; ++j)
            {
                cells[i][j] = null;
            }
        }
        if (layout == Layout.BOX)
        {
            //ApplyMask();
        }
    }

    public Cell[][] GetCells()
    {
        return cells;
    }

    bool CheckIfRoomFit(Room room)
    {
        var roomSize = room.GetSize();
        var roomPosition = room.GetPosition();

        for (int r = roomPosition.x; r <= roomPosition.x + roomSize.x; r++)
        {
            for (int c = roomPosition.y; c <= roomPosition.y + roomSize.y; c++)
            {
                var cell = cells[r][c];
                if (cell != null && cell.IsBlocking())
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void AddRoom(Room room)
    {
        if (CheckIfRoomFit(room))
        {
            var roomCells = room.GetCells();
            foreach (var cell in roomCells)
            {
                var pos = cell.GetPosition();
                if (cells[pos.x][pos.y] == null)
                {
                    cells[pos.x][pos.y] = cell;
                }
                else
                {
                    Debug.Log(pos.x);
                    Debug.Log(pos.y);
                    throw new Exception("Cell is already allocated");
                }
            }
            rooms.Add(room);

        }
        else
        {
            Debug.Log("Room don't fit in, skipping");
        }
    }


}

public class GenerateDungeon : MonoBehaviour
{
    // Tile Config
    public Tile bg_tile;
    public Tile wall_tile;

    public Tilemap backgroundLayer;
    public Tilemap collideLayer;
    public Tilemap foregroundLayer;
    public GameObject lightLayer;

    public GameObject shadowCasterPrefab;

    // Generator config.
    // dungeon config
    public Vector2Int dungeonSize = new Vector2Int(100, 100);
    // help to compute room position 
    // Vector (min, max)
    public Vector2Int roomSizeInterval = new Vector2Int(3, 10);

    public int seed;
    private System.Random currentRandom;

    private Dungeon dungeon;

    private enum DirectionType
    {
        NONE,
        NORTH,
        SOUTH,
        EST,
        WEST
    }

    static List<DirectionType> directions = new List<DirectionType>() { DirectionType.NORTH, DirectionType.SOUTH, DirectionType.EST, DirectionType.WEST };

    static Vector2Int GetDirectionVector(DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.NORTH: return new Vector2Int(1, 0);
            case DirectionType.SOUTH: return new Vector2Int(-1, 0);
            case DirectionType.EST: return new Vector2Int(0, 1);
            case DirectionType.WEST: return new Vector2Int(0, -1);
        }
        // DEfault
        return new Vector2Int(0, 0);
    }

    static Vector2 GetOppositeDirectionVector(DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.NORTH: return new Vector2Int(-1, 0);
            case DirectionType.SOUTH: return new Vector2Int(1, 0);
            case DirectionType.EST: return new Vector2Int(0, -1);
            case DirectionType.WEST: return new Vector2Int(0, 1);
        }
        // DEfault
        return new Vector2Int(0, 0);
    }

    public void Start()
    {

        dungeon = new Dungeon(dungeonSize.x, dungeonSize.y);
        currentRandom = new System.Random(seed);
        GenerateRooms();


        GenerateMap();
    }
    public void GenerateRooms()
    {
        uint dungeon_area = (uint)(dungeonSize.x * dungeonSize.y);
        uint room_area = (uint)(roomSizeInterval.y * roomSizeInterval.y);
        uint roomNum = (uint)Math.Ceiling((float)dungeon_area / room_area);

        Debug.Log("Creating " + roomNum + " rooms");

        for (uint i = 0; i < roomNum; i++)
        {
            Room room = CreateRoom();
            dungeon.AddRoom(room);
        }
    }
    public Room CreateRoom()
    {
        int roomBase = (int)(roomSizeInterval.x + 1) / 2;
        int roomRadix = (int)((roomSizeInterval.y - roomSizeInterval.x) / 2) + 1;

        int width, height, x, y;
        Debug.Log(roomRadix + " base. " + roomBase);
        height = currentRandom.Next(roomRadix) + roomBase;
        width = currentRandom.Next(roomRadix) + roomBase;
        x = currentRandom.Next(dungeonSize.x / 2 - width);
        y = currentRandom.Next(dungeonSize.y / 2 - height);

        Room room = new Room(x, y, width, height);

        return room;
    }

    private void GenerateMap()
    {
        // iterate through cells
        var cells = dungeon.GetCells();

        for (var i = 0; i < dungeon.dungeonSize.x; ++i)
        {
            for (var j = 0; j < dungeon.dungeonSize.y; ++j)
            {
                var cell = cells[i][j];
                if (cell != null)
                {
                    var cellPos = cell.GetPosition();
                    if(cell.IsBlocking())
                    {
                        Debug.Log("Cell is blocking, adding collider");
                        Debug.Log(i + " / " + j + " - " + cellPos);
                        
                        
                        AddCollider(cellPos.x, cellPos.y);
                        backgroundLayer.SetTile(new Vector3Int(i, j, 0), bg_tile);
                    }
                    else
                    {
                        backgroundLayer.SetTile(new Vector3Int(i, j, 0), bg_tile);
                    }
                }
                else
                {

                    Debug.Log("No cell");
                }
            }
        }
    }

    void AddCollider(int i, int j)
    {
        collideLayer.SetTile(new Vector3Int(i, j, 0), wall_tile);
        var obj = Instantiate(shadowCasterPrefab, lightLayer.transform);
        obj.transform.position = new Vector3(i + 0.5f, j + 0.5f, obj.transform.position.z);
    }
}

public class DungeonGenerator
{
    /// Config
    public uint roomMax = 5;
    public uint roomMin = 5;
    public Vector2Int dungeonSize = new Vector2Int(10, 10);
    public long randomSeed = 111111;
    private int lastRoomId = 0;

    private TileTypes[][] dungeon;
    private enum TileTypes
    {
        NOTHING = 0x00000000,
        BLOCKED = 0x00000001,
        ROOM = 0x00000002,
        CORRIDOR = 0x00000004,
        WALL = 0x00000003,
        ENTRANCE = 0x00000020,
        PERIMETER = 0x00000010,
        DOOR = 0x00000005,
        LOCKED = 0x00000006,
        ROOM_ID = 0x0000FFC0
    };
    public static uint OPENSPACE = (uint)(TileTypes.ROOM | TileTypes.CORRIDOR);
    //public static uint DOORSPACE = TileTypes.ARCH | TileTypes.DOOR | TileTypes.LOCKED | TileTypes.TRAPPED | TileTypes.SECRET | TileTypes.PORTC;
    public static uint ESPACE = (uint)(TileTypes.ENTRANCE);// | TileTypes.DOORSPACE | 0xFF000000;
                                                           // public static uint STAIRS = TileTypes.STAIR_DN | TileTypes.STAIR_UP;

    // public static uint BLOCK_ROOM = BLOCKED | ROOM;
    public static uint BLOCK_CORR = (uint)(TileTypes.BLOCKED | TileTypes.PERIMETER | TileTypes.CORRIDOR);
    // public static uint BLOCK_DOOR = BLOCKED | DOORSPACE;



    /*
      public static uint NOTHING      = 0x00000000;

    public static uint BLOCKED      = 0x00000001;
    public static uint ROOM         = 0x00000002;
    public static uint CORRIDOR     = 0x00000004;
    //                                0x00000008;
    public static uint PERIMETER    = 0x00000010;
    public static uint ENTRANCE     = 0x00000020;
    public static uint ROOM_ID      = 0x0000FFC0;

    public static uint ARCH         = 0x00010000;
    public static uint DOOR         = 0x00020000;
    public static uint LOCKED       = 0x00040000;
    public static uint TRAPPED      = 0x00080000;
    public static uint SECRET       = 0x00100000;
    public static uint PORTC        = 0x00200000;
    public static uint STAIR_DN     = 0x00400000;
    public static uint STAIR_UP     = 0x00800000;

public static uint LABEL = 0xFF000000;
         
         */
    System.Random currentRand;
    public SortedList<int, Dictionary<string, object>> rooms;
    int nRoom = 0;
    private enum DirectionType
    {
        NONE,
        NORTH,
        SOUTH,
        EST,
        WEST
    }

    static List<DirectionType> directions = new List<DirectionType>() { DirectionType.NORTH, DirectionType.SOUTH, DirectionType.EST, DirectionType.WEST };

    static Vector2Int GetDirectionVector(DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.NORTH: return new Vector2Int(1, 0);
            case DirectionType.SOUTH: return new Vector2Int(-1, 0);
            case DirectionType.EST: return new Vector2Int(0, 1);
            case DirectionType.WEST: return new Vector2Int(0, -1);
        }
        // DEfault
        return new Vector2Int(0, 0);
    }

    static Vector2 GetOppositeDirectionVector(DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.NORTH: return new Vector2Int(-1, 0);
            case DirectionType.SOUTH: return new Vector2Int(1, 0);
            case DirectionType.EST: return new Vector2Int(0, -1);
            case DirectionType.WEST: return new Vector2Int(0, 1);
        }
        // DEfault
        return new Vector2Int(0, 0);
    }

    public void Start()
    {
        Generate();
    }

    public void Generate(int seed = 0)
    {
        InitCells(seed);
        EmplaceRooms();
        OpenRooms();
        //LabelRooms();
        //Corridors();
        CleanDungeon();

        Debug.Log(dungeon);
    }

    public void InitCells(int seed)
    {
        dungeon = new TileTypes[dungeonSize.x][];

        for (int i = 0; i < dungeonSize.x; ++i)
        {
            dungeon[i] = new TileTypes[dungeonSize.y];
            for (int j = 0; j < dungeonSize.y; ++j)
            {
                Debug.Log(j);
                Debug.Log(i);
                dungeon[i][j] = TileTypes.NOTHING;
            }
        }

        currentRand = new System.Random(seed);
        rooms = new SortedList<int, Dictionary<string, object>>();
        nRoom = 0;

        var boxLayout = new int[][] { new int[] { 1, 1, 1 }, new int[] { 1, 0, 1 }, new int[] { 1, 1, 1 } };
        MaskCells(boxLayout);
    }

    private void MaskCells(int[][] mask)
    {
        float r_x = mask.Length * 1.0f / dungeonSize.x;
        float c_x = mask[0].Length * 1.0f / dungeonSize.y;

        for (int r = 0; r < dungeonSize.x; r++)
        {
            for (int c = 0; c < dungeonSize.y; c++)
            {
                if (mask[(int)Math.Ceiling(r * r_x)][(int)Math.Ceiling(c * c_x)] == 1)
                    dungeon[r][c] = TileTypes.BLOCKED;
            }
        }
    }

    private void EmplaceRooms()
    {
        ScatterRooms();
    }
    private void ScatterRooms()
    {
        uint dungeon_area = (uint)(dungeonSize.x * dungeonSize.y);
        uint room_area = roomMax * roomMax;
        uint n_r = dungeon_area / room_area;

        for (uint i = 0; i < n_r; i++)
            EmplaceRoom();
    }

    private void EmplaceRoom()
    {
        var proto = new Dictionary<string, int>();

        int r, c;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // room position and size

        set_room(proto);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // room boundaries

        int r1 = (proto["i"] * 2) + 1;
        int c1 = (proto["j"] * 2) + 1;
        int r2 = ((proto["i"] + proto["height"]) * 2) - 1;
        int c2 = ((proto["j"] + proto["width"]) * 2) - 1;

        if (r1 < 1 || r2 > dungeonSize.x - 1) return;
        if (c1 < 1 || c2 > dungeonSize.y - 1) return;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // check for collisions with existing rooms

        Dictionary<string, int> hit = sound_room(r1, c1, r2, c2);
        if (hit.ContainsKey("blocked")) return;
        int n_hits = hit.Count;
        int room_id;

        if (n_hits == 0)
        {
            lastRoomId = ++nRoom;
        }
        else //If collision, skip room generation
        {
            return;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // emplace room

        for (r = r1; r <= r2; r++)
        {
            for (c = c1; c <= c2; c++)
            {
                if ((dungeon[r][c] & TileTypes.ENTRANCE) != TileTypes.NOTHING)
                {
                    dungeon[r][c] &= (TileTypes)~ESPACE;
                }
                else if ((dungeon[r][c] & TileTypes.PERIMETER) != TileTypes.NOTHING)
                {
                    dungeon[r][c] &= ~TileTypes.PERIMETER;
                }
                dungeon[r][c] |= TileTypes.ROOM | (TileTypes)(lastRoomId << 6);
            }
        }
        int height = ((r2 - r1) + 1) * 10;
        int width = ((c2 - c1) + 1) * 10;

        Dictionary<string, object> room_data = new Dictionary<string, object>()
        {
            { "id", lastRoomId },
            { "row", r1 },
            { "col", c1 },
            { "north", r1 },
            { "south", r2 },
            { "west", c1 },
            { "east", c2 },
            { "height", height },
            { "width", width },
            { "area", height * width }

        };

        rooms.Add(lastRoomId, room_data);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // block corridors from room boundary
        // check for door openings from adjacent rooms

        for (r = r1 - 1; r <= r2 + 1; r++)
        {
            if ((dungeon[r][c1 - 1] & (TileTypes.ROOM | TileTypes.ENTRANCE)) == TileTypes.NOTHING)
            {
                dungeon[r][c1 - 1] |= TileTypes.PERIMETER;
            }
            if ((dungeon[r][c2 + 1] & (TileTypes.ROOM | TileTypes.ENTRANCE)) == TileTypes.NOTHING)
            {
                dungeon[r][c2 + 1] |= TileTypes.PERIMETER;
            }
        }
        for (c = c1 - 1; c <= c2 + 1; c++)
        {
            if ((dungeon[r1 - 1][c] & (TileTypes.ROOM | TileTypes.ENTRANCE)) == TileTypes.NOTHING)
            {
                dungeon[r1 - 1][c] |= TileTypes.PERIMETER;
            }
            if ((dungeon[r2 + 1][c] & (TileTypes.ROOM | TileTypes.ENTRANCE)) == TileTypes.NOTHING)
            {
                dungeon[r2 + 1][c] |= TileTypes.PERIMETER;
            }
        }
    }


    private void set_room(Dictionary<string, int> proto)
    {
        int roomBase = (int)(roomMin + 1) / 2;
        int roomRadix = (int)((roomMax - roomMin) / 2) + 1;
        // (10 - 5) /2 + 1 = 6
        // TODO refactor USE OBJECTS !

        // Compute room height
        if (!proto.ContainsKey("height"))
        {
            if (proto.ContainsKey("i"))
            {
                int a = (int)(dungeonSize.x / 2 - roomBase - proto["i"]);
                if (a < 0) a = 0;
                int r = (a < roomRadix) ? a : roomRadix;

                proto.Add("height", currentRand.Next(r) + roomBase);
            }
            else
            {
                proto.Add("height", currentRand.Next(roomRadix) + roomBase);
            }
        }

        // Compute room width
        if (!proto.ContainsKey("width"))
        {
            if (proto.ContainsKey("j"))
            {
                int a = dungeonSize.y / 2 - roomBase - proto["j"];
                if (a < 0) a = 0;
                int r = (a < roomRadix) ? a : roomRadix;

                proto.Add("width", currentRand.Next(r) + roomBase);
            }
            else
            {
                proto.Add("width", currentRand.Next(roomRadix) + roomBase);
            }
        }

        // Compute room positions
        if (!proto.ContainsKey("i"))
        {
            proto.Add("i", currentRand.Next(dungeonSize.x / 2 - proto["height"]));
        }
        if (!proto.ContainsKey("j"))
        {
            proto.Add("j", currentRand.Next(dungeonSize.y / 2 - proto["width"]));
        }
    }

    // Check for a collision 
    private Dictionary<string, int> sound_room(int r1, int c1, int r2, int c2)
    {
        Dictionary<string, int> hit = new Dictionary<string, int>();

        for (int r = r1; r <= r2; r++)
        {
            for (int c = c1; c <= c2; c++)
            {
                if ((dungeon[r][c] & TileTypes.BLOCKED) != TileTypes.NOTHING)
                {
                    hit.Add("blocked", 1);
                    return hit;
                }
                if ((dungeon[r][c] & TileTypes.ROOM) != TileTypes.NOTHING)
                {
                    uint id = ((uint)(dungeon[r][c] & TileTypes.ROOM_ID)) >> 6;

                    if (hit.ContainsKey(id.ToString()))
                        hit[id.ToString()]++;
                    else
                        hit.Add(id.ToString(), 1);
                }
            }
        }
        return hit;
    }

    private void OpenRooms()
    {
        for (int id = 1; id <= nRoom; id++)
        {
            OpenRoom(rooms[id]);
        }
        //connect.Clear();
    }

    // Create Room entrance
    private int AllocOpens(Dictionary<string, object> room)
    {
        int room_h = (((int)room["south"] - (int)room["north"]) / 2) + 1;
        int room_w = (((int)room["east"] - (int)room["west"]) / 2) + 1;
        int flumph = (int)Math.Sqrt(room_w * room_h);
        int n_opens = flumph + currentRand.Next(flumph);

        return n_opens;
    }

    private void OpenRoom(Dictionary<string, object> room)
    {/*
        //List<Dictionary<string, object>> list = door_sills(room);
        //if (list == null || list.Count == 0) return;
        int n_opens = AllocOpens(room);

        for (int i = 0; i < n_opens; i++)
        {
            Dictionary<string, object> sill = null;
            int door_r = 0;
            int door_c = 0;
            uint door_cell = 0;
            bool doContinue = false;
            do
            {
                doContinue = false;
                do
                {
                    sill = Splice(list, currentRand.Next(list.Count));
                    if (sill == null)
                        goto next;
                    door_r = (int)sill["door_r"];
                    door_c = (int)sill["door_c"];
                    door_cell = cell[door_r][door_c];
                }       
                while ((door_cell & DOORSPACE) != NOTHING);

                int out_id = 0;
                if (sill.ContainsKey("out_id"))
                {
                    out_id = (int)sill["out_id"];
                    string strConnect = Math.Min((int)room["id"], out_id).ToString() + "," + Math.Max((int)room["id"], out_id).ToString();

                    if (connect.ContainsKey(strConnect))
                    {
                        connect[strConnect]++;
                        doContinue = true;
                    }
                    else
                        connect.Add(strConnect, 1);
                }
            }
            while (doContinue);

            int open_r = (int)sill["sill_r"];
            int open_c = (int)sill["sill_c"];
            string open_dir = (string)sill["dir"];

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // open door

            for (int x = 0; x < 3; x++)
            {
                var dirVect = GetDirectionVector(openDir);
                int r = open_r + (dirVect.x * x);
                int c = open_c + (dirVect.y * x);

                dungeon[r][c] &= ~TileTypes.PERIMETER;
                dungeon[r][c] |= TileTypes.ENTRANCE;
            }

            if (sill.ContainsKey("out_id"))
                door.Add("out_id", (int)sill["out_id"]);

            if (!room.ContainsKey("door"))
                room.Add("door", new Dictionary<string, List<Dictionary<string, object>>>());
            Dictionary<string, List<Dictionary<string, object>>> d = (Dictionary<string, List<Dictionary<string, object>>>)room["door"];
            if (!d.ContainsKey(open_dir))
                d.Add(open_dir, new List<Dictionary<string, object>>());
            d[open_dir].Add(door);
        }
    next:;*/
    }

    // corridors generation
    private void Corridors()
    {
        for (int i = 1; i < dungeonSize.x; i++)
        {
            int r = (i * 2) + 1;
            for (int j = 1; j < dungeonSize.y; j++)
            {
                int c = (j * 2) + 1;

                if ((dungeon[r][c] & TileTypes.CORRIDOR) != TileTypes.NOTHING) continue;
                tunnel(i, j);
            }
        }
    }

    private void tunnel(int i, int j, DirectionType last_dir = DirectionType.NONE)
    {
        List<DirectionType> dirs = tunnel_dirs(last_dir);

        foreach (var dir in dirs)
        {
            if (open_tunnel(i, j, dir))
            {
                var dirVect = GetDirectionVector(dir);
                int next_i = i + dirVect.x;
                int next_j = j + dirVect.y;

                tunnel(next_i, next_j, dir);
            }
        }
    }

    private List<DirectionType> tunnel_dirs(DirectionType last_dir)
    {
        int p = 0;
        List<DirectionType> dirs = shuffle(directions);

        if (last_dir != DirectionType.NONE && p > 0 && currentRand.Next(100) < p)
            dirs.Insert(0, last_dir);

        return dirs;
    }


    // Get a random list of directions.
    private List<T> shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = currentRand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    private bool open_tunnel(int i, int j, DirectionType dir)
    {
        var dirVect = GetDirectionVector(dir);

        int this_r = (i * 2) + 1;
        int this_c = (j * 2) + 1;
        int next_r = ((i + dirVect.x) * 2) + 1;
        int next_c = ((j + dirVect.y) * 2) + 1;
        int mid_r = (this_r + next_r) / 2;
        int mid_c = (this_c + next_c) / 2;

        // Check Tunnel Collision
        if (sound_tunnel(mid_r, mid_c, next_r, next_c))
        {
            return delve_tunnel(this_r, this_c, next_r, next_c);
        }
        else
        {
            return false;
        }
    }

    private bool delve_tunnel(int this_r, int this_c, int next_r, int next_c)
    {
        int r1 = Math.Min(this_r, next_r);
        int r2 = Math.Max(this_r, next_r);
        int c1 = Math.Min(this_c, next_c);
        int c2 = Math.Max(this_c, next_c);

        for (int r = r1; r <= r2; r++)
        {
            for (int c = c1; c <= c2; c++)
            {
                dungeon[r][c] &= ~TileTypes.ENTRANCE;
                dungeon[r][c] |= TileTypes.CORRIDOR;
            }
        }
        return true;
    }


    private bool sound_tunnel(int mid_r, int mid_c, int next_r, int next_c)
    {
        if (next_r < 0 || next_r > dungeonSize.x) return false;
        if (next_c < 0 || next_c > dungeonSize.y) return false;

        int r1 = Math.Min(mid_r, next_r);
        int r2 = Math.Max(mid_r, next_r);
        int c1 = Math.Min(mid_c, next_c);
        int c2 = Math.Max(mid_c, next_c);

        for (int r = r1; r <= r2; r++)
        {
            for (int c = c1; c <= c2; c++)
            {
                if ((dungeon[r][c] & (TileTypes)BLOCK_CORR) != TileTypes.NOTHING) return false;
            }
        }

        return true;
    }


    private void EmptyBlocks()
    {
        for (int r = 0; r < dungeonSize.x; r++)
            for (int c = 0; c < dungeonSize.y; c++)
                if ((dungeon[r][c] & TileTypes.BLOCKED) != TileTypes.NOTHING)
                    dungeon[r][c] = TileTypes.NOTHING;
    }

    private void CleanDungeon()
    {
        EmptyBlocks();
    }
}
