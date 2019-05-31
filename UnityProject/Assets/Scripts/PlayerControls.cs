using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float speed;
    private bool isSneaking;
    public float sneakModifier;
    private Rigidbody2D rigidbody2D;
    private Transform transform;
    private Vector3 lastDirection;
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        // Direction
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            direction += Vector3.down;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction += Vector3.up;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            direction += Vector3.right;
        }

        // Sneak
        if(!isSneaking && Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            isSneaking = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            isSneaking = false;
        }

        // Compute movement and rotation
        if(direction != Vector3.zero)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation =  Quaternion.AngleAxis(angle, Vector3.forward);

            float currentSpeed = isSneaking ? speed / sneakModifier : speed;
            rigidbody2D.velocity = Vector3.Cross(direction, Vector3.one * (currentSpeed / direction.magnitude) * Time.deltaTime);
            lastDirection = direction;
        }
        else
        {
            rigidbody2D.velocity = Vector3.zero;
        }
    }
}
