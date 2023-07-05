using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float currentHealth;
    public float baseHealth;

    [HideInInspector]
    public bool isDead = false;

    [SerializeField]
    private HealthBar healthBar;
    
    [SerializeField]    
    private Animator animator;

    private Coin coin;
    
    [Tooltip("Coin to be Earned When Killed")]
    public int point = 15;
    
    public float healtBarShowTime = 4f;

    private float agentSpeed;

    [SerializeField]
    private Renderer enemyRenderer;

    private Material objectMaterial;

    Coroutine healthBarCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        coin = Coin.Instance;
        objectMaterial = new Material(enemyRenderer.sharedMaterial);
        enemyRenderer.sharedMaterial = objectMaterial;
    }
    
    // Update is called once per frame
    void Update()
    {
        
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
    public void GetDamage(float damage, float poisonDamage, float poisonDuration, float slowingRate, float armorPiercing, TowerMode bulletType)
    {
        if(healthBarCoroutine != null)
            StopCoroutine(healthBarCoroutine);

        healthBarCoroutine = StartCoroutine(HealthBarActivator(healtBarShowTime));

        if(bulletType == TowerMode.Standard || bulletType == TowerMode.Double)
        {
            NormalDamageTaken(damage);
        }
        else if(bulletType == TowerMode.Poison)
        {
            DamageOverTime(poisonDamage, poisonDuration);
        }
    }

    void NormalDamageTaken(float damage)
    {
        float takenDamage = damage;
        currentHealth -= takenDamage;
        objectMaterial.SetInt("_Is_Damaged", 1);
        Invoke(nameof(ReverseDamageEffect), .2f);
        healthBar.UpdateHealthBar(currentHealth / baseHealth);
    }
    void ReverseDamageEffect()
    {
        objectMaterial.SetInt("_Is_Damaged", 0);
    }

    public void DamageOverTime(float damageAmount, float damageDuration)
    {
        StartCoroutine (DamageOverTimeCoroutine(damageAmount, damageDuration));
    }
    IEnumerator DamageOverTimeCoroutine(float damageAmount, float damageDuration)
    {
        float amountDamaged = 0;
        float damagePerLoop = damageAmount / damageDuration;
        while(amountDamaged < damageAmount)
        {
            NormalDamageTaken(damagePerLoop);
            amountDamaged += damagePerLoop;
            yield return new WaitForSeconds(0.5f);
        }
        
    }
    IEnumerator HealthBarActivator(float delay)
    {
        healthBar.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(delay);
        healthBar.gameObject.SetActive(false);
    }

    //Things do when enemy is dead
    public void Die()
    {
        ReverseDamageEffect();
        objectMaterial.SetInt("_Is_Dead", 1);
        

        isDead = true;
        agentSpeed = gameObject.GetComponent<PathFinding>().agent.speed;
        gameObject.GetComponent<PathFinding>().agent.speed = 0;
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Die");

        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        healthBar.gameObject.SetActive(false);

        coin.SetCoin(point);

        StartCoroutine(Dissolve());
    }
    IEnumerator Dissolve()
    {
        float t = 0;

        while (t < 1)
        {
            yield return null;
            t += Time.deltaTime / 2;
            objectMaterial.SetFloat("_Dissolve_Value", Mathf.Lerp(0, 0.8f, t));
        }
        objectMaterial.SetFloat("_Dissolve_Value", .8f);

        ResetBeforePool();
    }
    private void ResetBeforePool()
    {
        StopAllCoroutines();
        objectMaterial.SetInt("_Is_Dead", 0);
        objectMaterial.SetFloat("_Dissolve_Value", 0);

        GameManager.Instance.SetEnemyDead();
        isDead = false;
        
        currentHealth = baseHealth;
        healthBar.gameObject.SetActive(false);

        gameObject.GetComponent<PathFinding>().agent.speed = agentSpeed;

        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        PoolManager.Instance.ReturnEnemyToPool(gameObject);
    }
}
