using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CopSpawner : MonoBehaviour
{
    public GameObject enemyCop1;
    public GameObject enemyCop2;
    public GameObject enemyCop3;
    public GameObject enemyCop4;
    public GameObject enemyCop5;
    public GameObject enemyCop6;
    public GameObject enemyCop7;
    public GameObject enemyCop8;
    public int rangeWidth = 119;
    public int rangeHeight = 119;
    public float rndIntWidth;
    public float rndIntHeight;
    public float waveTimer = 0;
    public int totalWaveNum = 0;
    public void Awake()
    {
    }
    public void Start()
    {
        for (var i = 0; i < 8; i++)
        {
            rndIntWidth = Random.Range (1, rangeWidth);
            rndIntHeight = Random.Range (1, rangeHeight);
            Instantiate (enemyCop1, new Vector3(rndIntWidth, rndIntHeight, 0), Quaternion.identity);
        }
    }
    public void Update()
    {
        waveTimer = waveTimer + Time.fixedDeltaTime;
        if (waveTimer >= 30)
        {
            Debug.Log ("NEW WAAAVE!");
            waveTimer = 0;
        }
    }
}
