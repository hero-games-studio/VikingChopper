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
        if (SceneManager.GetActiveScene().buildIndex != sceneCount-1)
        {
            Invoke("invokableLoad", 2f);
        }
    }

    private void invokableLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
