using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public List<TMP_Text> ListsOfTexts = new List<TMP_Text>();
    public List<string> ListsOfTextsStarters = new List<string>(); // EG Player One Score:
    public Camera mainCamera;

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

    public void SetScores(Vector2 scores)
    {
        // Update Score UI
        ListsOfTexts[1].text = ListsOfTextsStarters[1] + scores.x.ToString();
        ListsOfTexts[2].text = ListsOfTextsStarters[2] + scores.y.ToString();
    }

    public void PickupCollected(string Pickup)
    {
        ListsOfTexts[3].gameObject.SetActive(true);
        ListsOfTexts[3].text = Pickup + ListsOfTextsStarters[3];
        StopCoroutine(PickupDisplayTimer());
        StartCoroutine(PickupDisplayTimer());
    }

    public IEnumerator PickupDisplayTimer()
    {
        yield return new WaitForSeconds(2f);
        ListsOfTexts[3].gameObject.SetActive(false);
    }
}
