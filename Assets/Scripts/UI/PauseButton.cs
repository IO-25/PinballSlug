using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public GameObject settingsPanel;

    void Start()
    {
        PauseManager.Instance.SetSettingsPanel(settingsPanel);
    }
}
