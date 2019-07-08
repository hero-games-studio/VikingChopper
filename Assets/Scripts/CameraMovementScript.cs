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
    private float tapLimit = 0.15f;
    private bool isCoroutineRunning = false;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isCoroutineRunning)
            {
                StartCoroutine("processSwirl");
                isCoroutineRunning = true;
            }
            pressTime += Time.deltaTime;
        }
        else
        {
            if (isCoroutineRunning)
            {
                StopCoroutine("processSwirl");
                isCoroutineRunning=false;
            }
            if (pressTime < tapLimit && pressTime > 0f)
            {
                processTap();
            }
            pressTime = 0f;
        }
    }

    private IEnumerator processSwirl()
    {
        while (true)
        {
            float deltaRotation = Input.GetAxis("Mouse X");
            cameraRotation = new Vector3(0, deltaRotation * cameraRotationSpeedMultiplier, 0);
            playerObject.transform.eulerAngles = playerObject.transform.rotation.eulerAngles + cameraRotation;
            yield return new WaitForSecondsRealtime(1 / 60);
        }
    }

    private void processTap()
    {
        Debug.Log("Tapped");
    }
}
