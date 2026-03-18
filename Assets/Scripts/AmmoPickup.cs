using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 30;     // Amount of ammo this pickup gives

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAmmo playerAmmo = other.GetComponent<PlayerAmmo>();
            if (playerAmmo != null)
            {
                // Find the currently active weapon and get its weapon type
                string activeWeaponType = GetActiveWeaponType(other.gameObject);

                if (activeWeaponType != null)
                {
                    playerAmmo.AddReserveAmmo(activeWeaponType, ammoAmount);
                    Debug.Log("Added " + ammoAmount + " ammo for " + activeWeaponType);

                    // Update the ammo display on the currently active weapon
                    UpdateActiveWeaponDisplay(other.gameObject);

                    Destroy(gameObject);
                }
            }
        }
    }

    // Find which weapon is currently active and return its weapon type
    private string GetActiveWeaponType(GameObject player)
    {
        Gun ak = player.GetComponentInChildren<Gun>();
        Gun_Pistol pistol = player.GetComponentInChildren<Gun_Pistol>();
        Gun_Shotgun shotgun = player.GetComponentInChildren<Gun_Shotgun>();

        if (ak != null && ak.gameObject.activeSelf) return "Ak47";
        if (pistol != null && pistol.gameObject.activeSelf) return "Pistol";
        if (shotgun != null && shotgun.gameObject.activeSelf) return "Shotgun";

        return null;
    }

    // Update the ammo display on the currently active weapon
    private void UpdateActiveWeaponDisplay(GameObject player)
    {
        Gun ak = player.GetComponentInChildren<Gun>();
        Gun_Pistol pistol = player.GetComponentInChildren<Gun_Pistol>();
        Gun_Shotgun shotgun = player.GetComponentInChildren<Gun_Shotgun>();

        if (ak != null && ak.gameObject.activeSelf) ak.UpdateAmmoDisplay();
        if (pistol != null && pistol.gameObject.activeSelf) pistol.UpdateAmmoDisplay();
        if (shotgun != null && shotgun.gameObject.activeSelf) shotgun.UpdateAmmoDisplay();
    }
}