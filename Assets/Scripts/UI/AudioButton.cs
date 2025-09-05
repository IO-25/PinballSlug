using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class AudioButton : MonoBehaviour
{
    private AudioSource buttonClickSoundSource;

    public void Awake()
    {
        buttonClickSoundSource = GetComponent<AudioSource>();
    }

    public void OnAudioButtonClick()
    {
        StartCoroutine(AudioButtonAction());
    }

    public IEnumerator AudioButtonAction()
    {
        buttonClickSoundSource.Play();
        yield return new WaitForSeconds(buttonClickSoundSource.clip.length);
        LoadNextScene();
    }

    protected abstract void LoadNextScene();
}