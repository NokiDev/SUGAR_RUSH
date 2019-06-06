using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Station : MonoBehaviour, IPointerClickHandler
{
    public string name;
    public GameObject image;
    public List<GameObject> lines;

    public bool isHere = false;
    public bool isActive = false;
    public bool isSelect = false;
    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;


        if (isHere)
        {
            gameObject.GetComponent<CanvasRenderer>().SetColor(new Color(0.4f, 0.4f, 0.9f, 1f));
        }
        else if (isSelect)
        {
            gameObject.GetComponent<CanvasRenderer>().SetColor(new Color(0.4f, 0.9f, 0.4f, 1f));
        }
        else
        {
            float opacity = isActive ? ((Mathf.Cos(timer * 6) + 1) / 4) + 0.5f : 0.3f;
            gameObject.GetComponent<CanvasRenderer>().SetColor(new Color(1f, 1f, 1f, opacity));
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isActive && !isHere)
        {
            Debug.Log("CLICK" + eventData);
            foreach (GameObject station in GameObject.FindGameObjectsWithTag("station"))
            {
                station.GetComponent<Station>().isSelect = false;
            }
            isSelect = true;
        }
    }
}
