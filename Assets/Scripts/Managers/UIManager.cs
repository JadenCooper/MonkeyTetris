using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public List<TMP_Text> ListsOfTexts = new List<TMP_Text>();
    public List<string> ListsOfTextsStarters = new List<string>(); // EG Player One Score:
    public Camera mainCamera;
    public GameObject EndGameScreen;

    public TMP_Text Player1ControlsText;
    public TMP_Text Player2ControlText;
    public PlayerControls player1Controls;
    public PlayerControls player2Controls;
    private void Start()
    {
        //displays controls to the UI for player 1 and 2
        Player1ControlsText.text =
        "Move Left: " + player1Controls.MoveLeft.ToString() +
        "\nMove right: " + player1Controls.MoveRight.ToString() +
        "\nRotate: " + player1Controls.Rotate.ToString() +
        "\nSoft Drop : " + player1Controls.SoftDrop.ToString() +
        "\nHard Drop : " + player1Controls.HardDrop.ToString() + "\n";

        Player2ControlText.text =
        "Move Left: " + player2Controls.MoveLeft.ToString() +
        "\nMove right: " + player2Controls.MoveRight.ToString() +
        "\nRotate: " + player2Controls.Rotate.ToString() +
        "\nSoft Drop : " + player2Controls.SoftDrop.ToString() +
        "\nHard Drop : " + player2Controls.HardDrop.ToString() + "\n";
    }

    public void SetBoardSize(int BoardSize, int scoreGoal, Vector2 scores)
    {
        // Change Camera Sizes And Setup UI
        switch (BoardSize)
        {
            case 10:
                mainCamera.orthographicSize = 15;
                break;

            case 20:
                mainCamera.orthographicSize = 14;
                break;

            case 30:
                mainCamera.orthographicSize = 17;
                break;

            default:
                Debug.Log("SetBoardSize Broke");
                break;
        }

        ListsOfTexts[0].text = ListsOfTextsStarters[0] + scoreGoal.ToString();
        ListsOfTexts[1].text = ListsOfTextsStarters[1] + scores.x.ToString();
        ListsOfTexts[2].text = ListsOfTextsStarters[2] + scores.y.ToString();
    }

    public void PlayerChange(int PlayerNumber)
    {
        activePlayerText.text = "Player " + PlayerNumber + " Active";
    }

    public void GameOver(int PlayerNumber)
    {
        LostPlayerText.text = "Player " + PlayerNumber + " Lost";
    }



}
