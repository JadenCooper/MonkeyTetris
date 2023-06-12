using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_Text activePlayerText;
    public TMP_Text LostPlayerText;
    public TMP_Text Player1ControlsText;
    public TMP_Text Player2ControlText;

    public PlayerControls player1Controls;
    public PlayerControls player2Controls;

    void Start()
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

    public void PlayerChange(int PlayerNumber)
    {
        activePlayerText.text = "Player " + PlayerNumber + " Active";
    }

    public void GameOver(int PlayerNumber)
    {
        LostPlayerText.text = "Player " + PlayerNumber + " Lost";
    }



}
