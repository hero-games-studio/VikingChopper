using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private float cameraRotationSpeedMultiplier = 0.2f;
    private Vector3 cameraRotation;
    private float pressTime = 0f;
    private float tapLimit = 0.15f;
    private bool isCoroutineRunning = false;
    public static bool isTapped = false;
    private float angleSpace = 60;
    private float minAngle, maxAngle;
    public static float currentRotation;
    private Vector2 lastPos;
    private float touchSpeedMultiplier = 0.08f;



    void Update()
    {/*
#if UNITY_EDITOR
        mouseProcess();
#endif */

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
                GameManagerScript.axisMultiplier = 2f;
                GameManagerScript.changeShowTrajectory(false);
                isTapped = true;
            }
            pressTime = 0f;
        }
    }

    private Vector3 touchPos;
    private RaycastHit hit;
    private void touchProcess()
    {
        if (Input.touchCount > 0)
        {
            GameManagerScript.changeShowTrajectory(true);
            Touch touch = Input.GetTouch(0);




            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000, 11);
            if (touch.phase == TouchPhase.Began)
            {
                touchPos = hit.point;
            }
            Vector3 subtraction = hit.point - touchPos;
            float angle = Vector3.SignedAngle(subtraction, playerObject.transform.forward, Vector3.up);
            gameObject.transform.eulerAngles = new Vector3(0, 180 - angle, 0);

            GameManagerScript.axisMultiplier = Mathf.Clamp(subtraction.magnitude, 2.5f, 7.5f);

            float rot = calculateRotation(touchPos, touch.position);
            Debug.Log(GameManagerScript.axisMultiplier);
            playerObject.transform.rotation = Quaternion.Euler(0,rot,0);
        }
        else if (pressTime < tapLimit && pressTime > 0f && Input.touchCount == 0)
        {
            GameManagerScript.axisMultiplier = 2f;
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

    void Start()
    {
        float startrot = playerObject.transform.eulerAngles.y;
        minAngle = startrot - angleSpace / 2;
        maxAngle = startrot + angleSpace / 2f;
    }
}
