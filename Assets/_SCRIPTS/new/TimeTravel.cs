using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeTravel : MonoBehaviour
{

    public float distanceBetween; 
    public float originalDeLoreanTimer = 0;
    public float theDeLoreanTimer = 0;
    public bool isFluxing = false;
    public bool timeCircuitsOn = false;
    public bool backInTime = false;

    public Transform martyTransform;

    public Vector3 originalDeLoreanPosition;
    public Vector3 currentDeLoreanPosition;
    public Vector3 originalDeLoreanRotation;
    public Vector3 currentDeloreanRotation;

    public GameObject theTestDeLorean;
    public GameObject theDeLoreanClone;
    public bool isDeLoreanCloneSet = false;


    public List<Vector3> positionArray = new List<Vector3>();
    public List<Vector3> rotationArray = new List<Vector3>();

    public void Awake()
    {
        positionArray = new List<Vector3>();
        rotationArray = new List<Vector3>();
    }

    public void Start()
    {

    }

    public void Update()
    {

        if (isFluxing)
        {
            theDeLoreanTimer = theDeLoreanTimer + Time.fixedDeltaTime;
        }
    }

    public float MeasureDistance(Transform aGameObject)
    {
        distanceBetween = Vector3.Distance(theTestDeLorean.transform.position, aGameObject.transform.position);
        return distanceBetween;
    }

}
