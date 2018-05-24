using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;

public class SubmitButton : MonoBehaviour {

    //連携するGameObject
    public ToggleGroup toggleGroup;
    public ToggleGroup toggleGroup2;
    private string name;
    public string filepathname;
    private TextAsset csvfile;
    private string[] textdata;
    private string[] num;
    private int i = 0;
    private string scenename;
    private GameObject startbutton;

    // Use this for initialization
    void Start(){
        textdata = new string[2048];
        num = new string[2048];
        scenename = SceneManager.GetActiveScene().name;
        startbutton = GameObject.Find("Canvas/Start");
        Debug.Log(startbutton);
        if(scenename == "LoadScene"){
           GameObject.Find("Canvas/Start").SetActive(!startbutton.activeSelf);
        }
    }
    // Update is called once per frame
    void Update(){

    }

    public void onClick(){
        //Get the label in activated toggles
        string selectedLabel = toggleGroup.ActiveToggles()
            .First().GetComponentsInChildren<Text>()
            .First(t => t.name == "Label").text;
        Debug.Log("selected " + selectedLabel);
        name = InputManager.getInputValue();
        string gaku = toggleGroup2.ActiveToggles()
            .First().GetComponentsInChildren<Text>()
            .First(t => t.name == "Label").text;
        StreamWriter streamWriter;
        FileInfo fileInfo;
        fileInfo = new FileInfo(Application.dataPath + "/" + filepathname+"/" + name + ".csv");
        streamWriter = fileInfo.AppendText();
        streamWriter.WriteLine(selectedLabel + "," + 0);
        streamWriter.WriteLine(gaku + "," + 0);
        streamWriter.Flush();
        streamWriter.Close();
        SceneManager.LoadScene("stage1");
    }

    public void LoadOnClick(){
        name = InputManager.getInputValue();
        csvfile = Resources.Load("CSV/" + name) as TextAsset;
        Debug.Log(csvfile);
        if(csvfile == null){
            Debug.Log("データがありません");
        } else {
            StringReader reader = new StringReader(csvfile.text);
            while(reader.Peek() > -1){
                string line = reader.ReadLine();
                string[] values = line.Split(',');
                textdata[i] = values[0];
                num[i] = values[1];
                // if(textdata[0] == name)
                i++;
            }
            if(textdata[0] == name){
                Debug.Log("ろーどできました");
                startbutton.SetActive(true);
            }else{
                Debug.Log("ろーどできませんでした");
                return;
            }
        }
    }

    public void startButtonClick(){
        SceneManager.LoadScene("stage1");
    }
}
