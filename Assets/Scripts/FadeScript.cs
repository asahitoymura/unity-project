using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FadeScript : MonoBehaviour {
    float fadeSpeed = 0.2f;
    float red, green, blue, alfa;

    public bool isFadeOut = false;

    Image fadeImage;

    // Use this for initialization
    void Start (){
        fadeImage = GetComponent<Image>();
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
    }
	
	// Update is called once per frame
	void Update () {
        if (isFadeOut){
            StartFadeOut();

        }
    }

    void StartFadeOut(){
        fadeImage.enabled = true;  // a)パネルの表示をオンにする
        alfa += fadeSpeed;         // b)不透明度を徐々にあげる
        SetAlpha();               // c)変更した透明度をパネルに反映する
        if (alfa >= 1){             // d)完全に不透明になったら処理を抜ける
            isFadeOut = false;
        }
    }

    void SetAlpha(){
        fadeImage.color = new Color(red, green, blue, alfa);
    }
}
