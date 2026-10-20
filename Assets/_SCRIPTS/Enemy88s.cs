using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy88s : MonoBehaviour
{
    public int index = 1;
    public float originalEnemyTimer = 0;
    public float theEnemyTimer = 0;
    public bool isFluxing = false;
    public bool timeCircuitsOn = false;
    public bool backInTime = false;
    public Vector3 originalEnemyPosition;
    public Vector3 currentEnemyPosition;
    public GameObject theEnemyCop;
    public GameObject theEnemyClone;
    public bool isEnemyCloneSet = false;
    public List<Vector3> copArr = new List<Vector3>();
    public float duration = 1.0f;
    public CopController enemyControllerScript;
    public void Awake()
    {
        enemyControllerScript = theEnemyCop.gameObject.transform.GetComponent<CopController>();
        copArr = new List<Vector3>();
    }
    public void Start()
    {
    }
    public void Update()
    {
        if (isFluxing)
        {
            theEnemyTimer = theEnemyTimer + Time.fixedDeltaTime;
        }
        if (timeCircuitsOn)
        {
            timeCircuitsOn = false;
            originalEnemyTimer = theEnemyTimer;
            theEnemyTimer = 0;
            backInTime = true;
        }
        if (backInTime)
        {
            if (isEnemyCloneSet == false)
            {
                isEnemyCloneSet = true;
                theEnemyClone.transform.position = originalEnemyPosition;
                theEnemyClone.SetActive (true);
            }
            if (index < copArr.Count)
            {
                if (theEnemyClone != null)
                {
                    theEnemyClone.transform.position = Vector3.Lerp(copArr[index - 1], copArr[index], Time.time);
                }
                if (theEnemyClone.transform.position == copArr[index])
                {
                    index++;
                }
            }
            else
            {
                backInTime = false;
                copArr.Clear ();
                copArr = new List<Vector3>();
                index = 1;
                originalEnemyTimer = 0;
                theEnemyTimer = 0;
                isEnemyCloneSet = false;
                if (theEnemyClone != null)
                {
                    enemyControllerScript.moving = false;
                    enemyControllerScript.stillPursuiting = false;
                    theEnemyClone.SetActive (false);
                    theEnemyCop.SetActive (true);
                }
                else
                {
                    Debug.Log ("OFFICER DEAD!");
                }
            }
        }
        else
        {
        }
    }
}
