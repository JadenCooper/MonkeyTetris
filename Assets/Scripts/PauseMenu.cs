using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pausePanel;

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Set the time scale to 0 to pause the game
        Debug.Log("Game paused");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Set the time scale to 0 to pause the game
        pausePanel.SetActive(false);
    }

    public void Restart(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("SinglePlayerScene");
    }

    public void ExitToMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
