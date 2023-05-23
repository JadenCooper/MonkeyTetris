using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PickupType
{
    B
}
[System.Serializable]
public struct PickupData
{
    public PickupType pickUp;
    public Tile tile;
    public Vector2Int[] cells;
    public void Initalize()
    {
        this.cells = Data.Pickups[pickUp];
    }
}
