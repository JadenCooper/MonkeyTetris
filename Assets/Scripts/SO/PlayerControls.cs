using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewControlData", menuName = "Data/ControlData")]
public class PlayerControls : ScriptableObject
{
    public KeyCode MoveLeft = KeyCode.A;
    public KeyCode MoveRight = KeyCode.D;
    public KeyCode RotateLeft = KeyCode.Q;
    public KeyCode RotateRight = KeyCode.E;
    public KeyCode SoftDrop = KeyCode.S;
    public KeyCode HardDrop = KeyCode.Space;
}
