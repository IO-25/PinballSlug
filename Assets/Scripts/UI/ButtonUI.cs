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
        if (MapManager.Instance != null)
        {
            pauseButton.onClick.AddListener(MapManager.Instance.PauseGame);
            resumeButton.onClick.AddListener(MapManager.Instance.ResumeGame);
        }
        
        MapManager.Instance.SetSettingsPanel(settingsPanel);
    }
}
