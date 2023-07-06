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
    
    private Coin coin;
    
    [Tooltip("Coin to be Earned When Killed")]
    public int point = 15;
    
    public float healtBarShowTime = 4f;

    private float agentSpeed;

    [SerializeField]
    private Renderer enemyRenderer;

    private Material objectMaterial;

    Coroutine healthBarCoroutine;

    private PathFinding m_pathFinding;
    private Animator m_animator;
    private Collider m_collider;
    private Rigidbody m_rigidBody;
    private NavMeshAgent m_agent;

    // Getting necessery components
    private void Awake()
    {
        m_pathFinding = GetComponent<PathFinding>();
        m_animator = transform.GetChild(0).GetComponent<Animator>();
        m_collider = GetComponent<Collider>();
        m_rigidBody = GetComponent<Rigidbody>();
        
        objectMaterial = new Material(enemyRenderer.sharedMaterial); // Creating clone of enemy material.
        enemyRenderer.sharedMaterial = objectMaterial;// Sssigning the material.
    }
    
    // Getting coin Instance at start once.
    void Start()
    {
        coin = Coin.Instance;
        
    }
    
    // Check for is enemy dead
    void Update()
    {
        
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    /// <summary>
    /// Deal damage to Enemy.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="poisonDamage"></param>
    /// <param name="poisonDuration"></param>
    /// <param name="slowingRate"></param>
    /// <param name="armorPiercing"></param>
    /// <param name="bulletType"></param>
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

    /// <summary>
    /// Deals normal damage.
    /// </summary>
    /// <param name="damage"></param>
    void NormalDamageTaken(float damage)
    {
        float takenDamage = damage;
        currentHealth -= takenDamage;
        objectMaterial.SetInt("_Is_Damaged", 1);
        Invoke(nameof(ReverseDamageEffect), .2f);
        healthBar.UpdateHealthBar(currentHealth / baseHealth);
    }

    /// <summary>
    /// Deals posion damage.
    /// </summary>
    void ReverseDamageEffect()
    {
        objectMaterial.SetInt("_Is_Damaged", 0);
    }

    /// <summary>
    /// Setting damage per 0.5 seconds. (For poison damage)
    /// </summary>
    /// <param name="damageAmount"></param>
    /// <param name="damageDuration"></param>
    public void DamageOverTime(float damageAmount, float damageDuration)
    {
        StartCoroutine (DamageOverTimeCoroutine(damageAmount, damageDuration));
    }

    /// <summary>
    /// Setting damage per 0.5 seconds. (For poison damage)
    /// </summary>
    /// <param name="damageAmount"></param>
    /// <param name="damageDuration"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Enables health bar for desired time.
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator HealthBarActivator(float delay)
    {
        healthBar.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(delay);
        healthBar.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets enemy to die state.
    /// </summary>
    public void Die()
    {
        ReverseDamageEffect();
        objectMaterial.SetInt("_Is_Dead", 1);
        

        isDead = true;
        agentSpeed = m_pathFinding.agent.speed;
        m_pathFinding.agent.speed = 0;
        m_animator.Play("Die");

        m_collider.enabled = false;
        m_rigidBody.isKinematic = true;
        m_rigidBody.useGravity = false;
        m_agent.enabled = false;
        healthBar.gameObject.SetActive(false);

        coin.SetCoin(point);

        StartCoroutine(Dissolve());
    }

    /// <summary>
    /// Sets Shader state to dissolve when enemy dies.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Resets enemy to default before adding to pool.
    /// </summary>
    private void ResetBeforePool()
    {
        StopAllCoroutines();
        objectMaterial.SetInt("_Is_Dead", 0);
        objectMaterial.SetFloat("_Dissolve_Value", 0);

        GameManager.Instance.SetEnemyDead();
        isDead = false;
        
        currentHealth = baseHealth;
        healthBar.gameObject.SetActive(false);

        m_pathFinding.agent.speed = agentSpeed;

        m_collider.enabled = true;
        m_rigidBody.isKinematic = false;
        m_rigidBody.useGravity = true;
        m_agent.enabled = true;

        PoolManager.Instance.ReturnEnemyToPool(gameObject);
    }
}
