using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkPointScript : MonoBehaviour {

    private PauseScript pauseScript;
    public Sprite talkPanel;


	// Use this for initialization
	void Start () {
        pauseScript = GameObject.Find("Player").GetComponent<PauseScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision other){
        Debug.Log("お店に入りました collision");
        pauseScript.DispShoping(talkPanel);
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            Debug.Log("お店に入りました triger");
            pauseScript.DispShoping(talkPanel);
        }
    }
}
