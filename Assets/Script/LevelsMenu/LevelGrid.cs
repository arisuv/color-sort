using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] Button[] button;

     [Range(0, 15)]int unlockedLevel; 

    private void Start()
    {
        if(!PlayerPrefs.HasKey("LevelIsUnlocked"))
        {
            PlayerPrefs.SetInt("LevelIsUnlocked", 1);
        }

        unlockedLevel = PlayerPrefs.GetInt("LevelIsUnlocked");

        for(int i=0;i<button.Length;i++)
        {
            button[i].interactable = false;
        }
    }

    private void Update()
    {
        unlockedLevel = PlayerPrefs.GetInt("LevelIsUnlocked");

        for(int i=0;i<unlockedLevel;i++)
        {
            button[i].interactable = true;
        }
    }
}
