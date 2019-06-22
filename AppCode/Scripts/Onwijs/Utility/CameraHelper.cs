using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper : MonoBehaviour
{

    void Update()
    {
        transform.position = Camera.main.transform.position;
    }
}
