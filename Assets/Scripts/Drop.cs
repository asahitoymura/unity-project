using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class Drop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{

    // Use this for initialization
    void Start(){

    }

    // Update is called once per frame
    void Update(){

    }

    public void OnBeginDrag(PointerEventData eventData){
    }

    public void OnDrag(PointerEventData eventData){
        GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y, 0.0f);
    }

    public void OnEndDrag(PointerEventData eventData){
    }
}