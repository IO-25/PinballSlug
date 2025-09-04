using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public GameObject settingsPanel;
    private bool isPaused = false;

    void Start()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    void Update()
    {
        // ESC 키로 일시정지/재개 토글
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

