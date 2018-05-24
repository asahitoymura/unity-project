using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour{

    private string[] comm;
    private int[] number;
    private string[] c = CSVWriter.getCom();
    private List<string> comlist;
    private List<GameObject> list;
    private List<int> comindex;
    private int count = 0;
    private float posX = 0;
    private float posY = 0;
    private int x = 0;
    private string score;
    private Text Text;
    private GameObject retry;
    private GameObject next;
    private GameObject end;
    private string stagename;
    panelController panelController;
    PlayerController PlayerController;

    // Use this for initialization
    void Start(){
        comm = new string[1024];
        number = new int[1024];
        comlist = new List<string>();
        list = new List<GameObject>();
        LoadCSV.csvload();
        Text = GameObject.Find("Text").GetComponent<Text>();
        retry = GameObject.Find("Canvas/retry");
        retry.SetActive(false);
        next = GameObject.Find("Canvas/next");
        stagename = SceneManager.GetActiveScene().name;
        if(stagename == "stage1Result" || stagename == "stage2Result"){
            next.SetActive(false);
        }
        end = GameObject.Find("end");
        if(stagename == "stage3Result"){
            end.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update(){

    }

    public void load(){
        //count += LoadCSV.getI();
        comlist = PlayerController.GetMoveList();
        comindex = panelController.GetComnum();
        for (int i = 0; i < comlist.Count; i++){
            string a = comlist[i];
            //string[] values = a.Split(',');
            if (a.Equals("moveOn")){
                comm[i] = "まえにすすむ";
                GameObject prefab = (GameObject)Resources.Load(a);
                Vector3 postion = new Vector3(x: -8.5F + posX, y: 4.5F - posY, z: 0);
                Instantiate(prefab, postion, rotation: Quaternion.identity);
                GameObject yazirusi = (GameObject)Resources.Load("sita");
                Vector3 yazipos = new Vector3(-8.6f + posX, 4.5f - 1.0f - posY, 0);
                if(i < comlist.Count-1){
                    Instantiate(yazirusi, yazipos, Quaternion.identity);
                }
                posY += 1.0f + 1.0f;
                x++;
                if(x >= 5){
                    posX += 1.5f + 0.15f;
                    posY = 0;
                    x = 0;
                }
            } else if (a.Equals("turnRight")){
                comm[i] = "みぎにむく";
                GameObject prefab = (GameObject)Resources.Load(a);
                Vector3 postion = new Vector3(x: -8.5F + posX, y: 4.5F - posY, z: 0);
                Instantiate(prefab, postion, rotation: Quaternion.identity);
                GameObject yazirusi = (GameObject)Resources.Load("sita");
                Vector3 yazipos = new Vector3(-8.6f + posX, 4.5f - 1.0f - posY, 0);
                if(i < comlist.Count-1){
                    Instantiate(yazirusi, yazipos, Quaternion.identity);
                }
                posY += 1.0f + 1.0f;
                x++;
                if (x >= 5){
                    posX += 1.5f + 0.15f;
                    posY = 0;
                    x = 0;
                }
            } else if (a.Equals("turnLeft")){
                comm[i] = "ひだりにむく";
                GameObject prefab = (GameObject)Resources.Load(a);
                Vector3 postion = new Vector3(x: -8.5F + posX, y: 4.5F - posY, z: 0);
                Instantiate(prefab, postion, rotation: Quaternion.identity);
                GameObject yazirusi = (GameObject)Resources.Load("sita");
                Vector3 yazipos = new Vector3(-8.6f + posX, 4.5f - 1.0f - posY, 0);
                if(i < comlist.Count-1){
                    Instantiate(yazirusi, yazipos, Quaternion.identity);
                }
                posY += 1.0f + 1.0f;
                x++;
                if (x >= 5){
                    posX += 1.5f + 0.15f;
                    posY = 0;
                    x = 0;
                }
            } else if (a.Equals("buckOn")){
                comm[i] = "うしろにすすむ";
                GameObject prefab = (GameObject)Resources.Load(a);
                Vector3 postion = new Vector3(x: -8.5F + posX, y: 4.5F - posY, z: 0);
                Instantiate(prefab, postion, rotation: Quaternion.identity);
                GameObject yazirusi = (GameObject)Resources.Load("sita");
                Vector3 yazipos = new Vector3(-8.6f + posX, 4.5f - 1.0f - posY, 0);
                if(i < comlist.Count-1){
                    Instantiate(yazirusi, yazipos, Quaternion.identity);
                }
                posY += 1.0f + 1.0f;
                x++;
                if (x >= 5){
                    posX += 1.5f + 0.15f;
                    posY = 0;
                    x = 0;
                }
            } else if (a.Equals("Goal")){
                comm[i] = "ゴール";
            } else if (a.Equals("Miss")){
                comm[i] = "ミス";
            } else if (a.Equals("action")){
                if(stagename == "stage1"){
                    comm[i] = "はなしかけた";
                }else if(stagename == "stage3"){
                    comm[i] = "かいものした";
                }
            } else if(a.Equals("whilePanel")){
                comm[i] = "くりかえし";
            } else if(a.Equals("ifPanel")){
                comm[i] = "もし～なら";
            }
            
            number[i] = comindex[i];
            Debug.Log(comm[i] + ":" + number[i]);
            count++;
        }
        Text.text = count.ToString() + "回";
    }

    public void pushResultButton(){
        retry.SetActive(true);
        next.SetActive(true);
    }

    public void pushResultButton2(){
        retry.SetActive(true);
        end.SetActive(true);
    }
}
