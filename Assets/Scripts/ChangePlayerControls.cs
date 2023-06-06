using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerControls : MonoBehaviour
{
    //// This Script Handles All Player Control Changes \\\\

    // List of buttons for control customization
    public List<Button> controlButtons = new List<Button>();
    // List of text elements to display control bindings
    public List<TMP_Text> controlTexts = new List<TMP_Text>();

    // List of player control configurations
    public List<PlayerControls> playerControls = new List<PlayerControls>();
    // List of default player control configurations
    public List<PlayerControls> playerControlsDefaults = new List<PlayerControls>();

    // Reference to the main canvas text boxes for player controls
    public TMP_Text playerOneControlsText;
    public TMP_Text playerTwoControlsText;

    private bool listening = false; // Flag to indicate if listening for key input
    private int ControlIndex; // Index of the selected control
    private void Update()
    {
        if (listening && Input.anyKeyDown)
        {
            KeyCode keyCode = GetPressedKeyCode();
            SetControl(keyCode);
        }
    }

    private KeyCode GetPressedKeyCode()
    {
        // Get the KeyCode of the pressed key
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
        // Triggered when a control customization button is pressed
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
        // Enable/disable control customization buttons and toggle listening flag
        for (int i = 0; i < controlButtons.Count; i++)
        {
            controlButtons[i].interactable = !controlButtons[i].interactable;
        }
        listening = !listening;
    }

    public void SetControl(KeyCode keycode)
    {
        // Set the control binding based on the selected control index and KeyCode
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
        // Reset all control bindings to their defaults
        Debug.Log("Reset");
        for (int i = 0; i < playerControls.Count; i++)
        {
            playerControls[i].CopyFrom(playerControlsDefaults[i]);
        }
        SetControlTexts();
    }

    public void SetControlTexts()
    {
        // Update the control text elements with the current control bindings
        int IndexIncrement = 0;
        for (int i = 0; i < playerControls.Count; i++)
        {
            controlTexts[0 + IndexIncrement].text = playerControls[i].Rotate.ToString();
            controlTexts[1 + IndexIncrement].text = playerControls[i].MoveLeft.ToString();
            controlTexts[2 + IndexIncrement].text = playerControls[i].MoveRight.ToString();
            controlTexts[3 + IndexIncrement].text = playerControls[i].SoftDrop.ToString();
            controlTexts[4 + IndexIncrement].text = playerControls[i].HardDrop.ToString();
            IndexIncrement = 5;
        }
    }
}
