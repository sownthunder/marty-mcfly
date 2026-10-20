using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RollsRoyce : MonoBehaviour
{
    public Transform martyMcFly;
    public float distanceBetween;
    public float dist = 4.0f;
    public bool startGUIQuestion = false;
    public void Awake()
    {
        startGUIQuestion = false;
    }
    public void Start()
    {
    }
    public void Update()
    {
        if (MeasureDistance () < dist)
        {
            startGUIQuestion = true;
        }
    }
    public float MeasureDistance()
    {
        distanceBetween = Vector3.Distance (martyMcFly.transform.position, transform.position);
        return distanceBetween;
    }
    public void OnGUI()
    {
        if (startGUIQuestion == true)
        {
            GUI.Box (new Rect(20, 20, 20, 20), "Wanna race?");
        }
    }
}
