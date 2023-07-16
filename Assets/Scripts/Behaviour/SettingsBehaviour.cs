using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsBehaviour : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;
    public Image MusicImg, SFXImg;

    public Sprite MusicOn, MusicOff;
    public Sprite SFXOn, SFXOff;

    void Start()
    {
        save.Instance.LoadSettings();

        if (AudioManager.Instance.musicSource.mute)
        {
            MusicImg.sprite = MusicOff;
        }
        else
        {
            MusicImg.sprite = MusicOn;
        }

        if (AudioManager.Instance.sfxSource.mute)
        {
            SFXImg.sprite = SFXOn;
        }
        else
        {
            SFXImg.sprite = SFXOff;
        }

        _musicSlider.value = AudioManager.Instance.musicSource.volume;
        _sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        
        if (AudioManager.Instance.musicSource.mute)
        {
            MusicImg.sprite = MusicOff;
        }
        else
        {
            MusicImg.sprite = MusicOn;
        }
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();

        if (AudioManager.Instance.sfxSource.mute)
        {
            SFXImg.sprite = SFXOn;
        }
        else
        {
            SFXImg.sprite = SFXOff;
        }
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }

    public void SaveSettings()
    {
        save.Instance.SaveSettings();
    }
}
