using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVWriter : MonoBehaviour{

    public string filepathname;
    //InputManager input;
    private string name = InputManager.getInputValue();
    private static string[] com;
    private int num = 0;

    // Use this for initialization
    void Start(){
        com = new string[1024];
        LoadCSV.csvload();
    }

    // Update is called once per frame
    void Update(){

    }

    public void WriteCSV(string txt){
        if (LoadCSV.getI() > 0){
            StreamWriter streamWriter;
            FileInfo fileInfo;
            fileInfo = new FileInfo(Application.dataPath + "/" + filepathname + "/" + name + ".csv");
            streamWriter = fileInfo.AppendText();
            streamWriter.WriteLine(txt);
            com[num] = txt;
            num++;
            streamWriter.Flush();
            streamWriter.Close();
        } else {
            StreamWriter streamWriter;
            FileInfo fileInfo;
            fileInfo = new FileInfo(Application.dataPath + "/" + filepathname + "/" + name + ".csv");
            streamWriter = fileInfo.AppendText();
            streamWriter.WriteLine(txt);
            com[num] = txt;
            num++;
            streamWriter.Flush();
            streamWriter.Close();
        }
    }

    public static string[] getCom(){
        return com;
    }
}
