using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerScript : MonoBehaviour
{
    private int sceneCount;

    //DOTween for level finished sign
    [SerializeField]
    private RectTransform panelTransform;

    private int totalTreeCount;
    public int cuttedTreeCount = 0;
    void Start()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        totalTreeCount = FindObjectsOfType<TreeCutScript>().Length;
    }
    private void loadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != sceneCount - 1)
        {panelTransform.DOAnchorPos(new Vector2(0,0),1f);
            Invoke("invokableLoad", 2.5f);
        }
    }

    public void checkSceneLoadCondition()
    {
        Debug.Log(cuttedTreeCount + "  " + totalTreeCount );
        if (cuttedTreeCount == totalTreeCount)
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
