using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum scenes {
        MAIN_MENU,
        LOADING,
        GAME,
        GAME_OVER
    }

    // The player, Null at start. but for the demo it is already set.
    public MapGeneration generator;
    public GameObject playerPrefab;
    public float countdown = 120f;
    private Timer timer;
    private uint currentLevelAchieved = 0;
    private GameObject playerInstance;

    public List<string> dungeonMaps = new List<string>{"mini", "test", "test2", "test3"};


    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void StartGame()
    {
        playerInstance = Instantiate(playerPrefab, this.gameObject.transform);
        // FIXME react to event in game canvas.
        playerInstance.GetComponent<Damageable>().onDeath += GameOver;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void StartNormalGame()
    {
        // FIXME react to event in game canvas.
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
        GameObject.Destroy(this.generator.gameObject);
        GameObject.Destroy(this.gameObject);
    }


    void TeleportPlayer(Vector3 startPosition)
    {
        // Load the game scene in additive mode.
        SceneManager.LoadScene(2);
        playerInstance.transform.position = startPosition;
    }

    public void LoadAnotherLevel()
    {
        // Be sure to unsubscribe.
        GameObject.Destroy(generator.gameObject);
        SceneManager.LoadScene(1);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Main Menu Screen
        if(scene.buildIndex == 0)
        {

        }
        // We are loading a game (Loading screen)
        else if (scene.buildIndex == 1)
        {
            // Get the fresh new generator;
            generator = GameObject.FindGameObjectWithTag("mapGenerator").GetComponent<MapGeneration>();
            generator.onLoaded += (Vector3 startPos, Vector3 endPos) => {

                TeleportPlayer(startPos);
                //TODO instantiate end goal at end pos, and subscribe to the end reached event.
            };

            // TODO perform random on mapfiles
            
            var rand = new System.Random();
            int index = rand.Next(0, dungeonMaps.Count -1);
            generator.fileName = dungeonMaps[index];
            dungeonMaps.RemoveAt(index);
            StartCoroutine("LoadMap");
           

        }
        else if (scene.buildIndex == 2) // Game
        {
            generator.gameObject.transform.GetChild(0).gameObject.SetActive(true); // One child, grid
            timer = GameObject.FindGameObjectWithTag("ingameUI").GetComponentInChildren<Timer>();
            timer.onEnd += GameOver;

            timer.reset(countdown);
            timer.start();
        }
        else if (scene.buildIndex == 3) // Game Over
        {

        }
        else if (scene.buildIndex == 4)
        {
            playerInstance = GameObject.FindGameObjectWithTag("Player");
            playerInstance.GetComponent<Damageable>().onDeath += GameOver;
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene(3);
    }


    IEnumerator LoadMap()
    {
        yield return new WaitForSeconds(2.5f);
        generator.LoadMap();
        
    }
}
