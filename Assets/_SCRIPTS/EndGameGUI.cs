using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(AudioSource))]
public class EndGameGUI : MonoBehaviour
{
    public GameObject splashBackground;
    public string endButtonText = "End Game";
    public bool isShowingQButton = false;
    public bool isShowingRButton = false;
    public Texture btnTexture;
    public int imagePosition;
    public float outroAudioTimer = 0;
    public void Awake()
    {
        imagePosition = Screen.width / 4;
    }
    public void Start()
    {
    }
    public void Update()
    {
        outroAudioTimer = outroAudioTimer + Time.fixedDeltaTime;
        Debug.Log ("oUtRo +=+ " + outroAudioTimer);
        if (outroAudioTimer >= 14)
        {
            splashBackground.SetActive (false);
            isShowingQButton = true;
            isShowingRButton = true;
        }
    }
    public void OnGUI()
    {
        if (isShowingQButton && isShowingRButton)
        {
            GUI.Box (new Rect(imagePosition, ((Screen.height / 3) * 2), (Screen.width / 2), 100), "CLICK THE IMAGE ABOVE TO QUIT");
            if (GUI.Button (new Rect(10, 40, 90, 30), "Quit"))
            {
                Application.Quit ();
                Debug.Log ("Quit");
            }
            if (GUI.Button (new Rect(10, 70, 90, 30), "Or restart?"))
            LoadScene (1);
        }
    }
    private static void LoadScene(int buildIndex)
    {
#if UNITY_5_3_OR_NEWER
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
#else
        Application.LoadLevel(buildIndex);
#endif
    }
}
