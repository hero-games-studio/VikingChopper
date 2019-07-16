using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameManagerScript : MonoBehaviour
{
    [SerializeField]
    private SceneManagerScript sceneManage;
    [SerializeField]
    private CameraMovementScript cameraController;

    public static CameraMovementScript cameraMovementScript;
    public static SceneManagerScript sceneManagerScript;

    [SerializeField]
    private ThrowController throwController;
    public static ThrowController throwControllerScript;

    public static int tearDropSineMultiplier;

    private static TrajectoryScript trajectory;

    public RectTransform panelTransform;
    private static RectTransform levelFinishedPanel;
    private static int cuttedTreeCount = 0;
    private static int totalTreeCount = 0;
    void Start()
    {
        trajectory = gameObject.GetComponent<TrajectoryScript>();
        levelFinishedPanel = panelTransform;
        sceneManagerScript = sceneManage;
        throwControllerScript = throwController;
        RenderSettings.fog = false;
        Application.targetFrameRate = 60;
        cameraMovementScript = cameraController;
    }

    private static void findTotalTreeCount()
    {
        var objects = FindObjectsOfType<TreeCutScript>();
        totalTreeCount = objects.Length;
    }

    public static bool checkTapping()
    {
        if (CameraMovementScript.isTapped)
        {
            CameraMovementScript.isTapped = false;
            return true;
        }
        return false;
    }
    public static void incrementCuttedTreeCount()
    {
        if (totalTreeCount == 0)
            findTotalTreeCount();
        cuttedTreeCount++;
        if (cuttedTreeCount == totalTreeCount)
        {
            showLevelFinished();
            sceneManagerScript.loadNextScene();
            totalTreeCount = 0;
            cuttedTreeCount = 0;
        }
    }

    public static void changeShowTrajectory(bool set)
    {
        trajectory.showTrajectoryActive = set;
    }

    public static float axisMultiplier;

    private static void showLevelFinished()
    {
        Debug.Log("entered");
        levelFinishedPanel.DOAnchorPos(new Vector2(0, 0), 1f);
    }

    public static void enablePulling()
    {

        throwControllerScript.WeaponStartPull();
    }

    public static void enableCatch()
    {
        throwControllerScript.WeaponCatch();
    }

}
