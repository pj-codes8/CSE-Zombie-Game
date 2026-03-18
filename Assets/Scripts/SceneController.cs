using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneController : MonoBehaviour
{
    // Variables
    [SerializeField] private TMP_Text enemiesKilled;            // Text Display for enemies killed
    [SerializeField] private float countdown;                   // Countdown for when to spawn wave. Set in the inspector
    [SerializeField] private GameObject[] spawnPoints;          // array of spawn point for enemies to spawn at
    public Wave[] waves;                                        // Array of the amount of waves
                                     
    private GameObject enemy;                                   // Enemy reference
    private GameManager gameManager;                            // reference to gaem manager
    private int score;                                          //  score value. Amount of enemies killed
    public int currentWaveIndex = 0;                            // Integer to keep track of what wave the game is currently on
    private bool readyToCountDown;                              // Sets when to countdown for next wave

    // Score Tracker Funcction
    public void increaseScore(){                                                                        // Called by the Reactive Target when an enemy object is destroyed
        score++;
        enemiesKilled.text = $"Enemies Killed: "  + score.ToString();
    }

    // Enemy Spawner Function
    public GameObject SpawnNewEnemy(GameObject enemyPrefab, Vector3 position){                          // Method for spawning an enemy at a location
        GameObject enemy = Instantiate(enemyPrefab);                                                    // Create new enemy 
        float angle = Random.Range(0, 360);                     
        enemy.transform.Rotate(0, angle, 0);                                                           // ensure enemy is properly oriented
        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(position);                              // use warp to, place enemy at given position

        foreach(GameObject spawnPoint in spawnPoints){                                                 // Find the spawn point of an enemy. Set it as the parent
            if (spawnPoint.transform.position == position){
                enemy.transform.SetParent(spawnPoint.transform, true);
                break;
            }
        }
        return enemy;
    }

    // Wave Spawner Function
    private IEnumerator SpawnWave()
    {
        if (currentWaveIndex < waves.Length)                                                             // Only spawn if there are more waves left
        {
            for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)                             // for each enemy in the wave of that index 
            {
                //GameObject selectedSpawnPoint = spawnPoints[i % spawnPoints.Length];                      // Alternate the spawning between the different spawn points
               GameObject selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];       // Randomize which spawn point the enemy will spawn at
                SpawnNewEnemy(waves[currentWaveIndex].enemies[i], selectedSpawnPoint.transform.position); // Spawn enemy at the selected spawn point
                yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);               // Wait before spawning the next enemy
            }
        }
    }

    void Start(){ 
        score = 0;                                                                                      // Set the score to 0 at game start
        enemiesKilled.text = $"Enemies Killed: "  + score.ToString();                                   // Display the score to the canvas
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
    
        readyToCountDown = true;                                                                        // Start the countdown at game start
        for (int i = 0; i < waves.Length; i++)                                                          // Iterate through the wave array
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;                                             // Set the amount of enemies left to amount of enemies in the wave
        }
    }

    private void Update()
    {
        if (currentWaveIndex >= waves.Length)                                                           // If there are no more waves 
        {
            Debug.Log("You survived!");                                                                 // You Win!
            gameManager.Victory();                                                                      // Display victory screen
            return;                                                                                     // End this script
        }

        if (readyToCountDown == true)                                                                   // When ready to countdown
        {
            countdown -= Time.deltaTime;                                                                // count down
        }

        if (countdown <= 0)                                                                             // When countdown reaches 0
        {
            readyToCountDown = false;                                                                   // Stop counting down 

            countdown = waves[currentWaveIndex].timeToNextWave;                                         // Wait before starting the next wave, Set time in inspector

            StartCoroutine(SpawnWave());                                                                //  spawn in next wave
        }

        if (waves[currentWaveIndex].enemiesLeft == 0)                                                   // Once all enemies have been killed
        {
            readyToCountDown = true;                                                                     // Ready to countdown to next wave
            currentWaveIndex++;                                                                         // Move on to the next wave
        }
    }
}

// Wave Class
[System.Serializable]                           // Allows us to configure the waves in the unity inspector
public class Wave
{
    public GameObject[] enemies;                // Array of enemies to be spawned in each wave, set in the inspector
    public float timeToNextEnemy;               // Time in between when each enemy is spawned
    public float timeToNextWave;                // Time in between when each wave starts

    [HideInInspector] public int enemiesLeft;   // Keep track of the enemies still alive in the wave. When 0, countdown to next wave will start
}
