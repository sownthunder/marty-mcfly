using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DeLoreanController : MonoBehaviour
{
    public Rigidbody Rb;
    public float speed = 7.0f;
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
    public bool enteredBorder = false;
    public int plutesCollected = 0;
    public int plutesToFlux = 2;
    public int plutesToTravel = 2;
    public bool usePlutesTravel = false;
    public bool winCondition = false;
    public int rangeWidth = 119;
    public int rangeHeight = 119;
    public float rndIntWidth;
    public float rndIntHeight;
    public Transform eighty8MphObj;
    public Eighty8Mph eighty8MphScript;
    public Transform itemSpawnerObj;
    public ItemSpawner itemSpawnerScript;
    public Transform enemy88sObj;
    public Transform enemy88s2Obj;
    public Transform enemy88s3Obj;
    public Transform enemy88s4Obj;
    public Transform enemy88s5Obj;
    public Transform enemy88s6Obj;
    public Transform enemy88s7Obj;
    public Transform enemy88s8Obj;
    public Enemy88s enemy88sScript;
    public Enemy88s enemy88s2Script;
    public Enemy88s enemy88s3Script;
    public Enemy88s enemy88s4Script;
    public Enemy88s enemy88s5Script;
    public Enemy88s enemy88s6Script;
    public Enemy88s enemy88s7Script;
    public Enemy88s enemy88s8Script;
    public Transform enemyCop1;
    public Transform enemyCop2;
    public Transform enemyCop3;
    public Transform enemyCop4;
    public Transform enemyCop5;
    public Transform enemyCop6;
    public Transform enemyCop7;
    public Transform enemyCop8;
    public GameObject nextEnemyCop;
    public void Awake()
    {
        eighty8MphScript = eighty8MphObj.GetComponent<Eighty8Mph>();
        itemSpawnerScript = itemSpawnerObj.GetComponent<ItemSpawner>();
        enemy88sScript = enemy88sObj.GetComponent<Enemy88s>();
        enemy88s2Script = enemy88s2Obj.GetComponent<Enemy88s>();
        enemy88s3Script = enemy88s3Obj.GetComponent<Enemy88s>();
        enemy88s4Script = enemy88s4Obj.GetComponent<Enemy88s>();
        enemy88s5Script = enemy88s5Obj.GetComponent<Enemy88s>();
        enemy88s6Script = enemy88s6Obj.GetComponent<Enemy88s>();
        enemy88s7Script = enemy88s7Obj.GetComponent<Enemy88s>();
        enemy88s8Script = enemy88s8Obj.GetComponent<Enemy88s>();
        sRenderer = sRendererObj.GetComponent<SpriteRenderer>();
        Rb = transform.GetComponent<Rigidbody>();
    }
    public void Start()
    {
        pos = transform.position;
        for (var i = 0; i < 8; i++)
        {
            rndIntWidth = Random.Range (1, rangeWidth);
            rndIntHeight = Random.Range (1, rangeHeight);
            switch (i)
            {
                case 0:
                enemyCop1.position = new Vector3(rndIntWidth, rndIntHeight, 0);
                break;
                case 1:
                enemyCop2.position = new Vector3(rndIntWidth, rndIntHeight, 0);
                break;
                case 2:
                enemyCop3.position = new Vector3(rndIntWidth, rndIntHeight, 0);
                break;
                case 3:
                enemyCop4.position = new Vector3(rndIntWidth, rndIntHeight, 0);
                break;
                case 4:
                enemyCop5.position = new Vector3(rndIntWidth, rndIntHeight, 0);
                break;
                case 5:
                enemyCop6.position = new Vector3(rndIntWidth, rndIntHeight, 0);
                break;
                case 6:
                enemyCop7.position = new Vector3(rndIntWidth, rndIntHeight, 0);
                break;
                case 7:
                enemyCop8.position = new Vector3(rndIntWidth, rndIntHeight, 0);
                break;
            }
        }
    }
    public void Update()
    {
        var step = speed * Time.deltaTime;
        if (eighty8MphScript.isFluxing && enemy88sScript.isFluxing)
        {
            eighty8MphScript.arr.Add (new Vector3(transform.position.x, transform.position.y, 0f));
            eighty8MphScript.spriteArr.Add (sRenderer.sprite);
            enemy88sScript.copArr.Add (new Vector3(enemyCop1.transform.position.x, enemyCop1.transform.position.y, -0.5f));
            enemy88s2Script.copArr.Add (new Vector3(enemyCop2.transform.position.x, enemyCop2.transform.position.y, -0.5f));
            enemy88s3Script.copArr.Add (new Vector3(enemyCop3.transform.position.x, enemyCop3.transform.position.y, -0.5f));
            enemy88s4Script.copArr.Add (new Vector3(enemyCop4.transform.position.x, enemyCop4.transform.position.y, -0.5f));
            enemy88s5Script.copArr.Add (new Vector3(enemyCop5.transform.position.x, enemyCop5.transform.position.y, -0.5f));
            enemy88s6Script.copArr.Add (new Vector3(enemyCop6.transform.position.x, enemyCop6.transform.position.y, -0.5f));
            enemy88s7Script.copArr.Add (new Vector3(enemyCop7.transform.position.x, enemyCop7.transform.position.y, -0.5f));
            enemy88s8Script.copArr.Add (new Vector3(enemyCop8.transform.position.x, enemyCop8.transform.position.y, -0.5f));
        }
        if (isRight && inVehicle)
        {
            pos = Vector3.right;
            sRenderer.sprite = spriteRight;
            transform.Translate (pos * step);
        }
        else if (isLeft && inVehicle)
        {
            pos = Vector3.left;
            sRenderer.sprite = spriteLeft;
            transform.Translate (pos * step);
        }
        else if (isUp && inVehicle)
        {
            pos = Vector3.up;
            sRenderer.sprite = spriteUp;
            transform.Translate (pos * step);
        }
        else if (isDown && inVehicle)
        {
            pos = Vector3.down;
            sRenderer.sprite = spriteDown;
            transform.Translate (pos * step);
        }
        else if (Input.GetKeyDown (KeyCode.F) && inVehicle && (plutesCollected >= plutesToTravel))
        {
            plutesCollected = plutesCollected - plutesToTravel;
            eighty8MphScript.isFluxing = true;
            enemy88sScript.isFluxing = true;
            enemy88s2Script.isFluxing = true;
            enemy88s3Script.isFluxing = true;
            enemy88s4Script.isFluxing = true;
            enemy88s5Script.isFluxing = true;
            enemy88s6Script.isFluxing = true;
            enemy88s7Script.isFluxing = true;
            enemy88s8Script.isFluxing = true;
        }
        else if (Input.GetKeyDown (KeyCode.T) && eighty8MphScript.isFluxing
        && enemy88sScript.isFluxing && inVehicle && (plutesCollected >= plutesToTravel))
        {
            enemyCop1.gameObject.SetActive (false);
            enemyCop2.gameObject.SetActive (false);
            enemyCop3.gameObject.SetActive (false);
            enemyCop4.gameObject.SetActive (false);
            enemyCop5.gameObject.SetActive (false);
            enemyCop6.gameObject.SetActive (false);
            Debug.Log ("EnemyCop 4-6 gonezo");
            enemyCop7.gameObject.SetActive (false);
            enemyCop8.gameObject.SetActive (false);
            plutesCollected = plutesCollected - plutesToTravel;
            eighty8MphScript.isFluxing = false;
            enemy88sScript.isFluxing = false;
            enemy88s2Script.isFluxing = false;
            enemy88s3Script.isFluxing = false;
            enemy88s4Script.isFluxing = false;
            enemy88s5Script.isFluxing = false;
            enemy88s6Script.isFluxing = false;
            enemy88s7Script.isFluxing = false;
            enemy88s8Script.isFluxing = false;
            eighty8MphScript.timeCircuitsOn = true;
            enemy88sScript.timeCircuitsOn = true;
            enemy88s2Script.timeCircuitsOn = true;
            enemy88s3Script.timeCircuitsOn = true;
            enemy88s4Script.timeCircuitsOn = true;
            enemy88s5Script.timeCircuitsOn = true;
            enemy88s6Script.timeCircuitsOn = true;
            enemy88s7Script.timeCircuitsOn = true;
            enemy88s8Script.timeCircuitsOn = true;
            eighty8MphScript.originalDeLoreanPosition = transform.position;
            enemy88sScript.originalEnemyPosition = enemyCop1.transform.position;
            enemy88s2Script.originalEnemyPosition = enemyCop2.transform.position;
            enemy88s3Script.originalEnemyPosition = enemyCop3.transform.position;
            enemy88s4Script.originalEnemyPosition = enemyCop4.transform.position;
            enemy88s5Script.originalEnemyPosition = enemyCop5.transform.position;
            enemy88s6Script.originalEnemyPosition = enemyCop6.transform.position;
            enemy88s7Script.originalEnemyPosition = enemyCop7.transform.position;
            enemy88s8Script.originalEnemyPosition = enemyCop8.transform.position;
        }
        else
        {
            isRight = false;
            isLeft = false;
            isUp = false;
            isDown = false;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
        }
        else if (other.tag == "Grass")
        {
            speed = 5.0f;
        }
        else if (other.tag == "Border")
        {
            enteredBorder = true;
            CheckIfBoundary ();
        }
        else if (other.tag == "Building")
        {
            enteredBorder = true;
            CheckIfBoundary ();
        }
        else if (other.tag == "Plute")
        {
            Destroy (other.gameObject);
            int rndIntWidth = Random.Range (2, 119);
            int rndIntHeight = Random.Range (2, 119);
            Instantiate (itemSpawnerScript.thePlute, new Vector3(rndIntWidth, rndIntHeight, 0), Quaternion.identity);
            plutesCollected++;
            if (plutesCollected == itemSpawnerScript.totalPlutesNum)
            {
                winCondition = true;
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Border")
        {
            CheckIfBoundary ();
        }
        else if (other.tag == "Building")
        {
            CheckIfBoundary ();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
        }
        if (other.tag == "Grass")
        {
            speed = 7.0f;
        }
        if (other.tag == "Border")
        {
            enteredBorder = false;
        }
    }
    public void DestroyGameObjectWithTag(string tag)
    {
        var objectsWithTag = new List<GameObject>(GameObject.FindGameObjectsWithTag(tag));
        foreach (GameObject taggedObject in objectsWithTag)
        Destroy(taggedObject);
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
}
