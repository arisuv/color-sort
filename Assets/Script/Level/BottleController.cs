using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;


public class BottleController : MonoBehaviour
{
    [SerializeField] Color[] bottleColors;
    [SerializeField] SpriteRenderer bottleMaskSR;

    [SerializeField] AnimationCurve ScaleAndRotationMutiplaierCurve;
    [SerializeField] AnimationCurve FillAmountCurve;
    [SerializeField] AnimationCurve RotaationSpeedMultiplaier;

    [SerializeField] float[] fillAmounts;
    [SerializeField] float[] rotationValues;

    private int rotationIndex;


    [Range(0,4)] [SerializeField] public int numberOfColorsInBottle = 4;

    [SerializeField] public Color topColor; 
    [SerializeField] public int numberOfTopColorLayer = 0;

    [SerializeField] public BottleController bottleControllerRef;  //second bottle

    private int numberOfColorsToTranfer = 0;
    private int numberOfLayersToTranfer = 0;


    [SerializeField] Transform leftRotationPoint;
    [SerializeField] Transform rightRotationPoint;
    private Transform chosenRotationPoint;

    private float directionMultiplaier = 1.0f;

    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 originalPosition;

    public LineRenderer lineRenderer;

    [SerializeField] float timeToRotate = 1.0f;


    GameController myObj1 = new GameController();
 
    public AudioSource boilingSound;

    private GameObject[] levelbottles;
    private GameObject levelbottle;


    void Start()
    {
        originalPosition = transform.position;

        bottleMaskSR.material.SetFloat("_FillAmount", fillAmounts[numberOfColorsInBottle]);

        UpdateColorsOnShader();

        UpdateTopColorValue();
    }

    void Update()
    {

        numberOfColorsInBottle = numberOfColorsInBottle > 4? 4 : numberOfColorsInBottle;
        numberOfColorsInBottle = numberOfColorsInBottle < 0? 0 : numberOfColorsInBottle;

        if(Input.GetMouseButtonDown(0) && myObj1.FirstBottle != null )  
        {
            UpdateTopColorValue();

            if(bottleControllerRef.FillBottleCheck(topColor))
            {
                chosenRotationPointAndDirection();

                numberOfColorsToTranfer = Mathf.Min(numberOfTopColorLayer, 4 - bottleControllerRef.numberOfColorsInBottle);
                numberOfLayersToTranfer = Mathf.Min(numberOfTopColorLayer, 4 - bottleControllerRef.numberOfColorsInBottle);

                for(int i = 0 ; i < numberOfColorsToTranfer ; i++)
                {
                    bottleControllerRef.bottleColors[bottleControllerRef.numberOfColorsInBottle + i ] = topColor;
                }

                bottleControllerRef.UpdateColorsOnShader();
            }
            // else
            // {
            //     myObj1.FirstBottle = null;
            //     myObj1.SecondBottle = null;
            // }

            CalculateRotationIndex(4 - bottleControllerRef.numberOfColorsInBottle);

             StartCoroutine(MoveBottle()); 

        }

    }

    IEnumerator MoveBottle()
    {

        startPosition = transform.position;

        if(chosenRotationPoint == leftRotationPoint)
        {
            endPosition = bottleControllerRef.rightRotationPoint.position;

        }
        else
        {
            endPosition = bottleControllerRef.leftRotationPoint.position;
        }

        float t1 = 0;

        while(t1 <= 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t1);

            t1 += Time.deltaTime * 2;

            yield return new WaitForEndOfFrame();
        }

        transform.position = endPosition;

         StartCoroutine(RotateBottle());
    }

    IEnumerator MoveBottleBack()
    {


        startPosition = transform.position;
        endPosition = originalPosition;


        float t2 = 0;

        while(t2 <= 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t2);
            t2 += Time.deltaTime * 2;

            yield return new WaitForEndOfFrame();
        }

        transform.position = endPosition;

        transform.GetComponent<SpriteRenderer>().sortingOrder -= 2;
        bottleMaskSR.sortingOrder -= 2;
      
        UnlockAll();
        StartCoroutine( LockBottle() );  // if bottle is full lock it

    }

    public void startColorTransfer()
    {
        LockAll();

        chosenRotationPointAndDirection();

        numberOfColorsToTranfer = Mathf.Min(numberOfTopColorLayer, 4 - bottleControllerRef.numberOfColorsInBottle);
        numberOfLayersToTranfer = Mathf.Min(numberOfTopColorLayer, 4 - bottleControllerRef.numberOfColorsInBottle);

        for(int i = 0 ; i < numberOfColorsToTranfer ; i++)
        {
            bottleControllerRef.bottleColors[bottleControllerRef.numberOfColorsInBottle + i ] = topColor;

        }

        bottleControllerRef.UpdateColorsOnShader();

        CalculateRotationIndex(4 - bottleControllerRef.numberOfColorsInBottle);

        transform.GetComponent<SpriteRenderer>().sortingOrder += 2;
        bottleMaskSR.sortingOrder += 2;

          StartCoroutine(MoveBottle()); 
    }

    private void UpdateColorsOnShader()
    {
        bottleMaskSR.material.SetColor("_Color01", bottleColors[0]);
        bottleMaskSR.material.SetColor("_Color02", bottleColors[1]);
        bottleMaskSR.material.SetColor("_Color03", bottleColors[2]);
        bottleMaskSR.material.SetColor("_Color04", bottleColors[3]);
    }

    IEnumerator RotateBottle()
    {
        float t = 0f;
        float lerpValue;
        float angleVlaue;

        float lastAngleValue  = 0f;

        while(t < timeToRotate)
        {
            lerpValue = t / timeToRotate;
            angleVlaue = Mathf.Lerp(0.0f, directionMultiplaier * rotationValues[rotationIndex], lerpValue);
            
            transform.RotateAround(chosenRotationPoint.position, Vector3.forward, lastAngleValue - angleVlaue);

            bottleMaskSR.material.SetFloat("_ScaleAndRotationMultiplaier",
                                             ScaleAndRotationMutiplaierCurve.Evaluate(angleVlaue));
           
            if(fillAmounts[numberOfColorsInBottle] > FillAmountCurve.Evaluate(angleVlaue)  ) //+ 0.005
            {

                if(lineRenderer.enabled == false)
                {
                    PlayBoilingSound();

                    lineRenderer.startColor = topColor;
                    lineRenderer.endColor = topColor;

                    lineRenderer.SetPosition(0, chosenRotationPoint.position);
                    lineRenderer.SetPosition(1, chosenRotationPoint.position - Vector3.up * 1.45f);  

                    lineRenderer.enabled = true;
                }

                bottleMaskSR.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(angleVlaue)); // First bottle

                bottleControllerRef.FillUp(FillAmountCurve.Evaluate(lastAngleValue) - FillAmountCurve.Evaluate(angleVlaue));
            }

            t +=  Time.deltaTime * RotaationSpeedMultiplaier.Evaluate(angleVlaue);

            lastAngleValue = angleVlaue;

            yield return new WaitForEndOfFrame();
        }

        angleVlaue = directionMultiplaier * rotationValues[rotationIndex];

        bottleMaskSR.material.SetFloat("_ScaleAndRotationMultiplaier",
                                         ScaleAndRotationMutiplaierCurve.Evaluate(angleVlaue));
        bottleMaskSR.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(angleVlaue));

        numberOfColorsInBottle -= numberOfColorsToTranfer;

        bottleControllerRef.numberOfColorsInBottle += numberOfColorsToTranfer;
        bottleControllerRef.numberOfTopColorLayer += numberOfLayersToTranfer;
        
        lineRenderer.enabled = false;
        boilingSound.Stop();


        StartCoroutine(RotateBottlrBack());
    }

    IEnumerator RotateBottlrBack()
    {

        float t = 0f;
        float lerpValue;
        float angleVlaue;

        float lastAngleValue = directionMultiplaier * rotationValues[rotationIndex];


        while(t < timeToRotate)
        {
            lerpValue = t / timeToRotate;
            angleVlaue = Mathf.Lerp(directionMultiplaier * rotationValues[rotationIndex], 0f, lerpValue);

            transform.RotateAround(chosenRotationPoint.position, Vector3.forward, lastAngleValue - angleVlaue);

            bottleMaskSR.material.SetFloat("_ScaleAndRotationMultiplaier",
                                             ScaleAndRotationMutiplaierCurve.Evaluate(angleVlaue));
 
            lastAngleValue = angleVlaue;

            t+=  Time.deltaTime ;

            yield return new WaitForEndOfFrame();
        }

        UpdateTopColorValue();

        angleVlaue = 0;
        transform.eulerAngles = new Vector3(0, 0, angleVlaue);
        bottleMaskSR.material.SetFloat("_ScaleAndRotationMultiplaier",
                                         ScaleAndRotationMutiplaierCurve.Evaluate(angleVlaue));
   
        StartCoroutine(MoveBottleBack());
    }

    public int  UpdateTopColorValue()
    {
        if(numberOfColorsInBottle != 0)
        {
            numberOfTopColorLayer = 1;

            topColor = bottleColors[numberOfColorsInBottle - 1];

            if(numberOfColorsInBottle == 4)
            {

                 if(bottleColors[3].Equals(bottleColors[2]))
                 {
                    numberOfTopColorLayer = 2;

                    if(bottleColors[2].Equals(bottleColors[1]))
                    {

                        numberOfTopColorLayer = 3;

                        if(bottleColors[1].Equals(bottleColors[0]))
                        {
                            numberOfTopColorLayer = 4;
                        }
                    }
                 }
               // if(numberOfTopColorLayer == 4)
               //  {
               //      LockBottle();
               //  }
                    }
      
           else if(numberOfColorsInBottle == 3)
            {
                 if(bottleColors[2].Equals(bottleColors[1]))
                 {
                    numberOfTopColorLayer = 2;

                    if(bottleColors[1].Equals(bottleColors[0]))
                    {

                        numberOfTopColorLayer = 3;
     
                    }
                 }   
            }


           else if(numberOfColorsInBottle == 2)
            {
                 if(bottleColors[1].Equals(bottleColors[0]))
                 {
                    numberOfTopColorLayer = 2;
     
                 }   
            }

        rotationIndex = 3 - (numberOfColorsInBottle - numberOfTopColorLayer);
        }
  
    return numberOfTopColorLayer;
    }   

    public bool FillBottleCheck(Color colorToCheck)
    {
        if(numberOfColorsInBottle == 0)
        {
            return true;
        }
        else
        {
            if(numberOfColorsInBottle == 4)
            {
               return false;
            }
            else
            {
                if(topColor.Equals(colorToCheck))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    private void CalculateRotationIndex(int numberOfEmptyspacesInSecondBottle)
    {
        rotationIndex = 3 - (numberOfColorsInBottle - Mathf.Min(numberOfEmptyspacesInSecondBottle,
                             numberOfTopColorLayer));
    }

    private void FillUp(float fillAmounToAdd)
    {
        bottleMaskSR.material.SetFloat("_FillAmount", bottleMaskSR.material.GetFloat("_FillAmount") + fillAmounToAdd);
    }

    private void chosenRotationPointAndDirection()
    {
        if(transform.position.x > bottleControllerRef.transform.position.x)
        {
            chosenRotationPoint = leftRotationPoint;
            directionMultiplaier = -1.0f;
        }
        else
        {
           chosenRotationPoint = rightRotationPoint;
            directionMultiplaier = 1.0f;
        }
    }

    IEnumerator LockBottle() // lock bottle when it is full  
    {
        yield return new WaitForSeconds(0.5f);
       
        if(bottleControllerRef.numberOfTopColorLayer == 4 
        && bottleControllerRef.numberOfColorsInBottle == 4)
        {
            bottleControllerRef.GetComponent<Collider2D>().enabled = false;
            bottleControllerRef.tag ="Locked Bottle";
        }
    }

    private void PlayBoilingSound() // boilin sound
    {
        boilingSound.Play();
    }

    private void LockAll() // Cant move more than one bottle in the same time
    {
       levelbottles =  GameObject.FindGameObjectsWithTag("bottle");

         foreach (GameObject levelbottle in levelbottles) {
            levelbottle.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void UnlockAll()
    {
     levelbottles =  GameObject.FindGameObjectsWithTag("bottle");

         foreach (GameObject levelbottle in levelbottles) {
            levelbottle.GetComponent<Collider2D>().enabled = true;
        }

    }
}