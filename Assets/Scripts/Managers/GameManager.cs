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

    public void BannaCollected(int PlayerIndex)
    {
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
    }
}
