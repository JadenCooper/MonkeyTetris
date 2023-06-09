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
    public SoundManager soundManager;
    private void Awake()
    {
        UIManager = GetComponent<UIManager>();
        board.gameManager = this;
    }
    public void GameOver(int PlayerNumber)
    {
        soundManager.PlaySound(2);
        Time.timeScale = 0f;
        UIManager.GameOver(PlayerNumber);
    }

    public void SetBoardSize(int BoardSize)
    {
        UIManager.SetBoardSize(BoardSize, ScoreGoal, CurrentScore);
    }

    public void PickupCollected(string Pickup)
    {
        UIManager.PickupCollected(Pickup);
    }

    public void BananaCollected(int PlayerIndex, int ScoreChange)
    {
        PickupCollected("Banana");
        // Player Currently Gets 1 Score Per Banana
        int Playerscore = 0;
        if (PlayerIndex == 0)
        {
            //Player One
            CurrentScore.x += ScoreChange;
            Playerscore = (int)CurrentScore.x;
            PlayerIndex = 1;
        }
        else
        {
            //Player Two
            CurrentScore.y += ScoreChange;
            Playerscore = (int)CurrentScore.y;
            PlayerIndex = 2;
        }

        UIManager.SetScores(CurrentScore);

        if (Playerscore >= ScoreGoal)
        {
            GameOver(PlayerIndex);
        }
        else
        {
            bananaAmount--;
            if (bananaAmount <= 0)
            {
                // If All Bananas Are Collected Then Spawn Some More
                pickupManager.SpawnBananas();
            }
        }
    }
}
