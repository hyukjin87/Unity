using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCam : MonoBehaviour
{
    public GameObject Cam;
    // Start is called before the first frame update
    void Start()
    {
        // Find the camera GameObject with the "MainCamera" tag
        Cam = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        // Set the rotation of the object to match the rotation of the camera
        transform.rotation = Cam.transform.rotation;
    }

}
