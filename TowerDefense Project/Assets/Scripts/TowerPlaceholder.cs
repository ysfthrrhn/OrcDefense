using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Bu script kulenin konulabileceği yerleri tutacak ve üzerine basılınca kule koyma aracını çalıştıracak.
/// </summary>
public class TowerPlaceholder : MonoBehaviour
{
    
    public GameObject towerSelector;
    public Vector3 offset = new Vector3(0, 2f, 0);

    // Setting offset values to childs
    private void Awake()
    {
        towerSelector.SetActive(false);
        towerSelector.transform.position = transform.position + offset;
        towerSelector.transform.GetChild(0).GetChild(0).GetComponent<TowerSelectorChild>().offset = this.offset;
        towerSelector.transform.GetChild(0).GetChild(1).GetComponent<TowerSelectorChild>().offset = this.offset;
        towerSelector.transform.GetChild(0).GetChild(2).GetComponent<TowerSelectorChild>().offset = this.offset;
        towerSelector.transform.GetChild(0).GetChild(3).GetComponent<TowerSelectorChild>().offset = this.offset;
    }

    // For open and close UI
    private void OnMouseUpAsButton()
    {
        if (towerSelector.activeSelf)
        {
            towerSelector.SetActive(false);
            return;
        }
        towerSelector.SetActive(true);

    }
    
    
    
}
