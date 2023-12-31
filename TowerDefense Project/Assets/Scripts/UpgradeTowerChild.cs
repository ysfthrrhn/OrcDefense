using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public enum UpgradeType
{
    UpgradeEnum,
    DowngradeEnum
}
public class UpgradeTowerChild : MonoBehaviour
{
    public Mesh[] meshesBase; // Tower Mesh
    public Mesh[] meshesTop; // Gun Mesh (Next version!!)

    [SerializeField]
    private GameObject _tower;

    [SerializeField]
    private MeshFilter _towerMesh;

    public UpgradeType upgradeType;

    [SerializeField, Range(1, 10)]
    private float duration = 3;
    
    [SerializeField]
    private int upgradeCost = 200;
    [SerializeField]
    private int downgradeCost = 75;

    public TextMeshProUGUI cost;

    private Coin coin;
    string currentMesh;
    
    // Set Type, get Info, and auto disable upgrade UI
    void Start()
    {
        coin = Coin.Instance;
        currentMesh = _towerMesh.mesh.name;
        
        switch (upgradeType)
        {
            case UpgradeType.UpgradeEnum:
                cost.text = upgradeCost.ToString();
                break;
            case UpgradeType.DowngradeEnum:
                cost.text = downgradeCost.ToString();
                break;
            default:
                DisableParent();
                return;
        }

        Invoke(nameof(DisableParent), duration);
        
    }

    // For checking tower reached last level
    private void Update()
    {
        if (currentMesh == "tower3 Instance" && upgradeType == UpgradeType.UpgradeEnum)
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GetComponent<Collider>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    
    // Upgrading tower if player has enough coin or downgrading tower
    private void OnMouseUpAsButton()
    {
        switch (upgradeType)
        {
            case UpgradeType.UpgradeEnum:
                if(coin.GetCoin() >= 250)
                {
                    coin.SetCoin(-250);
                    UpgradeMesh();
                    DisableParent();
                }
                
                
                break;
            case UpgradeType.DowngradeEnum:
                DisableParent();
                DowngradeMesh();
                break;
            default:
                DisableParent();
                return;
        }
        
    }
    
    /// <summary>
    /// Upgrades towers mesh.
    /// </summary>
    private void UpgradeMesh()
    {
        currentMesh = _towerMesh.mesh.name;
        switch (currentMesh)
        {
            case "tower1 Instance":
                _towerMesh.mesh = meshesBase[1];
                UpgradeDamage();
                break;
            case "tower2 Instance":
                _towerMesh.mesh = meshesBase[2];
                UpgradeDamage();
                break;
            
        }
        
    }

    /// <summary>
    /// Downgrades towers mesh.
    /// </summary>
    private void DowngradeMesh()
    {
        currentMesh = _towerMesh.mesh.name;
        switch (currentMesh)
        {
            case "tower1 Instance":
                coin.SetCoin(75);
                Destroy(_tower);
                break;
            case "tower2 Instance":
                _towerMesh.mesh = meshesBase[0];
                DowngradeDamage();
                break;
            case "tower3 Instance":
                _towerMesh.mesh = meshesBase[1];
                DowngradeDamage();
                break;

        }
        
    }

    /// <summary>
    /// Upgrades towers damage.
    /// </summary>
    private void UpgradeDamage()
    {
        var towerName = _tower.name;
        var bulletInfo = _tower.transform.GetComponent<TowerBase>().bulletInfo;
        switch (towerName)
        {
            case "TowerStandard(Clone)":
                bulletInfo.damageValue *= 1.35f;
                break;
            case "TowerDoubleStandard(Clone)":
                bulletInfo.damageValue *= 1.20f;
                break;
            case "TowerPoison(Clone)":
                bulletInfo.poisonDamageValue *= 1.20f;
                break;
            case "TowerAreaOfEffect(Clone)":
                bulletInfo.damageValue *= 1.35f;
                break;
        }
    }

    /// <summary>
    /// Downgrades towers damage.
    /// </summary>
    private void DowngradeDamage()
    {
        var towerName = _tower.name;
        var bulletInfo = _tower.transform.GetComponent<TowerBase>().bulletInfo;
        switch (towerName)
        {
            case "TowerStandard(Clone)":
                bulletInfo.damageValue /= 1.35f;
                break;
            case "TowerDoubleStandard(Clone)":
                bulletInfo.damageValue /= 1.20f;
                break;
            case "TowerPoison(Clone)":
                bulletInfo.poisonDamageValue /= 1.20f;
                break;
            case "TowerAreaOfEffect(Clone)":
                bulletInfo.damageValue /= 1.35f;
                break;
        }
        coin.SetCoin(150);
    }

    /// <summary>
    /// Closing UI
    /// </summary>
    private void DisableParent()
    {
        transform.parent.parent.gameObject.SetActive(false);
    }


}
