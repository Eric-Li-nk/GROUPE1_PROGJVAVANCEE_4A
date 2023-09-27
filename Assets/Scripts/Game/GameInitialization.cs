using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameInitialization : MonoBehaviour
{
    [SerializeField] private Transform blocList;
    [SerializeField] private GameObject blocPrefab;

    void Start()
    {
        SpawnBlocs();
    }

    private void SpawnBlocs()
    {
        string fileName = "Assets/Map/map1.txt";
        StreamReader reader = new StreamReader(fileName);
        char c = (char) reader.Read();
        int x = -1, z = 1;
        while (!reader.EndOfStream)
        {
            c = (char) reader.Read();
            
            Vector3 position = new Vector3(x, blocPrefab.transform.position.y, z);
            switch (c)
            {
                case '0':
                    Instantiate(blocPrefab, position, blocPrefab.transform.rotation, blocList);
                    x += 1;
                    break;
                case '\n':
                    x = -1;
                    z -= 1;
                    break;
                default:
                    x += 1;
                    break;
            }
        }
        reader.Close();

    }
}
