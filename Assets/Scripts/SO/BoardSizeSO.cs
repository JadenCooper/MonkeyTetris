using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBoardSizeData", menuName = "Data/BoardSizeData")]
public class BoardSizeSO : ScriptableObject
{
    public Vector2 pickupXRange = new Vector2();
    public Vector2 pickupYRange = new Vector2();
}
