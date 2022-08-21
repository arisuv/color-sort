using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScene : MonoBehaviour
{
    public int sceneIndex;

    public void OpenScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(1);
    }


    
}
