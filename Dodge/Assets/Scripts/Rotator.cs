using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 0f;
    public bool isRotate = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If rotation is enabled
        // Rotate the object around the Y-axis based on the rotation speed and deltaTime
        if (isRotate == true)
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }        
    }
}
