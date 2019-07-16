using UnityEngine;

public class TrajectoryScript : MonoBehaviour
{
    public GameObject dot;
    public GameObject dotParent;
    private Quaternion playerRotation;
    public GameObject playerObject;
    public float maxDotAngle = 200;
    public int dotCount;
    private GameObject[] dots;
    public bool showTrajectoryActive;
    private Vector3 originalDotPos;
    private Quaternion throwStartRot;

    void Start()
    {
        dots = new GameObject[dotCount];
        originalDotPos = new Vector3(-100, 100, -100);
        createDots(originalDotPos);
    }
    void Update()
    {
        if (showTrajectoryActive)
        {
            throwStartRot = Quaternion.Euler(playerObject.transform.rotation.eulerAngles.x,
            playerObject.transform.rotation.eulerAngles.y,
            playerObject.transform.rotation.eulerAngles.z);
            showTrajectory();
        }
        else
        {
            resetDots();
        }
    }

    private void createDots(Vector3 pos)
    {
        for (float i = 0; i < dotCount; i++)
        {
            float alpha = 1 - (i / dotCount);
            dots[(int)i] = Instantiate(dot, pos, Quaternion.Euler(0, 0, 0), dotParent.transform);
            dots[(int)i].GetComponent<MeshRenderer>().material.SetVector("_alpha", new Vector4(0, 0, 0, alpha));
        }
    }

    private void resetDots()
    {
        for (int i = 0; i < dotCount; i++)
        {
            dots[i].transform.position = originalDotPos;
        }
    }


    public void showTrajectory()
    {
        float axisMultiplier = GameManagerScript.axisMultiplier;

        float incrementationValue = maxDotAngle / dotCount;

        for (int i = 0; i < dotCount; i++)
        {
            Vector3 result = calcPos(i * incrementationValue) - new Vector3(10, 0, 0) * axisMultiplier;
            Quaternion rotatedVector = Quaternion.Euler(0, throwStartRot.eulerAngles.y + 90, 0);
            Vector3 direction = rotatedVector * result;
            dots[i].transform.position = direction;
        }

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
        return new Vector3(Mathf.Cos(time) * GameManagerScript.axisMultiplier,
                             0.15f,
                             zVal * GameManagerScript.axisMultiplier / 2) * 10;
    }
}