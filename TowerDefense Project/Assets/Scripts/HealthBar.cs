using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;

    /// <summary>
    /// Updates Health Bar's fill amount
    /// </summary>
    /// <param name="fraction"></param>
    public void UpdateHealthBar(float fraction)
    {
        healthBar.fillAmount = fraction;
    }
}
