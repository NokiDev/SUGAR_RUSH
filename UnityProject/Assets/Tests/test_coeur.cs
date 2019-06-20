using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_coeur : MonoBehaviour
{
    public int hp;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (hp > 0)
                collision.GetComponent<Damageable>().Heal((uint)hp);
            else
                collision.GetComponent<Damageable>().DealDamage((uint)Mathf.Abs(hp));
        }
    }
}
