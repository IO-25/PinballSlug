using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject settingsPanel;
    
    public Button pauseButton; // 일시정지 버튼
    public Button resumeButton; // 일시정지에서 다시 게임으로

    void Start()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame);
        }

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    // 게임 일시정지
    public void PauseGame()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // 게임 재개
    public void ResumeGame()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
