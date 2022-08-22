using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Threading;


public class GameController : MonoBehaviour
{

    public BottleController FirstBottle;
    public BottleController SecondBottle;
    public BottleController[] bottles;

    private bool allFull = false; // all bottles are full

    public int levelToUnlock;
    int numberOfUnlockedLevel;

    public GameObject LevelCompleted;

    private float bottleUp = 0.3f; // select bottle
    private float bottleDown = -0.3f; // deselect bottle


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x , mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if(hit.collider != null)
            {
                if(hit.collider.GetComponent<BottleController>() != null)
                {
                    if(FirstBottle == null)
                    {
                        FirstBottle = hit.collider.GetComponent<BottleController>();

                        if(FirstBottle.numberOfColorsInBottle != 0)
                        {
                        FirstBottle.transform.position = new Vector3(FirstBottle.transform.position.x,
                                                                     FirstBottle.transform.position.y + bottleUp,
                                                                     FirstBottle.transform.position.z);
                        }
                    }
                    else
                    {
                        if(FirstBottle == hit.collider.GetComponent<BottleController>())
                        {
                            if(FirstBottle.numberOfColorsInBottle != 0)
                            {
                            FirstBottle.transform.position = new Vector3(FirstBottle.transform.position.x,
                                             FirstBottle.transform.position.y + bottleDown,
                                             FirstBottle.transform.position.z);
                            }
                            FirstBottle = null;
                        }
                        else
                        {
                            SecondBottle = hit.collider.GetComponent<BottleController>();
                            FirstBottle.bottleControllerRef = SecondBottle;

                            FirstBottle.UpdateTopColorValue();
                            SecondBottle.UpdateTopColorValue();

                            if(SecondBottle.FillBottleCheck(FirstBottle.topColor) == true)
                            {
                                FirstBottle.startColorTransfer();
                               FirstBottle = null;
                                SecondBottle = null;
                            }
                            else {
                                if(FirstBottle.numberOfColorsInBottle != 0)
                                {
                                FirstBottle.transform.position = new Vector3(FirstBottle.transform.position.x,
                                                                             FirstBottle.transform.position.y + bottleDown,
                                                                             FirstBottle.transform.position.z);
                                }
                                FirstBottle = null;
                                SecondBottle = null;
                            }
                        }
                    }
                }
            }
            else // tab anywhere on the screen to deslecet bottles
            {      
                if(FirstBottle.numberOfColorsInBottle != 0)
                {
                FirstBottle.transform.position = new Vector3(FirstBottle.transform.position.x,
                                                             FirstBottle.transform.position.y + bottleDown,
                                                             FirstBottle.transform.position.z);
                }
               FirstBottle = null;
               SecondBottle = null;
            }
        }

        if(allFull == false) // keep checking on bottles
        {
            StartCoroutine(AllBottlesAreFull());
        }
    }

    IEnumerator AllBottlesAreFull() // check to completing the level
    {

      if( bottles.All( y => y.numberOfColorsInBottle == 0 ||  y.numberOfTopColorLayer == 4))
      {
        allFull =true;

        yield return new WaitForSeconds(1f);
       
        Win();
      }

    }

    private void Win()
    {
        
        if(allFull == true)
        {

            numberOfUnlockedLevel = PlayerPrefs.GetInt("LevelIsUnlocked");

            if(numberOfUnlockedLevel <= levelToUnlock )
            {
                PlayerPrefs.SetInt("LevelIsUnlocked", numberOfUnlockedLevel + 1);
            }

            if(LevelCompleted.activeSelf == false)
            {
                LevelCompleted.SetActive(true);
            }

        }   

       
    }


}
