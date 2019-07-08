using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
       void Start()
    {
        RenderSettings.fog=false; 
        Application.targetFrameRate=60;
    }
}
