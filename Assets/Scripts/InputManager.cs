using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class InputManager : MonoBehaviour{

    InputField inputField;
    public string nextscenename;
    CSVWriter2 CSV;
    public static string inputValue;
    public string filepathname;
    private SubmitButton sb;
    /// <summary>
    /// Startメソッド
    /// InputFieldコンポーネントの取得および初期化メソッドの実行
    /// </summary>
    void Start(){
        inputField = GetComponent<InputField>();
        InitInputField();
        CSV = GameObject.Find("CSVWriter").GetComponent<CSVWriter2>();
    }

    /// <summary>
    /// Log出力用メソッド
    /// 入力値を取得してLogに出力し、初期化
    /// </summary>

    public void InputLogger(){
        inputValue = inputField.text;
        //Debug.Log(inputValue);
        //sb.SetName(inputValue);
        StreamWriter sW;
        FileInfo fi;
        fi = new FileInfo(Application.dataPath + "/"+ filepathname+"/" + inputValue + ".csv");
        //CSV.WriteCSV(inputValue + "," + 0);

        sW = fi.AppendText();
        sW.WriteLine(inputValue + "," + 0);
        sW.Flush();
        sW.Close();
        // sW.Flush();
        // sW.Close();
        //InitInputField();
        //SceneManager.LoadScene(nextscenename);
    }

    /// <summary>
    /// InputFieldの初期化用メソッド
    /// 入力値をリセットして、フィールドにフォーカスする
    /// </summary>

    void InitInputField(){
        // 値をリセット
        inputField.text = "";
        // フォーカス
        inputField.ActivateInputField();
    }

    public static string getInputValue(){
        return inputValue;
    }
}