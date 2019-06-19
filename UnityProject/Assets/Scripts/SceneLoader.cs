using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneAtIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadSceneWithName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
