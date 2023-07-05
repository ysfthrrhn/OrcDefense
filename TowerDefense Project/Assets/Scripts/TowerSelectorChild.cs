using System;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public enum TowerTypes {
    Archer,
    Mage,
    Aoe, //Area of effect
    Double
}
public class TowerSelectorChild : MonoBehaviour
{
    #region TowerPrefabs + Offsets
    //declare the tower prefabs
    public GameObject archerTowerPrefab;
    public GameObject mageTowerPrefab;
    public GameObject aoeTowerPrefab;
    public GameObject doubleTowerPrefab;
    public Vector3 offset;
    #endregion
    private Coin coin;

    [SerializeField, Range(1, 10)]
    private float duration = 3;

    public int archerTowerCost;
    public int mageTowerCost;
    public int aoeTowerCost;
    public int doubleTowerCost;

    public TextMeshProUGUI cost;

    private bool active;
    void Start()
    {
        coin = Coin.Instance;
        switch (type)
        {
            case TowerTypes.Archer:
                cost.text = archerTowerCost.ToString();
                break;
            case TowerTypes.Mage:
                cost.text = mageTowerCost.ToString();
                break;
            case TowerTypes.Aoe:
                cost.text = aoeTowerCost.ToString();
                break;
            case TowerTypes.Double:
                cost.text = doubleTowerCost.ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void Update()
    {
        if(gameObject.activeSelf && !active)
        {
            active = true;
            Invoke(nameof(DisableParent), duration);
        }
    }

    public TowerTypes type;
    private void OnMouseUpAsButton()
    {
        active = false;
        var parent = transform.parent.parent;
        Vector3 towerPosition = new Vector3(parent.position.x, parent.position.y - offset.y / 2, parent.position.z);
        //switch case for tower type
        switch (type)
        {
            case TowerTypes.Archer:
                if(coin.GetCoin() >= archerTowerCost)
                {
                    Instantiate(archerTowerPrefab, towerPosition, new Quaternion(0, 0, 0, 0));
                    coin.SetCoin(-archerTowerCost);
                }
                
                parent.gameObject.SetActive(false);
                break;
            case TowerTypes.Mage:
                if (coin.GetCoin() >= mageTowerCost)
                {
                    Instantiate(mageTowerPrefab, towerPosition, new Quaternion(0, 0, 0, 0));
                    coin.SetCoin(-mageTowerCost);
                }

                parent.gameObject.SetActive(false);
                break;
            case TowerTypes.Aoe:
                if (coin.GetCoin() >= aoeTowerCost)
                {
                    Instantiate(aoeTowerPrefab, towerPosition, new Quaternion(0, 0, 0, 0));
                    coin.SetCoin(-aoeTowerCost);
                }

                parent.gameObject.SetActive(false);
                break;
            case TowerTypes.Double:
                if (coin.GetCoin() >= doubleTowerCost)
                {
                    Instantiate(doubleTowerPrefab, towerPosition, new Quaternion(0, 0, 0, 0));
                    coin.SetCoin(-doubleTowerCost);
                }
                
                parent.gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void DisableParent()
    {
        transform.parent.parent.gameObject.SetActive(false);
        active = false;
    }
}
