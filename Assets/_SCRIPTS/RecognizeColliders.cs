using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RecognizeColliders : MonoBehaviour
{
    public float moveSpeed = 8.0f;
    public float rotationSpeed = 2.0f;
    public float minDist = 4.0f;
    public float maxDist = 45.0f;
    private float minSqrDist;
    private float maxSqrDist;
    private Transform myTransform;
    private Transform target;
    private Rigidbody myRigidbody;
    private Vector3 desiredVelocity;
    public void Awake()
    {
    }
    public void Start()
    {
        minSqrDist = minDist * minDist;
        maxSqrDist = maxDist * maxDist;
        myTransform = transform;
        myRigidbody = GetComponent<Rigidbody>();
    }
    public void Update()
    {
    }
}
