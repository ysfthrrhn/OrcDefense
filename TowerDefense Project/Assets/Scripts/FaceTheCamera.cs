using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTheCamera : MonoBehaviour
{
    //Make This component face the camera at all times
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        transform.LookAt(2*transform.position - mainCamera.transform.position);

    }
}
