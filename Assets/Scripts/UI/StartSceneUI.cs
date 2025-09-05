using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneUI : MonoBehaviour
{
    public Button startButton;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            startButton.onClick.AddListener(() => AudioManager.Instance.ButtonClickSoundAndLoadScene("GameScene"));
        }
        else
        {
            Debug.LogError("AudioManager 인스턴스를 찾을 수 없습니다.");
        }
    }
}