using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // UI 스크립트가 연결해 줄 설정 패널
    [SerializeField] private GameObject settingsPanel;
    private bool isPaused = false;

    void Start()
    {
        isPaused = false;
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        Debug.Log(settingsPanel);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }
}