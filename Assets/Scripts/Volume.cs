using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Volume : MonoBehaviour
{
    public Slider SoundSettingSlider;
    public AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
    }

    public void SetVolume(float volume)
    {
        RefreshSlider(volume);
        PlayerPrefs.SetFloat("SavedMasterVolume", volume);
        audioMixer.SetFloat("volume", volume);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(SoundSettingSlider.value);
    }

    public void RefreshSlider(float value)
    {
        SoundSettingSlider.value = value;
    }
}
