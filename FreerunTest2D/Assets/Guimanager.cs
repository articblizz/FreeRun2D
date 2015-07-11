using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Guimanager : MonoBehaviour {

    public GameObject canvas;
    public PlayerScript plyScript;

    public GameObject firstPlayPanel;

    // Use this for initialization
    void Start () {
        
        
        var firstPlay = PlayerPrefs.GetFloat("firstplay");

        print(firstPlay);
        if (firstPlay == 0)
            firstPlayPanel.SetActive(true);
    
    }

    public void StartTheGame()
    {
        PlayerPrefs.SetFloat("firstplay", 1);
        canvas.SetActive(false);
        plyScript.disableInput = false;
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}
