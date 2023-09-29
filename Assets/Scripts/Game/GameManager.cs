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

    private bool isGameOver = false;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if(context.performed)
            OpenMenu(menuCanvas);
    }

    public void Return()
    {
        Time.timeScale = 1f;
        menuCanvas.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game Scene");
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start Menu");
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            OpenMenu(gameoverCanvas);
            StartCoroutine(ChangeText());
        }
    }

    private void OpenMenu(GameObject canvas)
    {
        canvas.SetActive(!canvas.activeSelf);
        Time.timeScale = canvas.activeSelf ? 0f : 1f;
    }

    private IEnumerator ChangeText()
    {
        yield return new WaitForSecondsRealtime(0.05f);
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
