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
    public List<GameObject> PickedBananas = new List<GameObject>();

    public void StartGame()
    {
        SpawnBananas();
        SpawnPickups();
    }
    private void Update()
    {
        if(PickedBananas.Count > 0){
            for(int i = 0; i < PickedBananas.Count; i++){
                // Check if the position of the current PickedBanana is valid on the game board
                if(gameBoard.IsValidPosition(pickups[0].cells, PickedBananas[i].GetComponent<Banana>().Position, false, false)){
                    // Set the tile at the current PickedBanana's position on the tilemap
                    gameBoard.tilemap.SetTile(PickedBananas[i].GetComponent<Banana>().Position, BananaRipenessTiles[PickedBananas[i].GetComponent<Banana>().RipenessIndex]);
                    // Remove the current PickedBanana from the PickedBananas list
                    PickedBananas.RemoveAt(i);
                }
            }
            
        }
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

            FreePosition = gameBoard.IsValidPosition(pickup.cells, pickupSpawn, false, false); // Checks If Pickup Position Is Free
        } while (!FreePosition);

        for (int cell = 0; cell < pickup.cells.Length; cell++)
        {
            Vector3Int tilePosition = pickup.cells[cell] + pickup.position;
            gameBoard.tilemap.SetTile(tilePosition, pickup.data.tile);
        }

        return pickup.position;
    }


    // Check if the tile at the given tilePosition is a pickup tile
    // If it is a yellow banana tile, find and add matching bananas to the PickedBananas list
    // Return true if the tile is a pickup tile, false otherwise
    public bool CheckForPickUp(Vector3Int tilePosition, int PieceIndex, bool pieceLocked)
    {
        Debug.Log("here");
        // Checks If Tile Is One Of The Pickup Tiles, If Pickup, Triggers Their Effect
        TileBase tileBase = gameBoard.tilemap.GetTile(tilePosition);
        if (tileBase.name.Contains("Yellow"))
        {
            // Banana Tile
            // Iterate through the BananaList and check if each banana's position matches the given tilePosition
            // If a match is found, add the banana to the PickedBananas list
            for(int i = 0; i < BananaList.Count; i++){
                if(BananaList[i].GetComponent<Banana>().Position == tilePosition){
                    PickedBananas.Add(BananaList[i]);
                }
            }
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

    public void RemoveBanana(Vector3Int Position, bool rotten)
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
                if(rotten){
                    gameBoard.tilemap.SetTile(Position, null);
                }
                
            }
        }
    }
}
