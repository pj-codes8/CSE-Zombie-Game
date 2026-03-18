using UnityEngine;

public class PlayerAmmo : MonoBehaviour
{
    // Reserve ammo for each weapon
    private int Ak47ReserveAmmo;
    private int pistolReserveAmmo;
    private int shotgunReserveAmmo;

    // Max reserve ammo for each weapon
    public int maxAk47ReserveAmmo = 300;
    public int maxPistolReserveAmmo = 300;
    public int maxShotgunReserveAmmo = 300;

    void Start(){
        // Set reserve ammo to max at game start
        Ak47ReserveAmmo = 50;
        pistolReserveAmmo = 25;
        shotgunReserveAmmo = 12;
    }

    // Get reserve ammo for a specific weapon
    public int GetReserveAmmo(string weaponType){
        switch (weaponType){
            case "Ak47": return Ak47ReserveAmmo;
            case "Pistol": return pistolReserveAmmo;
            case "Shotgun": return shotgunReserveAmmo;
            default: return 0;
        }
    }

    // Add reserve ammo for a specific weapon
    public void AddReserveAmmo(string weaponType, int amount){
        switch (weaponType){
            case "Ak47":
                Ak47ReserveAmmo = Mathf.Clamp(Ak47ReserveAmmo + amount, 0, maxAk47ReserveAmmo);
                break;
            case "Pistol":
                pistolReserveAmmo = Mathf.Clamp(pistolReserveAmmo + amount, 0, maxPistolReserveAmmo);
                break;
            case "Shotgun":
                shotgunReserveAmmo = Mathf.Clamp(shotgunReserveAmmo + amount, 0, maxShotgunReserveAmmo);
                break;
        }
    }

    // Spend reserve ammo for a specific weapon
    public void SpendReserveAmmo(string weaponType, int amount){
        switch (weaponType){
            case "Ak47":
                Ak47ReserveAmmo = Mathf.Clamp(Ak47ReserveAmmo - amount, 0, maxAk47ReserveAmmo);
                break;
            case "Pistol":
                pistolReserveAmmo = Mathf.Clamp(pistolReserveAmmo - amount, 0, maxPistolReserveAmmo);
                break;
            case "Shotgun":
                shotgunReserveAmmo = Mathf.Clamp(shotgunReserveAmmo - amount, 0, maxShotgunReserveAmmo);
                break;
        }
    }
}