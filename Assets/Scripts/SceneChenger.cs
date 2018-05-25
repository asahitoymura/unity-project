using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneChenger : MonoBehaviour {

    public string nextscenename;
    public string backscenename;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void nextpush(){
        SceneManager.LoadScene(nextscenename);
    }

    public void retryPush(){
        SceneManager.LoadScene(backscenename);
    }

    public void endPush(){
        //Directory.Move(Application.dataPath + "/Resouces/CSV/","C:/Users/atoyomura/Desktop");
        Application.Quit();
    }
}
