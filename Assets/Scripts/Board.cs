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

    private bool locked = false;

    public SpriteRenderer grid;
    public Transform border;
    public Ghost ghost;
    public GameManager gameManager;
    public List<Tile> playerColors = new List<Tile>();
    public BoardSizeSO boardSizeData;

    public PickupManager pickupManager;
    private int BoardSizeSetting = 10;
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
    }

    public void SpawnPiece()
    {
        // Check if there are picked bananas in the PickedBananas list
        if(pickupManager.PickedBananas.Count > 0){
            // Iterate through the PickedBananas list
            // Perform actions for each picked banana
            for(int i = 0; i <= pickupManager.PickedBananas.Count; i++){
                Banana BananaToBeRemoved = pickupManager.PickedBananas[i].GetComponent<Banana>();
                // Notify the gameManager that a banana with the specified PieceIndex has been collected
                gameManager.BananaCollected(PieceIndex, BananaToBeRemoved.Score[BananaToBeRemoved.RipenessIndex]);
                // Get the position of the current picked banana
                Vector3Int position = BananaToBeRemoved.Position;
                // Remove the current picked banana from the PickedBananas list
                pickupManager.PickedBananas.RemoveAt(i);
                // Remove the banana from the pickupManager using the obtained position
                pickupManager.RemoveBanana(position, false);
            }
        };
        TetrominoData data = tetrominos[Random.Range(0, tetrominos.Length)];
        PieceIndex++;
        PieceIndex = activePiece.Wrap(PieceIndex, 0, ControlDataList.Count);
        gameManager.PlayerChange(PieceIndex + 1);
        activePiece.pieceControls = ControlDataList[PieceIndex];
        data.tile = playerColors[PieceIndex];
        activePiece.Initialize(this, spawnPosition , data);
        ghost.trackingPiece = activePiece;

        if (!IsValidPosition(activePiece.cells, spawnPosition, true, false))
        {
            BoardFull();
            SpawnPiece(); // Makes Winner Start The Next Round
            return;
        }

        Set(activePiece);
    }
    private void BoardFull()
    {
        ClearLines();
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

    public bool IsValidPosition(Vector3Int[] cells, Vector3Int position, bool CheckPickup, bool pieceLocked)
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


    private void SpawnRandomObstacles()
    {
        int[] heightMap = new int[boardSize.x]; //Declare empty heightmap (Array of Ints)

        int currVal = Random.Range(0,4); //Starting elevation

        heightMap[0] = (currVal);

        for (int i = 1; i < boardSize.x; i++) //Generate Heightmap based on boardwidth
        {
            int rng = Random.Range(0,4); //Odds for changing elevatino on each step.
            //25% Increase, 25% Decrease, 50% Stay Same
            switch(rng)
            {
                case 0:
                    currVal++;
                    break;
                case 1:
                    currVal--;
                    break;
                default:
                    break;
            }
            currVal = Mathf.Clamp(currVal, 0, (boardSize.y / 3)); //Clamp heightmap val between zero and a third up the board.
            heightMap[i] = (currVal); //Commit the value to heightmap.
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
