using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenManager : MonoBehaviour, ISaveableSettingsData
{
    [SerializeField] private Toggle _fullscreenToggle;

    public void ToggleFullscreen(bool val)
    {
        StartCoroutine(Fullscreen(val));
    }

    private IEnumerator Fullscreen(bool val)
    {
        yield return new WaitForSeconds(0.05f);
        Screen.fullScreen = val;
    }
    
    public void Save(Settings data)
    {
        data.fullscreen = _fullscreenToggle.isOn;
    }

    public void Load(Settings data)
    {
        _fullscreenToggle.isOn = data.fullscreen;
    }
}
