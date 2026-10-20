using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class VehicleController : MonoBehaviour
{
    public Rigidbody Rb;
    private float speed = 4.5f;
    public Vector3 pos;
    public bool inVehicle = false;
    public bool isRight = false;
    public bool isLeft = false;
    public bool isUp = false;
    public bool isDown = false;
    public Transform sRendererObj;
    public SpriteRenderer sRenderer;
    public Sprite spriteLeft;
    public Sprite spriteRight;
    public Sprite spriteUp;
    public Sprite spriteDown;
    public void Awake()
    {
        sRenderer = sRendererObj.GetComponent<SpriteRenderer>();
        Rb = transform.GetComponent<Rigidbody>();
    }
    public void Start()
    {
        pos = transform.position;
    }
    public void Update()
    {
        var step = speed * Time.deltaTime;
        if (Input.GetKey (KeyCode.RightArrow) && inVehicle)
        {
            pos = Vector3.right;
            transform.Translate (pos * step);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && inVehicle)
        {
            pos = Vector3.left;
            transform.Translate (pos * step);
        }
        else if (Input.GetKey (KeyCode.UpArrow) && inVehicle)
        {
            pos = Vector3.up;
            transform.Translate (pos * step);
        }
        else if (Input.GetKey (KeyCode.DownArrow) && inVehicle)
        {
            pos = Vector3.down;
            transform.Translate (pos * step);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
        }
    }
}
