using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("destroySelf", 2f);
    }
    private void destroySelf()
    {
        Destroy(gameObject);
    }
}
