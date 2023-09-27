using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCustomizationMenu : AMenuManager
{

    [SerializeField] private string _gameStartScene;

    public void Play()
    {
        Debug.Log("Starting game !");
        //SceneManager.LoadScene(scene);
    }
    
}
