using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public Transform enemy;
        public int count;
        public float delay;
    }
    public Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves = 5.0f;
    public float waveCountDown;

    public Transform targetPoint;
    SpawnState state = SpawnState.COUNTING;

    [Header("Active Enemies Transform")]
    public Transform enemies;

    

    // Checking for is current wave finished spawning
    private void Update()
    {
        if (waveCountDown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(Spawn_Wave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Spawns given wave
    /// </summary>
    /// <param name="wave"></param>
    /// <returns></returns>
    IEnumerator Spawn_Wave(Wave wave)
    {
        state = SpawnState.SPAWNING;
        for(int i = 0; i < wave.count; i++)
        {
            Spawn_Enemy(wave.enemy);
            yield return new WaitForSeconds(wave.delay);
        }
        nextWave++;
        waveCountDown=timeBetweenWaves;
        state = SpawnState.WAITING;
        if(nextWave + 1 > waves.Length)
        {
            GameManager.Instance.spawners.Remove(this.gameObject);
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
        yield break;
    }

    //EnemyType parameter will be used when more enemy types added!
    /// <summary>
    /// Spawns enemy at GameObject's positon.
    /// </summary>
    /// <param name="enemyType"></param>
    void Spawn_Enemy(Transform enemyType)
    {
        GameObject newEnemy = PoolManager.Instance.GetPooledEnemy();
        newEnemy.transform.parent = enemies.transform;
        newEnemy.transform.position = transform.position;
        newEnemy.GetComponent<PathFinding>().destination = targetPoint;
        newEnemy.SetActive(true);
        GameManager.Instance.enemies.Add(false);
        //Instantiate(enemyType, transform);
    }
}
