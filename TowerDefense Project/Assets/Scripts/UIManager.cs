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

    // Starts UI. For opening right panel. (Menu or InGame)
    private void Start()
    {
        if(inGameMenuPanel != null)
            inGameMenuPanel.SetActive(true);
        else
            mainMenuPanel.SetActive(true);
    }

    /// <summary>
    /// Loads level by given Index.
    /// </summary>
    /// <param name="index"></param>
    public void LoadLevelByIndex(int index)
    {
        OpenLoadingPanel();
        StartCoroutine(LoadScene(index));
    }

    /// <summary>
    /// Loads level by given Index.
    /// </summary>
    /// <param name="index"></param>
    public void LoadNextLevel()
    {
        OpenLoadingPanel();
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    /// <summary>
    /// Loads current level.
    /// </summary>
    /// <param name="index"></param>
    public void LoadCurrentScene()
    {
        OpenLoadingPanel();
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    /// <summary>
    /// Prepare for loading scene and loads scene by Index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
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

    // Panel changers
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
    
    /// <summary>
    /// Disables all childs of Transform(Canvas).
    /// </summary>
    public void ClearUI()
    {
        foreach(Transform panel in transform)
            panel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Changes timeScale with desired value.
    /// </summary>
    /// <param name="speed"></param>
    public void ChangeGameSpeed(float speed)
    {
        foreach (Button button in gameSpeedButtons)
            button.interactable = true;
        Time.timeScale = speed; 
    }
    
}
