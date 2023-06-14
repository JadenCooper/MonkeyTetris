using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource SoundEffectPlayer;
    public List<AudioClip> SoundEffectClips = new List<AudioClip>();

    public void PlaySound(int clipIndex)
    {
        SoundEffectPlayer.clip = SoundEffectClips[clipIndex];
        SoundEffectPlayer.Play();
    }
}
