using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartMenu : MonoBehaviour
{

   [SerializeField] GameObject settingsWindow;


    public void PlayButton()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowSettings()
    {
        settingsWindow.SetActive(true); 
    }

    public void ExitPanel()
    {
        settingsWindow.SetActive(false); 
    }

}
