using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Mixer & UI")]
    public AudioMixer mainMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    // PlayerPrefs에 사용할 볼륨 키
    private const string MasterVolumeKey = "MasterVolume";
    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";
    private const float MinVolumeDb = -80f;

    void Start()
    {
        // 믹서에 볼륨 파라미터가 노출되어 있는지 확인
        if (mainMixer == null)
        {
            Debug.LogError("오디오 믹서가 연결되지 않았습니다! 인스펙터에서 mainMixer에 AudioMixer 에셋을 연결해주세요.");
            return;
        }

        // 각 볼륨 값 로드 및 초기 설정
        LoadVolume(MasterVolumeKey, "MasterVolume", masterSlider);
        LoadVolume(BGMVolumeKey, "BGMVolume", bgmSlider);
        LoadVolume(SFXVolumeKey, "SFXVolume", sfxSlider);
    }
    
    // 볼륨 설정 및 저장 함수 (마스터, BGM, SFX 공통)
    private void LoadVolume(string playerPrefsKey, string mixerParamName, Slider slider)
    {
        float savedVolume;
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            savedVolume = PlayerPrefs.GetFloat(playerPrefsKey);
        }
        else
        {
            // 저장된 값이 없으면 기본값으로
            savedVolume = 1f;
        }

        SetMixerVolume(mixerParamName, savedVolume);
        if (slider != null)
        {
            slider.value = savedVolume;
        }
    }

    private void SetMixerVolume(string mixerParamName, float volume)
    {
        float mixerVolume = (volume > 0) ? Mathf.Log10(volume) * 20 : MinVolumeDb;
        mainMixer.SetFloat(mixerParamName, mixerVolume);
        
        // 볼륨 값 저장
        PlayerPrefs.SetFloat(mixerParamName, volume);
    }
    
    // UI 슬라이더에 연결할 공용 함수
    public void SetMasterVolume(float volume)
    {
        SetMixerVolume(MasterVolumeKey, volume);
    }
    
    public void SetBGMVolume(float volume)
    {
        SetMixerVolume(BGMVolumeKey, volume);
    }
    
    public void SetSFXVolume(float volume)
    {
        SetMixerVolume(SFXVolumeKey, volume);
    }
}