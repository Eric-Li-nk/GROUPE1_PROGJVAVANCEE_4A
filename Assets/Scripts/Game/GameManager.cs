using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject gameoverCanvas;

    [SerializeField] private TMP_Text endingText;

    [HideInInspector] public GameObject player1;
    [HideInInspector] public GameObject player2;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void OpenMenu(InputAction.CallbackContext context)
    {
        menuCanvas.SetActive(!menuCanvas.activeSelf);
        if (menuCanvas.activeSelf)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    public void Return()
    {
        Time.timeScale = 1f;
        menuCanvas.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void GameOver()
    {
        gameoverCanvas.SetActive(true);
        StartCoroutine(ChangeText());
    }

    private IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(0.05f);
        if (player1 == null)
        {
            endingText.text = "Player 2 is the winner !";
        }
        else if (player2 == null)
        {
            endingText.text = "Player 1 is the winner !";
        }
    }
}
