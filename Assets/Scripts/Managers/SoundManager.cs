using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField]
    List<AudioSource> _audioList;
    Queue<AudioSource> _audioPool;

    [SerializeField]
    AudioSource _audio;

    public void PlaySound(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }
}
