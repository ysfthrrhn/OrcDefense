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

    public void InitializeEnemyPool(int initialPoolSize)
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            enemies.Add(obj);
        }
    }
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
    public void ReturnEnemyToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = enemiesTransform;
        enemies.Add(obj);
    }

    public void InitializeBulletPool(int initialPoolSize)
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            enemies.Add(obj);
        }
    }
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
    public void ReturnBulletToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = bulletsTransform;
        bullets.Add(obj);
    }
}
