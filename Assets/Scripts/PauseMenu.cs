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
    pausePanel.SetActive(true); // Show the pause panel or menu
    Time.timeScale = 0f; // Set the time scale to 0 to pause the game
    Debug.Log("Game paused");
}

void Update()
{
    // Check if the Escape key is pressed, if so pause game
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
}

public void ResumeGame()
{
    isPaused = false;
    Time.timeScale = 1f; // Set the time scale back to 1 to resume the game
    pausePanel.SetActive(false); // Hide the pause panel or menu
}

public void Restart()
{
    Time.timeScale = 1f; // Set the time scale back to 1 before loading the scene
    SceneManager.LoadScene("SinglePlayerScene"); // Load the scene named "SinglePlayerScene"
}

public void ExitToMenu()
{
    Time.timeScale = 1f; // Set the time scale back to 1 before loading the scene
    SceneManager.LoadScene("Menu"); // Load the scene named "Menu"
}

public void ExitGame()
{
    Application.Quit(); // Quit the application
}

}
