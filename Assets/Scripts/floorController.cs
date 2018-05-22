using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class floorController : MonoBehaviour{
    Renderer MainSpriteRenderer;
    Renderer ParentRenderer;

    public Material Standby;
    public Material Passed;

    // Use this for initialization
    void Start(){
        MainSpriteRenderer = gameObject.GetComponent<Renderer>();
        foreach (Transform child in gameObject.transform){
            child.GetComponent<Renderer>().material = Standby;
        }
    }

    // Update is called once per frame
    void Update(){

    }

    public void ChangeMaterial(GameObject oya){
        ParentRenderer = oya.GetComponent<Renderer>();
        ParentRenderer.material = Passed;
    }

    public void ResetMaterial(){
        foreach (Transform child in gameObject.transform){
            child.GetComponent<Renderer>().material = Standby;
        }
    }
}
