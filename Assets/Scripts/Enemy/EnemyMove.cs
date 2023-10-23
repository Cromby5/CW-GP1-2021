using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    private GameObject player;
    NavMeshAgent agent;

    public float maxVisionRange;
    [SerializeField] private bool canSeePlayer;
    public LayerMask playerMask;

    // Point to go to when not actively tracking the player
    public Vector3 roamPath;
    public float roamDistance;
    private bool hasRoamPath;

    [SerializeField] private float damage;
    void Start()
    {
        hasRoamPath = false;
        player = References.player;
        // Get the nav mesh agent
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        canSeePlayer = Physics.CheckSphere(transform.position, maxVisionRange, playerMask); // Checks if the enemy can see the player in their vision range represented by a sphere
        if (canSeePlayer)
        {
            GoTowardsPlayer();
        }
        else
        {
            // Enemy will start to walk around the nav mesh at random points
            Roam(); 
        }
            
    }

    void GoTowardsPlayer()
    {
        // Make the agent go towards the players location
        if (player != null)
        { 
            agent.SetDestination(player.transform.position); 
        }
        else
        {
            player = References.player;
        }
    }
    void Roam()
    {
        // If the enemy does not have a destination to roam to get a random point inside a sphere and check with the nav mesh that it is an area the enemy can get to
        if(!hasRoamPath)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * roamDistance;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPoint, out hit, roamDistance, NavMesh.AllAreas); // Finds the nearest point on the navmesh specificed with the range
            roamPath = hit.position;
            hasRoamPath = true;
        }
        else
        {
            // Go towards the destination
            agent.SetDestination(roamPath);
            Vector3 distanceToDestination = transform.position - roamPath;
            // If our distance to the destination is less than x set roam path to false to create a new point for the enemy to go towards
            if (distanceToDestination.magnitude < 10f)
            {
                hasRoamPath = false;
            }

        }
       
    }
    private void OnCollisionEnter(Collision collision)
    {

        GameObject HitObject = collision.gameObject;

        // Checks our hit object for a rigidbody and a player health system
        if (HitObject.GetComponent<PlayerMoveRB>() != null)
        {
            HealthSystem thierHealthSystem = HitObject.GetComponent<HealthSystem>();
            if (thierHealthSystem != null)
            {
                thierHealthSystem.TakeDamage(damage); // Deal the damage set to their health system 
            }
        }
    }
}
