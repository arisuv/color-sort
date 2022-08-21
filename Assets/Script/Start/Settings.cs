using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundEffectsSlider;

    [SerializeField] GameObject settingsWindow;

    [SerializeField] AudioSource music;
    [SerializeField] AudioSource sound;

    private void Awake()
    {
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.HasKey("musicVolume");
            LoadMusic();

            musicSlider.value = 0;
            music.volume = 0;
        }
        else
        {
            LoadMusic();
        }

        if(!PlayerPrefs.HasKey("soundVolume"))
        {
            PlayerPrefs.HasKey("soundVolume");
            LoadSound();

            soundEffectsSlider.value = 0;
            sound.volume = 0;
        }
        else
        {
            LoadSound();
        }
    }

    public void UpdateMusicVolume()
    {
        music.volume = musicSlider.value;
    }

    public void UpdateSoundVolume()
    {
      sound.volume = soundEffectsSlider.value;
    }

    private void LoadMusic()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void LoadSound()
    {
        soundEffectsSlider.value = PlayerPrefs.GetFloat("soundVolume");
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("soundVolume", soundEffectsSlider.value);

        settingsWindow.SetActive(false);
    }
}
