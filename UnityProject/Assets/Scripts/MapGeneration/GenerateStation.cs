using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateStation : MonoBehaviour
{
    public int[,] map;
    public const int MAX_HEIGHT = 40;
    public const int MAX_WIDTH = 25;
    public const int BG_LAYER_Z = 1;
    public const int COLLIDE_LAYER_Z = 0;
    public const int FG_LAYER_Z = -1;
    public const int LIGHT_LAYER_Z = 3;

    public Tile bg_tile;
    public Tile wall_tile;

    public Tilemap backgroundLayer;
    public Tilemap collideLayer;
    public Tilemap foregroundLayer;
    public GameObject lightLayer;

    public GameObject shadowCasterPrefab;

    public void Start()
    {
        Generate();    

    }

    private void Generate ()
    {   
        for (var i = 0; i <= MAX_HEIGHT; ++i) {
            for (var j = 0; j <= MAX_WIDTH; ++j)
            {
                // Generate wall if one of the coordinates is at the border. 
                if ((i == 0 || j== 0) || i == MAX_HEIGHT || j == MAX_WIDTH)
                {
                    AddCollider(i, j);
                }
                else
                {
                    //place background
                    backgroundLayer.SetTile(new Vector3Int(j, i, 0), bg_tile);
                    if(i == MAX_HEIGHT /2 && j == MAX_WIDTH/2)
                    {
                        AddRoom(i, j);
                    }

                }
            }
        }
    }



    void AddCollider(int i, int j)
    {
        collideLayer.SetTile(new Vector3Int(j, i, 0), wall_tile);
        var obj = Instantiate(shadowCasterPrefab, lightLayer.transform);
        obj.transform.position = new Vector3(j + 0.5f, i + 0.5f, obj.transform.position.z);
    }

    void AddRoom(int centerY, int centerX)
    {
        int space = 3;
        int size = 5;
        // Borders
        for (var i = 0; i <= size; ++i)
        {
            for (var j = 0; j <= size; ++j) 
            {
                if ((i == 0 || j == 0 || i == size || j == size) && (i != (int)(size/2) && j != (int)(size/2)))
                {
                    AddCollider(centerX -space + i, centerY -space + j);
                    Debug.Log(centerX - space + i);
                    Debug.Log(centerY - space + j);
                }
            }
        }
    }

}
