using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockDownScript : BaseObstacle
{
    public Rigidbody rb;
    public float explosionForce = 135;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Weapon")
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            rb.AddExplosionForce(explosionForce, gameObject.transform.position, 45);
            GameManagerScript.obstacleDestroyed();
        }
    }
}
