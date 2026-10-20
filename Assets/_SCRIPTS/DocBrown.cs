using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DocBrown : MonoBehaviour
{
    public Transform theMarty;
    public int numOfPhrases = 5;
    public bool isRight = false;
    public bool isLeft = false;
    public Sprite spriteTalkingLeft;
    public Sprite spriteIdleRight;
    public Sprite spriteTalkingRight;
    public string docBrownSaying1 = "No, no, no, no, no, this sucker's electrical, but I need a nuclear reaction to generate the 1.21 gigawatts of electricity I need.";
    public string docBrownSaying2 = "Of course. From a group of Libyan nationalists. They wanted me to build them a bomb, so I took their plutonium and, in turn, gave them a shoddy bomb casing full of used pinball machine parts. Come on! Let's get you a radiation suit. We must prepare to reload.";
    public string docBrownSaying3;
    public string docBrownSaying4;
    public string docBrownSaying5;
    public void Awake()
    {
    }
    public void Start()
    {
    }
    public void Update()
    {
    }
    public void OnGUI()
    {
    }
}
