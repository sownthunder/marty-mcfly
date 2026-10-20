using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Flocking : MonoBehaviour
{
    public int enemiesRem = 8;
    public float distanceBetween;
    public Transform theDeLorean;
    public float dist = 2.0f;
    public Transform enemyCop1;
    public Transform enemyCop2;
    public Transform enemyCop3;
    public Transform enemyCop4;
    public Transform enemyCop5;
    public Transform enemyCop6;
    public Transform enemyCop7;
    public Transform enemyCop8;
    public CopController EnemyCop1Script;
    public CopController EnemyCop2Script;
    public CopController EnemyCop3Script;
    public CopController EnemyCop4Script;
    public CopController EnemyCop5Script;
    public CopController EnemyCop6Script;
    public CopController EnemyCop7Script;
    public CopController EnemyCop8Script;
    public Transform enemyCopClone1;
    public Transform enemyCopClone2;
    public Transform enemyCopClone3;
    public Transform enemyCopClone4;
    public Transform enemyCopClone5;
    public Transform enemyCopClone6;
    public Transform enemyCopClone7;
    public Transform enemyCopClone8;
    public CopCloneController EnemyCopClone1Script;
    public CopCloneController EnemyCopClone2Script;
    public CopCloneController EnemyCopClone3Script;
    public CopCloneController EnemyCopClone4Script;
    public CopCloneController EnemyCopClone5Script;
    public CopCloneController EnemyCopClone6Script;
    public CopCloneController EnemyCopClone7Script;
    public CopCloneController EnemyCopClone8Script;
    public void Awake()
    {
        EnemyCop1Script = enemyCop1.GetComponent<CopController>();
        EnemyCop2Script = enemyCop2.GetComponent<CopController>();
        EnemyCop3Script = enemyCop3.GetComponent<CopController>();
        EnemyCop4Script = enemyCop4.GetComponent<CopController>();
        EnemyCop5Script = enemyCop5.GetComponent<CopController>();
        EnemyCop6Script = enemyCop6.GetComponent<CopController>();
        EnemyCop7Script = enemyCop7.GetComponent<CopController>();
        EnemyCop8Script = enemyCop8.GetComponent<CopController>();
        EnemyCopClone1Script = enemyCopClone1.GetComponent<CopCloneController>();
        EnemyCopClone2Script = enemyCopClone2.GetComponent<CopCloneController>();
        EnemyCopClone3Script = enemyCopClone3.GetComponent<CopCloneController>();
        EnemyCopClone4Script = enemyCopClone4.GetComponent<CopCloneController>();
        EnemyCopClone5Script = enemyCopClone5.GetComponent<CopCloneController>();
        EnemyCopClone6Script = enemyCopClone6.GetComponent<CopCloneController>();
        EnemyCopClone7Script = enemyCopClone7.GetComponent<CopCloneController>();
        EnemyCopClone8Script = enemyCopClone8.GetComponent<CopCloneController>();
    }
    public void Start()
    {
    }
    public void Update()
    {
    }
    public float MeasureDistance(Vector3 vectorA, Vector3 vectorB)
    {
        distanceBetween = Vector3.Distance (vectorA, vectorB);
        return distanceBetween;
    }
}
