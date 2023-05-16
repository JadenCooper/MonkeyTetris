using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap;
    public Piece activePiece;
    public List<PlayerControls> ControlDataList = new List<PlayerControls>();
    public int PieceIndex = 0;
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public SpriteRenderer grid;
    public Transform border;
    public Ghost ghost;
    public GameManager gameManager;
    public List<Tile> playerColors = new List<Tile>();
    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-PlayerPrefs.GetInt("BoardSize") / 2, -20 / 2);
            return new RectInt(position, new Vector2Int(PlayerPrefs.GetInt("BoardSize"), 20));
        }
    }
    private void Awake()
    {
        Debug.Log(PlayerPrefs.GetInt("BoardSize"));
        tilemap = GetComponentInChildren<Tilemap>();

        activePiece = GetComponentInChildren<Piece>();
        activePiece.pieceControls = ControlDataList[0];
        ghost.trackingPiece = activePiece;

        // Set the boardSize to a new Vector2Int using PlayerPrefs.GetInt("BoardSize") as the x component and 20 as the y component
        boardSize = new Vector2Int(PlayerPrefs.GetInt("BoardSize"), 20);

        // Set the size of the grid using PlayerPrefs.GetInt("BoardSize") as the x component and 20 as the y component
        grid.size = new Vector2(PlayerPrefs.GetInt("BoardSize"), 20);

        // Scale the border GameObject based on the current local scale multiplied by PlayerPrefs.GetInt("BoardSize") for the x component 
        // and 20 for the y component, while keeping the z component unchanged
        border.transform.localScale = new Vector3((border.transform.localScale.x * PlayerPrefs.GetInt("BoardSize")), (border.transform.localScale.y * 20), 1);

        for (int i = 0; i < tetrominos.Length; i++)
        {
            tetrominos[i].Initalize();
        }
    }

    private void Start()
    {
        TetrominoData data = tetrominos[Random.Range(0, tetrominos.Length)];
        data.tile = playerColors[0];
        activePiece.Initialize(this, spawnPosition, data);
        Set(activePiece);
    }

    public void SpawnPiece()
    {
        TetrominoData data = tetrominos[Random.Range(0, tetrominos.Length)];
        PieceIndex++;
        PieceIndex = activePiece.Wrap(PieceIndex, 0, ControlDataList.Count);
        gameManager.PlayerChange(PieceIndex + 1);
        activePiece.pieceControls = ControlDataList[PieceIndex];
        data.tile = playerColors[PieceIndex];
        activePiece.Initialize(this, spawnPosition , data);
        ghost.trackingPiece = activePiece;

        if (!IsValidPosition(activePiece, spawnPosition))
        {
            GameOver();
            SpawnPiece(); // Makes Winner Start The Next Round
            return;
        }

        Set(activePiece);
    }
    private void GameOver()
    {
        tilemap.ClearAllTiles();
        gameManager.GameOver(PieceIndex + 1);
        Debug.Log("Game Over Player" + (PieceIndex + 1) + " Lost");
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

    //public void ClearLines()
    //{
    //    int row = Bounds.yMin;

    //    while (row < Bounds.yMax)
    //    {
    //        if (IsLineFull(row))
    //        {
    //            LineClear(row);
    //        }
    //        else
    //        {
    //            row++;
    //        }
    //    }
    //}

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
