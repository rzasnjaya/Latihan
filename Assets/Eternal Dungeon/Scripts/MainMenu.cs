using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioManager3 audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager3>();

        audioManager.PlayMenuMusic();
    }
    public void ContinueGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
