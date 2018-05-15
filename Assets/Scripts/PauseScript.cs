using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour
{

    PlayerController playerController;

    //　ポーズした時に表示するUI
    [SerializeField]
    public GameObject pauseUI;
    public GameObject Obstacle;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseUI.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pauseUI.SetActive(false);
                Time.timeScale = 1f;
                playerController.SetIfCheckFlg();
            }
        }
        else if (Obstacle.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Obstacle.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "checkpoint"){
            pauseUI.SetActive(!pauseUI.activeSelf);
            if (pauseUI.activeSelf){
                Time.timeScale = 0f;
            }else{
                Time.timeScale = 1f;
            }
        }else 
        if (other.gameObject.tag == "wall")
        {
            Obstacle.SetActive(!Obstacle.activeSelf);
            if (Obstacle.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void dispCheckPoint()
    {
        pauseUI.SetActive(!pauseUI.activeSelf);
        if (pauseUI.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

}