using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserHealthBar : MonoBehaviour
{
    public HealthBar healthBar;
    float healt=20;
    float totalHealth=20;
    
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

    public void GameOver()
    {
        UIManager.Instance.OpenGameOverPanel();
    }
}
