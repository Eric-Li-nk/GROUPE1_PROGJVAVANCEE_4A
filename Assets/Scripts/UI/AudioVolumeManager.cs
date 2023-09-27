using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeManager : MonoBehaviour, ISaveableSettingsData
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private string volumeName;
    
    public void SetVolume(float val)
    {
        _mixer.SetFloat(volumeName, Mathf.Log10(val) * 20);
        valueText.text = Math.Round(val * 100) + " %";
    }

    public void Save(Settings data)
    {
        typeof(Settings).GetField(volumeName).SetValue(data, (float)Math.Round(_slider.value, 2));
    }

    public void Load(Settings data)
    {
        _slider.value = (float) typeof(Settings).GetField(volumeName).GetValue(data);
    }
}
