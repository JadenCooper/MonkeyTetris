using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIManager UIManager;
    public Board board;
    public Vector2 Score = Vector2.zero;
    private void Awake()
    {
        UIManager = GetComponent<UIManager>();
        board.gameManager = this;
    }
    public void GameOver(int PlayerNumber)
    {
        UIManager.GameOver(PlayerNumber);
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
    }
}
