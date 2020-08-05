

// ReSharper disable once CheckNamespace

using UnityEngine;

[System.Serializable]
public class AudioTrack
{
    public AudioSource source;
    public AudioObject[] audio;
    public bool pauseWithTimer = false;
}