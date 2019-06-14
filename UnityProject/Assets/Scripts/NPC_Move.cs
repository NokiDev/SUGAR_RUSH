using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Move : MonoBehaviour
{

    //recoit sa direction de Controller_NPC et move avec Lerp

    private GameObject direction;
    public float Speed;
    private Vector3 StartPos;
    private float timer = 0;
    public bool OnCollision = false;
    public float action1;
    public float action2;


    public GameObject Direction
    {
        get => direction;
        set
        {
            timer = 0;
            if (direction != null)
                StartPos = direction.transform.position;
            direction = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
      
        StartPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime * Speed;
        if (OnCollision)
        {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.zero);
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, -Vector3.Lerp(StartPos, direction.transform.position, timer), 0.5f));


        }
        else
        {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, Vector3.Lerp(StartPos, direction.transform.position, timer), 0.05f));

        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "NPC")
        {
            OnCollision = true;
        }
        

               
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "NPC")
        {
            OnCollision = false;
        }

       
    }



}
