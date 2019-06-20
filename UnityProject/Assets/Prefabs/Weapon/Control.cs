using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    private Transform transform;
    public GameObject bullet;

    public float bullet_speed;

    private Vector3 mouse_pos;
    private Vector3 weapon_pos;
    private Vector3 weapon_vect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        weapon_pos = gameObject.transform.position;
        weapon_vect = mouse_pos - weapon_pos;

        float rot = Mathf.Atan2(weapon_vect.y, weapon_vect.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot + 90);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Shot();
        
    }

    void Shot()
    {

        var shot_vector = (Vector2)(weapon_vect);
        shot_vector = shot_vector.normalized * bullet_speed;

        var bullet_start_position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        var b = Instantiate(bullet, bullet_start_position, Quaternion.identity, gameObject.transform.parent.parent);

        b.GetComponent<Rigidbody2D>().velocity = (shot_vector);
    }
}
