using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinding : MonoBehaviour
{
    public Transform destination;
    public UnityEngine.AI.NavMeshAgent agent;
    private Enemy enemy;
    
    private void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        agent.enabled = true;
    }

    // Setting TargetPoint to agent
    void Update()
    {
        if (destination != null && !enemy.isDead)
        {
            agent.SetDestination(destination.position);
            
        }
    }

    // When enemy reachs the banner
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Target")
        {
            enemy.currentHealth = 0;
        }
    }
}
