using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    [SerializeField] TowerBase ParentTower;
    private Enemy enemy;
    [SerializeField] private DetectMode mode;
    [SerializeField] private List<Enemy> enemies;
    private Enemy selectedTarget; // This variable for store storngest enemy into targets.
    public Transform partToRotate;
    public float aimSpeed;
    public TowerMode Type;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0, 0.5f); // Updates target per 0.5 seconds
        mode = ParentTower.Mod;
    }


    private void Update()
    {
        // Checking is targets still alive
        foreach(Enemy enemy in enemies.ToArray())// Add listener for Next Version!!!!
        {
            if (enemy.currentHealth <= 0)
            {
                DeleteEnemy(enemy);
            }
        }

        // Aiming to target
        if (selectedTarget != null)
        {
            if(Type != TowerMode.Area)
            {
                Quaternion OriginalRot = partToRotate.rotation;
                partToRotate.LookAt(selectedTarget.transform.position);
                Quaternion NewRot =partToRotate.rotation;
                partToRotate.rotation = OriginalRot;
                partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, NewRot, aimSpeed * Time.deltaTime);
            }
            else if(Type == TowerMode.Area)
            {
                var lookPos = selectedTarget.transform.position - partToRotate.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                partToRotate.rotation = Quaternion.Slerp(partToRotate.rotation, rotation, Time.deltaTime * aimSpeed);
            }
        }
    }

    /// <summary>
    /// Sets current target by selected DetectMode.
    /// </summary>
    private void UpdateTarget()
    {
        switch (mode)
        {
            case DetectMode.Normal:
                selectedTarget = null;
                float shortestDistance = Mathf.Infinity;
                Enemy nearestEnemy = null;
                foreach (Enemy enemy in enemies)
                {
                    if (enemy != null)
                    {
                        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distanceToEnemy < shortestDistance)
                        {
                            shortestDistance = distanceToEnemy;
                            nearestEnemy = enemy;
                        }
                    }
                }

                if (nearestEnemy != null && shortestDistance <= ParentTower.range)
                {
                    selectedTarget = nearestEnemy;
                }
                else
                {
                    selectedTarget = null;
                }

                ParentTower.SetTarget(selectedTarget); 
                
                break;

            case DetectMode.Weakest:
                selectedTarget = null;
                float minHealth = Mathf.Infinity;
                foreach (Enemy enemy in enemies)
                {
                    if (enemy != null)
                    {
                        if (enemy.baseHealth < minHealth)
                        {
                            selectedTarget = enemy;
                            minHealth = selectedTarget.baseHealth;
                        }
                        else continue;
                    }
                }
                ParentTower.SetTarget(selectedTarget);
                break;

            case DetectMode.Strongest:
                selectedTarget = null;
                float maxHealth = 0;
                foreach (Enemy enemy in enemies)
                {
                    if (enemy != null)
                    {
                        if (enemy.baseHealth > maxHealth)
                        {
                            selectedTarget = enemy;
                            maxHealth = selectedTarget.baseHealth;
                        }
                        else continue;
                    }
                }
                ParentTower.SetTarget(selectedTarget);
                break;
        }
       
    }

    //PHYSICS OVERLAP

    //COLLIDERS 
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out enemy))
        {
            enemies.Add(enemy);
        }

    }

    // Removes enemy that is out of range, from targets
    private void OnTriggerExit(Collider other)
    {
        ParentTower.SetTarget(null);
        Enemy delete = other.GetComponent<Enemy>();
        DeleteEnemy(delete);
    }

    /// <summary>
    /// Changes Tower target detection type.
    /// </summary>
    /// <param name="m"></param>
    public void SetMode(DetectMode m) 
    {                                        
        mode = m;
    }

    /// <summary>
    /// Sets selected tower type.
    /// </summary>
    /// <param name="m"></param>
    public void SetType(TowerMode m) 
    {                                        
        Type = m;
    }

    /// <summary>
    /// Remove enemy from target list.
    /// </summary>
    /// <param name="enemy"></param>
    public void DeleteEnemy(Enemy enemy)
    {
        for(int i=0; i < enemies.Count; i++)
        {
            if(enemy == enemies[i])
            {
                enemies.Remove(enemy);
                if (ParentTower.GetTarget() == enemy)
                {
                    ParentTower.SetTarget(null);
                }
                return;
            }

        }
        

    }

}
