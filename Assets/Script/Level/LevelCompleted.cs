using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleted : MonoBehaviour
{

    public Button nextLevel;
    public Button playAgain;
    public Button home;

    private int sceneIndex;

    public AudioSource winngSound;

    private void Start()
    {
        PlaySound();

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }


    public void NextLevel()
    {
        if(sceneIndex == 16)
        {
            SceneManager.LoadScene(0);
        }
        else{
            SceneManager.LoadScene(sceneIndex + 1);
        }   
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }

    public void PlaySound()
    {
        winngSound.Play();
    }

}