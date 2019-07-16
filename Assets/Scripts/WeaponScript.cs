using System.Collections;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    public bool activated;

    public float rotationSpeed;
    public float throwPower;
    private float passedTimeTillThrow = 0f;
    public int tearDropSineMultiplier = 3;
    public float timeMultiplier = 10;
    private float axisMultiplier = 1;
    private Quaternion throwStartRot;
    public GameObject playerObject;

    void Start()
    {
        GameManagerScript.tearDropSineMultiplier = tearDropSineMultiplier;
    }
    void Update()
    {
        if (activated)
        {
            transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Wall" && passedTimeTillThrow < 180)
        {
            //passedTimeTillThrow = 180;
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
        GameManagerScript.changeShowTrajectory(false);
        throwStartRot = Quaternion.Euler(playerObject.transform.rotation.eulerAngles.x,
        playerObject.transform.rotation.eulerAngles.y,
        playerObject.transform.root.eulerAngles.z);
        while (true)
        {
            if (passedTimeTillThrow >= 360f)
            {
                GameManagerScript.enableCatch();
                passedTimeTillThrow = 0f;
                GameManagerScript.changeShowTrajectory(true);
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
