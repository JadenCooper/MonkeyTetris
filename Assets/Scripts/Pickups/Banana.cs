using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public PickupManager pickupManager;
    public Vector3Int Position;
    public int RipenessIndex = 0;
    private int[] RipenessTimes = new int[3];
    public int[] Score = new int[3];
    public void Setup(PickupManager pickupManager, Vector3Int Position)
    {
        // Set up the banana with initial values
        this.pickupManager = pickupManager;
        this.Position = Position;
        // Randomly determine ripeness times and scores
        RipenessTimes[0] = Random.Range(8, 12);
        RipenessTimes[1] = Random.Range(14, 18);
        RipenessTimes[2] = Random.Range(4, 8);
        Score[0] = Random.Range(8, 14);
        Score[1] = Random.Range(15, 20);
        Score[2] = Random.Range(4, 8);
        StartCoroutine(RipenessTimer());
    }

    public IEnumerator RipenessTimer()
    {
        // Coroutine that handles the ripeness timer
        yield return new WaitForSeconds(RipenessTimes[RipenessIndex]);
        RipenessIndex++;
        if (RipenessIndex != RipenessTimes.Length)
        {
            // Banana Is Still Valid And Not Yet Rotten
            Debug.Log("Ripeness Stage " + RipenessIndex);
            pickupManager.ChangeBananaTile(RipenessIndex, Position);
            StartCoroutine(RipenessTimer());
        }
        else
        {
            // Banana Is Rotten So Remove
            Debug.Log("Banana Is Rotten");
            pickupManager.RemoveBanana(Position);
        }
    }


}
