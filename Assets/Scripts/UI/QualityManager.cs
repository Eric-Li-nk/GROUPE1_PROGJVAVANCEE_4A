using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QualityManager : MonoBehaviour, ISaveableSettingsData
{
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    private void Start()
    {
        GenerateDropdown();
    }

    public void SetQualityLevel(int val)
    {
        QualitySettings.SetQualityLevel(val);
    }

    private void GenerateDropdown()
    {
        _qualityDropdown.ClearOptions();

        List<string> options = new List<string>();
        options.AddRange(QualitySettings.names);

        _qualityDropdown.AddOptions(options);
        _qualityDropdown.value = QualitySettings.GetQualityLevel();
    }

    public void Save(Settings data)
    {
        data.quality = _qualityDropdown.value;
    }

    public void Load(Settings data)
    {
        GenerateDropdown();
        _qualityDropdown.value = data.quality;
    }
}
