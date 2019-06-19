using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public float playerDetectionDistance = 5f;
    public float distanceFromStartPosition = 50f;
    public LayerMask layerMask;

    public delegate void PlayerDetectedCB(GameObject player);
    public PlayerDetectedCB onPlayerDetected;
    public PlayerDetectedCB onPlayerLost;

    public bool permanent;
    private GameObject player = null;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!permanent && player != null)
        {
            if (Vector3.Distance(gameObject.transform.position, startPosition) >= distanceFromStartPosition)
            {
                player = null;
                onPlayerLost(null);
            }
        }   
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var direction = collision.gameObject.transform.position - gameObject.transform.position;
            
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, direction, playerDetectionDistance, layerMask.value);
            if (hit.collider != null)
            {
                if(hit.collider.gameObject.tag == "Player")
                {
                    player = hit.collider.gameObject;
                    onPlayerDetected?.Invoke(player);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
    }



}
