using System.Collections;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    public bool activated;

    public float rotationSpeed;
    private float passedTimeTillThrow = 0f;
    public int tearDropSineMultiplier = 3;
    public float timeMultiplier = 10;
    private float axisMultiplier = 1;
    private Quaternion throwStartRot;
    public GameObject playerObject;

    void Start()
    {
        tearDropSineMultiplier = GameManagerScript.tearDropSineMultiplier;
    }

    public void enableSpinning()
    {
        StartCoroutine(spinBoomerang());
    }

    public void disableSpinning()
    {
        StopCoroutine(spinBoomerang());
    }

    IEnumerator spinBoomerang()
    {
        while (true)
        {
            transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }


    public void moveWeapon()
    {
        passedTimeTillThrow = 0f;
        axisMultiplier = GameManagerScript.axisMultiplier;
        StartCoroutine(incrementWeaponPos());
    }
    IEnumerator incrementWeaponPos()
    {
        GameManagerScript.setShowTrajectory(false);
        throwStartRot = playerObject.transform.rotation;
        while (true)
        {
            if (passedTimeTillThrow >= 300)
            {
                GameManagerScript.setWeaponPulling();
            }
            if (passedTimeTillThrow >= 360f)
            {
                GameManagerScript.enableCatch();
                passedTimeTillThrow = 0f;
                GameManagerScript.setShowTrajectory(true);
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
    private Vector3 getPos(float time)
    {
        Vector3 result = calcPos(time);

        result += new Vector3((-10) * axisMultiplier, 0, 0);
        Quaternion rotatedVector = Quaternion.Euler(0, throwStartRot.eulerAngles.y + 90, 0);
        Vector3 direction = rotatedVector * result;

        return direction;
    }
    private Vector3 calcPos(float time)
    {
        time = time * Mathf.Deg2Rad;
        float zVal;
        zVal = Mathf.Sin(time);
        for (int i = 0; i < tearDropSineMultiplier; i++)
        {
            zVal *= Mathf.Sin(time / 2);
        }
        return new Vector3(Mathf.Cos(time) * axisMultiplier, 0.15f, zVal * axisMultiplier / 2) * 10;
    }
}
