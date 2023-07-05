using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Main Menu UI")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;
    
    [Header("In Game UI")]
    public GameObject pauseMenuPanel;
    public GameObject inGameMenuPanel;
    public GameObject gameOverPanel;
    public GameObject levelSucceedPanel;

    [Header("Loading Level UI")]
    public GameObject loadingPanel;
    public HealthBar loadingBar;

    [Header("Game Speed")]
    public List<Button> gameSpeedButtons = new List<Button>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        if(inGameMenuPanel != null)
            inGameMenuPanel.SetActive(true);
        else
            mainMenuPanel.SetActive(true);
    }
    public void LoadLevelByIndex(int index)
    {
        OpenLoadingPanel();
        StartCoroutine(LoadScene(index));
    }
    public void LoadNextLevel()
    {
        OpenLoadingPanel();
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void LoadCurrentScene()
    {
        OpenLoadingPanel();
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }
    private IEnumerator LoadScene(int index)
    {
        if (GameManager.Instance)
        {
            foreach (GameObject spawner in GameManager.Instance.spawners)
                spawner.GetComponent<Spawner>().StopAllCoroutines();
        }
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        while (!operation.isDone)
        {
            loadingBar.UpdateHealthBar(operation.progress);
            yield return null;
        }
        
    }
    private void OpenLoadingPanel()
    {
        ClearUI();
        loadingPanel.SetActive(true);
        Time.timeScale = 1;
    }
    public void Pause()
    {
        Time.timeScale = 0;
        ClearUI();
        pauseMenuPanel.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1;
        ClearUI();
        inGameMenuPanel.SetActive(true);
    }
    
    public void OpenGameOverPanel()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }
    public void OpenLevelSucceedPanel()
    {
        Time.timeScale = 0;
        ClearUI();
        levelSucceedPanel.SetActive(true);
    }
    public void OpenMainMenuPanel()
    {
        ClearUI();
        mainMenuPanel.SetActive(true);
    }
    public void OpenLevelSelectPanel()
    {
        ClearUI();
        levelSelectPanel.SetActive(true);
    }
    public void OpenSettingsPanel()
    {
        ClearUI();
        settingsPanel.SetActive(true);
    }
    
    public void ClearUI()
    {
        foreach(Transform panel in transform)
            panel.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeGameSpeed(float speed)
    {
        foreach (Button button in gameSpeedButtons)
            button.interactable = true;
        Time.timeScale = speed; 
    }
    
}
