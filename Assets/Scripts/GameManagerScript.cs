using UnityEngine;
public class GameManagerScript : MonoBehaviour
{
    //Non static variables
    [SerializeField]
    private SceneManagerScript sceneManager;
    private CameraMovementScript cameraController;
    [SerializeField]
    private ThrowController throwController;
    private RectTransform panelTransform;
    [SerializeField]
    private int tearDropSineMultiplierNonStatic;
    [SerializeField]
    private TrajectoryScript trajectoryNonStatic;
    [SerializeField]
    private WeaponScript weaponScriptNonStatic;
    [SerializeField]
    private TrailRenderer trailRenderer;

    //Camera controller script as static
    public static CameraMovementScript cameraMovementScript;

    //Variables for Throwing and trajectory displaying with static property for static access
    public static ThrowController throwControllerScript;
    public static int tearDropSineMultiplier;
    private static TrajectoryScript trajectory;
    public static float axisMultiplier = 1;
    public static bool hasWeapon = true;
    public static WeaponScript weaponScript;
    public static TrailRenderer weaponTrailRenderer;


    //Variables for scene manager for static access
    public static SceneManagerScript sceneManagerScript;
    private static RectTransform levelFinishedPanel;
    private static int cuttedTreeCount = 0;
    private static int totalTreeCount = 0;
    void Start()
    {
        weaponTrailRenderer = trailRenderer;
        weaponScript = weaponScriptNonStatic;
        Application.targetFrameRate = 60;
        levelFinishedPanel = panelTransform;
        sceneManagerScript = sceneManager;
        throwControllerScript = throwController;
        cameraMovementScript = cameraController;
        trajectory = trajectoryNonStatic;
        tearDropSineMultiplier = tearDropSineMultiplierNonStatic;
    }
    public static void setShowTrajectory(bool set)
    {
        trajectory.showTrajectoryActive = set;
        if (!set)
        {
            trajectory.resetDots();
        }
    }
    public static void enableCatch()
    {
        throwControllerScript.WeaponCatch();
    }
    public static void throwWeapon()
    {
        throwControllerScript.setAim(true);
    }
    public static void setWeaponPulling()
    {
        throwControllerScript.WeapoonPulling();
    }
    public static void incrementCuttedTreeCount()
    {
        sceneManagerScript.addTreeCut();
        sceneManagerScript.checkSceneLoadCondition();
    }

    public static void enableSpinning()
    {
        weaponScript.enableSpinning();
    }

    public static void disableSpinning()
    {
        weaponScript.disableSpinning();
    }
    public static void enableTrailRenderer()
    {
        weaponTrailRenderer.enabled = true;
    }
    public static void disableTrailRenderer()
    {
        weaponTrailRenderer.enabled = false;
    }
}
