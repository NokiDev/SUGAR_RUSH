using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public GameObject Camera;
    public GameObject StartPosition;
    public GameObject EndPosition;

    private float distance = 30f;
    private float LerpTime = 5;
    private float CurrentLerpTime = 0;
    private bool keyHit = false;

    public bool isInZone = false;
    private Vector3 stPosition;
    private Vector3 EdPosition;
    public bool isCollisoion = false;


    private void Start()
    {
         stPosition = StartPosition.transform.position;
         EdPosition = EndPosition.transform.position;

    }

    private void Update()
    {
        stPosition = StartPosition.transform.position;
        


    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            StepCameraMove();
            isCollisoion = true;
        }

    }

    public void StepCameraMove()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            keyHit = true;
            EdPosition = new Vector3(EndPosition.transform.position.x, EndPosition.transform.position.y, Camera.transform.position.z);

        }


        return;
    }


}


