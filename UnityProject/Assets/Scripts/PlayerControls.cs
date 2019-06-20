using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float defautSpeed;
    public float sneakModifier;
    private MovingScript movingScript;
    private bool blockControls;
    private Damageable isDamageable;
    private void Awake()
    {
        movingScript = GetComponent<MovingScript>();
        isDamageable = GetComponent<Damageable>();
        if (isDamageable)
        {
            isDamageable.onHit += new Damageable.HitCb(BlockControl);
            isDamageable.onDeath += new Damageable.DeathCb(() => { BlockControl(199999f, 0); });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!blockControls)
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
            float currentSpeed = defautSpeed;
            if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                currentSpeed = defautSpeed / sneakModifier;
            }

            movingScript.Move(direction, currentSpeed);
        }
    }

    void BlockControl(float timer, uint health)
    {
        BlockFor(timer);
        movingScript.Stun(timer);
    }

    IEnumerator BlockFor(float timer)
    {
        blockControls = true;
        yield return new WaitForSeconds(timer);
        blockControls = false;
    }
}
