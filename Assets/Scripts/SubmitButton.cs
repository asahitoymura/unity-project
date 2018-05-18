using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.IO;

public class SubmitButton : MonoBehaviour {

    //連携するGameObject
    public ToggleGroup toggleGroup;
    private string name;
    public string filepathname;

    // Use this for initialization
    void Start(){
        
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
        StreamWriter streamWriter;
        FileInfo fileInfo;
        fileInfo = new FileInfo(Application.dataPath + "/" + filepathname+"/" + name + ".csv");
        streamWriter = fileInfo.AppendText();
        streamWriter.WriteLine(selectedLabel + "," + 1);
        streamWriter.Flush();
        streamWriter.Close();
    }

    public void SetName(string n){
        name = n;
    }
}
