using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap;
    public Piece activePiece;
    public List<Piece> PieceList = new List<Piece>();
    public List<PlayerControls> ControlDataList = new List<PlayerControls>();
    public int PieceIndex = 0;
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Ghost ghost;
    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }
    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();

        PieceList.AddRange(GetComponentsInChildren<Piece>()); // Get All Pieces And Assign Them To The List
        activePiece = PieceList[0];
        for (int i = 0; i < PieceList.Count; i++)
        {
            // Assign The Player Controls To The Pieces
            PieceList[i].pieceControls = ControlDataList[i];
            if (i != 0)
            {
                // Makes Every Piece Besides The First Piece To NonActive
                PieceList[i].gameObject.SetActive(false);
            }
        }

        ghost.trackingPiece = activePiece;

        for (int i = 0; i < tetrominos.Length; i++)
        {
            tetrominos[i].Initalize();
        }
    }

    private void Start()
    {
        TetrominoData data = tetrominos[Random.Range(0, tetrominos.Length)];

        activePiece.Initialize(this, spawnPosition, data);

        if (!IsValidPosition(activePiece, spawnPosition))
        {
            GameOver();
        }

        Set(activePiece);
    }

    public void SpawnPiece()
    {
        TetrominoData data = tetrominos[Random.Range(0, tetrominos.Length)];

        int temp = PieceIndex;
        PieceIndex++;
        PieceIndex = activePiece.Wrap(PieceIndex, 0, PieceList.Count);
        PieceList[temp].gameObject.SetActive(false); // Make Previous Piece Non Active
        PieceList[PieceIndex].gameObject.SetActive(true); // Make New Piece Active
        activePiece = PieceList[PieceIndex];
        activePiece.Initialize(this, spawnPosition , data);
        ghost.trackingPiece = activePiece;

        if (!IsValidPosition(activePiece, spawnPosition))
        {
            GameOver();
        }

        Set(activePiece);
    }
    private void GameOver()
    {
        tilemap.ClearAllTiles();
        Debug.Log("Game Over");
    }
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            // Add Piece Position To Get The Actual Location Compared To Local Position
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        // Checks If All Cells Would Be Valid After Move
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!Bounds.Contains((Vector2Int)tilePosition))
            {
                // Checks If Movement Would Still Be Contained Within The Bounds Of The Board If Not Then False
                return false;
            }

            if (tilemap.HasTile(tilePosition))
            {
                // Checks If Tile Already Present If So Cant Move There, Therefore False
                return false;
            }
        }

        // Movement Is Valid
        return true;
    }

    public void ClearLines()
    {
        int row = Bounds.yMin;

        while (row < Bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        for (int col = Bounds.xMin; col < Bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }

    private void LineClear(int row)
    {
        for (int col = Bounds.xMin; col < Bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        while (row < Bounds.yMax)
        {
            for (int col = Bounds.xMin; col < Bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }

}
