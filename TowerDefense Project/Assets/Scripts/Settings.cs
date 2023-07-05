using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    bool VSync = false;
    public GameObject isVSyncbject;
    private void Start()
    {
        if(GetVSync() == 1)
            VSync = true;
        else VSync = false;
        UpdateSettings();
    }

    public void UpdateSettings()
    {
        if (VSync)
        {
            isVSyncbject.SetActive(true);
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            isVSyncbject.SetActive(false);
            QualitySettings.vSyncCount = 0;
        }
        SetPlayerPrefs();
    }
    public int GetVSync()
    {
        return PlayerPrefs.GetInt("VSync");
    }
    public void ChangeVSync()
    {
        VSync = !VSync;
        UpdateSettings();
    }
    public void SetPlayerPrefs()
    {
        int value;
        
        PlayerPrefs.SetInt("VSync", value = VSync ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
