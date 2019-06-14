using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextPosition : MonoBehaviour
{

    public GameObject NextPosition1;
    public GameObject NextPosition2;
    public GameObject NextPosition3;
    public string NPCtag;
    public GameObject WillBe;
    public float choice;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         choice = Random.Range(1, 4);

    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
   
        switch (choice)
        {
            case 1:
                WillBe = NextPosition1;
                break;
            case 2:
                WillBe = NextPosition2;
                break;
            case 3:
                WillBe = NextPosition3;
                break;

        }
        Debug.Log("Coucou1");

        if (collision.gameObject.tag == NPCtag)
        {
            Debug.Log("Coucou2");

            collision.GetComponent<NPC_Move>().Direction = WillBe;
            //collision.gameObject.GetComponent<NPC_Move>().Direction = WillBe;

        }

    }
}
