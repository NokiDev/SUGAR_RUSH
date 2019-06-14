using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
   
    public GameObject Spawned;
    public float timeBeforeSpawn = 2f;
    public float CurrentTime = 0f;
    public bool WillSpawn = false;
    public GameObject FirstTarget;
    public int wantedNb;
    public int isNumber = 0 ;

    // Start is called before the first frame update
    void Start()
    {
        
    }
     
    // Update is called once per frame
    void Update()
    {
        CurrentTime += Time.deltaTime;
        WillSpawn = CurrentTime > timeBeforeSpawn;
        if (CurrentTime > timeBeforeSpawn)
        {
            if (isNumber < wantedNb)
            {
                GameObject g = Instantiate(Spawned, transform.position, transform.rotation);
                //g.GetComponent<NPC_Move>().Direction = FirstTarget;
                WillSpawn = false;
                CurrentTime = 0f;
                isNumber++;
            }
            
        }
        
        
    }
}
