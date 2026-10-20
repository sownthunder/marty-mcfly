using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    public Rigidbody rb;
    public Transform target;
    public float speed = 4.0f;
    public Vector3 pos;

    public bool isInVehicle = false;

    public Transform DControllerObj;
    public DeLoreanController DControllerScript;

    void Awake()
    {

        DControllerScript = DControllerObj.GetComponent<DeLoreanController>();

    }

    void Start()
    {

    }

    public void MeasureDistance()
    {
        
    }
    
}
