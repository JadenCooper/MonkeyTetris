using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerControls : MonoBehaviour
{
    public List<Button> controlButtons = new List<Button>();
    public List<TMP_Text> controlTexts = new List<TMP_Text>();
    public List<PlayerControls> playerControls = new List<PlayerControls>();
    public List<PlayerControls> playerControlsDefaults = new List<PlayerControls>();
    private bool listening = false;
    private int ControlIndex;
    private void Update()
    {
        if (Input.anyKeyDown && listening)
        {
            KeyCode keyCode = GetPressedKeyCode();
            SetControl(keyCode);
            //Debug.Log("Key Pressed: " + keyCode.ToString());
        }
    }

    private KeyCode GetPressedKeyCode()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                AlterButtons();
                controlTexts[ControlIndex].text = keyCode.ToString();
                return keyCode;
            }
        }

        return KeyCode.None;
    }

    public void AlterListening(Button PressedButton)
    {
        for (int i = 0; i < controlButtons.Count; i++)
        {
            if (PressedButton == controlButtons[i])
            {
                ControlIndex = i;
                Debug.Log(ControlIndex);
            }
        }
        AlterButtons();
    }

    public void AlterButtons()
    {
        for (int i = 0; i < controlButtons.Count; i++)
        {
            controlButtons[i].interactable = !controlButtons[i].interactable;
        }
        listening = !listening;
    }

    public void SetControl(KeyCode keycode)
    {
        int PlayerIndex = 0;
        if (ControlIndex >= 5)
        {
            ControlIndex -= 5;
            PlayerIndex++;

        }
        switch (ControlIndex)
        {
            case 0:
                playerControls[PlayerIndex].Rotate = keycode;
                break;

            case 1:
                playerControls[PlayerIndex].MoveLeft = keycode;
                break;

            case 2:
                playerControls[PlayerIndex].MoveRight = keycode;
                break;

            case 3:
                playerControls[PlayerIndex].SoftDrop = keycode;
                break;

            case 4:
                playerControls[PlayerIndex].HardDrop = keycode;
                break;

            default:
                Debug.Log("Set Control Broke");
                break;
        }
    }

    public void ResetControls()
    {
        Debug.Log("Reset");
        for (int i = 0; i < playerControls.Count; i++)
        {
            playerControls[i].CopyFrom(playerControlsDefaults[i]);
        }
    }
}
