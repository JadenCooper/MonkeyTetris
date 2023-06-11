using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_Text activePlayerText;
    public TMP_Text LostPlayerText;
    public Camera mainCamera;
    public void PlayerChange(int PlayerNumber)
    {
        activePlayerText.text = "Player " + PlayerNumber + " Active";
    }

    public void GameOver(int PlayerNumber)
    {
        LostPlayerText.text = "Player " + PlayerNumber + " Lost";
    }

    public void SetBoardSize(int BoardSize)
    {
        switch (BoardSize)
        {
            case 10:
                mainCamera.orthographicSize = 12;
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
    }
}
