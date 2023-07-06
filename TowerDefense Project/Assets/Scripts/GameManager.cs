using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> spawners = new List<GameObject>();
    public List<bool> enemies = new List<bool>();

    public bool isFinished = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        InvokeRepeating(nameof(CheckEnemies), 10, 2.5f); // Checks is level clear per 2.5 seconds
    }

    /// <summary>
    /// Checks for spawners finished spawning and all enemies are dead.
    /// </summary>
    private void CheckEnemies()
    {
        if(spawners.Count == 0)
        {
            foreach(bool enemy in enemies) 
            {
                if (!enemy)
                {
                    return;
                }

            }
            PlayerPrefs.SetInt("Level" + SceneManager.GetActiveScene().buildIndex, 1);
            PlayerPrefs.Save();
            isFinished = true;
            UIManager.Instance.OpenLevelSucceedPanel();
        }
    }

    /// <summary>
    /// Sets one enemy as dead from spawned enemies
    /// </summary>
    public void SetEnemyDead()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i])
            {
                enemies[i] = true;
                return;
            }
        }
    }

}
