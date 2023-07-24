using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class AudioAssetBundle
    {
        public AudioClip BGM_Lobby;
    }

    public enum BGMKind
    {
        BGM_Lobby,
        BGM_PlayScene,
        End
    }

    public enum FSXKind
    {
        FSX_Fire,
        FSX_Explosion,
        End
    }
}

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField]
    List<AudioSource> _audioList;
    Queue<AudioSource> _audioPool;

    [SerializeField]
    AudioSource _audio;

    [SerializeField]
    AudioAssetBundle _audioBundle;

    protected override void Awake()
    {
        base.Awake();

        TryGetComponent(out _audio);
    }

    public void PlayBGM(Data.BGMKind bgm)
    {
        switch (bgm)
        {
            case BGMKind.BGM_Lobby:
                _audio.clip = _audioBundle.BGM_Lobby;
                _audio.Play();
                break;
            case BGMKind.BGM_PlayScene:
                break;
            default:
                break;
        }
    }

    public void PlaySound(Data.FSXKind fsx)
    {
        //_audio.PlayOneShot();
    }
}
