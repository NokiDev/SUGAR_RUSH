using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key_trigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Pocket>().addKey();

            Destroy(gameObject);
        }
    }
}
