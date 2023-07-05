using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static Coin Instance;

    [SerializeField]
    private int coin = 0;
    
    public TextMeshProUGUI text;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    private void Start()
    {
        SetCoin();
    }

    public void SetCoin(int point = 0)
    {
        coin += point;
        text.text = coin.ToString();
    }
    public int GetCoin()
    {
        return coin;
    }
}
