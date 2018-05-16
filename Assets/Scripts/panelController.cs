using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelController : MonoBehaviour{

    private List<GameObject> panelList;
    private List<GameObject> startPanelList;
    private List<GameObject> ifPanelList;
    private List<GameObject> ifActionPanelList;
    private IDictionary<string, List<GameObject>> ifActionMap;

    // Use this for initialization
    void Start(){
        panelList = new List<GameObject>();
        startPanelList = new List<GameObject>();
        ifPanelList = new List<GameObject>();
        ifActionPanelList = new List<GameObject>();
        ifActionMap = new Dictionary<string, List<GameObject>>();
    }

    // Update is called once per frame
    void Update(){

    }


    public void AddPanel(GameObject obj){
        panelList.Add(obj);
    }
    public void AddIfPanel(GameObject obj){
        ifPanelList.Add(obj);
    }
    public void AddIfActionPanel(GameObject obj){
        ifActionPanelList.Add(obj);
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

        foreach(GameObject tmp in ifActionPanelList){
            Destroy(tmp);
        }
        ifActionPanelList.Clear();
    }

    // パネルの並び替え
    private void SortPanel(){
        List<GameObject> tmpList = new List<GameObject>();
        List<GameObject> tmpList2 = new List<GameObject>();
        foreach (GameObject tmpCube in panelList){
            if (tmpCube.tag == "action"){
                AddIfActionPanel(tmpCube);
                continue;
            }
            // エディットフィールドの左側を最初に実行する
            if (tmpCube.transform.position.x < 7.0F){
                int index = 0;
                foreach (GameObject tmpObj in tmpList){
                    Vector3 tmpObjPos = tmpObj.transform.position;
                    Vector3 tmpCubePos = tmpCube.transform.position;
                    if (tmpObjPos.y < tmpCubePos.y){
                        tmpList.Insert(index, tmpCube);
                        break;
                    }
                    index++;
                }
                if (!tmpList.Contains(tmpCube)){
                    if (tmpCube.tag != "action" && tmpCube.tag != "ifPanel"){
                        tmpList.Add(tmpCube);
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
                        break;
                    }
                    index++;
                }
                if (!tmpList2.Contains(tmpCube)){
                    if (tmpCube.tag != "action" && tmpCube.tag != "ifPanel"){
                        tmpList2.Add(tmpCube);
                    }
                }
            }
        }
        startPanelList = tmpList;
        startPanelList.AddRange(tmpList2);
        if (ifActionPanelList.Count != 0){
            ifActionMap.Add("ifPanel", ifActionPanelList);
        }
    }
}
