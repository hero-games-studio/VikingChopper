using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerScript : MonoBehaviour
{
    private int sceneCount;
    void Start()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
    }
    public void loadNextScene()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        Debug.Log(sceneCount);
        if (SceneManager.GetActiveScene().buildIndex != sceneCount)
        {
            Invoke("invokableLoad", 2f);
        }
    }

    private void invokableLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
