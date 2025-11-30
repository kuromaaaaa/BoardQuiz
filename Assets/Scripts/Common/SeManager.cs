using System.Collections.Generic;
using UnityEngine;

public class SeManager : SingletonMonoBehavior<SeManager>
{
    AudioSource _seAudioSource;
    private readonly Dictionary<SoundType, AudioClip> _soundDic = new();

    [SerializeField] private List<SoundEntry> soundList = new();


    private void Start()
    {
        _seAudioSource = GetComponent<AudioSource>();
        foreach (SoundEntry sfxEntry in soundList)
        {
            _soundDic.Add(sfxEntry.Type, sfxEntry.Clip);
        }
        
        
    }
    
    public void BgmChangeVolume(float volume)
    {
        _seAudioSource.volume = volume;
    }

    public void Play(SoundType type)
    {
        _seAudioSource.PlayOneShot(_soundDic[type]);
    }
}
