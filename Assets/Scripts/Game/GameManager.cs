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
        SceneManager.LoadScene("Game Scene");
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void GameOver()
    {
        OpenMenu(gameoverCanvas);
        StartCoroutine(ChangeText());
    }

    private void OpenMenu(GameObject canvas)
    {
        canvas.SetActive(!canvas.activeSelf);
        Time.timeScale = canvas.activeSelf ? 0f : 1f;
    }

    private IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(0.1f);
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
