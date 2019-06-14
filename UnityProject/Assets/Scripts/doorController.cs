using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class doorController : MonoBehaviour
{
    public int keysRequired = 2;
    public GameObject helper;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<pocket>().useKeys(keysRequired))
            {
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(showHelper());
            }
        }
    }

    IEnumerator showHelper()
    {
        GameObject instanceHelper = Instantiate(helper);
        instanceHelper.GetComponentInChildren<Text>().text = keysRequired + " keys are required to open the door.";
        yield return new WaitForSeconds(3);
        Destroy(instanceHelper);
    }
}
