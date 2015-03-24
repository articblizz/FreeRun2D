using UnityEngine;
using System.Collections;

public class Guimanager : MonoBehaviour {

    public GameObject canvas;
    public PlayerScript plyScript;

    // Use this for initialization
    void Start () {
    
    }

    public void StartTheGame()
    {
        canvas.SetActive(false);
        plyScript.disableInput = false;
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}
