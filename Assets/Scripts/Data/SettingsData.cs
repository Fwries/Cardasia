using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    public bool MusicMute, SFXMute;
    public float MusicVolume, SFXVolume;

    public SettingsData(bool _MusicMute, bool _SFXMute, float _MusicVolume, float _SFXVolume)
    {
        MusicMute = _MusicMute;
        SFXMute = _SFXMute;
        MusicVolume = _MusicVolume;
        SFXVolume = _SFXVolume;
    }
}
