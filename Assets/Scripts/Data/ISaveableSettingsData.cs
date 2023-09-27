using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveableSettingsData
{
    public void Save(Settings data);
    public void Load(Settings data);
}
