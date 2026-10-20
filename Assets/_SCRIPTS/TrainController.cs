using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TrainController : MonoBehaviour
{
    public int index = 1;
    public Vector3 originalTrainPosition;
    public GameObject theTrain;
    public float step;
    public float trainTimer = 10;
    public float speed = 3.5f;
    public Vector3 pos;
    public bool isMoving = false;
    public bool switchHit = false;
    public bool isRight = false;
    public bool isLeft = false;
    public bool isUp = false;
    public bool isDown = false;
    public List<Vector3> trainArr = new List<Vector3>();
    public void Awake()
    {
        trainArr = new List<Vector3>();
    }
    public void Start()
    {
        originalTrainPosition = theTrain.transform.position;
    }
    public void Update()
    {
        trainTimer = trainTimer - Time.fixedDeltaTime;
        step = speed * Time.deltaTime;
        if (trainTimer <= 0)
        {
            switchHit = true;
            theTrain.SetActive (true);
        }
        if (!isMoving && switchHit)
        {
            TravelTrack ();
        }
    }
    public void TravelTrack()
    {
        if (index < trainArr.Count)
        {
            theTrain.transform.position = Vector3.MoveTowards (theTrain.transform.position, trainArr[index], step);
            if (theTrain.transform.position == trainArr[index])
            {
                index++;
            }
        }
        else
        {
            switchHit = false;
            trainTimer = 10.0f;
            Debug.Log ("switchHit == falsO!");
            theTrain.SetActive (false);
            theTrain.transform.position = originalTrainPosition;
            index = 1;
        }
    }
}
