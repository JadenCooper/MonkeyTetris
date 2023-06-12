using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pausePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public void ExitToMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
