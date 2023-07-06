using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField] private Enemy target;
    [SerializeField] private FindTarget findTargetScript;
    Enemy lastTarget;
    
    [Header("Tower Attributes")]

    public float range;
    //public float damage;
    public Transform firePoint;

    public DetectMode Mod;
    public TowerMode Type;

    [System.Serializable]
    public class BulletRates
    {
        public float damageValue;
        public float poisonDamageValue;
        public float poisonDuration;
        public float slowingRateValue;
        public float armorPiercingValue;
        public float speedValue;
        public float fireRate;
        public float coolDown;
    }
    public BulletRates bulletInfo;
    

    private void Update()
    {
        if (gameObject.transform.name == "TowerDoubleStandard(Clone)" && bulletInfo.coolDown <= 0f && target != null)
        {
            lastTarget = target;
            target = lastTarget;
            Shoot();
            bulletInfo.coolDown = 1f / bulletInfo.fireRate;
            if (target != null)
            {
                target = lastTarget;
                Invoke("Shoot", 0.1f);
            }     
        }
        else if (bulletInfo.coolDown <= 0f && target != null)
        {
            lastTarget = target;
            Shoot();
            bulletInfo.coolDown = 1f / bulletInfo.fireRate;
        }

        bulletInfo.coolDown -= Time.deltaTime; // Cooldown starting
        
    }

    /// <summary>
    /// Shoots a selected type projectile 
    /// </summary>
    void Shoot()
    {
        target = lastTarget;
        if (target != null)
        {
            GameObject projectile = PoolManager.Instance.GetPooledBullet();
            projectile.SetActive(true);
            projectile.transform.position = firePoint.position;
            projectile .transform.rotation = firePoint.rotation;
            projectile.transform.parent = null;
            Projectile arrow = projectile.GetComponent<Projectile>();
        
        
            if (gameObject.transform.name == "TowerStandard(Clone)")
            {
                arrow.SetType(TowerMode.Standard);
            }
            else if (gameObject.transform.name == "TowerDoubleStandard(Clone)")
            {
                arrow.SetType(TowerMode.Double);
            }
            else if (gameObject.transform.name == "TowerPoison(Clone)")
            {
                arrow.SetType(TowerMode.Poison);
            }
            else if (gameObject.transform.name == "TowerAreaOfEffect(Clone)")
            {
                arrow.SetType(TowerMode.Area);
                arrow.SetAEOStart(firePoint);
            }
            
        
            arrow.SetDamageRates(bulletInfo.damageValue, bulletInfo.poisonDamageValue, bulletInfo.poisonDuration, bulletInfo.slowingRateValue, bulletInfo.armorPiercingValue, bulletInfo.speedValue);
            if (arrow != null)
            {
                arrow.Chase(target.gameObject);
            }
        }
        

    }


    /// <summary>
    /// Sets Enemy as current target.
    /// </summary>
    /// <param name="enemy"></param>
    public void SetTarget(Enemy enemy)
    {
        target = enemy;
    }

    /// <summary>
    /// Returns current Enemy
    /// </summary>
    /// <returns></returns>
    public Enemy GetTarget() {
        return target;
    }

}
