using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Eighty8Mph : MonoBehaviour
{
    public GUIStyle timerGuiStyle;
    public int index = 1;
    public float distanceBetween;
    public float originalDeLoreanTimer = 0;
    public float theDeLoreanTimer = 0;
    public bool isFluxing = false;
    public bool timeCircuitsOn = false;
    public bool backInTime = false;
    public Transform martyTransform;
    public Vector3 originalDeLoreanPosition;
    public Vector3 currentDeLoreanPosition;
    public GameObject theTestDeLorean;
    public GameObject theDeLoreanClone;
    public bool isDeLoreanCloneSet = false;
    public Transform sRendererObj;
    public SpriteRenderer sRenderer;
    public List<Vector3> arr = new List<Vector3>();
    public List<Sprite> spriteArr = new List<Sprite>();
    public float duration = 1.0f;
    public void Awake()
    {
        arr = new List<Vector3>();
        spriteArr = new List<Sprite>();
        sRenderer = sRendererObj.GetComponent<SpriteRenderer>();
    }
    public void Start()
    {
    }
    public void Update()
    {
        if (isFluxing)
        {
            theDeLoreanTimer = theDeLoreanTimer + Time.fixedDeltaTime;
        }
        if (timeCircuitsOn)
        {
            timeCircuitsOn = false;
            originalDeLoreanTimer = theDeLoreanTimer;
            theDeLoreanTimer = 0;
            backInTime = true;
        }
        if (backInTime)
        {
            if (isDeLoreanCloneSet == false)
            {
                isDeLoreanCloneSet = true;
                theDeLoreanClone.transform.position = originalDeLoreanPosition;
                theTestDeLorean.SetActive (false);
                theDeLoreanClone.SetActive (true);
            }
            if (index < arr.Count)
            {
                theDeLoreanClone.transform.position = Vector3.Lerp (arr[index - 1], arr[index], Time.time);
                sRenderer.sprite = spriteArr[index];
                if (theDeLoreanClone.transform.position == arr[index])
                {
                    index++;
                }
            }
            else
            {
                backInTime = false;
                arr.Clear ();
                arr = new List<Vector3>();
                spriteArr.Clear ();
                spriteArr = new List<Sprite>();
                index = 1;
                originalDeLoreanTimer = 0;
                theDeLoreanTimer = 0;
                isDeLoreanCloneSet = false;
                if ((MeasureDistance (martyTransform)) <= 2.0f)
                {
                    Debug.Log ("Looks like you ain't a slacker");
                }
                else
                {
                    Debug.Log ("YOU ARENT NEAR THE VEHICLE!! YOU DIED!!!");
                }
                theDeLoreanClone.SetActive (false);
                theTestDeLorean.SetActive (true);
            }
        }
        else
        {
        }
    }
    public void OnGUI()
    {
        GUI.Box (new Rect(15, 40, 180, 60), "DeLorean Time: " + theDeLoreanTimer, timerGuiStyle);
    }
    public float MeasureDistance(Transform aGameObject)
    {
        distanceBetween = Vector3.Distance (theTestDeLorean.transform.position, aGameObject.position);
        return distanceBetween;
    }
}
