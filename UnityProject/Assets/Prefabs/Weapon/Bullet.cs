﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int collision_counter_limit = 0;
    private int collision_counter = 0;
    public uint bullet_damage = 1;


    public ParticleSystem wall_collision_particule;
    public ParticleSystem human_collision_particule;


    // Update is called once per frame
    void Update()
    {

        float rot = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot + 180);

        if (collision_counter >= collision_counter_limit){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision_counter++;

        var bullet_effect = Instantiate(wall_collision_particule, gameObject.transform.position, Quaternion.identity, gameObject.transform.parent.parent);
        var effect_transform = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        bullet_effect.transform.position = effect_transform;

        float rot = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
        bullet_effect.transform.rotation = Quaternion.Euler(0f, 0f, rot -90);

        bullet_effect.Play();
        
        var damageable = collision.gameObject.GetComponent<Damageable>();
        if(damageable != null)
        {
            damageable.DealDamage(bullet_damage);
        }
    }

}
