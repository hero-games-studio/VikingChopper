using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private float cameraRotationSpeedMultiplier = 0.2f;
    private Vector3 cameraRotation;

    private float pressTime = 0f;
    private float tapLimit = 0.3f;
    private bool isCoroutineRunning = false;

    public static bool isTapped = false;
    private float angleSpace = 90;
    private float minAngle, maxAngle;



    void Update()
    {
#if UNITY_EDITOR
        mouseProcess();
#endif

#if UNITY_ANDROID
        touchProcess();
#endif
    }

    private IEnumerator mouseSwirl()
    {
        while (true)
        {
            float deltaRotation = Input.GetAxis("Mouse X");
            cameraRotation = new Vector3(0, deltaRotation * cameraRotationSpeedMultiplier, 0);
            playerObject.transform.eulerAngles = playerObject.transform.rotation.eulerAngles + cameraRotation;
            yield return new WaitForSecondsRealtime(1 / 60);
        }
    }

    private void mouseProcess()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isCoroutineRunning)
            {
                StartCoroutine("mouseSwirl");
                isCoroutineRunning = true;
            }
            pressTime += Time.deltaTime;
        }
        else
        {
            if (isCoroutineRunning)
            {
                StopCoroutine("mouseSwirl");
                isCoroutineRunning = false;
            }
            if (pressTime < tapLimit && pressTime > 0f)
            {
                isTapped = true;
            }
            pressTime = 0f;
        }
    }
    public static float currentRotation;
    private Vector2 lastPos;
    private float touchSpeedMultiplier = 0.08f;
    private void touchProcess()
    {
        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastPos = touch.position;
            }
            float rot = calculateRotation(lastPos, touch.position);

            pressTime += Time.deltaTime;
            cameraRotation = new Vector3(0, rot * touchSpeedMultiplier, 0);
            lastPos = touch.position;
            Vector3 res = getLimitedRotation(playerObject.transform.rotation.eulerAngles + cameraRotation);
            currentRotation=res.y;
            playerObject.transform.eulerAngles = res;
        }
        else if (pressTime < tapLimit && pressTime > 0f && Input.touchCount == 0)
        {
            isTapped = true;
            pressTime = 0f;
        }
        else
        {
            pressTime = 0f;
            isTapped = false;
        }
    }

    private float calculateRotation(Vector2 beganPos, Vector2 currentPos)
    {
        float val = -(beganPos.x - currentPos.x);
        return val;
    }

    private Vector3 getLimitedRotation(Vector3 rotation)
    {
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

    void Start()
    {
        float startrot = playerObject.transform.eulerAngles.y;
        minAngle = startrot - angleSpace / 2;
        maxAngle = startrot + angleSpace / 2f;
    }
}
