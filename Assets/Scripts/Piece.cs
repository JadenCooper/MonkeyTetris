using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board;
    public TetrominoData data;
    public Vector3Int position;
    public Vector3Int[] cells;
    public PlayerControls pieceControls;
    public int rotationIndex = 0;

    public bool locked = false;

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;

    private bool hard = false;
    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        rotationIndex = 0;
        stepTime = Time.time + stepDelay;
        lockTime = 0f;

        cells = new Vector3Int[data.cells.Length];

        for (int i = 0; i < data.cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }
    private void Update()
    {
        board.Clear(this);
        lockTime += Time.deltaTime;
        if (Input.GetKeyDown(pieceControls.RotateLeft))
        {
            // Rotate Left
            Rotate(-1);
        }
        else if(Input.GetKeyDown(pieceControls.RotateRight))
        {
            // Rotate Right
            Rotate(1);
        }

        if (Input.GetKeyDown(pieceControls.MoveLeft))
        {
            Move(Vector2Int.left);
        }
        else if(Input.GetKeyDown(pieceControls.MoveRight))
        {
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(pieceControls.SoftDrop))
        {
            // Soft Drop: Move Down One Row
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(pieceControls.HardDrop))
        {
            hard = true;
        }

        if (hard) //Run HardDrop every update instead of every loop tick to slow it down a little.
        {
            HardDrop();
        }

        if (Time.time >= stepTime)
        {
            Step();
        }

        board.Set(this);
    }
    private void Step()
    {
        stepTime = Time.time + stepDelay;

        Move(Vector2Int.down);

        if (lockTime >= lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {

        Debug.Log("Lock");
        board.Set(this);
        board.IsValidPosition(cells, position, true, true);
        //board.ClearLines();
        board.SpawnPiece();
        
    }

    private void HardDrop() //Run an instance of hard dropping, ending it if we are locked down.
    {
        if (!Move(Vector2Int.down))
        {
            hard = false;
            Lock();
        }
    }

    public void ForceLock() //Called by the ghost when it ends the timer for the tracked piece
    {
        Move(Vector2Int.down);
        Lock();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        if (board.IsValidPosition(cells, newPosition, true, false))
        {
            position = newPosition;
            lockTime = 0f;
            return true;
        }

        return false;
    }

    private void Rotate(int direction)
    {
        int orgianlRotation = rotationIndex;
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);

        if (!TestWallKicks(rotationIndex, direction))
        {
            rotationIndex = orgianlRotation;
            ApplyRotationMatrix(-direction);
        }
    }
    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];

            int x, y;

            switch (data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }
            cells[i] = new Vector3Int(x, y, 0);
        }
    }
    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);
        for (int i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;
        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    }
    public int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
