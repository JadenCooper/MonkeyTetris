using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public PickupManager pickupManager;
    public Vector3Int Position;
    public int RipenessIndex = 0;
    private int[] RipenessTimes = new int[3];
    public void Setup(PickupManager pickupManager, Vector3Int Position)
    {
        this.pickupManager = pickupManager;
        this.Position = Position;
        RipenessTimes[0] = 10;
        RipenessTimes[1] = 15;
        RipenessTimes[2] = 5;
        StartCoroutine(RipenessTimer());
    }

    public IEnumerator RipenessTimer()
    {
        yield return new WaitForSeconds(RipenessTimes[RipenessIndex]);
        RipenessIndex++;
        if (RipenessIndex != RipenessTimes.Length)
        {
            // Banana Is Still Valid
            StartCoroutine(RipenessTimer());
        }
        else
        {
            // Banana Is Rotten

        }
    }


}
