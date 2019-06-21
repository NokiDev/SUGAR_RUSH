using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorInput : MonoBehaviour
{
    public Color initialColor;
    public Color startColor = Color.black;
    public bool canTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTrigger)
        {

            StartCoroutine(stepAndWait(initialColor));



            GetComponent<SpriteRenderer>().color = Color.white;
        }

    }

    IEnumerator stepAndWait(Color initialColor)
    {
        transform.parent.GetComponent<Simon_miniGam>().PlayerStep(initialColor);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().color = initialColor;

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().color = initialColor;
    }
}
