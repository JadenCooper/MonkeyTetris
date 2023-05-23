using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBoardSizeData", menuName = "Data/BoardSizeData")]
public class BoardSizeSO : ScriptableObject
{
    // Holds Data For The Board Size
    public Vector2Int pickupXRange = new Vector2Int(); // Safe Pickup Spawn Range On The X Axis
    public Vector2Int pickupYRange = new Vector2Int(); // Safe Pickup Spawn Range On The Y Axis
}
