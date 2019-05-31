using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedToMove : MonoBehaviour
{

    
    public bool isCollision = false;
    public GameObject TheCamera;

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isCollision = true;
            TheCamera.GetComponent<MoveCamera>().StepCameraMove();
            
        }
    }

    
    public void OnCollisionExit(Collision collision)
    {
        isCollision = false;    

    }
}
