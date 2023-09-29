using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }

    [SerializeField] private string filename;
    private Settings _settings;

    private FileDataHandler _fileDataHandler;
    private List<ISaveableSettingsData> _listSaveableData;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Multiple instance of DataManager detected !");
        instance = this;
    }

    private void Start()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, filename);
        _listSaveableData = new List<ISaveableSettingsData>(FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveableSettingsData>());
        Load();
    }

    private void NewData()
    {
        _settings = new Settings();
        Resolution currentRes = Screen.resolutions.Last();
        _settings.width = currentRes.width;
        _settings.height = currentRes.height;
    }

    public void Save()
    {
        foreach (var data in _listSaveableData)
        {
            data.Save(_settings);
        }
        _fileDataHandler.Save(_settings);
    }

    public void Load()
    {
        _settings = _fileDataHandler.Load();

        if (_settings == null)
        {
            NewData();
        }
        
        Load(_settings);
    }

    private void Load(Settings settings)
    {
        foreach (var data in _listSaveableData)
        {
            data.Load(settings);
        }
    }

    public void Reset()
    {
        NewData();
        Load(_settings);
    }
}
