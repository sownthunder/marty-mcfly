using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    public Rigidbody Rb;
    public Transform target;
    public float speed = 4.0f;
    public Vector3 pos;
    public bool canMove = true;
    public bool trainSwitched = false;
    public GameObject gridTile;
    public GameObject theBullet;
    public GameObject theDeLorean;
    public GameObject theCTruck;
    public GameObject theMTruck;
    public float distanceBetweenD;
    public float distanceBetweenC;
    public float distanceBetweenM;
    public float distanceBetweenV;
    public bool isInVehicle = false;
    public bool isRight = false;
    public bool isLeft = false;
    public bool isUp = false;
    public bool isDown = false;
    public bool enteredBorder = false;
    public Transform CTruckControllerObj;
    public CTruckController CTruckControllerScript;
    public Transform MTruckControllerObj;
    public MTruckController MTruckControllerScript;
    public Transform DControllerObj;
    public DeLoreanController DControllerScript;
    public void Awake()
    {
        DControllerScript = DControllerObj.GetComponent<DeLoreanController>();
        CTruckControllerScript = CTruckControllerObj.GetComponent<CTruckController>();
        MTruckControllerScript = MTruckControllerObj.GetComponent<MTruckController>();
        Rb = transform.GetComponent<Rigidbody>();
    }
    public void Start()
    {
        pos = transform.position;
    }
    public void Update()
    {
        var step = speed * Time.deltaTime;
        if (isRight && canMove)
        {
            pos = Vector3.right;
            transform.Translate (pos * step);
        }
        else if (isLeft && canMove)
        {
            pos = Vector3.left;
            transform.Translate (pos * step);
        }
        else if (isUp && canMove)
        {
            Debug.Log("moving up?");
            pos = Vector3.up;
            transform.Translate (pos * step);
        }
        else if (isDown && canMove)
        {
            pos = Vector3.down;
            transform.Translate (pos * step);
        }
        else if (Input.GetKeyDown (KeyCode.E) && canMove)
        {
            MeasureDistance ();
            if (distanceBetweenD <= 2.00f)
            {
                Debug.Log ("INSIDE DELOREAN");
                DControllerScript.inVehicle = true;
            }
            else if (distanceBetweenC <= 2.00f)
            {
                Debug.Log ("INSIDE C-TRUCK");
                CTruckControllerScript.inVehicle = true;
            }
            else if (distanceBetweenM <= 2.00f)
            {
                Debug.Log ("INSIDE M-TRUCK");
                MTruckControllerScript.inVehicle = true;
            }
        }
        else if (Input.GetKeyDown (KeyCode.Space) && canMove)
        {
            if (isRight)
            {
                Debug.Log ("Fire a bullet right");
            }
            else if (isLeft)
            {
                Debug.Log ("Fire a bullet left");
            }
            else if (isUp)
            {
                Debug.Log ("Fire a bullet up");
            }
            else if (isDown)
            {
                Debug.Log ("Fire a bullet down");
            }
        }
        else if (Input.GetKeyDown (KeyCode.P) && canMove)
        {
            trainSwitched = true;
            Debug.Log ("trainSwitched= " + trainSwitched);
        }
        if (DControllerScript.inVehicle)
        {
            speed = 7.0f;
            transform.position = theDeLorean.transform.position;
            canMove = false;
        }
        else if (CTruckControllerScript.inVehicle)
        {
            speed = 4.5f;
            transform.position = theCTruck.transform.position;
            canMove = false;
        }
        else if (MTruckControllerScript.inVehicle)
        {
            speed = 4.5f;
            transform.position = theMTruck.transform.position;
            canMove = false;
        }
        if (DControllerScript.eighty8MphScript.backInTime)
        {
            speed = 4.0f;
        }
    }
    public void OnGUI()
    {
        if (canMove)
        {
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GridTile")
        {
        }
        else if (other.tag == "Road")
        {
        }
        else if (other.tag == "Border")
        {
            enteredBorder = true;
            CheckIfBoundary ();
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Border")
        {
            CheckIfBoundary ();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        enteredBorder = false;
    }
    public void CheckIfBoundary()
    {
        if (isRight)
        {
            transform.Translate (new Vector3(-0.1f, 0f, 0f));
        }
        else if (isLeft)
        {
            transform.Translate (new Vector3(0.1f, 0f, 0f));
        }
        else if (isUp)
        {
            transform.Translate (new Vector3(0f, -0.1f, 0f));
        }
        else if (isDown)
        {
            transform.Translate (new Vector3(0f, 0.1f, 0f));
        }
    }
    public void MeasureDistance()
    {
        distanceBetweenD = Vector3.Distance (theDeLorean.transform.position, transform.position);
        distanceBetweenC = Vector3.Distance (theCTruck.transform.position, transform.position);
        distanceBetweenM = Vector3.Distance (theMTruck.transform.position, transform.position);
    }
}
