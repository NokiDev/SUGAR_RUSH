using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public bool isSeen = false;
    public Transform targetTransform;
    public Transform OwnTransform;
    public Vector3 StartPosition;
    public Vector3 direction;
    public float Angle;

    private float offset = 90f;



    // Start is called before the first frame update
    void Start()
    {
        //Rb2d = GetComponent<Rigidbody2D>();
        //Vector3 transformRight = transform.position + new Vector3(0.12f,0);

        StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        OwnTransform = GetComponent<Rigidbody2D>().transform;


        if (isSeen)
        {
            
            Vector2 direction = targetTransform.position - new Vector3(transform.position.x, transform.position.y);

            direction.Normalize();
            var KeepedAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Angle = 180 + KeepedAngle;
            transform.rotation = Quaternion.Euler(Vector3.forward * (Angle + offset));
        }

      




    }


    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isSeen = true;
        targetTransform = collision.GetComponent<Rigidbody2D>().transform;
       
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isSeen = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isSeen = false;
        transform.position = StartPosition;
    }

}




//RaycastHit2D hit = Physics2D.Raycast(transform.position, collision.transform.position, lenght, ContactFilter2D.layerMask);
//if (hit.collider != null)
//{
//    distance = hit.distance;
//}
