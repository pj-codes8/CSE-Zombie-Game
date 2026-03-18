
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Zombie : MonoBehaviour
{
    // Zombie Varaibles
    private Animator anim;                              // Animator
    public NavMeshAgent agent;                          // variable for zombie nav mesh agent
    public Transform player;                            // variable for the players coordinates
    public LayerMask Ground, Player;                    // layers for the ground and player for the navmesh 
    private bool isAlive;                                                           // bool for whether the zombie is still alive
    public float PatrolSpeed, ChaseSpeed, PatrolAcceleration, ChaseAcceleration;    // Zombie movement speeds

    //Patroling
    public Vector3 walkPoint;                           // Zombie walk destination
    bool walkPointSet;                                  // bool for whether a walkpoint is set
    public float walkPointRange;                        // range for the walkpoint

    //Attacking
   bool alreadyAttacked;
   public int damage;
   public float timeBetweenAttacks;

    // Zombie Ranges 
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Start()
    {
        isAlive = true;                                 // zombie is alive at game start
        PatrolSpeed = 3.0f;                             // zombie patrol speed
        ChaseSpeed = 6.0f;                              // zombie chase speed
        PatrolAcceleration = 10.0f;                     // patrol acceleration. How quickly will reach max speed
        ChaseAcceleration = 20.0f;                      // chase acceleration. How quickly will reach max speed
        walkPointRange = 10.0f;                         // range for settings a walkpoint
        sightRange = 10.0f;                             // range of zombie's sight
        attackRange = 1.5f;                             // attack range of the zombie
        alreadyAttacked = false;                        // zombie not already attacked at game start
        timeBetweenAttacks = 1.0f / 3.0f;               // 1 / n seconds between attacks = 3 attacks per second = 0.3f
        damage = 5;                                     // Damage a zombie does 
        agent = GetComponent<NavMeshAgent>();                   // get a reference of the zombie nav mesh agent
        player = GameObject.Find("PlayerObject").transform;     // get a reference of the players coordinates

        anim = GetComponentInChildren<Animator>();              //Get a reference of the animator component
        if (anim)                                               
        {
            anim.SetBool("Chasing", false);
        }
    }
    
    private void Update()
    {
        if (isAlive){                                                                           // If zombie is alive check its sight ranges
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);   // Check if player is within the radius of zombie sight range
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player); // Check if player is within the radius of zombie attack range

            if (!playerInSightRange && !playerInAttackRange) Patroling();                       // If player not found patrol
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();                      // If player found but not in attack range, chase player
            if (playerInAttackRange && playerInSightRange) AttackPlayer();                      // If player in attack range, attack player
        }
    }

    private void Patroling()
    {
        anim.SetBool("Chasing", false);
        agent.speed = PatrolSpeed;                                                              // max speed for patroling
        agent.acceleration = PatrolAcceleration;                                                // acceleration is how fast it will reach max  speed 
        if (!walkPointSet) SearchWalkPoint();           // Continue making comments from here

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)                                                 //Walkpoint reached
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, Ground))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        anim.SetBool("Chasing", true);
        agent.SetDestination(player.position);
        agent.speed = ChaseSpeed;
        agent.acceleration = ChaseAcceleration;
        
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            //agent.isStopped = true;                                                             // Keep zombie in place while attacking
            //agent.velocity = Vector3.zero;                                                      // set nav agent velocity to zero
            //anim.SetBool("Attacking", true);                                                    // Animation for attacking the player
            PlayerCharacter playerChar = player.GetComponent<PlayerCharacter>();                // Get a reference to the PlayerCharacter
            if (playerChar != null)
            {
                playerChar.Hurt(damage);                                                        // if PlayerCharacter exists damage player
            }

            alreadyAttacked = true;                                                             
            Invoke(nameof(ResetAttack), timeBetweenAttacks);                                    // Reset the attack after a delay 
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        //anim.SetBool("Attacking", false);                                                       // Stop animation for attacking the player
        //agent.isStopped = false;
        //agent.velocity = agent.desiredVelocity; // restore velocity back to where the agent wants to go
    }

    // Public method to set to alive by outside scritps
    public void SetAlive(bool alive){
        isAlive = alive;   
        if (!isAlive){
            agent.SetDestination(transform.position);                                         // Keep zombie nav agent in place after being killed
        } 
    }
}
