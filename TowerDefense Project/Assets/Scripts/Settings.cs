using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    bool VSync = false;
    public GameObject isVSyncbject;

    private void Start() // Set settings at start
    {
        if(GetVSync() == 1)
            VSync = true;
        else VSync = false;
        UpdateSettings();
    }

    /// <summary>
    /// Updates settings
    /// </summary>
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

    /// <summary>
    /// Returns current value of VSync from PlayerPrefs
    /// </summary>
    /// <returns></returns>
    public int GetVSync()
    {
        return PlayerPrefs.GetInt("VSync");
    }

    /// <summary>
    /// Change Current VSync value
    /// </summary>
    public void ChangeVSync()
    {
        VSync = !VSync;
        UpdateSettings();
    }

    /// <summary>
    /// Sets VSync value to PlayerPrefs.
    /// </summary>
    public void SetPlayerPrefs()
    {
        int value;
        
        PlayerPrefs.SetInt("VSync", value = VSync ? 1 : 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Resets PlayerPrefs for resetting game save.
    /// </summary>
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
