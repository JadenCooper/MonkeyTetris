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
    public SoundManager soundManager;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    private bool locked = false;

    public SpriteRenderer grid;
    public Transform border;
    public Ghost ghost;
    public GameManager gameManager;
    public List<Tile> playerColors = new List<Tile>();
    public BoardSizeSO boardSizeData;

    public PickupManager pickupManager;
    public int BoardSizeSetting = 10;
    private bool spawning = false;
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

        BoardSizeSetting = PlayerPrefs.GetInt("BoardSize");

        // Set the boardSize to a new Vector2Int using PlayerPrefs.GetInt("BoardSize") as the x component and 20 as the y component
        boardSize = new Vector2Int(BoardSizeSetting, 20);

        // Set the size of the grid using PlayerPrefs.GetInt("BoardSize") as the x component and 20 as the y component
        grid.size = new Vector2(BoardSizeSetting, 20);

        // Scale the border GameObject based on the current local scale multiplied by PlayerPrefs.GetInt("BoardSize") for the x component 
        // and 20 for the y component, while keeping the z component unchanged
        border.transform.localScale = new Vector3((border.transform.localScale.x * BoardSizeSetting), (border.transform.localScale.y * 20), 1);
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
        pickupManager.StartGame();
        // Spawn random cubes
        SpawnRandomObstacles();

        switch (BoardSizeSetting)
        {
            // Sets Score To Win The Game Based Off Of The Size Of The Board
            case 10:
                // Quick And Small
                gameManager.ScoreGoal = 100;
                break;

            case 20:
                // Medium
                gameManager.ScoreGoal = 200;
                break;

            case 30:
                // Long And large
                gameManager.ScoreGoal = 300;
                break;

            default:
                Debug.Log("Setting Score Goal Broke");
                break;
        }
        gameManager.SetBoardSize(BoardSizeSetting);
    }

    public void SpawnPiece()
    {
        if (!spawning) // This Stops Multiple Spawnings From Happening Concurrently Breaking The Game
        {
            spawning = true;
            // Check if there are picked bananas in the PickedBananas list
            if (pickupManager.PickedBananas.Count > 0)
            {
                // Iterate through the PickedBananas list
                // Perform actions for each picked banana
                for (int i = 0; i <= pickupManager.PickedBananas.Count; i++)
                {
                    Banana banana = pickupManager.PickedBananas[i].GetComponent<Banana>();
                    // Notify the gameManager that a banana with the specified PieceIndex has been collected
                    gameManager.BananaCollected(PieceIndex, banana.Score[banana.RipenessIndex]);
                    // Get the position of the current picked banana
                    Vector3Int position = banana.Position;
                    // Remove the current picked banana from the PickedBananas list
                    pickupManager.PickedBananas.RemoveAt(i);
                    // Remove the banana from the pickupManager using the obtained position
                    pickupManager.RemoveBanana(position, false);
                    // line/lines clear when banana is collected
                    LineClear(Bounds.yMin);
                }
                soundManager.PlaySound(0);
            };

            for (int i = 0; i < activePiece.cells.Length; i++)
            {
                // Change Piece Tiles To The Set Version
                Vector3Int tilePosition = activePiece.cells[i] + activePiece.position;
                tilemap.SetTile(tilePosition, playerColors[PieceIndex + 2]);
            }

            TetrominoData data = tetrominos[Random.Range(0, tetrominos.Length)];
            PieceIndex++;
            PieceIndex = activePiece.Wrap(PieceIndex, 0, ControlDataList.Count);
            activePiece.pieceControls = ControlDataList[PieceIndex];
            data.tile = playerColors[PieceIndex];
            activePiece.Initialize(this, spawnPosition, data);
            ghost.trackingPiece = activePiece;

            if (!IsValidPosition(activePiece.cells, spawnPosition, true, false, false))
            {
                BoardFull();
                return;
            }
            else
            {
                spawning = false;
            }

            Set(activePiece);
        }
    }
    private void BoardFull()
    {
        ClearLines();
        pickupManager.ResetPickups();
        SpawnRandomObstacles();
        soundManager.PlaySound(1);
        spawning = false;
        //SpawnPiece();
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

    public bool IsValidPosition(Vector3Int[] cells, Vector3Int position, bool CheckPickup, bool pieceLocked, bool Ghost)
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
                    return pickupManager.CheckForPickUp(tilePosition, PieceIndex, pieceLocked);
                }
                else if (ghost)
                {
                    return pickupManager.GhostCheckForPickUp(tilePosition);
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

    public void ClearLines()
    {
        // Clear Bottom Five Lines When Board Full
        int row = Bounds.yMin;

        while (row < 5)
        {
            LineClear(row);
            row++;
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

    public void LineClear(int row)
    {
        int rowsAffected = Random.Range(1, 4); // Generate a random number between 1 and 3 (inclusive)
        //deletes the amount of rows from the random number
        for (int i = 0; i < rowsAffected; i++)
        {
            for (int col = Bounds.xMin; col < Bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, null);
            }
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



    public void SpawnRandomObstacles()
    {
        int[] heightMap = new int[boardSize.x]; //Declare empty heightmap (Array of Ints)

        float startX = Random.Range(0f,1f); //Starting elevation

        for (int i = 0; i < boardSize.x; i++) //Generate Heightmap based on boardwidth
        {
            heightMap[i] = (int)(Mathf.PerlinNoise(startX, i * 0.05f) * 10f); //Generate height value using Perlin noise, with a randomized X component. Amplitude of 10 and Frequency of 0.05
        }

        for (int i = 0; i < boardSize.x; i++) //For the width of our board...
        {
            for (int j = 0; j < heightMap[i]; j++) //And for how high each row is...
            {
                //Place an obstacle tile at the right position.
                TileBase obstacleTile = obstacleTiles[Random.Range(0, obstacleTiles.Length)];

                if (obstacleTile != null)
                {
                    int x = i - boardSize.x/2;
                    int y = j - boardSize.y/2;
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    tilemap.SetTile(tilePosition, obstacleTile);
                }
            }
        }
    }
}
