using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("destroy_effect", this.GetComponent<ParticleSystem>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator destroy_effect(ParticleSystem bullet_effect)
    {
        yield return new WaitUntil(() => {return bullet_effect.isStopped; });
        //yield return new WaitForSeconds(1);
        GameObject.Destroy(this.gameObject);
    }
}
