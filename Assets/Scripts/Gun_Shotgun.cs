using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun_Shotgun : MonoBehaviour
{
    public float damage = 50f;                          // Gun Damage
    public float range = 30f;                          // Gun Range
    public float fireRate = 15f;                        // Gun Fire Rate
    private float nextTimeToFire = 0f;                  //
    private bool allowedToShoot;                        // Boolean to lock the player from shooting
    private int ammo_count;                             // Ammo count variable
    private int maxAmmo = 6;                           // Max ammount count variable

    private int pelletCount = 8;                       // Number of pellets per shot
    private float spreadAngle = 15f;                    // How wide the spread is in degrees

    private PlayerAmmo playerAmmo;                      // Reference to the players ammo reserves
    private string weaponType = "Shotgun";               // Weapon type identifier


    //private int reserveAmmo;                            // Ammo reserves

    //private int maxReserveAmmo = 25;                   // Max ammo Reserves
    public float reloadTime = 1f;                       // Time to reload
    private bool isReloading = false;                   // boolean to check if player is reloading

    public Camera fpsCam;                               // Reference to Player Camera
    public ParticleSystem ShotgunMuzzle;                  // Reference to the Muzzle FLash 
    //public GameObject impactEffect;                   // Reference to the Hit effect
    public TextMeshProUGUI ammoCountText;               // Reference to the ammo count display 

    // Crosshair expansion
    [SerializeField] private CrosshairController crosshairController;

    void Start(){
        playerAmmo = GetComponentInParent<PlayerAmmo>(); // Get PlayerAmmo
        fpsCam = GetComponentInParent<Camera>();                // Get access to the camera component
        allowedToShoot = true;                          // player allowed to shoot at game start
        ammo_count = maxAmmo;                           // Set ammo to 50
        //reserveAmmo = maxReserveAmmo;                   // Set reserve ammo to 100
        Cursor.lockState = CursorLockMode.Locked;       // Lock the mouse cursor
        Cursor.visible = false;                         // Hide the mouse cursor
        SetAmmoCountText();                             // Display ammo count in canvas

    }

    void Update()
    {    
        if (isReloading) return;                                                                            // Exit update entirely if reloading, nothing can happen during reload

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && ammo_count < maxAmmo && playerAmmo.GetReserveAmmo(weaponType) > 0)           // Reload when R is pressed, not already reloading or clip is not already full
        {
            StartCoroutine(Reload());
            return;
        }

        if(Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && ammo_count !=0 && allowedToShoot){     // Fire1 is the default left click in Unity
            nextTimeToFire = Time.time + 1f/ fireRate;                                                       // delay before shooting for fire rate
            Shoot();
        }
        
    }

    // Function for setting if the player is allowed to shoot
    public void SetShooting(bool b){
        allowedToShoot = b;
    }

    // FUnction for shooting
    void Shoot(){
    for (int i = 0; i < pelletCount; i++)
    {
        // Calculate a random spread direction for each pellet
        float spreadX = Random.Range(-spreadAngle, spreadAngle);
        float spreadY = Random.Range(-spreadAngle, spreadAngle);
        Vector3 spreadDirection = Quaternion.Euler(spreadX, spreadY, 0) * fpsCam.transform.forward;

        if (ShotgunMuzzle != null)
        {
            if (crosshairController != null) crosshairController.ExpandCrosshair();
            // Set the particle system rotation to match the pellet spread direction
            ShotgunMuzzle.transform.rotation = Quaternion.LookRotation(spreadDirection);

            // Emit a single particle in that direction
            ShotgunMuzzle.Emit(1);
        }

        RaycastHit hit;                                                 // Stores information about what we hit
        if (Physics.Raycast(fpsCam.transform.position, spreadDirection, out hit, range)){
            Debug.Log(hit.transform.name);
            ReactiveTarget target = hit.transform.GetComponent<ReactiveTarget>();
            if(target != null){
                target.TakeDamage(damage);                              // Each pellet deals damage individually
            }
        }
            //GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impact, 2f);
        }
                
        ammo_count--;                                               // Decrement the ammo count
        SetAmmoCountText();                                         // display the change in the canvas
    }
    // Method to display the ammo count in the unity canvas
    void SetAmmoCountText(){
        if (ammoCountText != null && gameObject.activeSelf && playerAmmo != null)
        {
            ammoCountText.text = "Ammo: " + ammo_count.ToString() + " / " + playerAmmo.GetReserveAmmo(weaponType).ToString();
        }
    }

    public void UpdateAmmoDisplay(){
    if (ammoCountText != null && gameObject.activeSelf && playerAmmo != null)
        ammoCountText.text = "Ammo: " + ammo_count.ToString() + " / " + playerAmmo.GetReserveAmmo(weaponType).ToString();
    }

/*
    public void AddAmmo(int ammo)
    {
        reserveAmmo += ammo;
        reserveAmmo = Mathf.Clamp(reserveAmmo, 0, maxReserveAmmo);
        SetAmmoCountText();
    }
*/
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        int ammoToFill = maxAmmo - ammo_count;                                     // Ammo needed to fill the clip
        int reserveAmmo = playerAmmo.GetReserveAmmo(weaponType);
        if (reserveAmmo >= ammoToFill)                                              // If enought ammo in reserves
        {
            playerAmmo.SpendReserveAmmo(weaponType, ammoToFill);                    // decrement the reserve count
            ammo_count = maxAmmo;                                                   // refill clip
        }
        else
        {
            ammo_count += reserveAmmo;                                              // If not enough to fill all the way, fill whatever left
            playerAmmo.SpendReserveAmmo(weaponType, reserveAmmo);                   // decrement reserve count
        }
        isReloading = false;
        SetAmmoCountText();
    }

}
