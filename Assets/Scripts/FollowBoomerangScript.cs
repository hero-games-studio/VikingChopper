using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBoomerangScript : MonoBehaviour
{
    private GameObject parentObject;
    void Update()
    {
        gameObject.transform.position=parentObject.transform.position;
    }
}
