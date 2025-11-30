using System;
using UnityEngine;

[Serializable]
public class SoundEntry
{
    [SerializeField] private SoundType type;
    [SerializeField] private AudioClip clip;

    public SoundType Type => type;
    public AudioClip Clip => clip;
}


public enum SoundType
{
    Questions,
    Correct,
    Incorrect,
    PA,
    DrumRoll,
    BGMTEST
}