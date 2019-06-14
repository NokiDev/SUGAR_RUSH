﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingScript : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private Rigidbody2D rigidbody2D;
    private Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move only if direction is not equal to Vector 0
        if (direction != Vector3.zero)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            rigidbody2D.velocity = Vector3.Cross(direction, Vector3.one * (speed / direction.magnitude) * Time.deltaTime);
        }
        else // reset the velocity
        {
            rigidbody2D.velocity = Vector3.zero;
        }
    }


    //Move at speed
    public void Move(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }
}
