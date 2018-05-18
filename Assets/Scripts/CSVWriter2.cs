using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVWriter2 : MonoBehaviour{

    public string filepathname;
    //InputManager input;
    private string name = InputManager.getInputValue();
    

    // Use this for initialization
    void Start(){
        //input = GameObject.Find("InputField").GetComponent<InputManager>();
        //        Directory.CreateDirectory("C:/Users/atoyomura/p/edupgm2/test_Data/Resources/CSV/0");
        //Directory.CreateDirectory(filepathname);
    }

    // Update is called once per frame
    void Update(){

    }

    public void WriteCSV(string txt){
        StreamWriter streamWriter;
        FileInfo fileInfo;
        fileInfo = new FileInfo(Application.dataPath + "/" +filepathname+"/" + name + ".csv");
        streamWriter = fileInfo.AppendText();
        streamWriter.WriteLine(txt);
        streamWriter.Flush();
        streamWriter.Close();
    }
}
