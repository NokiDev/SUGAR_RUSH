using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finish_trigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.LoadAnotherLevel();

        }
    }
}
