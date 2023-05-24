using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewControlData", menuName = "Data/ControlData")]
public class PlayerControls : ScriptableObject
{
    // This Scriptable Object Holds The Keycodes For The Player Controls
    public KeyCode MoveLeft = KeyCode.A;
    public KeyCode MoveRight = KeyCode.D;
    public KeyCode Rotate = KeyCode.W;
    public KeyCode SoftDrop = KeyCode.S;
    public KeyCode HardDrop = KeyCode.Space;

    public void CopyFrom(PlayerControls other)
    {
        MoveLeft = other.MoveLeft;
        MoveRight = other.MoveRight;
        Rotate = other.Rotate;
        SoftDrop = other.SoftDrop;
        HardDrop = other.HardDrop;
    }
}
