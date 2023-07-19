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

    public float PrevmusicSlider, PrevsfxSlider;
    public bool musicMute, sfxMute;

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
            SFXImg.sprite = SFXOff;
        }
        else
        {
            SFXImg.sprite = SFXOn;
        }

        _musicSlider.value = PrevmusicSlider = AudioManager.Instance.musicSource.volume;
        _sfxSlider.value = PrevsfxSlider = AudioManager.Instance.sfxSource.volume;
        musicMute = AudioManager.Instance.musicSource.mute;
        sfxMute = AudioManager.Instance.sfxSource.mute;
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
            SFXImg.sprite = SFXOff;
        }
        else
        {
            SFXImg.sprite = SFXOn;
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
        PrevmusicSlider = AudioManager.Instance.musicSource.volume;
        PrevsfxSlider = AudioManager.Instance.sfxSource.volume;
        musicMute = AudioManager.Instance.musicSource.mute;
        sfxMute = AudioManager.Instance.sfxSource.mute;

        Debug.Log("Saved");
    }

    public void ResetSettings()
    {
        if (!gameObject.activeSelf) { return; }

        _musicSlider.value = PrevmusicSlider;
        _sfxSlider.value = PrevsfxSlider;

        if (musicMute != AudioManager.Instance.musicSource.mute)
        {
            ToggleMusic();
        }
        if (sfxMute)
        {
            MusicImg.sprite = MusicOff;
        }
        else
        {
            MusicImg.sprite = MusicOn;
        }

        if (musicMute != AudioManager.Instance.sfxSource.mute)
        {
            ToggleSFX();
        }
        if (sfxMute)
        {
            SFXImg.sprite = SFXOff;
        }
        else
        {
            SFXImg.sprite = SFXOn;
        }
    }
}
