using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{

    public float sceneWidth = 1080f;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.orthographicSize = sceneWidth / (float)Screen.width * (float)Screen.height / 2f;
    }
}
