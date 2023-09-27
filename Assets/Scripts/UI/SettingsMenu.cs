using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : AMenuManager
{
    
    public void Save()
    {
        DataManager.instance.Save();
    }

    public void Load()
    {
        DataManager.instance.Load();
    }

    public void Reset()
    {
        DataManager.instance.Reset();
    }
}
