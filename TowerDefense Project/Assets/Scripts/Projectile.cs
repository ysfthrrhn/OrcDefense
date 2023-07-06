using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float damage;
    float poisonDamage;
    float poisonDuration;
    float slowingRate;
    float armorPiercing;
    float speed;

    [Header ("Area Damage")]
    public float blastRange;
    public AnimationCurve curve;
    public string enemyTag = "Enemy";
    private Vector3 start;
    
    public Transform target;
    private TowerMode Type;

    bool hitCheck = false;
    
    // Default values of projectile;
    public void Awake()
    {
        damage = 0;
        poisonDamage = 0;
        slowingRate = 0;
        armorPiercing = 0;
        speed = 10f;
    }

    /// <summary>
    /// Change projectile by tower mode
    /// </summary>
    public void InstantiateBullet()
    {
        if (Type==TowerMode.Standard)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        if (Type == TowerMode.Double)
        {
            GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        if (Type == TowerMode.Poison)
        {
            GetComponent<MeshRenderer>().material.color = Color.magenta;
        }
        if (Type == TowerMode.Area)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    /// <summary>
    /// Set target to projectile
    /// </summary>
    /// <param name="_target"></param>
    public void Chase(GameObject _target) {
        target = _target.transform;
    }

    
    void Update() {
        
        if (target == null || target.GetComponent<Enemy>().isDead || !target.gameObject.activeSelf) // Check for target
        {
            SendToPool();
            return; 
        }

        if (Type == TowerMode.Area) // Checking for Aoe tower for how to move the projectile to target.
        {
            StartCoroutine((Curve())); // Moving projectile with curve to the target
        }
        else // Moving projectile directly to the target
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            float distanceThisFrame = speed * Time.deltaTime;

            if (hitCheck)
            {
                return;
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
    }

    // When reached to target
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy" && Type != TowerMode.Area)
        {
            hitCheck = true;
            collision.gameObject.GetComponent<Enemy>().GetDamage(damage, poisonDamage, poisonDuration, slowingRate, armorPiercing, Type);
            SendToPool();
        }
    }
    
    /// <summary>
    /// Sets projectile's rates.
    /// </summary>
    /// <param name="damageValue"></param>
    /// <param name="poisonDamageValue"></param>
    /// <param name="poisonDurationValue"></param>
    /// <param name="slowingRateValue"></param>
    /// <param name="armorPiercingValue"></param>
    /// <param name="speedValue"></param>
    public void SetDamageRates(float damageValue, float poisonDamageValue, float poisonDurationValue, float slowingRateValue, float armorPiercingValue, float speedValue)
    {
        damage=damageValue;
        poisonDamage=poisonDamageValue;
        poisonDuration=poisonDurationValue;
        slowingRate=slowingRateValue;
        armorPiercing=armorPiercingValue;
        speed=speedValue;
    }


    public void SetType(TowerMode m) //Change tower type (Will be used next versions)
    {                                        
        Type = m;
        InstantiateBullet();
    }

    public void SetAEOStart(Transform t) // Setting start point for aoe projectile
    {
        start = t.position;
    }

    /// <summary>
    /// Moves projectile with curve to the target and if hits deals damage.
    /// </summary>
    /// <returns></returns>
    IEnumerator Curve()
    {
        float duration = 1.50f; //projectile flight duration
        float time = 0f;

        Vector3 end = target.position - (target.forward * 0.55f); // lead the target a bit to account for travel time, your math will vary

        while (time < duration)
        {
            time += Time.deltaTime;

            float linearT = time / duration;
            float heightT = curve.Evaluate(linearT);

            float height = Mathf.Lerp(0f, 5.0f, heightT); // change 3 to however tall you want the arc to be

            transform.position = Vector3.Lerp(start, end, linearT) + new Vector3(0f, height, 0f);

            yield return null;
        }

        // impact 
        //Find target who got hit
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < blastRange)
            {
                enemy.GetComponent<Enemy>().GetDamage(damage, poisonDamage, poisonDuration, slowingRate, armorPiercing, TowerMode.Standard);
            }
        }
        SendToPool();
        StopCoroutine(Curve());
    }

    /// <summary>
    /// Sends projectile back to the pool.
    /// </summary>
    void SendToPool()
    {
        hitCheck = false;
        PoolManager.Instance.ReturnBulletToPool(gameObject);
    }
}
