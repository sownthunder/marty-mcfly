using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(AudioSource))]
public class MenuGUI : MonoBehaviour
{
    public GUIStyle menuGUIStyle;
    public GameObject splashBackground;
    public string playButtonText = "Play!";
    public string introRulesText = "You found da secret (not really)!";
    public bool isShowingRules = false;
    public Texture btnTexture;
    public int imagePosition;
    public float introAudioTimer = 0;
    public AudioClip otherClip1;
    public AudioClip otherClip2;
    public string passwordToEdit = "2015";
    public void Awake()
    {
        imagePosition = Screen.width / 4;
        Debug.Log ("img pos =" +imagePosition);
    }
    public void Start()
    {
    }
    public void Update()
    {
        introAudioTimer = introAudioTimer + Time.fixedDeltaTime;
        if (introAudioTimer >= 14)
        {
            splashBackground.SetActive (false);
            isShowingRules = true;
        }
    }
    public void OnGUI()
    {
        if (isShowingRules)
        {
            GUI.Box (new Rect(imagePosition, ((Screen.height / 3) * 2), (Screen.width / 2), 100), "CLICK MARTY & DOC ", menuGUIStyle);
            if (GUI.Button (new Rect(imagePosition, 50, (Screen.width / 2), (Screen.width / 3)), btnTexture))
            {
                LoadScene (1);
                Debug.Log ("Clicked the button with an image");
            }
        }
        if (!btnTexture)
        {
            Debug.LogError("Please assign a texture on the inspector");
            return;
        }
    }
    public void LoadHighScores()
    {
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
