using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSettings : MonoBehaviour
{
   public GameObject settingsPanel;
    // Start is called before the first frame update

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

     public void GoToMenu(){
        settingsPanel.SetActive(false);
    }
    private void Start()
    {
        GoToMenu();
    }
}
