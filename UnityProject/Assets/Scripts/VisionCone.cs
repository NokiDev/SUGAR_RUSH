using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VisionCone : MonoBehaviour
{
    public bool isSeen = false;

    private Transform OwnTransform;
    public Transform targetTransform;
    private Quaternion StartRotation;
    private Vector3 direction;
    private float Angle;

    public class SeenObject{
        public string tag
        {
            get{ return tag; }
            set{if (value != null)
                {this.tag = value;}
                else {this.tag = "NC";}
            } }

        public float distanceobj { get; set; }
        public int number { get; set; }
        public Vector3 vector3 { get; set; }
        }
    

    private float offset = 90f;
    public ContactFilter2D FilterLayer;
    
    public float distance;

    public float lenght = 5f;
    public List<SeenObject> SeenObjects = new List<SeenObject>();


    // Start is called before the first frame update
    void Start()
    {
        //Rb2d = GetComponent<Rigidbody2D>();
        //Vector3 transformRight = transform.position + new Vector3(0.12f,0);

        StartRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        OwnTransform = GetComponent<Rigidbody2D>().transform;


        if (SeenObjects != null)
        {
            isSeen = true;
        }
        else
        {
            isSeen = false;
        }
        if (isSeen)
        {

            float i = SeenObjects.Min(u => u.distanceobj);



            Vector2 direction = targetTransform.position - new Vector3(transform.position.x, transform.position.y);

            direction.Normalize();
            var KeepedAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Angle = 180 + KeepedAngle;
            transform.rotation = Quaternion.Euler(Vector3.forward * (Angle + offset));


        }






    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, collision.transform.position);
        //if (hit.collider != null)
        //{

        //    isSeen = true;
        //    targetTransform = collision.GetComponent<Rigidbody2D>().transform;
        //}

        SeenObjects.Add(new SeenObject
        {
            tag = collision.tag,
            distanceobj = Vector3.Distance(collision.transform.position, transform.position),
            number = SeenObjects.Count + 1,
            vector3 = collision.GetComponent<Rigidbody2D>().position
        });

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        distance = Vector3.Distance(collision.transform.position, transform.position);

     
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isSeen = false;
        transform.rotation = StartRotation;
    }

    public void PolicemanAction(Collider2D collider)
    {

    }

}




//RaycastHit2D hit = Physics2D.Raycast(transform.position, collision.transform.position, lenght, ContactFilter2D.layerMask);
//if (hit.collider != null)
//{
//    distance = hit.distance;
//}

//RaycastHit2D hit = Physics2D.Raycast(transform.position,transform.up,lenght);
//Debug.DrawRay(transform.position, transform.up * lenght, Color.green);
