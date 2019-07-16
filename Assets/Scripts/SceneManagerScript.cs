using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerScript : MonoBehaviour
{
    private int sceneCount;

    private int treeCount;
    public int cuttedTreeCount = 0;
    void Start()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        treeCount = FindObjectsOfType<TreeCutScript>().Length;
    }
    private void loadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != sceneCount - 1)
        {
            Invoke("invokableLoad", 2f);
        }
    }

    public void checkSceneLoadCondition()
    {
        if (cuttedTreeCount == treeCount)
        {
            loadNextScene();
        }
    }

    public void addTreeCut()
    {
        cuttedTreeCount++;
    }

    private void invokableLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
