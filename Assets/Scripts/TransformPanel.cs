using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// パネルの変形を行うクラス
/// 
/// 繰り返しパネル、条件パネルにパネルを
/// 重ねたと、外したとき下括弧の位置を変更する。
/// 
/// </summary>

public class TransformPanel : MonoBehaviour {

    List<GameObject> downBlockList;

	// Use this for initialization
	void Start () {
        downBlockList = new List<GameObject>();
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag == "downBrack")
            {
                downBlockList.Add(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update () {
	}

    // 下括弧の位置をさげる。
    public void TransfaPanelDown(float y)
    {
        foreach (GameObject tmpObj in downBlockList)
        {
            tmpObj.transform.position = new Vector3(tmpObj.transform.position.x, tmpObj.transform.position.y + y*2, tmpObj.transform.position.z);
        }
    }

    // 下括弧の位置を上げる。
    public void TransfaPanelUp(float y)
    {
        foreach (GameObject tmpObj in downBlockList)
        {
            tmpObj.transform.position = new Vector3(tmpObj.transform.position.x, tmpObj.transform.position.y - y * 2, tmpObj.transform.position.z);
        }
    }

}
