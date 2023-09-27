using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCustomizationMenu : AMenuManager
{
    
    [SerializeField] private GameConfig _gameConfig;
    
    [SerializeField] private string _gameStartScene;

    [SerializeField] private TMP_Dropdown _agent1Dropdown;
    [SerializeField] private TMP_Dropdown _agent2Dropdown;

    public void Play()
    {
        SaveGameConfig();
        SceneManager.LoadScene(_gameStartScene);
    }

    private void SaveGameConfig()
    {
        _gameConfig.player1 = GetAgent(_agent1Dropdown);
        _gameConfig.player2 = GetAgent(_agent2Dropdown);
    }

    private PlayerType GetAgent(TMP_Dropdown agentDropdown)
    {
        switch (agentDropdown.value)
        {
            case 0:
                return PlayerType.Human;
                break;
            case 1:
                return PlayerType.Random;
                break;
            case 2:
                return PlayerType.MCTS;
                break;
            default:
                return PlayerType.None;
        }
    }
}
