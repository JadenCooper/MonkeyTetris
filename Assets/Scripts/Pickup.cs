using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Pickup
{
    B
}
public struct PickupData
{
    public Pickup pickUp;
    public Tile tile;
    public Vector2Int[] cells;
    public void Initalize()
    {
        this.cells = Data.Pickups[pickUp];
    }
}
