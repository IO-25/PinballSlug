using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    public Button pauseButton;
    public Button resumeButton;
    public GameObject settingsPanel;

    void Start()
    {
        // 맵 매니저의 인스턴스가 있는지 확인 후 버튼에 함수 연결
        if (MapManager.Instance != null)
        {
            pauseButton.onClick.AddListener(MapManager.Instance.PauseGame);
            resumeButton.onClick.AddListener(MapManager.Instance.ResumeGame);
        }

        // 설정 패널 참조를 맵 매니저에 전달
        MapManager.Instance.SetSettingsPanel(settingsPanel);
    }
}
