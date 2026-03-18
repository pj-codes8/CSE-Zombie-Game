using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
        
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        if(Input.GetAxis("Mouse ScrollWheel") > 0f){            // Scroll up
            if (selectedWeapon >= transform.childCount -1){    // if reached last weapon
                selectedWeapon = 0;                            // Wrap back around
            }
            else {
                selectedWeapon++;                              // select next weapon
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f){        // Scroll down
                if (selectedWeapon <= 0){                        //  if reached last weapon
                    selectedWeapon = transform.childCount -1;    //  wrap back around 
                }
                else{
                    selectedWeapon--;                             // select next weapon
                }
            }
        }   

        if (previousSelectedWeapon != selectedWeapon){             
            SelectWeapon();
        } 
    }

    void SelectWeapon(){
        int i = 0;
        foreach(Transform weapon in transform){
            if(i == selectedWeapon){
                weapon.gameObject.SetActive(true);
                // Update ammo display for whichever weapon is now active
                Gun ak = weapon.GetComponent<Gun>();
                Gun_Pistol pistol = weapon.GetComponent<Gun_Pistol>();
                Gun_Shotgun shotgun = weapon.GetComponent<Gun_Shotgun>();

                if (ak != null) ak.UpdateAmmoDisplay();
                if (pistol != null) pistol.UpdateAmmoDisplay();
                if (shotgun != null) shotgun.UpdateAmmoDisplay();
            }
            else{
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
