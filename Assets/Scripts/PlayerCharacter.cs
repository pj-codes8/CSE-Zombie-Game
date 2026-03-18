using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    /*
    Changes made to this script to display a player healthbar using a slider.
    When the player runs out of health, calls the gameOver method from the GameManager script to
    display the game over screen
    */
    private int health;
    public int maxHealth = 100; 
    public Slider healthSlider;     // Slider used to display the player health bar 
    public GameManager gameManager; // Used to display the game over screen when the player runs out of health
    private bool isDead;            // Bool checked to prevent bugs with the game over screen

    // References to components to prevent player actions after death
    private FPS_Input fpsInput;
    private WeaponSwitching weaponSwitching;    // Reference to weapon switching to disable all guns on death

    // Start is called before the first frame update
    void Start()
    {
        // Player Health
        health = maxHealth;          
        healthSlider.maxValue = maxHealth;       // Set the max value for the healthbar 
        healthSlider.value = health;             // Set the value of the players health in the healthbar

        // Enable Player movement
        fpsInput = GetComponent<FPS_Input>();
        weaponSwitching = GetComponentInChildren<WeaponSwitching>(); // Get weapon switching component
    }

    // Take damage
    public void Hurt(int damage)
    {
        if(isDead) return;                                          // Exit if already dead

        health -= damage;                                           // Apply damage first
        health = Mathf.Clamp(health, 0, maxHealth);                 // Clamp health between 0 and max
        healthSlider.value = health;                                // Update the healthbar slider
        Debug.Log($"Health: {health}");

        if(health <= 0 && !isDead){                                            // Check for death after applying damage
            isDead = true;
            fpsInput.SetMovement(false);                            // Lock player movement

            // Disable shooting on all guns by disabling weapon switching
            if (weaponSwitching != null)
            {
                foreach (Transform weapon in weaponSwitching.transform)
                {
                        // Disable the entire weapons GameObject so all gun scripts stop running
                        weaponSwitching.gameObject.SetActive(false);

                }
            }

            gameManager.gameOver();                                 // Show game over screen
            Debug.Log("You are Dead");
        }
    }
}