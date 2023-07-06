using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [Header("Enemy")]
    public GameObject enemyPrefab;
    public List<GameObject> enemies = new List<GameObject>();
    [SerializeField]
    private Transform enemiesTransform;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public List<GameObject> bullets = new List<GameObject>();
    [SerializeField]
    private Transform bulletsTransform;

    //Making Instance
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    //Checking for pools at startup.
    void Start()
    {
        if (enemies.Count == 0)
            InitializeEnemyPool(20);
        else
        {
            foreach(GameObject enemy in enemies)
                enemy.SetActive(false);
        }
        if (bullets.Count == 0)
            InitializeBulletPool(20);
        else
        {
            foreach (GameObject bullet in bullets)
                bullet.SetActive(false);
        }

    }

    /// <summary>
    /// Instantiate desired number of enemies.
    /// </summary>
    /// <param name="initialPoolSize"></param>
    public void InitializeEnemyPool(int initialPoolSize)
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            enemies.Add(obj);
        }
    }

    /// <summary>
    /// Get enemy from pool.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledEnemy()
    {
        if (enemies.Count == 0)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            enemies.Add(obj);
        }

        GameObject pooledObject = enemies[0];
        //pooledObject.SetActive(true);
        enemies.RemoveAt(0);

        return pooledObject;
    }

    /// <summary>
    /// Return enemy to pool.
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnEnemyToPool(GameObject enemyObject)
    {
        enemyObject.SetActive(false);
        enemyObject.transform.parent = enemiesTransform;
        enemies.Add(enemyObject);
    }

    /// <summary>
    /// Instantiate desired number of bullets.
    /// </summary>
    /// <param name="initialPoolSize"></param>
    public void InitializeBulletPool(int initialPoolSize)
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            enemies.Add(obj);
        }
    }

    /// <summary>
    /// Get bullet from pool.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledBullet()
    {
        if (bullets.Count == 0)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            bullets.Add(obj);
        }

        GameObject pooledObject = bullets[0];
        //pooledObject.SetActive(true);
        bullets.RemoveAt(0);
        return pooledObject;
    }
    /// <summary>
    /// Return bullet to pool.
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnBulletToPool(GameObject bulletObject)
    {
        bulletObject.SetActive(false);
        bulletObject.transform.parent = bulletsTransform;
        bullets.Add(bulletObject);
    }
}
