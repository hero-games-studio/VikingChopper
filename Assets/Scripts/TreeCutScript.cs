using UnityEngine;

public class TreeCutScript : MonoBehaviour
{
    [SerializeField]
    private GameObject destructionParticle;
    [SerializeField]
    public GameObject breakable;

    private GameObject instanced;

    public GameObject cam;

    void OnTriggerEnter(Collider other)
    {
        instanced = Instantiate(breakable, transform.position, Quaternion.identity);
        if (other.transform.tag == "Weapon")
        {
            Rigidbody[] rbs = instanced.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                Vector3 spawnPoint = cam.transform.position - gameObject.transform.position;
                spawnPoint = spawnPoint / 10 + transform.position;
                Instantiate(destructionParticle, spawnPoint, Quaternion.identity);
                rb.AddExplosionForce(60, transform.position, 30);
            }
        }
        GameManagerScript.incrementCuttedTreeCount();
        Destroy(gameObject);
    }
}
