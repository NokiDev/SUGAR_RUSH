using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{
    public float blinkTimer = 2.0f;
    public float blinkduration = 0.2f;
    private float currentTimer = 0.0f;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();   
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;
        if(currentTimer >= blinkTimer)
        {
            currentTimer = 0.0f;
            StartCoroutine("Blink");
        }
    }

    IEnumerator Blink()
    {
        string txt = text.text;
        text.text = "";
        yield return new WaitForSeconds(blinkduration);
        text.text = txt;

    }
}
