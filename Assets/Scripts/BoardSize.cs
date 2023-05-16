using UnityEngine;
using UnityEngine.UI;

public class BoardSize : MonoBehaviour
{
    public Toggle[] checkboxes;

    private void Start()
    {
        // Attach event listeners to checkboxes
        for (int i = 0; i < checkboxes.Length; i++)
        {
            int index = i; // Store the index to avoid closure issues
            checkboxes[i].onValueChanged.AddListener((value) => OnCheckboxValueChanged(index, value));
        }
        PlayerPrefs.SetInt("BoardSize", int.Parse(checkboxes[0].name));
    }

    private void OnCheckboxValueChanged(int index, bool value)
    {
        if (value)
        {
            // Uncheck other checkboxes
            for (int i = 0; i < checkboxes.Length; i++)
            {
                if (i != index)
                {
                    checkboxes[i].isOn = false;
                }
            }
            PlayerPrefs.SetInt("BoardSize", int.Parse(checkboxes[index].name));
        }
    }
}
