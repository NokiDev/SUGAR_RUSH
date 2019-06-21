using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBeyblade : MonoBehaviour
{
    public float speed = 10;
    PlayerDetector detector;
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        detector = GetComponentInChildren<PlayerDetector>();
        detector.onPlayerDetected += (GameObject obj) => {
            stop = true;
        };
        detector.onPlayerLost += (GameObject obj) => {
            stop = false;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(!stop)
        {
            transform.Rotate(new Vector3(0, 0, 0.5f * speed));
        }
    }
}
