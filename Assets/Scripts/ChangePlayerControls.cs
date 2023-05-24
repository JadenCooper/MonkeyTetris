using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerControls : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            KeyCode keyCode = GetPressedKeyCode();
            Debug.Log("Key Pressed: " + keyCode.ToString());
        }
    }

    private KeyCode GetPressedKeyCode()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                return keyCode;
            }
        }

        return KeyCode.None;
    }
}
