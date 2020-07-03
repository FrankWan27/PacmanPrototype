using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

    }

    public void ShowHelp()
    {
        SceneManager.LoadScene("Help", LoadSceneMode.Single);

    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);

    }
}
