﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButtons : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    public void ChangeResolution1920x1080()
    {
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }

    public void ChangeResolution1600x1200()
    {
        Screen.SetResolution(1600, 1200, Screen.fullScreen);
    }

    public void ChangeResolution1280x720()
    {
        Screen.SetResolution(1280, 720, Screen.fullScreen);
    }

    public void ChangeQualityFast()
    {
        QualitySettings.SetQualityLevel(0, true);
    }

    public void ChangeQualityGood()
    {
        QualitySettings.SetQualityLevel(1, true);
    }

    public void ChangeQualityFantastic()
    {
        QualitySettings.SetQualityLevel(2, true);
    }

    public void ChangeQualityUltra()
    {
        QualitySettings.SetQualityLevel(3, true);
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    
    public void OnValueChanged()
    {
        AudioManager.SetMusicVolume(musicSlider.value);
        AudioManager.SetSFXVolume(sfxSlider.value);
    }
}

