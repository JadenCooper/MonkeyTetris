using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z
}
[System.Serializable]
public struct TetrominoData
{
    
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells;
    public Vector2Int[,] wallKicks;
    public void Initalize()
    {
        this.cells = Data.Cells[tetromino];
        wallKicks = Data.WallKicks[tetromino];
    }
}
