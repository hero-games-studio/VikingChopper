﻿using System.Collections;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    public bool activated;

    public float rotationSpeed;
    private float passedTimeTillThrow = 0f;
    public float timeMultiplier = 400;
    private Quaternion throwStartRot;
    public GameObject playerObject;

    private IEnumerator spinningCoroutine;

    private float originalTimeMultiplier;

    void Start()
    {
        originalTimeMultiplier = timeMultiplier;
        spinningCoroutine = spinBoomerang();
    }


    public void enableSpinning()
    {
        if (spinningCoroutine != null)
            StartCoroutine(spinningCoroutine);
    }

    public void disableSpinning()
    {
        if (spinningCoroutine != null)
        {
            StopCoroutine(spinningCoroutine);
        }
    }

    IEnumerator spinBoomerang()
    {
        while (true)
        {
            transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private Vector3 startPos;
    private Vector3 startRot;
    public void moveWeapon()
    {
        passedTimeTillThrow = 0f;
        timeMultiplier = originalTimeMultiplier * (1f + (2.5f - GameManagerScript.axisMultiplier));
        startPos = gameObject.transform.position;
        startRot = gameObject.transform.rotation.eulerAngles;
        StartCoroutine(incrementWeaponPos());
    }

    IEnumerator incrementWeaponPos()
    {
        GameManagerScript.setShowTrajectory(false);
        throwStartRot = playerObject.transform.rotation;
        bool isTriggered = false;
        while (true)
        {

            if (passedTimeTillThrow >= 300 && !isTriggered)
            {
                GameManagerScript.triggerIdle();
                isTriggered = true;
            }
            if (passedTimeTillThrow >= 360)
            {
                passedTimeTillThrow = 0f;
                GameManagerScript.disableSpinning();
                GameManagerScript.catchWeapon();
                GameManagerScript.setShowTrajectory(false);
                yield break;
            }
            else
            {
                passedTimeTillThrow += 0.005f * timeMultiplier;
                gameObject.transform.position = getPos(passedTimeTillThrow);
                yield return new WaitForSecondsRealtime(0.005f);
            }
        }
    }

    public Vector3 correctionVector;
    private Vector3 getPos(float time)
    {
        Vector3 result = calcPos(time);

        result += new Vector3((-10) * GameManagerScript.axisMultiplier, 0, 0);
        Quaternion rotatedVector = Quaternion.Euler(0, throwStartRot.eulerAngles.y + 90, 0);
        Vector3 direction = rotatedVector * result;

        return direction + correctionVector;
    }
    private Vector3 calcPos(float time)
    {
        time = time * Mathf.Deg2Rad;
        float zVal;
        zVal = Mathf.Sin(time);
        for (int i = 0; i < GameManagerScript.tearDropSineMultiplier; i++)
        {
            zVal *= Mathf.Sin(time / 2);
        }
        return new Vector3(Mathf.Cos(time) * GameManagerScript.axisMultiplier, 0.15f, zVal * GameManagerScript.axisMultiplier / 2) * 10;
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.GetComponent<TreeCutScript>()!=null){

        }
    }
}
