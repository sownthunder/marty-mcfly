using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CopCloneController : MonoBehaviour
{
    public Rigidbody Rb;
    public GameObject theObject;
    public Transform myTransform;
    public void Awake()
    {
        myTransform = transform;
        Rb = transform.GetComponent<Rigidbody>();
    }
    public void Start()
    {
    }
    public void Update()
    {
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Train")
        {
            Debug.Log ("MONEY SHOT!----TrAaAaAiiinnn!!!");
            Destroy (theObject);
        }
        else if (other.tag == "Cement")
        {
            Debug.Log ("CEEEMEMMMENET!");
        }
        else if (other.tag == "Manure")
        {
            Debug.Log ("Manure!");
        }
        else if (other.tag == "Player")
        {
            Debug.Log ("BROKE SPACE TIME CONTINUEM YOU DUMB FUCK!");
        }
    }
}
