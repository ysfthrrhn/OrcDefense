using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserHealthBar : MonoBehaviour
{
    public HealthBar healthBar;
    float healt=20;
    float totalHealth=20;
    
    // Updating players health bar when enemy reached and killing the enemy
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.transform.tag == "Enemy")
        {
            healt -= 1;
            if (healt <= 0)
            {
                GameOver();
            }
            healthBar.UpdateHealthBar(healt / totalHealth);
        }
    }

    /// <summary>
    /// Calls function to open UI for showing level failed.
    /// </summary>
    public void GameOver()
    {
        UIManager.Instance.OpenGameOverPanel();
    }
}
