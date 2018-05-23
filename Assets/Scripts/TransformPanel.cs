using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void TransfaPanel(float y)
    {
        foreach (GameObject tmpObj in downBlockList)
        {
            tmpObj.transform.position = new Vector3(tmpObj.transform.position.x, tmpObj.transform.position.y + y*2, tmpObj.transform.position.z);
        }
    }

}
