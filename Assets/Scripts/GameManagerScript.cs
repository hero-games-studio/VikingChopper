using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField]
    private CameraMovementScript cameraController;

    public static CameraMovementScript cameraMovementScript;

    [SerializeField]
    private ThrowController throwController;
    public static ThrowController throwControllerScript;

    void Start()
    {
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
    private static int cuttedTreeCount = 0;
    private static int totalTreeCount = 0;
    public static void incrementCuttedTreeCount()
    {
        if (totalTreeCount == 0)
            findTotalTreeCount();
        cuttedTreeCount++;
        if (cuttedTreeCount == totalTreeCount)
        {
            if(SceneManager.GetActiveScene().buildIndex!=2)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            totalTreeCount = 0;
            cuttedTreeCount = 0;
        }
    }

    public static void enablePulling(Vector3 pos){
        throwControllerScript.WeaponStartPull();
    }

}
