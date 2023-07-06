using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static Coin Instance;

    [SerializeField]
    private int coin = 0;
    
    public TextMeshProUGUI text; //UI text that shows coin value

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    // Setting coin value at start
    private void Start()
    {
        SetCoin();
    }

    /// <summary>
    /// Adds given value to coin. If no value given, It just updates text.
    /// </summary>
    /// <param name="point"></param>
    public void SetCoin(int point = 0)
    {
        coin += point;
        text.text = coin.ToString();
    }

    /// <summary>
    /// Returns Coin value.
    /// </summary>
    /// <returns></returns>
    public int GetCoin()
    {
        return coin;
    }
}
