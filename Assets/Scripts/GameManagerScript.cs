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

    public RectTransform panelTransform;
    private static RectTransform levelFinishedPanel;
    private static int cuttedTreeCount = 0;
    private static int totalTreeCount = 0;
    void Start()
    {
        levelFinishedPanel=panelTransform;
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

    private static void showLevelFinished()
    {
        Debug.Log("entered");
        levelFinishedPanel.DOAnchorPos(new Vector2(0, 0), 1f);
    }

    public static void enablePulling(Vector3 pos)
    {

        throwControllerScript.WeaponStartPull();
    }

}
