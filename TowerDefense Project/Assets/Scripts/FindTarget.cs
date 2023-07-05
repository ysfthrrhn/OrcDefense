using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    [SerializeField] TowerBase ParentTower;
    private Enemy enemy;
    [SerializeField] private DetectMode mode;
    [SerializeField] private List<Enemy> enemies;
    private Enemy selectedTarget; //Bu deðiþken en güçlü ve en zayýfý bulurken deðer tutacak
    public Transform partToRotate;
    public float aimSpeed;
    public TowerMode Type;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0, 0.5f);
        mode = ParentTower.Mod;
    }
    private void Update()
    {
        foreach(Enemy enemy in enemies.ToArray())
        {
            if (enemy.currentHealth <= 0)
            {
                DeleteEnemy(enemy);
            }
        }
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

    private void OnTriggerExit(Collider other)
    {
        ParentTower.SetTarget(null);
        Enemy delete = other.GetComponent<Enemy>();
        DeleteEnemy(delete);
    }

    public void SetMode(DetectMode m) //Bunu oyun içinde deðiþtirildiðinde kullanýp deðiþtiricez
    {                                        // TowerBase içinden çaðýrýrýz
        mode = m;
    }
    public void SetType(TowerMode m) //Bunu oyun içinde deðiþtirildiðinde kullanýp deðiþtiricez
    {                                        // TowerBase içinden çaðýrýrýz
        Type = m;
    }

    public void DeleteEnemy(Enemy enemy)
    {
        for(int i=0; i < enemies.Count; i++)
        {
            if(enemy == enemies[i])
            {
                enemies.Remove(enemy); //hedef ölür veya alandan çýkarsa Hedef bu scripti kendi çaðýrýp adýný sildirecek
                if (ParentTower.GetTarget() == enemy)
                {
                    ParentTower.SetTarget(null);
                }
                return;
            }

        }
        

    }

}
