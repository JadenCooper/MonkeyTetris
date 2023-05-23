using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Vector3Int[] cells;
    public PickupData data;
    public Vector3Int position;

    public void Initialize(Vector3Int position, PickupData data)
    {
        this.position = position;
        this.data = data;
        cells = new Vector3Int[data.cells.Length];

        for (int i = 0; i < data.cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }

    public virtual void PreformEffect()
    {

    }
}
