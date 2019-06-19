using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The player, Null at start. but for the demo it is already set.
    public GameObject player = null;


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void StartGame()
    {

    }

}
