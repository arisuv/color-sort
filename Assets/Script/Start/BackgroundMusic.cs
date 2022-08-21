using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
 
    [SerializeField] AudioSource musicIsPlaying;

    private static BackgroundMusic instance;


     private void Awake()
     {
         if(!PlayerPrefs.HasKey("musicVolume"))
         {
               PlayerPrefs.HasKey("musicVolume");
         }

         musicIsPlaying.volume = PlayerPrefs.GetFloat("musicVolume");
         musicIsPlaying = GetComponent<AudioSource>();

        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if(instance != this)
        {
            DestroyImmediate(instance.gameObject);            
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
     }
            

 }
