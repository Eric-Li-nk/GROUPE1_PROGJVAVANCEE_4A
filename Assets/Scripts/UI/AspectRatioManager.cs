using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AspectRatioManager : MonoBehaviour, ISaveableSettingsData
{
    [SerializeField] private ResolutionManager _resolutionManager;
    [SerializeField] private TMP_Dropdown _aspectRatioDropdown;

    public void SwitchAspectRatio(int val)
    {
        float aspectRatio = GetAspectRatio(val);
        _resolutionManager.SwitchAspectRatio(aspectRatio);
    }

    private float GetAspectRatio(int val)
    {
        switch (val)
        {
            case 0:
                return 16f / 9f;
            case 1:
                return 4f / 3f;
            default:
                return 16f / 9f;
        }
    }
    
    private int GetAspectRatioValue(float val)
    {
        switch (val)
        {
            case 16f/9f:
                return 0;
            case 4f/3f:
                return 1;
            default:
                return 0;
        }
    }
    
    public void Save(Settings data)
    {
        data.aspectRatio = GetAspectRatio(_aspectRatioDropdown.value);
    }

    public void Load(Settings data)
    {
        _aspectRatioDropdown.value = GetAspectRatioValue(data.aspectRatio);
    }
}
