using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundEmitter : Singleton<EnemySoundEmitter>
{
    AudioClip hitAudioClip;
    AudioClip deadAudioClip;

    AudioSource hitAudioSource;
    AudioSource deadAudioSource;

    protected override void Initialize()
    {
        dontDestroyOnLoad = false;
        hitAudioClip = Resources.Load("Sounds/SoundEffect/HitSound") as AudioClip;
        deadAudioClip = Resources.Load("Sounds/SoundEffect/MonsterDeath") as AudioClip;

        hitAudioSource = new GameObject("HitAudioSource").AddComponent<AudioSource>();
        hitAudioSource.transform.SetParent(transform);
        hitAudioSource.clip = hitAudioClip;
        deadAudioSource = new GameObject("DeadAudioSource").AddComponent<AudioSource>();
        deadAudioSource.transform.SetParent(transform);
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
