using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RayShooter : MonoBehaviour
{
    /**
    Changes made to the script to display the ammo count.
    Using a textmeshpro to display text in a canvas. 
    The text displayed is changed when the game starts and 
    when the player shoots.
    **/

    // Boolean to lock the player from shooting
    private bool allowedToShoot;

    // Reference to the Camera
    private Camera cam;

    // Method for setting if the player is allowed to shoot
    public void SetShooting(bool b){
        allowedToShoot = b;
    }


    // Ammo counter variable
    private int ammo_count;
    public TextMeshProUGUI ammoCountText;

    // Method to display the ammo count in the unity canvas
    void SetAmmoCountText(){
        ammoCountText.text = "Ammo Left: " + ammo_count.ToString();
    }

    // Coroutine
    // This places a sphere at a set of coords then removes the sphere after 1 sec
    private IEnumerator SphereIndicator(Vector3 pos){
        // Create a new game object that is a sphere
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Then place it at the given coordinates
        sphere.transform.position = pos;

        // then wait one second
        yield return new WaitForSeconds(1);
        
        // Then destroy the sphere
        Destroy(sphere);
    }

    // OnGUI method
    // For drawing crosshairs on the screen
    private void OnGUI(){
        // Font Size
        int size = 15;

        // Coords where the crosshair is displayed
        float posX = cam.pixelWidth/2 - size/4;
        float posY = cam.pixelHeight/2 - size/2;

        // Draw the crosshairs as text
        GUI.Label(new Rect(posX, posY, size, size), "+");
    }


    // Start is called before the first frame
    void Start()
    {
        // set if allowed to shoot
        allowedToShoot = true;

        // Set amount of ammo to 5
        ammo_count = 25;
        // Use GetComponent to retrieve the camera object
        cam = GetComponent<Camera>();

        // Hide the mouse cursor 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set the ammo count in canvas to 5
        SetAmmoCountText();
    }

    // Update is called once per frame
    void Update()
    {
        // When player clicks left mouse button and ammo not 0
        if (Input.GetMouseButtonDown(0) && ammo_count !=0 && allowedToShoot){
            // Use a vector3 to store the location of the middle of the screen
            // Divide the width and height by 2 to get the midpoint; these become
            // the x and y values of the vector, with the z value being 0
            Vector3 point = new Vector3(cam.pixelWidth/2, cam.pixelHeight/2, 0);

            // Create a ray by calling ScreenPointToRay
            // Pass in the point as this is used as the origin for the ray
            Ray ray = cam.ScreenPointToRay(point);

            // Create a RaycastHit object to figure out wherethe ray hit 
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)){
                // For now, print the coord of where the ray hit 
                //Debug.Log("Hit: " + hit.point);

                // Get a reference to the object that was hit, then 
                // Get a reference to the object's ReactiveTarget Script,
                // if there is one.
                GameObject hitObject = hit.transform.gameObject;
                ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();

                // If the ray hit an enemy, (that is, if the "target" is not null),
                // indicate that an enement was hit. Otherwise place a sphere.
                if (target != null){
                    //Debug.log("Target hit!");
                    target.ReactToHit();
                }
                else{
                    StartCoroutine(SphereIndicator(hit.point));
                }
                
                
                
            }
        // Decrement the ammo count and display the change in the canvas
        ammo_count--;
        SetAmmoCountText(); 
        }    
    }
}
