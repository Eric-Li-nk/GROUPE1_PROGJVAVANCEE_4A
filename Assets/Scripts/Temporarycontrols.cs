using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Temporarycontrols : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject gameoverCanvas;

    [SerializeField] private TMP_Text endingText;

    public GameObject player1;
    public GameObject player2;

    public static Temporarycontrols instance;

    private void Awake()
    {
        instance = this;
    }

    public void OpenMenu(InputAction.CallbackContext context)
    {
        if (menuCanvas.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        menuCanvas.SetActive(!menuCanvas.activeSelf);
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

    public IEnumerator ChangeText()
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
