using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Input : MonoBehaviour
{
    public float speed = 3.3f;
    public float gravity = -9.8f;

    // bool to lock the player movement
    private bool allowedToMove;

    // Character COntroller Variable
    private CharacterController charController;

    // method to set whether player movement is allowed
    public void SetMovement(bool b){
        allowedToMove = b;
    }

    // Start is called before the first frame update
    void Start()
    {
        allowedToMove = true;
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (allowedToMove){
            float deltax = Input.GetAxis("Horizontal") * speed /* * Time.deltaTime*/;
            float deltaz = Input.GetAxis("Vertical") * speed  /* * Time.deltaTime*/;
            //transform.Translate(deltax, 0, deltaz);
            
            Vector3 movement = new Vector3(deltax, 0, deltaz);

            // Ensures the player does not move too fast
            movement = Vector3.ClampMagnitude(movement, speed);

            // Apply gravity
            movement.y = gravity;

            // Since speed * time equals distance,
            // multiply by Time.deltaTIme to move a certain amount within one frame.
            movement *= Time.deltaTime;

            // Transform movement from the local to global coordinates
            movement = transform.TransformDirection(movement);

            // Move the character controller using the Move() method
            charController.Move(movement);
        }
    }
}
