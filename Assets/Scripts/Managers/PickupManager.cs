using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public PickupData[] pickups;
    public GameManager gameManager;
    public Board gameBoard;
    private BoardSizeSO boardSizeData;

    private void Start()
    {
        boardSizeData = gameBoard.boardSizeData;
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

            FreePosition = gameBoard.IsValidPosition(pickup.cells, pickupSpawn, false); // Checks If Pickup Position Is Free
        } while (!FreePosition);

        for (int cell = 0; cell < pickup.cells.Length; cell++)
        {
            Vector3Int tilePosition = pickup.cells[cell] + pickup.position;
            gameBoard.tilemap.SetTile(tilePosition, pickup.data.tile);
        }
    }
}
