using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Button pauseButton;
    public Button resumeButton;
    public GameObject settingsPanel;

    void Start()
    {
        if (PauseManager.Instance != null)
        {
            pauseButton.onClick.AddListener(PauseManager.Instance.PauseGame);
            resumeButton.onClick.AddListener(PauseManager.Instance.ResumeGame);
        }
        
        PauseManager.Instance.SetSettingsPanel(settingsPanel);
    }
}
