using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Rotation Axes
    public enum RotationAxes{
        MouseXandY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXandY;

    // Sensitivity variables
    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;

    // Limits for Vertical Movement and angle for vertical rotation
    public float minimumVert = -45f;
    public float maximumVert = 45f;
    private float verticalRot = 0;

    //Prevents the player object from tumbling around
    private void start(){
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null){
            body.freezeRotation = true;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (axes == RotationAxes.MouseX) {
            transform.Rotate(0, sensitivityHor * Input.GetAxis("Mouse X"), 0);
        }
        else if (axes == RotationAxes.MouseY) {
            verticalRot -= Input.GetAxis("Mouse Y") * sensitivityVert;
            verticalRot = Mathf.Clamp(verticalRot, minimumVert, maximumVert);

            float horizontalRot = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(verticalRot, horizontalRot, 0);
        }
        else {
            verticalRot -= Input.GetAxis("Mouse Y") * sensitivityVert;
            verticalRot = Mathf.Clamp(verticalRot, minimumVert, maximumVert);

            float delta = Input.GetAxis("Mouse X") * sensitivityHor;
            float horizontalRot = transform.localEulerAngles.y + delta;

            transform.localEulerAngles = new Vector3(verticalRot, horizontalRot, 0);
            
        }
        
    }
}
