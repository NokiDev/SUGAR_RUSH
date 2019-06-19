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
    private BotTargetSelector targetSelector;
    
    void Start()
    {
        targetSelector = GetComponent<BotTargetSelector>();
        targetSelector.SetTarget(LocationsLists[0]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LocationHelper") {
            if(LocationsLists.Count > 0 && collision.gameObject.transform == LocationsLists[LocationStep]) {

                LocationStep++;

                if (LocationStep >= LocationsLists.Count) {
                    LocationStep = 0;
                }

                targetSelector.SetTarget(LocationsLists[LocationStep]);
            }
        }
    }
}
