using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float Speed = 1;
    public float StepBeforeCameraMove;
    private float LimiteXUp;
    private float LimiteXDown;
    private float LimiteYUp;
    private float LimiteYDown;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player != null)
        {
            LimiteXUp = player.position.x + StepBeforeCameraMove;
            LimiteXDown = player.position.x - StepBeforeCameraMove;

            LimiteYUp = player.position.y + StepBeforeCameraMove;
            LimiteYDown = player.position.y - StepBeforeCameraMove;
            bool WillMove = transform.position.x < LimiteXUp || transform.position.x > LimiteXDown || transform.position.y < LimiteYUp || transform.position.y > LimiteYDown;

            if (WillMove)
            {
                transform.position = Vector3.Lerp(transform.position, player.position, Speed);
                transform.position += new Vector3(0, 0, -10);
            }
        
        

        }
    }
}
