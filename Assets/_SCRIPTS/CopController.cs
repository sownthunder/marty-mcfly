using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CopController : MonoBehaviour
{
    public Rigidbody Rb;
    public float distanceBetween;
    public float step;
    private float speed = 5.0f;
    public Transform target;
    public Transform myTransform;
    public int precedenceNum;
    public float dist = 10.0f;
    public bool moving = false;
    public bool stillPursuiting = false;
    public bool isWaiting = false;
    public Vector3 pos;
    public bool isRight = false;
    public bool isLeft = false;
    public bool isUp = false;
    public bool isDown = false;
    public Sprite spriteLeft;
    public Sprite spriteRight;
    public Sprite spriteUp;
    public Sprite spriteDown;
    public FluxCapacitor theFluxCapacitor;
    public void Awake()
    {
        myTransform = transform;
        Rb = transform.GetComponent<Rigidbody>();
        theFluxCapacitor = GameObject.FindWithTag("Flux").GetComponent<FluxCapacitor>();
    }
    public void Start()
    {
        if (myTransform.gameObject.CompareTag ("Cop1"))
        {
            precedenceNum = 1;
        }
        else if (myTransform.gameObject.CompareTag ("Cop2"))
        {
            precedenceNum = 2;
        }
        else if (myTransform.gameObject.CompareTag ("Cop3"))
        {
            precedenceNum = 3;
        }
        else if (myTransform.gameObject.CompareTag ("Cop4"))
        {
            precedenceNum = 4;
        }
        else if (myTransform.gameObject.CompareTag ("Cop5"))
        {
            precedenceNum = 5;
        }
        else if (myTransform.gameObject.CompareTag ("Cop6"))
        {
            precedenceNum = 6;
        }
        else if (myTransform.gameObject.CompareTag ("Cop7"))
        {
            precedenceNum = 7;
        }
        else if (myTransform.gameObject.CompareTag ("Cop8"))
        {
            precedenceNum = 8;
        }
        pos = transform.position;
    }
    public void Update()
    {
        step = speed * Time.deltaTime;
        if (isWaiting)
        {
            stillPursuiting = false;
        }
        if (!moving && MeasureDistance () < dist)
        {
            stillPursuiting = true;
            StartCoroutine(InPursuit());
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cop1")
        {
            if (precedenceNum > 1)
            {
                isWaiting = true;
            }
        }
        else if (other.tag == "Cop2")
        {
            if (precedenceNum > 2)
            {
                isWaiting = true;
            }
        }
        else if (other.tag == "Cop3")
        {
            if (precedenceNum > 3)
            {
                isWaiting = true;
            }
        }
        else if (other.tag == "Cop4")
        {
            if (precedenceNum > 4)
            {
                isWaiting = true;
            }
        }
        else if (other.tag == "Cop5")
        {
            if (precedenceNum > 5)
            {
                isWaiting = true;
            }
        }
        else if (other.tag == "Cop6")
        {
            if (precedenceNum > 6)
            {
                isWaiting = true;
            }
        }
        else if (other.tag == "Cop7")
        {
            if (precedenceNum > 7)
            {
                isWaiting = true;
            }
        }
        else if (other.tag == "Cop8")
        {
            if (precedenceNum > 8)
            {
                isWaiting = true;
            }
        }
        else if (other.tag == "Border")
        {
            CheckIfBoundary ();
        }
        else if (other.tag == "Grass")
        {
            speed = 3.0f;
        }
        else if (other.tag == "Building")
        {
            moving = false;
        }
        else if (other.tag == "Cement")
        {
            Debug.Log ("CEEEMEMMMENET!");
            speed = 1.0f;
        }
        else if (other.tag == "Manure")
        {
            Debug.Log ("Manure! SLOoooooOOOw down!");
            speed = 3.0f;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cop1" || other.tag == "Cop2" ||
        other.tag == "Cop3" || other.tag == "Cop4" ||
        other.tag == "Cop5" || other.tag == "Cop6" ||
        other.tag == "Cop7" || other.tag == "Cop8")
        {
            isWaiting = false;
        }
        else if (other.tag == "Grass")
        {
            speed = 5.0f;
        }
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
    public float MeasureDistance()
    {
        distanceBetween = Vector3.Distance (target.transform.position, transform.position);
        return distanceBetween;
    }
    public IEnumerator InPursuit()
    {
        moving = true;
        while ((transform.position != target.position) && stillPursuiting)
        {
            transform.position = Vector3.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
            if (transform.position == target.position)
            {
                theFluxCapacitor.DeLoreanHealth = theFluxCapacitor.DeLoreanHealth - 10;
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.3f);
        moving = false;
    }
}
