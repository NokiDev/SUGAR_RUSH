using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateLightCubes : MonoBehaviour
{
    // which tile map contains, colliders.
    public Tilemap tilemap;
    public GameObject lightCubePrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Read tilemap.
        BoundsInt bounds = tilemap.cellBounds;
       
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {

                Vector3Int localPlace = (new Vector3Int(x, y, (int)tilemap.transform.position.y));
                Vector3 place = tilemap.CellToWorld(localPlace);

                // TODO use vertices for better perf.
                if (tilemap.HasTile(localPlace))
                {
                    var lightCube = Instantiate(lightCubePrefab, this.gameObject.transform);
                    lightCube.transform.position = new Vector3(place.x + 0.5f, place.y + 0.5f, lightCube.transform.position.z);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
