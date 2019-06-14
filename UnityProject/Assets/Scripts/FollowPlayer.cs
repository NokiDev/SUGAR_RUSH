using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform Player;
    public float Speed = 1;
    public float StepBeforeCameraMove;
    private float LimiteXUp;
    private float LimiteXDown;
    private float LimiteYUp;
    private float LimiteYDown;


    // Start is called before the first frame update
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        LimiteXUp = Player.position.x + StepBeforeCameraMove;
        LimiteXDown = Player.position.x - StepBeforeCameraMove;

        LimiteYUp = Player.position.y + StepBeforeCameraMove;
        LimiteYDown = Player.position.y - StepBeforeCameraMove;
        bool WillMove = transform.position.x < LimiteXUp && transform.position.x > LimiteXDown && transform.position.y < LimiteYUp && transform.position.y > LimiteYDown;

        if (WillMove)
        {
            transform.position = Vector3.Lerp(transform.position, Player.position, Speed);
            transform.position += new Vector3(0, 0, -10);
        }
        
        
    }
}
