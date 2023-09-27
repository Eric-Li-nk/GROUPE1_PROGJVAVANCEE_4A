using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public int width;
    public int height;
    public float aspectRatio;
    public bool fullscreen;

    public int quality;

    public float masterVolume;
    public float effectsVolume;
    public float musicVolume;

    public Settings()
    {
        fullscreen = true;
        quality = 2;
        masterVolume = 1f;
        effectsVolume = 1f;
        musicVolume = 1f;
    }

    public void SetResolution(Resolution resolution)
    {
        width = resolution.width;
        height = resolution.height;
        aspectRatio = (float) width / height;
    }

}
