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
    private bool listening = false;
    private int ControlIndex;
    private void Update()
    {
        if (Input.anyKeyDown && listening)
        {
            KeyCode keyCode = GetPressedKeyCode();
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
}
