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
    bool targetLocked = false;
    // Start is called before the first frame update

    private void Awake()
    {
        if (detector == null)
            detector = GetComponentInChildren<PlayerDetector>();
        destination = GetComponent<AIDestinationSetter>();
    }
    void Start()
    {
        detector.onPlayerDetected += (GameObject obj) => {
            GetComponent<AIPath>().maxSpeed= 5;
            GetComponent<SpriteRenderer>().color = Color.red;
            SetTarget(obj.transform);
            targetLocked = true;
        };
        detector.onPlayerLost += (GameObject obj) => {
            targetLocked = false;
            GetComponent<AIPath>().maxSpeed = 2;
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
