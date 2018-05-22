using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class button : MonoBehaviour{

    public GameObject Cube;
    private panelController panelController;
    private Vector3 screenPoint;
    private Vector3 offset;
    private bool click;
    private bool contactFlg;

    RectTransform rectTransform;
    InputField inputField;

    private List<GameObject> whilePanelList;

    // Use this for initialization
    void Start(){
        click = false;
        GameObject edit = GameObject.Find("edit");
        panelController = GameObject.Find("edit").GetComponent<panelController>();
        contactFlg = false;
        whilePanelList = new List<GameObject>();
        if(transform.tag == "whilePanel"){
            GameObject child = transform.Find("Canvas").gameObject;
            Transform whileCount = child.transform.Find("InputField");
            rectTransform = whileCount.GetComponent<RectTransform>();
            inputField = whileCount.GetComponent<InputField>();
            Vector3 initPosition = new Vector3(transform.position.x - 0.70F, transform.position.y + 0.85f, transform.position.z);
            rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, initPosition);
        }
    }

    public int GetWhileCount(){
        if (inputField.text.Equals("")){
            inputField.text = "0";
        }
        return Int32.Parse(inputField.text);
    }

    public void AddWhilePanel(GameObject obj){
        whilePanelList.Add(obj);
    }

    public List<GameObject> GetWhilePanelList(){
        if (whilePanelList.Count > 0){
            return whilePanelList;
        } else {
            return null;
        }
    }

    public void ResetWhilePanelList(){
        whilePanelList.Clear();
    }

    // Update is called once per frame
    void Update(){
        /*
        Cube.transform.position = (new Vector3(Mathf.Clamp(Cube.transform.position.x, 3F, 9F),
            Mathf.Clamp(Cube.transform.position.y, -6.5F, 6F),
            Cube.transform.position.z));
            */
    }
    void OnMouseDown(){
        // マウスカーソルは、スクリーン座標なので、
        // 対象のオブジェクトもスクリーン座標に変換してから計算する。

        // このオブジェクトの位置(transform.position)をスクリーン座標に変換。
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        // ワールド座標上の、マウスカーソルと、対象の位置の差分。
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        contactFlg = false;

        if (transform.parent != null){
            if(transform.parent.tag == "whilePanel"){
                List<GameObject> tmpList = transform.parent.GetComponent<button>().GetWhilePanelList();
                if (tmpList.Find(x => x == transform.gameObject)){
                    tmpList.Remove(transform.gameObject);
                }
            }
            transform.parent = null;
        }

        if (!click){
            Instantiate(Cube, transform.position, transform.rotation);
        }
    }

    void OnMouseUp(){
        if (!click){
            panelController.AddPanel(Cube);
            click = true;
        }

        Vector3 rayDirectionUp = new Vector3(0F, 1F, 0F);
        Ray upRay = new Ray(transform.position, rayDirectionUp);
        RaycastHit upHit;

        Vector3 rayDirectionDown = new Vector3(0F, -1F, 0F);
        Ray downRay = new Ray(transform.position, rayDirectionDown);
        RaycastHit downHit;

        Vector3 rayDirectionZ = new Vector3(0F, 0F, 1F);
        Ray zRay = new Ray(transform.position, rayDirectionZ);
        RaycastHit zHit;

        if (transform.tag == "whilePanel" || transform.tag == "ifPanel"){
            if (Physics.Raycast(zRay, out zHit, -1.0F)){
                GameObject hitObj = zHit.collider.gameObject;
                if (hitObj.tag != "ifPanel" && hitObj.tag != "whilePanel"){
                    hitObj.transform.parent = transform;
                    if(transform.tag == "whilePanel"){
                        transform.GetComponent<button>().AddWhilePanel(hitObj.transform.gameObject);
                        panelController.DeletePanelList(hitObj.transform.gameObject);
                    }
                }
            }
            return;
        }

        if(Physics.Raycast(zRay,out zHit, 1.0F)){
            GameObject hitObj = zHit.collider.gameObject;
            if(hitObj.tag == "ifPanel"){
                transform.parent = hitObj.transform;
            }
            if (hitObj.tag == "whilePanel"){
                transform.parent = hitObj.transform;
                hitObj.GetComponent<button>().AddWhilePanel(transform.gameObject);
                panelController.DeletePanelList(transform.gameObject);
            }
        }


        if (Physics.Raycast(upRay, out upHit, 0.7F)){
            // 上方向にパネルがある場合、パネルの下にくっつく。
            GameObject hitObj = upHit.collider.gameObject;
            // ゴミ箱の場合何もしない。
            if (hitObj.tag == "dustbox"){
                return;
            }
            // 見つけたパネルの親が自分の場合、何もしない
            if (hitObj.transform.parent == null || !hitObj.transform.parent.gameObject.Equals(transform.gameObject)){
                Vector3 hitOnjPos = hitObj.transform.position;
                Vector3 hitObjScal = hitObj.transform.localScale;

                if (hitObj.transform.parent == null){
                    Vector3 jointPos = new Vector3(hitOnjPos.x, hitOnjPos.y + hitObjScal.y, hitOnjPos.z);

                    transform.position = jointPos;

                } else {
                    Vector3 jointPos = new Vector3(hitOnjPos.x, hitOnjPos.y - hitObjScal.y, hitOnjPos.z);

                    transform.position = jointPos;
                }
                contactFlg = true;

                transform.parent = hitObj.transform;
            }

        } else if (transform.childCount == 0 && Physics.Raycast(downRay, out downHit, 0.7F)){
            // 下方向にパネルがある場合、パネルの上にくっつく。
            GameObject hitObj = downHit.collider.gameObject;
            // ゴミ箱の場合何もしない。
            if (hitObj.tag == "dustbox"){
                return;
            }
            // 見つけたパネルの親が自分の場合、何もしない
            if (hitObj.transform.parent == null || !hitObj.transform.parent.gameObject.Equals(transform.gameObject)){
                Vector3 hitOnjPos = hitObj.transform.position;
                Vector3 hitObjScal = hitObj.transform.localScale;
                Vector3 jointPos = new Vector3(hitOnjPos.x, hitOnjPos.y - hitObjScal.y, hitOnjPos.z);

                transform.position = jointPos;

                contactFlg = true;

                hitObj.transform.parent = transform;
            }
        }
    }

    void OnMouseDrag(){
        Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + this.offset;
        transform.position = currentPosition;

        if (rectTransform != null){
            Vector3 currentPositionText = new Vector3(currentPosition.x - 0.7F, currentPosition.y + 0.85f, currentPosition.z);
            rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, currentPositionText);
        }
    }


    public bool GetContactFlg(){
        return contactFlg;
    }
    public void SetContactFlg(bool flg){
        contactFlg = flg;
    }
}