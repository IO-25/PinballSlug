using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public GameObject settingsPanel;

    // Update is called once per frame
    void Update()
    {
        // ESC 키가 눌렸을 때
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 설정 패널이 활성화되어 있다면 (게임이 멈춘 상태)
            if (settingsPanel.activeSelf)
            {
                ResumeGame();
            }
            // 설정 패널이 비활성화되어 있다면 (게임이 진행 중인 상태)
            else
            {
                PauseGame();
            }
        }
    }

    // 게임 일시정지 (설정창 표시)
    public void PauseGame()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // 게임 재개 (설정창 닫기)
    public void ResumeGame()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
