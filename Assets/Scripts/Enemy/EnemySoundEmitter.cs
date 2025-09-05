using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundEmitter : Singleton<EnemySoundEmitter>
{
    [SerializeField]AudioClip hitAudioClip;
    [SerializeField]AudioClip deadAudioClip;

    [SerializeField] AudioSource hitAudioSource;
    [SerializeField] AudioSource deadAudioSource;

    protected override void Initialize()
    {
        hitAudioSource.clip = hitAudioClip;
        deadAudioSource.clip = deadAudioClip;
    }

    public void Onhit()
    {
        if (hitAudioSource.isPlaying)
            return;
        hitAudioSource.Play();
    }

    public void OnDead()
    {
        if (deadAudioSource.isPlaying)
            return;
        deadAudioSource.Play();
    }
}
