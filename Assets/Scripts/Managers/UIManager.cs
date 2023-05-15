using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_Text activePlayerText;
    public TMP_Text LostPlayerText;
    public void PlayerChange(int PlayerNumber)
    {
        activePlayerText.text = "Player " + PlayerNumber + " Active";
    }

    public void GameOver(int PlayerNumber)
    {
        LostPlayerText.text = "Player " + PlayerNumber + " Lost";
    }
}
