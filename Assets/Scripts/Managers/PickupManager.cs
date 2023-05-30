using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PickupManager : MonoBehaviour
{
    public PickupData[] pickups;
    public GameObject BananaHolder;
    public List<GameObject> BananaList = new List<GameObject>();
    public GameManager gameManager;
    public Board gameBoard;
    public List<Tile> BananaRipenessTiles = new List<Tile>();
    public BoardSizeSO boardSizeData;
    public int BananaAmount;

    public void StartGame()
    {
        SpawnBananas();
        SpawnPickups();
    }
    public void SpawnBananas()
    {
        BananaAmount = Random.Range(1, 4);
        for (int i = 0; i < BananaAmount; i++)
        {
            BananaList.Add(Instantiate(BananaHolder));
            BananaList[i].GetComponent<Banana>().Setup(this, SpawnPickup());
        }
        gameManager.bananaAmount = BananaAmount;
    }
    public void SpawnPickups()
    {
        // Normal Pickups Will Go Here
    }

    public Vector3Int SpawnPickup()
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

            FreePosition = gameBoard.IsValidPosition(pickup.cells, pickupSpawn, false); // Checks If Pickup Position Is Free
        } while (!FreePosition);

        for (int cell = 0; cell < pickup.cells.Length; cell++)
        {
            Vector3Int tilePosition = pickup.cells[cell] + pickup.position;
            gameBoard.tilemap.SetTile(tilePosition, pickup.data.tile);
        }

        return pickup.position;
    }

    public bool CheckForPickUp(Vector3Int tilePosition, int PieceIndex)
    {
        // Checks If Tile Is One Of The Pickup Tiles, If Pickup, Triggers Their Effect
        TileBase tileBase = gameBoard.tilemap.GetTile(tilePosition);
        if (tileBase.name.Contains("Yellow"))
        {
            // Banana Tile
            RemoveBanana(tilePosition);
            gameManager.BananaCollected(PieceIndex);
            return true;
        }
        else
        {
            // Normal Tile
            return false;
        }
    }

    public void ChangeBananaTile(int RipenessIndex, Vector3Int Position)
    {
        if (gameBoard.tilemap.GetTile(Position).name.Contains("Yellow"))
        {
            gameBoard.tilemap.SetTile(Position, BananaRipenessTiles[RipenessIndex]);
        }
    }

    public void RemoveBanana(Vector3Int Position)
    {
        for (int i = 0; i < BananaList.Count; i++)
        {
            if (BananaList[i].GetComponent<Banana>().Position == Position)
            {
                // This Is The Correct Holder
                Destroy(BananaList[i]);
                BananaList.RemoveAt(i);
                BananaAmount--;
                if (BananaAmount <= 0)
                {
                    SpawnBananas();
                }
                gameBoard.tilemap.SetTile(Position, null);
            }
        }
    }
}
