using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private AudioSource buttonClickSound;
    private static AudioManager instance;
    
    public static AudioManager Instance
    {
        get { return instance; }
    }

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

    public void ButtonClickSoundAndLoadScene(string sceneName)
    {
        if (buttonClickSound != null && buttonClickSound.clip != null)
        {
            Debug.Log("ButtonClickSoundAndLoadScene 호출: 코루틴 시작을 시도합니다.");
            buttonClickSound.Play();
            StartCoroutine(LoadSceneAfterSound(sceneName, buttonClickSound.clip.length));
        }
    }

    private IEnumerator LoadSceneAfterSound(string sceneName, float soundLength)
    {
        Debug.Log("코루틴 시작: " + soundLength + "초 기다립니다.");
        
        // 이 부분에서 코루틴이 멈추거나 파괴되는지 확인합니다.
        yield return new WaitForSeconds(soundLength);

        Debug.Log("코루틴 대기 완료: 씬 로드를 시도합니다.");
        SceneManager.LoadScene(sceneName);
    }
    
    private void OnDestroy()
    {
        Debug.Log("AudioManager: OnDestroy - 오브젝트가 파괴되었습니다. 코루틴이 중단될 수 있습니다.");
    }
}