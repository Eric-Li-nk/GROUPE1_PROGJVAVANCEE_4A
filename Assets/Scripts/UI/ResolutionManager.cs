using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionManager : MonoBehaviour, ISaveableSettingsData
{
    [SerializeField] private AspectRatioManager _aspectRatioManager;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    private List<Resolution> resolutions;
    private float aspectRatio = 16f/9f;

    private void Start()
    {
        GenerateDropdown();
    }

    public void SwitchResolution(int val)
    {
        Resolution resolution = resolutions[val];
        int width = resolution.width;
        int height = resolution.height;
        Screen.SetResolution(width, height, Screen.fullScreenMode);
    }

    public void SwitchAspectRatio(float aspectRatio)
    {
        this.aspectRatio = aspectRatio;
        GenerateDropdown();
        _resolutionDropdown.value = _resolutionDropdown.options.Count - 1;
    }

    private void GenerateResolutionList()
    {
        Resolution[] resolutionlist = Screen.resolutions;
        this.resolutions = new List<Resolution>();
        
        foreach (var resolution in resolutionlist)
        {
            if(((float)resolution.width / resolution.height).Equals(aspectRatio))
                this.resolutions.Add(resolution);
        }
    }

    private void GenerateDropdown()
    {
        _resolutionDropdown.ClearOptions();
        
        GenerateResolutionList();
        
        List<string> options = new List<string>();

        int i = 0;
        int currentResolutionIndex = 0;
        foreach (var resolution in resolutions)
        {
            options.Add(resolution.ToString());
            if (resolution.Equals(Screen.currentResolution))
                currentResolutionIndex = i;
            i++;
        }
        
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
    }

    public void Save(Settings data)
    {
        data.SetResolution(resolutions[_resolutionDropdown.value]);
    }

    public void Load(Settings data)
    {
        if (!data.aspectRatio.Equals(aspectRatio) )
        {
            _aspectRatioManager.Load(data);
        }
        GenerateDropdown();
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (resolutions[i].width == data.width && resolutions[i].height == data.height)
                _resolutionDropdown.value = i;
        }
    }
}
