using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DoorTile: Tile

{
    public Sprite m_Preview;
    // This refreshes itself and other RoadTiles that are orthogonally and diagonally adjacent
    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        for (int yd = -1; yd <= 1; yd++)
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if (HasDoorTile(tilemap, position))
                    tilemap.RefreshTile(position);
            }
    }
    // This determines which sprite is used based on the RoadTiles that are adjacent to it and rotates it to fit the other tiles.
    // As the rotation is determined by the RoadTile, the TileFlags.OverrideTransform is set for the tile.
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        int mask = HasDoorTile(tilemap, location + new Vector3Int(0, 1, 0)) && HasDoorTile(tilemap, location + new Vector3Int(0, -1, 0)) ? 1 : 0;
        mask += HasDoorTile(tilemap, location + new Vector3Int(1, 0, 0)) && HasDoorTile(tilemap, location + new Vector3Int(-1, 0, 0)) ? 2 : 0;

        if ( mask != 3)
        {
            tileData.color = Color.white;
            var m = tileData.transform;
            m.SetTRS(Vector3.zero, GetRotation((byte)mask), Vector3.one);
            tileData.transform = m;
            tileData.flags = TileFlags.LockTransform;
        }
        else
        {
            Debug.LogError("Can't create a door with three wall around");
        }
        
    }

    // This determines if the Tile at the position is the same RoadTile.
    private bool HasDoorTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile<WallTileScripted>(position) != null;
    }

    // The following determines which rotation to use based on the positions of adjacent RoadTiles
    private Quaternion GetRotation(byte mask)
    {
        switch (mask)
        {
            case 1:
                return Quaternion.Euler(0f, 0f, -270f);
        }
        return Quaternion.Euler(0f, 0f, 0f);
    }

#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/DoorTile")]
    public static void CreateDoorTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Door Tile", "New Door Tile", "Asset", "Save Door Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<DoorTile>(), path);
    }
#endif
}
