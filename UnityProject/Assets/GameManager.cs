using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The player, Null at start. but for the demo it is already set.
    public MapGeneration generator;
    public GameObject player = null;


    private void Awake()
    {
        DontDestroyOnLoad(this);
        player = GameObject.FindGameObjectWithTag("Player");
        generator.onLoaded += TeleportPlayer;
    }

    void StartGame()
    {
    }

    void TeleportPlayer(Vector3 startPosition)
    {
        player.transform.position = startPosition;
    }

}
