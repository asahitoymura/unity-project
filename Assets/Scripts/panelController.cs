using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelController : MonoBehaviour{

    private List<GameObject> panelList;
    private static List<GameObject> startPanelList;
    private List<GameObject> ifPanelList;
    private List<GameObject> ifActionPanelList;
    private IDictionary<string, List<GameObject>> ifActionMap;
    private GameObject edit;
    private GameObject edit_2;
    private List<string> commandList;
    private List<string> startcomlist;
    private static List<int> comnum;
    private int comindex = 1;
    private float centerPos;
    CSVWriter csvwriter;
    private int ifcount;
    // Use this for initialization
    void Start(){
        panelList = new List<GameObject>();
        startPanelList = new List<GameObject>();
        ifPanelList = new List<GameObject>();
        ifActionPanelList = new List<GameObject>();
        ifActionMap = new Dictionary<string, List<GameObject>>();

        edit = GameObject.Find("edit");
        edit_2 = GameObject.Find("edit_2");

        commandList = new List<string>();
        startcomlist = new List<string>();
        comnum = new List<int>();
        csvwriter = GameObject.Find("CSVWriter").GetComponent<CSVWriter>();

        centerPos = (edit.transform.position.x + edit_2.transform.position.x) / 2;
        ifcount = 0;
    }

    // Update is called once per frame
    void Update(){

    }


    public void AddPanel(GameObject obj){
        panelList.Add(obj);
        commandList.Add(obj.tag);
        //csvwriter.WriteCSV(obj.tag + "," + comindex);        
        comnum.Add(comindex);
        comindex++;
    }
    public void AddIfPanel(GameObject obj){
        ifPanelList.Add(obj);
    }
    public void AddIfActionPanel(GameObject obj){
        ifActionPanelList.Add(obj);
        commandList.Add(obj.tag);
        //csvwriter.WriteCSV(obj.tag + "," + comindex);
        comnum.Add(comindex);
        comindex++;
    }

    public void DeletePanel(GameObject obj){
        Destroy(panelList.Find(x => x == obj));
        panelList.Remove(obj);

        Destroy(ifActionPanelList.Find(x => x == obj));
        ifActionPanelList.Remove(obj);
    }

    public void DeletePanelList(GameObject obj){
        panelList.Remove(obj);
    }

    public List<GameObject> GetPanelList(){
        SortPanel();
        return startPanelList;
    }
    public IDictionary<string, List<GameObject>> GetIfActionMap(){
        if (ifActionMap.ContainsKey("ifPanel")){
            return ifActionMap;
        }
        return null;
    }

    // パネルリストを空にする
    public void ResetPanelList(){
        foreach (GameObject tmp in panelList){
            Destroy(tmp);
        }
        panelList.Clear();
        commandList.Clear();

        foreach(GameObject tmp in ifActionPanelList){
            Destroy(tmp);
        }
        ifActionPanelList.Clear();
        commandList.Clear();
    }

    // パネルの並び替え
    private void SortPanel(){
        List<GameObject> tmpList = new List<GameObject>();
        List<GameObject> tmpList2 = new List<GameObject>();

        List<string> tmpcomlist = new List<string>();
        List<string> tmpcomlist2 = new List<string>();

        foreach (GameObject tmpCube in panelList){
            if (tmpCube.tag == "action"){
                AddIfActionPanel(tmpCube);
                continue;
            }

            // エディットフィールドの左側を最初に実行する
            if (tmpCube.transform.position.x < centerPos){
                int index = 0;
                foreach (GameObject tmpObj in tmpList){
                    Vector3 tmpObjPos = tmpObj.transform.position;
                    Vector3 tmpCubePos = tmpCube.transform.position;

                    if (tmpObjPos.y < tmpCubePos.y){
                        tmpList.Insert(index, tmpCube);
                        tmpcomlist.Insert(index, tmpCube.tag);
                        break;
                    }
                    index++;
                }
                if (!tmpList.Contains(tmpCube)){
                    if (tmpCube.tag != "action"){
                        tmpList.Add(tmpCube);
                        tmpcomlist.Add(tmpCube.tag);
                    }
                }
            } else {
                // エディットフィールドの右側を次に実行する
                int index = 0;
                foreach (GameObject tmpObj in tmpList2){
                    Vector3 tmpObjPos = tmpObj.transform.position;
                    Vector3 tmpCubePos = tmpCube.transform.position;

                    if (tmpObjPos.y < tmpCubePos.y){
                        tmpList2.Insert(index, tmpCube);
                        tmpcomlist2.Insert(index, tmpCube.tag);
                        break;
                    }
                    index++;
                }
                if (!tmpList2.Contains(tmpCube)){
                    if (tmpCube.tag != "action"){
                        tmpList2.Add(tmpCube);
                        tmpcomlist2.Add(tmpCube.tag);
                    }
                }
            }
        }
        startPanelList = tmpList;
        startPanelList.AddRange(tmpList2);
        startcomlist = tmpcomlist;
        startcomlist.AddRange(tmpcomlist2);
        // foreach (string spl in startcomlist){
        //     Debug.Log(spl);
        // }
        // Debug.Log(ifActionPanelList.Count);
        if (ifActionPanelList.Count != 0){
            ifActionMap.Add("ifPanel"+ ifcount, ifActionPanelList);
            ifcount++;
        }
    }

    public List<string> GetCommndList(){
        SortPanel();
        return startcomlist;
    }

    public static List<int> GetComnum(){
        return comnum;
    }
}
