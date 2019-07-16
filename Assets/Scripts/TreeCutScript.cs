using UnityEngine;

public class TreeCutScript : MonoBehaviour
{
    public GameObject breakable;

    private GameObject instanced;

    void OnTriggerEnter(Collider other)
    {
        instanced = Instantiate(breakable, transform.position, Quaternion.identity);
        if (other.transform.tag == "Weapon")
        {
            Rigidbody[] rbs = instanced.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                rb.AddExplosionForce(30, transform.position, 30);
            }
        }
        GameManagerScript.incrementCuttedTreeCount();
        Destroy(gameObject);
    }
}
