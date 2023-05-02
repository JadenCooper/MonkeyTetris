using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board board;
    public Piece trackingPiece;

    public Tilemap tilemap;
    public Vector3Int position;
    public Vector3Int[] cells;

    private void Awake()
    {
        tilemap= GetComponentInChildren<Tilemap>();
        cells = new Vector3Int[4];
    }
    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }
    private void Clear()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            tilemap.SetTile(tilePosition, null);
        }
    }
    private void Copy()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = trackingPiece.cells[i];
        }
    }
    private void Drop()
    {
        Vector3Int Position = trackingPiece.position;

        int current = position.y;
        int bottem = - board.boardSize.y / 2 - 1;

        board.Clear(trackingPiece);

        for (int row = current; row >= bottem;  row--)
        {
            position.y = row;
            if (board.IsValidPosition(trackingPiece, position))
            {
                position = Position;
            }
            else
            {
                break;
            }
        }

        board.Set(trackingPiece);
    }
    private void Set()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            tilemap.SetTile(tilePosition, tile);
        }
    }
}
