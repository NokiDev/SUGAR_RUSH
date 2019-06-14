using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{
    

    private Transform thisTransform;
    public float moveSpeed = 0.5f;

    
    public float TimeOfDecisionMin = 1f;
    public float TimeOfDecisionMax = 4f;

    private float TimeCount = 0f;

    private Vector3[] MovingDirection = new Vector3[] { Vector3.right, Vector3.left, Vector3.up, Vector3.down/*, Vector3.zero*/ };
    private int actualMovingDirection;
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = this.transform;
        TimeCount = Random.Range(TimeOfDecisionMin, TimeOfDecisionMax);
        ChooseDirection();


    }

    // Update is called once per frame
    void Update()
    {

        thisTransform.position += MovingDirection[actualMovingDirection] * Time.deltaTime * moveSpeed;

        if (TimeCount > 0)
        {
            TimeCount -= Time.deltaTime;

        }
        else
        {
            TimeCount = Random.Range(TimeOfDecisionMin, TimeOfDecisionMax);
            ChooseDirection();
        }


    }

    void ChooseDirection()
    {
        actualMovingDirection = Mathf.FloorToInt(Random.Range(0, MovingDirection.Length));
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    ChooseDirection();
    //}


}
