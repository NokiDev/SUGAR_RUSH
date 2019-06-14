using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public string name;
    public Color color;
    public List<GameObject> stations;
    public bool isActive = false;
    public bool sensA = true;

    public string tag;

    private List<GameObject> chucks;

    // Start is called before the first frame update
    void Start()
    {
        chucks = new List<GameObject>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            chucks.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float opacity = isActive ? 1f : 0.3f;

        foreach (GameObject chuck in chucks)
        {
            //chuck.GetComponent<CanvasRenderer>().SetColor(new Color(color.r, color.g, color.b, opacity));
            chuck.GetComponent<CanvasRenderer>().SetColor(new Color(1, 1, 1, opacity));
        }

        if (isActive)
        {
            if (!sensA) stations.Reverse();

            bool hasSelect = false;

            foreach (GameObject station in stations)
            {
                //Debug.Log("select " + station.GetComponent<Station>().isSelect);
                if (station.GetComponent<Station>().isHere)
                {
                    hasSelect = true;
                }
                else
                {
                    station.GetComponent<Station>().isActive = hasSelect;
                }
            }

            if (!sensA) stations.Reverse();
        }
    }
}
