using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinding : MonoBehaviour
{
    public Transform destination;
    public UnityEngine.AI.NavMeshAgent agent;
    private Enemy enemy;
    // Update is called once per frame
    private void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        agent.enabled = true;
    }
    void Update()
    {
        //targetPoint = GameObject.Find("TargetPoint");
        if (destination != null && !enemy.isDead)
        {
            agent.SetDestination(destination.position);
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Target")
        {
            enemy.currentHealth = 0;
        }
    }
}
