using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dustbox : MonoBehaviour {

    private panelController panelController;

    // Use this for initialization
    void Start () {
        panelController = GameObject.Find("edit").GetComponent<panelController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other){
        panelController.DeletePanel(other.gameObject);
        Destroy(other.gameObject);

    }
}
