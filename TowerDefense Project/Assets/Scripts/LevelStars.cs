using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStars : MonoBehaviour
{
    public int level;

    public GameObject starsEmpty;
    public GameObject starsFilled;

    private Button levelButton;

    private void Awake()
    {
        levelButton = GetComponent<Button>();
    }

    // Checking for is levels passed and available
    private void Update()
    {
        if(gameObject.activeSelf)
        {
            if (levelButton != null && levelButton.interactable)
            {
                if (PlayerPrefs.GetInt("Level" + level.ToString()) == 1)
                {
                    starsEmpty.SetActive(false);
                    starsFilled.SetActive(true);
                }
                else
                {
                    starsEmpty.SetActive(true);
                    starsFilled.SetActive(false);
                }
            }
            else
            {
                starsEmpty.SetActive(false);
                starsFilled.SetActive(false);
            }
        }
    }

}
