using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIManager UIManager;
    public Board board;
    public PickupManager pickupManager;
    public int bananaAmount;
    public Vector2 CurrentScore = Vector2.zero;
    public int ScoreGoal = 100;
    private void Awake()
    {
        UIManager = GetComponent<UIManager>();
        board.gameManager = this;
    }
    public void GameOver(int PlayerNumber)
    {
        Time.timeScale = 0f;
        UIManager.GameOver(PlayerNumber);
        pickupManager.StartGame();
    }

    public void PlayerChange(int PlayerNumber)
    {
        UIManager.PlayerChange(PlayerNumber);
    }

    public void SetBoardSize(int BoardSize)
    {
        UIManager.SetBoardSize(BoardSize);
    }

    public void BananaCollected(int PlayerIndex, int ScoreChange)
    {
        // Player Currently Gets 1 Score Per Banana
        int Playerscore = 0;
        if (PlayerIndex == 0)
        {
            //Player One
            CurrentScore.x += ScoreChange;
            Playerscore = (int)CurrentScore.x;
        }
        else
        {
            //Player Two
            CurrentScore.y += ScoreChange;
            Playerscore = (int)CurrentScore.y;
        }

        if (Playerscore >= ScoreGoal)
        {
            GameOver(PlayerIndex);
        }
        else
        {
            Debug.Log("Score Now Stands At Player One: " + CurrentScore.x + " Player Two: " + CurrentScore.y);
            bananaAmount--;
            if (bananaAmount <= 0)
            {
                // If All Bananas Are Collected Then Spawn Some More
                pickupManager.SpawnBananas();
            }
        }
    }
}
