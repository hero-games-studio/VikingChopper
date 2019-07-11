using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    public bool activated;
    public Vector3 activeForce;
    private Rigidbody rb;

    public float rotationSpeed;
    public float throwPower;
    public Vector3 forwardVector;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (activated)
        {
            transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
            if(rb.velocity.magnitude<1){
                rb.AddForce(forwardVector * throwPower, ForceMode.Impulse);
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == 11)
        {
            rb.Sleep();

            rb.isKinematic = true;
            rb.collisionDetectionMode=CollisionDetectionMode.ContinuousSpeculative;
            GameManagerScript.enablePulling(gameObject.transform.position);
        }
        else
        {
            rb.AddForce(activeForce);
        }
    }
}
