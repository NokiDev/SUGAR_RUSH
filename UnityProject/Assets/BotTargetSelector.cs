using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BotTargetSelector : MonoBehaviour
{
    public PlayerDetector detector;
    private AIDestinationSetter destination;
    Transform currentTarget;
    Transform lastTarget;
    bool targetLocked;
    // Start is called before the first frame update
    void Start()
    {
        detector.onPlayerDetected += (GameObject obj) => {
            bool targetLocked = true;
            GetComponent<AIPath>().speed = 5;
            GetComponent<SpriteRenderer>().color = Color.red;
            SetTarget(obj.transform);
        };
        detector.onPlayerLost += (GameObject obj) => {
            bool targetLocked = false;
            GetComponent<AIPath>().speed = 2;
            GetComponent<SpriteRenderer>().color = Color.yellow;
            SetTarget(lastTarget);
        };
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void SetTarget(Transform transform)
    {
        if(!targetLocked) {
            lastTarget = currentTarget;
            currentTarget = transform;
            destination.target = currentTarget.transform;
        }
    }


}
