using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseGame : MonoBehaviour
{
    
    public GameObject pauseMenu;
    public Toggle pause;

    public GameObject winWindow;

    [SerializeField] Button resume;
    [SerializeField] Button playAgain;
    [SerializeField] Button home;

    private int sceneIndex;

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void Pause()
    {
        if(pause.isOn == true)
        {
            if(!winWindow.activeSelf)
            {
                pauseMenu.SetActive(true); 
            }
        }

    }

    public void Resume()
    {
        if(pause.isOn == false)
        {
            pauseMenu.SetActive(false); 
        }
    }

    public void ResumeButton()
    {
        if(pause.isOn == true)
        {
            pauseMenu.SetActive(false); 
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }

}
