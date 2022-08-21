using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class SoundEffects : MonoBehaviour
{
    public AudioSource soundEffect;

   private void Start()
    {
        if(!PlayerPrefs.HasKey("soundVolume"))
        {
            PlayerPrefs.HasKey("soundVolume");
        }
        
        soundEffect = GetComponent<AudioSource>();
        soundEffect.volume = PlayerPrefs.GetFloat("soundVolume");
    }
}
