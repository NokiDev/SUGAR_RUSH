using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


// type de trajet : aleatoire, ronde, ligne ...



public class PathManager : MonoBehaviour
{
    public List<Transform> LocationsLists = new List<Transform>();
    private AIDestinationSetter AIDestinationSetter;
    private int LocationStep = 0;

    void Start()
    {
        AIDestinationSetter = GetComponent<AIDestinationSetter>();
        AIDestinationSetter.target = LocationsLists[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LocationHelper") {

            LocationStep++;

            if (LocationStep == LocationsLists.Count) {
                LocationStep = 0;
            }

            AIDestinationSetter.target = LocationsLists[LocationStep];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
