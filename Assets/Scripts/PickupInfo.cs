using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PickupType
{
    B, // Bannana
    L, // Line Clear
    E, //  Explosion
    J, // Junk
}
[System.Serializable]
public struct PickupData
{
    public PickupType pickUp;
    public Tile tile;
    public Vector3Int[] cells;
    public void Initalize()
    {
        this.cells = Data.Pickups[pickUp];
    }
}
