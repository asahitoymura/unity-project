using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour{

    PlayerController playerController;

    //　ポーズした時に表示するUI
    [SerializeField]
    public GameObject pauseUI;
    public GameObject Obstacle;
    public GameObject goalUI;

    private void Start(){
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update(){
        if (pauseUI.activeSelf){
            if (Input.GetMouseButtonDown(0)){
                pauseUI.SetActive(false);
                Time.timeScale = 1f;
                playerController.SetIfCheckFlg();
                playerController.ChangeMoveFlg();
            }
        } else if (Obstacle.activeSelf){
            if (Input.GetMouseButtonDown(0)){
                Obstacle.SetActive(false);
                Time.timeScale = 1f;
                playerController.ChangeMoveFlg();
            }
        } else if (goalUI.activeSelf){
            if (Input.GetMouseButtonDown(0)){
                goalUI.SetActive(false);
                Time.timeScale = 1f;
                playerController.ChangeMoveFlg();
            }
        }
    }
    
    public void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "wall"){
            Obstacle.SetActive(!Obstacle.activeSelf);
            if (Obstacle.activeSelf){
                Time.timeScale = 0f;
            } else {
                Time.timeScale = 1f;
            }
        }
    }

    public void dispCheckPoint(){
        pauseUI.SetActive(!pauseUI.activeSelf);
        playerController.ChangeMoveFlg();
        if (pauseUI.activeSelf){
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }

    public void DispGoal(Sprite sprite){
        GameObject panelObj = goalUI.transform.Find("Panel").gameObject;
        panelObj.GetComponent<Image>().sprite = sprite;
        goalUI.SetActive(!goalUI.activeSelf);
        if (goalUI.activeSelf){
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }
}