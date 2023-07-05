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

    private void OnMouseUpAsButton()
    {
        
        if (upgradeTool.activeSelf)
        {
            StopAllCoroutines();
            upgradeTool.SetActive(false);
            return;
        }
        upgradeTool.SetActive(true);
        StartCoroutine(DisableGameObject());
    }
    IEnumerator DisableGameObject()
    {
        yield return new WaitForSecondsRealtime(duration);
        upgradeTool.SetActive(false);
        StopAllCoroutines();
    }
}
