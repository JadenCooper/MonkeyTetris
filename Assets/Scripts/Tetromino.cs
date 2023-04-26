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

public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
}
