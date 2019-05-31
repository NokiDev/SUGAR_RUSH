using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public GameObject Camera;
    public GameObject CameraDebut;
    public GameObject CameraFin;

    private float distance = 30f;
    private float LerpTime = 5;
    private float CurrentLerpTime = 0;
    private bool keyHit = false;

    public bool isInZone = false;
    private Vector3 stPosition;
    private Vector3 EdPosition;
    public bool isCollisoion = false;

    public float Min =  -1.0f;
    public float Max = -1.0f;
    public float NextMove = 0.0f;

    public float CameraTransformX;
    public float CameraTransformY;


    private void Start()
    {
         stPosition = CameraDebut.transform.position;
         EdPosition = CameraFin.transform.position;

    }

    private void Update()
    {
        stPosition = CameraDebut.transform.position;

        //var CameraTransformX = Mathf.Lerp(stPosition.x, EdPosition.x, NextMove);
        //var CameraTransformY = Mathf.Lerp(stPosition.y, EdPosition.y, NextMove);
        NextMove += 0.5f * Time.deltaTime;


    }


    public void StepCameraMove()
    {
          keyHit = true;
          EdPosition = new Vector3(stPosition.x, EdPosition.y, Camera.transform.position.z);

        Camera.transform.position = EdPosition;
        


        return;
    }


}


