using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTower : MonoBehaviour
{
    public GameObject upgradeTool;
    public Vector3 offSet = new Vector3(0, 2, 0);
    
    [SerializeField, Range(1, 10)]
    private float duration = 3;

    private void Start()
    {
        upgradeTool.transform.position = transform.position + offSet;
    }

    // For open and close Tower Upgare UI
    private void OnMouseUpAsButton()
    {
        
        if (upgradeTool.activeSelf)
        {
            StopAllCoroutines();// If there is coroutine still working after object enabled again, It will be disable the GameObject
            upgradeTool.SetActive(false);
            return;
        }
        upgradeTool.SetActive(true);
        StartCoroutine(DisableGameObject());
    }

    /// <summary>
    /// Disables Upgrade UI
    /// </summary>
    /// <returns></returns>
    IEnumerator DisableGameObject()
    {
        yield return new WaitForSecondsRealtime(duration);
        upgradeTool.SetActive(false);
        StopAllCoroutines(); // If there is coroutine still working after object enabled again, It will be disable the GameObject
    }
}
