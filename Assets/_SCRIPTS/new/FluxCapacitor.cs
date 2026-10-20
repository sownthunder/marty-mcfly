using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluxCapacitor : MonoBehaviour
{
    public int martyHealth = 100;
    public int DeLoreanHealth = 150;
    public int playerScore = 0;

    public bool isDisplayingMap = false;
    public bool isMartyMoving = false;
    public bool isDeLoreanMoving = false;

    public float gameTimer = 0;
    public Transform PControllerObj;
    public PlayerController PControllerObjthing;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
