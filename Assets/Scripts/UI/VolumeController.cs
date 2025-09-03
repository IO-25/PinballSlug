using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider volumeSlider; // 슬라이더 UI 연결

    private const string MasterVolumeName = "MasterVolume";
    private const float MinVolumeDb = -80f;

    void Start()
    {
        // PlayerPrefs에 저장된 볼륨 값이 있는지 확인
        if (PlayerPrefs.HasKey(MasterVolumeName))
        {
            // 저장된 볼륨 값 로드
            float savedVolume = PlayerPrefs.GetFloat(MasterVolumeName);
            
            // 믹서 볼륨 설정
            SetVolume(savedVolume);
            
            // 슬라이더를 저장된 볼륨 값으로 이동
            volumeSlider.value = savedVolume;
        }
        else
        {
            // 저장된 값이 없으면 기본값으로
            SetVolume(1f);
            volumeSlider.value = 1f;
        }
    }
    
    public void SetVolume(float volume)
    {
        float mixerVolume = (volume > 0) ? Mathf.Log10(volume) * 20 : MinVolumeDb;
        masterMixer.SetFloat(MasterVolumeName, mixerVolume);
        
        // 볼륨 값 저장
        PlayerPrefs.SetFloat(MasterVolumeName, volume);
    }
}