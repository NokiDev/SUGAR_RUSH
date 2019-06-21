using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{

    public MapGeneration generator;

    public string dotForm = "-";
    private Text text;
    public float dotTimer = 1f;
    private float currTimer = 0f;
    private int dotAmount = 0;
    // Start is called before the first frame update
    void Start()
    {

        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        currTimer += Time.deltaTime;
        if(currTimer >= dotTimer)
        {
            NextDot();
            currTimer = 0.0f;
        }
    }

    private void NextDot()
    {
        string txt = "Loading ";
        for (int dot=0; dot < dotAmount; ++dot)
        {
            txt += dotForm;
        }
        text.text = txt;
        if (dotAmount == 3)
            dotAmount = 0;
        else
            ++dotAmount;
    }
}
