using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(GameObject.Find("GameManager"));
        GameObject.Destroy(GameObject.FindGameObjectWithTag("mapGenerator"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
