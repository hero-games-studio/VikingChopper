using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerScript : MonoBehaviour
{
    private int sceneCount;
    public GameObject levelFinished;
    [SerializeField]
    private GameObject confetti;

    private int totalTreeCount;
    public int cuttedTreeCount = 0;
    public float scale = 4.5f;

    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    void Start()
    {
        GameManagerScript.isLevelFinished = false;
        sceneCount = SceneManager.sceneCountInBuildSettings;
        totalTreeCount = FindObjectsOfType<BaseObstacle>().Length;
    }
    private void loadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != sceneCount - 1)
        {
            loadProcess();
            Invoke("invokableLoad", 2.5f);
        }
        else
        {
            loadProcess();
            Invoke("InvokableReload", 2.5f);
        }
    }

    private void loadProcess()
    {
        GameObject instance1 = Instantiate(confetti, spawnPoint1.transform.position, Quaternion.Euler(-90, 0, 0), gameObject.transform);
        instance1.transform.localScale = new Vector3(scale, scale, scale);
        GameObject instance2 = Instantiate(confetti, spawnPoint2.transform.position, Quaternion.Euler(-90, 0, 0), gameObject.transform);
        instance2.transform.localScale = new Vector3(scale, scale, scale);
        levelFinished.SetActive(true);
        GameManagerScript.isLevelFinished = true;
    }

    private void InvokableReload()
    {
        SceneManager.LoadScene(0);
    }

    public void checkSceneLoadCondition()
    {
        Debug.Log(cuttedTreeCount + "    " + totalTreeCount );
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
