using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingScript : MonoBehaviour
{

    public GameObject settingsPanel;
    public AudioMixer audioMixer;
    // Start is called before the first frame update
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
    
    private void Start(){
        settingsPanel.SetActive(false);
    }
    
}
