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
        UIManager.GameOver(PlayerNumber);
        pickupManager.StartGame();
    }

    public void PlayerChange(int PlayerNumber)
    {
        UIManager.PlayerChange(PlayerNumber);
    }

    public void BananaCollected(int PlayerIndex, int ScoreChange)
    {
        // Player Currently Gets 1 Score Per Banana
        if (PlayerIndex == 0)
        {
            //Player One
            CurrentScore.x += ScoreChange;
        }
        else
        {
            //Player Two
            CurrentScore.y += ScoreChange;
        }
        Debug.Log("Score Now Stands At Player One: " + CurrentScore.x + " Player Two: " + CurrentScore.y);
        bananaAmount--;
        if (bananaAmount <= 0)
        {
            // If All Bananas Are Collected Then Spawn Some More
            pickupManager.SpawnBananas();
        }
    }
}
