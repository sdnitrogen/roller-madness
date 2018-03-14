using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkyboxCamera : MonoBehaviour
{
    // set the main camera in the inspector
    public Camera mainCamera;
   
    // set the sky box camera in the inspector
    public Camera skyCamera;
   
    // the amount to increment the rotation of the skyCamera
    // the larger the increment value the faster the rotation
    public float increment = 0.02f;

    private Vector3 skyBoxRotation;
    
    private float degrees = 0.0f;

    // Use this for initialization
    void Start()
    {
        if (skyCamera == null)
        {
            Debug.LogError("Please create a camera object to represent the Sky Camera and drag/drop the object on the skyCamera property in the inspector");
            return;
        }
        if (mainCamera == null)
        {
            Debug.LogError("Please drag/drop the Main Camera on the mainCamera property in the inspector");
            return;
        }
        if (skyCamera.depth >= mainCamera.depth)
        {
            Debug.Log("Set the skybox camera depth to a value 1 less than the main camera depth in inspector");
        }
        if (mainCamera.clearFlags != CameraClearFlags.Nothing)
        {
            Debug.Log("The main camera clearFlags needs to be set to \"Dont Clear\" in the inspector");
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        skyCamera.transform.position = mainCamera.transform.position;
        skyCamera.transform.rotation = mainCamera.transform.rotation;
        degrees += increment;
        if (degrees > 360)
        {
            degrees = 0;
        }
        skyBoxRotation = new Vector3(skyBoxRotation.x, degrees, skyBoxRotation.y);
        skyCamera.transform.Rotate(skyBoxRotation);
    }
}