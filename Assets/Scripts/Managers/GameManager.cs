using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIManager UIManager;
    public Board board;
    public PickupManager pickupManager;
    public int bananaAmount;
    public Vector2 Score = Vector2.zero;
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

    public void BananaCollected(int PlayerIndex)
    {
        // Player Currently Gets 1 Score Per Banana
        if (PlayerIndex == 0)
        {
            //Player One
            Score.x++;
        }
        else
        {
            //Player Two
            Score.y++;
        }
        Debug.Log("Score Now Stands At Player One: " + Score.x + " Player Two: " + Score.y);
        bananaAmount--;
        if (bananaAmount <= 0)
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
