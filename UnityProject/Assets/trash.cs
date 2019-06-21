using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class trash : MonoBehaviour
{
    public GameObject key;

    void keySpawn()
    {
        Vector3 pos = gameObject.transform.position;
        GameObject.FindGameObjectWithTag("mapGenerator").GetComponent<MapGeneration>().deleteTile(new Vector3Int((int)pos.x, (int)pos.y-1, (int)pos.z));
        Instantiate(key, pos, Quaternion.identity);
    }

    private void OnDestroy()
    {
        keySpawn();
    }
}
