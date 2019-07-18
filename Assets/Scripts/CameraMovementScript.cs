using System.Collections;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private float cameraRotationSpeedMultiplier = 0.2f;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private GameObject PressIndicator;
    public static bool isTapped = false;
    private float angleSpace = 60;
    private float minAngle, maxAngle;
    private RaycastHit hit;
    private Vector3 firstDownPosition;

    private Vector3 initialForward;

    void Start()
    {
        float startrot = playerObject.transform.eulerAngles.y;
        minAngle = startrot - angleSpace / 2;
        maxAngle = startrot + angleSpace / 2f;
        initialForward = playerObject.transform.forward;
    }
    void Update()
    {
        mouseProcess();
    }



    //Get mouse position for player rotation
    private IEnumerator mouseSwirl()
    {
        PressIndicator.transform.position = getRaycastWorldPos();
        while (true)
        {
            float deltaPosition = Vector3.Magnitude(firstDownPosition - getRaycastWorldPos());


            Vector3 subtraction = hit.point - firstDownPosition;
            float angle = Vector3.SignedAngle(subtraction, initialForward, Vector3.up);
            playerObject.transform.eulerAngles = new Vector3(0, 90 - angle, 0);


            if (GameManagerScript.hasWeapon)
            {
                GameManagerScript.axisMultiplier = Mathf.Clamp(deltaPosition / 10f, 0.25f, 2.5f);
            }
            yield return null;
        }
    }

    //Get raycast point in the world space for trajectory pointing player rotating and throwing boomerang
    private Vector3 getRaycastWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask);
        return hit.point;
    }

    //Mouse input controls for using in unity for test purpose
    private void mouseProcess()
    {
        //Mouse pressed
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManagerScript.hasWeapon)
            {
                GameManagerScript.triggerPulling();
                firstDownPosition = getRaycastWorldPos();
                GameManagerScript.setShowTrajectory(true);
            }
            StartCoroutine("mouseSwirl");
        }
        //Mouse released
        else if (Input.GetMouseButtonUp(0))
        {
            if (GameManagerScript.hasWeapon)
            {
                //Trigger throw animation from game manager
                GameManagerScript.triggerThrowing();
                GameManagerScript.setShowTrajectory(false);
                PressIndicator.transform.position = new Vector3(-100, 100, -100);
            }
            StopCoroutine("mouseSwirl");
        }
    }

    //Touch controls
    private void touchProcess()
    {
        if (Input.touchCount > 0)
        {
            // GameManagerScript.changeShowTrajectory(true);
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000, 11);
            if (touch.phase == TouchPhase.Began)
            {
                firstDownPosition = hit.point;
            }
            Vector3 subtraction = hit.point - firstDownPosition;
            float angle = Vector3.SignedAngle(subtraction, playerObject.transform.forward, Vector3.up);
            gameObject.transform.eulerAngles = new Vector3(0, 180 - angle, 0);

            GameManagerScript.axisMultiplier = Mathf.Clamp(subtraction.magnitude, 2.5f, 7.5f);

            float rot = calculateRotation(firstDownPosition, touch.position);
            playerObject.transform.rotation = Quaternion.Euler(0, rot, 0);
        }
    }

    //Calculate 2d position difference for rotation
    private float calculateRotation(Vector2 beganPos, Vector2 currentPos)
    {
        float val = -(beganPos.x - currentPos.x);
        return val;
    }

    //Later use for limiting rotation of player object
    private Vector3 getLimitedRotation(Vector3 rotation)
    {
        Debug.Log(rotation);
        if (rotation.y < minAngle)
        {
            return new Vector3(0, minAngle, 0);
        }
        else if (rotation.y > maxAngle)
        {
            return new Vector3(0, maxAngle, 0);
        }
        else return rotation;
    }
}
