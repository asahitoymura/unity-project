using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour{

    private string[] comm;
    private int[] number;
    private string[] c = CSVWriter.getCom();
    private int count = 0;
    private float posX = 0;
    private float posY = 0;
    private int x = 0;
    private string score;
    private Text Text;
    private GameObject retry;
    private GameObject next;
    private GameObject end;

    // Use this for initialization
    void Start(){
        comm = new string[1024];
        number = new int[1024];
        LoadCSV.csvload();
        Text = GameObject.Find("Text").GetComponent<Text>();
        retry = GameObject.Find("Canvas/retry");
        retry.SetActive(false);
        next = GameObject.Find("Canvas/next");
        next.SetActive(false);
        end = GameObject.Find("end");
        end.SetActive(false);
    }

    // Update is called once per frame
    void Update(){

    }

    public void load(){
        count += LoadCSV.getI();
        while (!(c[count] == null)){
            string a = c[count];
            string[] values = a.Split(',');
            if (values[0].Equals("moveOn")){
                comm[count] = "まえにすすむ";
                GameObject prefab = (GameObject)Resources.Load(values[0]);
                Vector3 postion = new Vector3(x: -8.5F + posX, y: 4.5F - posY, z: 0);
                Instantiate(prefab, postion, rotation: Quaternion.identity);
                GameObject yazirusi = (GameObject)Resources.Load("sita");
                Vector3 yazipos = new Vector3(-8.6f + posX, 4.5f - 1.0f - posY, 0);
                Instantiate(yazirusi, yazipos, Quaternion.identity);
                posY += 1.0f + 1.0f;
                x++;
                if(x >= 5){
                    posX += 1.5f + 0.15f;
                    posY = 0;
                    x = 0;
                }
            } else if (values[0].Equals("turnRight")){
                comm[count] = "みぎにむく";
                GameObject prefab = (GameObject)Resources.Load(values[0]);
                Vector3 postion = new Vector3(x: -8.5F + posX, y: 4.5F - posY, z: 0);
                Instantiate(prefab, postion, rotation: Quaternion.identity);
                GameObject yazirusi = (GameObject)Resources.Load("sita");
                Vector3 yazipos = new Vector3(-8.6f + posX, 4.5f - 1.0f - posY, 0);
                Instantiate(yazirusi, yazipos, Quaternion.identity);
                posY += 1.0f + 1.0f;
                x++;
                if (x >= 5){
                    posX += 1.5f + 0.15f;
                    posY = 0;
                    x = 0;
                }
            } else if (values[0].Equals("turnLeft")){
                comm[count] = "ひだりにむく";
                GameObject prefab = (GameObject)Resources.Load(values[0]);
                Vector3 postion = new Vector3(x: -8.5F + posX, y: 4.5F - posY, z: 0);
                Instantiate(prefab, postion, rotation: Quaternion.identity);
                GameObject yazirusi = (GameObject)Resources.Load("sita");
                Vector3 yazipos = new Vector3(-8.6f + posX, 4.5f - 1.0f - posY, 0);
                Instantiate(yazirusi, yazipos, Quaternion.identity);
                posY += 1.0f + 1.0f;
                x++;
                if (x >= 5){
                    posX += 1.5f + 0.15f;
                    posY = 0;
                    x = 0;
                }
            } else if (values[0].Equals("buckOn")){
                comm[count] = "うしろにすすむ";
                GameObject prefab = (GameObject)Resources.Load(values[0]);
                Vector3 postion = new Vector3(x: -8.5F + posX, y: 4.5F - posY, z: 0);
                Instantiate(prefab, postion, rotation: Quaternion.identity);
                GameObject yazirusi = (GameObject)Resources.Load("sita");
                Vector3 yazipos = new Vector3(-8.6f + posX, 4.5f - 1.0f - posY, 0);
                Instantiate(yazirusi, yazipos, Quaternion.identity);
                posY += 1.0f + 1.0f;
                x++;
                if (x >= 5){
                    posX += 1.5f + 0.15f;
                    posY = 0;
                    x = 0;
                }
            } else if (values[0].Equals("Goal")){
                comm[count] = "ゴール";
            } else if (values[0].Equals("Miss")){
                comm[count] = "ミス";
            }
            number[count] = int.Parse(values[1]);
            Debug.Log(comm[count] + ":" + number[count]);
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
