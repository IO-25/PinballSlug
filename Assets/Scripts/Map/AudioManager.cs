using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private AudioSource buttonClickSound;
    private static AudioManager instance;
    private string nextSceneName;

    // 버튼을 누르면 소리가 나고 넘어가도록 만듬
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            buttonClickSound = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 버튼에 연결
    public void ButtonClickSoundAndLoadScene(string sceneName)
    {
        if (buttonClickSound != null && buttonClickSound.clip != null)
        {
            // 다음 씬 이름
            nextSceneName = sceneName;
            
            buttonClickSound.Play();
            
            // 소리 재생 시간만큼 지연 후 LoadScene 호출
            Invoke("LoadScene", buttonClickSound.clip.length);
        }
    }

    // 씬 전환
    private void LoadScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}