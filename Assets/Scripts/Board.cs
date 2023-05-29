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
    public PickupData[] pickups;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    private bool locked = false;

    public SpriteRenderer grid;
    public Transform border;
    public Ghost ghost;
    public GameManager gameManager;
    public List<Tile> playerColors = new List<Tile>();
    public BoardSizeSO boardSizeData;
    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }
    public TileBase[] obstacleTiles; // Array to hold obstacle tiles
    private void Awake()
    {
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
        SpawnPickups();
        // Spawn random cubes
        SpawnRandomObstacles();
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

        if (!IsValidPosition(activePiece.cells, spawnPosition, true))
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
        SpawnRandomObstacles();
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

    public bool IsValidPosition(Vector3Int[] cells, Vector3Int position, bool CheckPickup)
    {
        // Checks If All Cells Would Be Valid After Move
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;

            if (!Bounds.Contains((Vector2Int)tilePosition))
            {
                // Checks If Movement Would Still Be Contained Within The Bounds Of The Board If Not Then False
                return false;
            }

            if (tilemap.HasTile(tilePosition))
            {
                // Checks If Tile Already Present If So Cant Move There, Therefore False Unless Its A Pickup
                if (CheckPickup) // The CheckPickup Bool Is Used To Make Sure Only The Player Piece Can Trigger The Pickup
                {
                    return CheckForPickUp(tilePosition);
                }
                else
                {
                    return false;
                }
            }
        }

        // Movement Is Valid
        return true;
    }

    public bool CheckForPickUp(Vector3Int tilePosition)
    {
        // Checks If Tile Is One Of The Pickup Tiles, If Pickup, Triggers Their Effect
        TileBase tileBase = tilemap.GetTile(tilePosition);
        switch (tileBase.name)
        {
            case "Yellow": // Banana Tile
                
                if(activePiece.locked){
                    Debug.Log("here");
                    tilemap.SetTile(tilePosition, null);
                    gameManager.BananaCollected(PieceIndex);
                    return true;
                }else{
                    return false;
                }
                

            default: // Normal Tile
                return false;
        }
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

    public void SpawnPickups()
    {
        int bananaAmount = Random.Range(1, 4);
        for (int i = 0; i < bananaAmount; i++)
        {
            SpawnPickup();
        }
        gameManager.bananaAmount = bananaAmount;
    }

    public void SpawnPickup()
    {
        Pickup pickup = new();
        PickupData data = pickups[Random.Range(0, pickups.Length)];
        bool FreePosition = true;
        do
        {
            // -5 X  Left 4 X Right  9 Y Top -10 Y Bottom For Default 10/20 Board
            Vector3Int pickupSpawn = new Vector3Int(Random.Range(boardSizeData.pickupXRange.x, boardSizeData.pickupXRange.y),
                Random.Range(boardSizeData.pickupYRange.x, boardSizeData.pickupYRange.y), 0);
            pickup.Initialize(pickupSpawn, data);

            FreePosition = IsValidPosition(pickup.cells, pickupSpawn, false); // Checks If Pickup Position Is Free
        } while (!FreePosition);

        for (int cell = 0; cell < pickup.cells.Length; cell++)
        {
            Vector3Int tilePosition = pickup.cells[cell] + pickup.position;
            tilemap.SetTile(tilePosition, pickup.data.tile);
        }
    }

    private void SpawnRandomObstacles()
    {
        int obstacleCount = Random.Range(3, 6); // Randomly choose the number of obstacles to spawn (between 3 and 5)

        for (int i = 0; i < obstacleCount; i++)
        {
            // Generate a random position within the board bounds
            int x = Random.Range(0, boardSize.x) - boardSize.x/2;
            int y = Random.Range(0, boardSize.y) - boardSize.y/2;

            // Generate a random number between 0 and the number of obstacle tiles
            int randomIndex = Random.Range(0, obstacleTiles.Length);

            // Get the random obstacle tile from the tile palette
            TileBase obstacleTile = obstacleTiles[randomIndex];

            // If the obstacle tile exists, spawn it at the randomly chosen position
            if (obstacleTile != null)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                tilemap.SetTile(tilePosition, obstacleTile);
            }
        }
    }
}
