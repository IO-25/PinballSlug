using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : Singleton<MapManager>
{
    // UI 스크립트가 연결해 줄 설정 패널
    private GameObject settingsPanel;
    private bool isPaused = false;

    // 다른 스크립트가 설정 패널을 연결
    public void SetSettingsPanel(GameObject panel)
    {
        settingsPanel = panel;
        // 초기 상태를 비활성화로 설정
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    protected override void Initialize()
    {
        dontDestroyOnLoad = false;
    }

    void Update()
    {
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