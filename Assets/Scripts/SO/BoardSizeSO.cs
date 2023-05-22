using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBoardSizeData", menuName = "Data/BoardSizeData")]
public class BoardSizeSO : ScriptableObject
{
    public Vector2Int pickupXRange = new Vector2Int();
    public Vector2Int pickupYRange = new Vector2Int();
}
