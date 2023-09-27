using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public Settings Load()
    {
        string fullpath = Path.Combine(dataDirPath, dataFileName);
        Settings loadedData = null;

        if (File.Exists(fullpath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullpath, FileMode.Open)) 
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                
                // Deserialize the data from Json to C# object
                loadedData = JsonUtility.FromJson<Settings>(dataToLoad);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured when trying to load data from file: " + fullpath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(Settings data)
    {
        string fullpath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // Create directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));
            
            // Game settings serialized to Json
            string dataToStore = JsonUtility.ToJson(data, true);

            // Write the datas to the specified file
            using (FileStream stream = new FileStream(fullpath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullpath + "\n" + e);
        }
    }
}
