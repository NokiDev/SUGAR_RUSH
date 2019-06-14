using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// temps 
public class grenade : MonoBehaviour
{
    public float timer = 2.0f;
    public float force = 150f;
    public float radius = 1;
    private Rigidbody2D R;

    private float time_elapsed = 0;
    private List<GameObject> targets = new List<GameObject>();


    void Start()
    {
        R = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        time_elapsed += Time.deltaTime;

        if (time_elapsed >= timer)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject)
            targets.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject)
            targets.Remove(collision.gameObject);

    }

    private void Explode ()
    {
        foreach (var o in targets) {
            var r = o.GetComponent<Rigidbody2D>();
            if (r != null)
            {
                var damageable = o.GetComponent<Damageable>();
                if(damageable)
                {
                    damageable.DealDamage(1);
                }

                r.AddForce((o.transform.position - gameObject.transform.position)* force, ForceMode2D.Impulse);
            }
       
        }
        Destroy(gameObject);
    }
}
