using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadCSV : MonoBehaviour{

    public static string filePass;
    private static string[] command;
    private static int[] num;
    InputManager input;
    public static string name = InputManager.getInputValue();
    static TextAsset csv;
    private bool flag = true;
    private static int i = 0;
    private static int icount = 0;

    // Use this for initialization
    void Start(){
        command = new string[1024];
        num = new int[1024];
        //csvload();
    }

    // Update is called once per frame
    void Update(){

    }

    public static void csvload(){
        //int i = 0;
        //csv = Resources.Load("CSV/" + name) as TextAsset;
        //while (flag){
        //    if (csv == null){
        //        StartCoroutine(wait(0.1f));
        //    }else{
        //        StringReader reader = new StringReader(csv.text);
        //        while (reader.Peek() > -1){
        //            string line = reader.ReadLine();
        //            string[] values = line.Split(',');
        //            command[i] = values[0];
        //            num[i] = int.Parse(values[1]);
        //            i++;
        //            Debug.Log(command[i]);
        //            Debug.Log(num[i]);
        //        }
        //        flag = false;
        //    }
        //}

        csv = Resources.Load(filePass +"/"+ name) as TextAsset;
        if (csv == null){

        } else {
            StringReader reader = new StringReader(csv.text);
            while (reader.Peek() > -1){
                string line = reader.ReadLine();
                string[] values = line.Split(','); //csvでは,でコマンド(命令)と回数を区切っているので,を境に分ける
                command[i] = values[0];
                num[i] = int.Parse(values[1]);   //String型をint型に変換する
                i++;
            }
            icount++;
        }
    }

    IEnumerator wait(float n){
        yield return new WaitForSeconds(n);
    }

    public static int getI(){
        return i;
    }

    public static int getIcount(){
        return icount;
    }
}
