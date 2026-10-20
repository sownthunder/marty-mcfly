using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ItemSpawner : MonoBehaviour
{
    public GameObject thePlute;
    public int totalPlutesNum = 10;
    public int rangeWidth = 119;
    public int rangeHeight = 119;
    public float rndIntWidth;
    public float rndIntHeight;
    public void Awake()
    {
    }
    public void Start()
    {
        for (var i = 0; i < totalPlutesNum; i++)
        {
            rndIntWidth = Random.Range (1, rangeWidth);
            rndIntHeight = Random.Range (1, rangeHeight);
            Instantiate (thePlute, new Vector3(rndIntWidth, rndIntHeight, 0), Quaternion.identity);
            Debug.Log ("Plute " + (i + 1) + " test position = " + new Vector3(rndIntWidth, rndIntHeight, 0));
        }
    }
    public void Update()
    {
    }
}
