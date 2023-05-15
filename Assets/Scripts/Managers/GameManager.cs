using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIManager UIManager;
    public Board board;
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
}
