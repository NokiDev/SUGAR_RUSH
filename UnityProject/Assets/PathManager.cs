using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


// type de trajet : aleatoire, ronde, ligne ...
public class PathManager : MonoBehaviour
{
    public List<Transform> LocationsLists = new List<Transform>();
    private AIDestinationSetter AIDestinationSetter;
    public int LocationStep = 0;
    private bool stop = false;
    public PlayerDetector detector;
    
    void Start()
    { 
        detector.onPlayerDetected += FollowPlayer;
        detector.onPlayerLost += UnFollowPlayer;
        AIDestinationSetter = GetComponent<AIDestinationSetter>();
        AIDestinationSetter.target = LocationsLists[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!stop && collision.tag == "LocationHelper") {

            if(LocationsLists.Count > 0) {

            LocationStep++;

            if (LocationStep >= LocationsLists.Count) {
                LocationStep = 0;
            }

            AIDestinationSetter.target = LocationsLists[LocationStep];

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FollowPlayer(GameObject player)
    {
        stop = true;
        GetComponent<AIPath>().speed = 5;
        GetComponent<SpriteRenderer>().color = Color.red;
        AIDestinationSetter.target = player.transform;
        
    }

    void UnFollowPlayer(GameObject player)
    {
        GetComponent<AIPath>().speed = 2;
        GetComponent<SpriteRenderer>().color = Color.yellow;
        stop = false;
        AIDestinationSetter.target = LocationsLists[LocationStep];
    }


}
