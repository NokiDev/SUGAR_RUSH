using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform Player;
    public float Speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        transform.position = Vector3.Lerp(transform.position, Player.position, Speed);
        transform.position += new Vector3(0, 0, -10);
    }
}
