using System.Collections.Generic;
using UnityEngine;

public class BgmManager : SingletonMonoBehavior<BgmManager>
{
    AudioSource _bgmAudioSource;
    
    private readonly Dictionary<SoundType, AudioClip> _soundDic = new();

    [SerializeField] private List<SoundEntry> soundList = new();


    private void Awake()
    {
        base.Awake();
        _bgmAudioSource = GetComponent<AudioSource>();
        foreach (SoundEntry sfxEntry in soundList)
        {
            _soundDic.Add(sfxEntry.Type, sfxEntry.Clip);
        }

        foreach (var soundEntry in _soundDic)
        {
            Debug.Log(soundEntry.Key);
        }
    }

    public void BgmChangeVolume(float volume)
    {
        _bgmAudioSource.volume = volume;
    }
    
    public void Play(SoundType type)
    {
        _bgmAudioSource.PlayOneShot(_soundDic[type]);
    }
}
