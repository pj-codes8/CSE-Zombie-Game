using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour{
    private SceneController sceneController;  
    public float health = 50f;    
     private bool isDead = false;                                        // Prevents Die() from being called multiple times
    [SerializeField] private GameObject AmmoBox;        // Reference to the ammo crate prefab, set in Inspector
    [Range(0f, 1f)]
    [SerializeField] private float ammoCrateDropChance; // 25% chance to drop an ammo crate, set in Inspector                  

    void Start(){
        sceneController = FindObjectOfType<SceneController>();                      // reference to the scene scontroller
    }

    // Enemy Damaged Function
    public void ReactToHit(){
        if (isDead) return;                                             // If already dead, do nothing
        isDead = true;                                                  // Mark as dead immediately
        StartCoroutine(Die());                                          // Kill enemy and adjust score and enemies left
    }

    // Death Animation, as a couroutine
    // Death Coroutine
    public IEnumerator Die(){
        yield return new WaitForSeconds(0.2f);                         // Wait

        Zombie enemyBehaviour = GetComponent<Zombie>();                 // Get reference to the Zombie Script
        if (enemyBehaviour != null){
            enemyBehaviour.SetAlive(false);                             // Stop the zombie moving after the delay
        }

        sceneController.increaseScore();                                // Increase the score for amount of enemies killed
        sceneController.waves[sceneController.currentWaveIndex].enemiesLeft--;  // When enemy dies, decrease amount of enemies left for that wave
        TrySpawnAmmoCrate();                                            // Roll for ammo crate drop
        Destroy(this.gameObject);                                       // Despawn enemy object. Enemy is dead
    }


    // Updated Enemy Damaged Function
    public void TakeDamage(float damageTaken){
        if (isDead) return;                                             // If already dead ignore any further damage
        health -= damageTaken;
        if(health <= 0f){
            isDead = true;                                              // Mark as dead IMMEDIATELY before starting coroutine
            StartCoroutine(Die());                                      // Start death coroutine
        }
    }

    // Randomly spawn an ammo crate at the enemy's location on death
    private void TrySpawnAmmoCrate(){
        if (AmmoBox == null) return;                    // If no ammo crate prefab assigned, skip

        float roll = Random.Range(0f, 1f);                      // Roll a random number between 0 and 1
        if (roll <= ammoCrateDropChance)                        // If roll is within the drop chance
        {
            // Raycast downward from the enemy position to find the ground
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                // Offset the spawn position upward so the crate sits on the ground
                Vector3 spawnPosition = hit.point + new Vector3(0, 0.3f, 0);
                Instantiate(AmmoBox, spawnPosition, Quaternion.identity);
                Debug.Log("Ammo crate dropped!");
            }
            else
            {
                // Fallback if raycast fails, just use enemy position
                Instantiate(AmmoBox, transform.position, Quaternion.identity);
                Debug.Log("Ammo crate dropped!");
            }
        }
    }
}