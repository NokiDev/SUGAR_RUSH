﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public uint healthMax = 1;
    public float invicibilityTimer = 0.5f;
    public float timeBeforeDestroy = 1f;
    public delegate void DeathCb();
    public DeathCb onDeath;
    public delegate void HitCb(float invicibilityTimer, uint health);
    public HitCb onHit;
    public delegate void HealCb(uint health);
    public HealCb onHeal;

    public uint health = 1;
    private float hitTimer = 0.0f;

    private void Awake()
    {
        health = healthMax;
        onDeath += new DeathCb(OnDeath);
        onHit += (float invicibility, uint health) => { OnHit(); };
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        hitTimer += Time.deltaTime;
    }

    public void Heal(uint amount)
    {
        health += amount;
        if (health > healthMax)
        {
            health = healthMax;
        }
        onHeal?.Invoke(health);
    }

    public void DealDamage(uint damage)
    {
        if (hitTimer >= invicibilityTimer)
        {
            hitTimer = 0.0f;

            int currHealth = (int)(health) - (int)(damage);
            if (currHealth < 0)
            {
                health = 0;
            }
            else
            {
                health = (uint)currHealth;
            }
            if (health == 0)
            {
                onDeath?.Invoke();
            }
            else
            {
                onHit?.Invoke(invicibilityTimer, health);
            }
        }
    }

    void OnHit()
    {
        // Add blink;
        StartCoroutine("HitAnimation");
    }

    void OnDeath()
    {
        StartCoroutine("DeathAnimation");
    }

    IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);

        Destroy(gameObject);
        yield return null;
    }

    IEnumerator HitAnimation()
    {
        var sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.white;
        yield return new WaitForSeconds(invicibilityTimer / 4);
        sprite.color = Color.red;
        yield return new WaitForSeconds(invicibilityTimer / 4);
        sprite.color = Color.white;
        yield return new WaitForSeconds(invicibilityTimer / 4);
        sprite.color = Color.red;
        yield return new WaitForSeconds(invicibilityTimer / 4);
        sprite.color = Color.white;
    }
}
