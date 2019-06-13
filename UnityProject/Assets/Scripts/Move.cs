using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour

{
    private Rigidbody2D rigidbody2D;
    private Transform transform;
    private List<Vector3> Positions;
    
    public float speed = 1.0f;
    public Vector3 test = new Vector3(1, 3, 0);

    public void AddPosition(Vector3 Position)
    {
        Positions.Add(Position);
    }

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
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, test, step);

        if (Vector3.Distance(transform.position, test) < 0.001f)
        {
            // Swap the position of the cylinder.
            test *= -1.0f;
        }
    }
}
