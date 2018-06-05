using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour{


    // 床操作スクリプト
    private floorController floarController;
    // パネル操作スクリプト
    private panelController panelController;
    // 停止操作スクリプト
    private PauseScript pauseScript;

    // サウンド系変数
    private AudioSource[] seList;
    private AudioSource ashioto;
    private AudioSource awa;

    // イラスト系変数
    public Sprite goalSprite;       // ゴールイラスト格納用
    public Sprite throwSprite;      // 失敗イラスト格納用
    public Sprite checkPointSprite; // 会話イラスト格納用

    // 定数宣言
    private const string FRONT = "front";  // 前方
    private const string RIGHT = "right";  // 右方
    private const string LEFT = "left";    // 左方
    private const string BACK = "back";    // 後方
    private string meDirection;//プレイヤーの現在の向き格納用
    private const float maxMove = 2.0F;    // プレイヤーの移動量の最大値

    // ゲームオブジェクト格納用
    private GameObject player;  // プレイヤー
    private GameObject miss;    // 失敗
    private GameObject goal;    // ゴール
    private GameObject talk;    // 会話
    private GameObject goalobj; // ゴールあたり判定

    // プレイヤーの位置情報格納用
    Vector3 pos;                // プレイヤーの現在位置
    Vector3 defaultPos;         // プレイヤーの初期位置
    Vector3 defaultRot;         // プレイヤーの初期向き
    
    private static List<string> moveList;
    List<GameObject> moveObjList;
    private bool checkpointflag;
    private int commandcount = 1;

    // マテリアル格納用
    public Material rightmaterial;  // プレイヤーの右向きマテリアル
    public Material leftmaterial;   // プレイヤーの左向きマテリアル
    public Material frontmaterial;  // プレイヤーの前向きマテリアル
    public Material backmaterial;   // プレイヤーの後向きマテリアル

    // 次のシーン格納用（unity上のインスペクターで次のシーン名を指定する)
    public string nextScenename;
    private CSVWriter CSV;  //CSVWriterクラスを読み込み

    // プレイヤーが動作中にボタンが押せなくなるようにするフラグ
    private bool buttonFreezFlg;

    // 並べたパネルにIFパネルがあるかチェックするフラグ
    private bool ifCheckFlg;

    // プレイヤーの動きを制御するフラグ
    private bool moveFlg;
    // 行動パネルを保持したリストのインデックス
    private int panelIndex;
    // 行動パネルを一覧保持するリスト
    private List<GameObject> panelList;
    // IFパネルに置かれたアクションパネルを保持するマップ
    IDictionary<string, List<GameObject>> ifActionMap;
    // プレイヤーの動く量を保持する変数
    private float moveCount;
    // 繰り返しパネルの一覧保持するリスト
    private List<GameObject> whilePanelList;
    // 繰り返しパネルに設定された繰り返す回数を保持する変数
    private int whileCount;
    // 繰り返しパネルのリストのインデックス
    private int whileIndex;

    // ステージ3用チェックポイントの状態格納変数
    private bool stage3check1;
    private bool stage3check2;
    private bool stage3check3;

    // 現在のステージ名を格納する変数
    private string stage;

    // ゴール/スタートの切り替え用変数
    private bool switching;

    /// <summary>
    /// アプリ起動時に一回だけ実行される関数
    /// 基本的には変数の初期化処理を行う
    /// </summary>
    void Start(){
        player = GameObject.Find("Player");
        pos = GetComponent<Transform>().position;
        defaultPos = player.transform.position;
        defaultRot = player.transform.localEulerAngles;
        checkpointflag = false;
        meDirection = BACK;
        moveObjList = new List<GameObject>();
        moveList = new List<string>();
        CSV = GameObject.Find("CSVWriter").GetComponent<CSVWriter>();  //GameObject CSVWriterを探し出してアタッチする
        panelController = GameObject.Find("edit").GetComponent<panelController>();
        floarController = GameObject.Find("floar").GetComponent<floorController>();
        seList = GetComponents<AudioSource>();
        //ashioto = seList[0];
        //awa = seList[1];
        ifCheckFlg = false;
        pauseScript = player.GetComponent<PauseScript>();
        moveFlg = false;
        panelList = new List<GameObject>();
        whilePanelList = new List<GameObject>();
        ifActionMap = null;
        panelIndex = 0;
        moveCount = 0F;
        whileCount = 0;
        whileIndex = 0;
        stage = SceneManager.GetActiveScene().name;
        CSV.WriteCSV(stage + "," + 0);
        switching = false;
        
    }

    /// <summary>
    /// アプリ実行中、常時実行される関数
    /// リアルタイムで実行させたい処理を記述する
    /// </summary>
    void Update(){

        // スタートボタンが押されたときに実行する処理
        if (moveFlg){
            // スタート/リセットボタンの押下付加状態に変更
            buttonFreezFlg = true;
            // IFパネルが置かれていた場合
            if (ifActionMap != null && ifCheckFlg){
                List<GameObject> ifActionObjList = ifActionMap["ifPanel"];
                MoveCommand(ifActionObjList[0]);
            }else if (panelList.Count > panelIndex){
                // IFパネル以外でパネルの数が実行した数より多い場合
                GameObject tmpObj = panelList[panelIndex];
                MoveCommand(tmpObj);
            } else {
                // 上記以外
                moveFlg = false;  // プレイヤーの移動処理停止
                buttonFreezFlg = false;  // スタート/リセットボタンの押下可能状態に変更
            }
        } else {
            // スタート/リセットボタンの押下可能状態に変更
            buttonFreezFlg = false;
        }
    }

    /// <summary>
    /// スタートボタンが押されたときの処理
    /// 　　ステージの状態を初期化
    /// 　　CSVへの書き出し準備
    /// </summary>
    public void MoveStart(){
        // ボタンが押下不可状態なら処理終了
        if (buttonFreezFlg){
            return;
        }

        // ステージの初期化処理
        StatusReset();

        // パネルリストからオブジェクトのタグを取得
        for (int i = 0; i < panelList.Count; i++){
            moveList.Add(panelList[i].tag);
            if(moveList[i] == "ifPanel"){
                moveList.Add("action");
            }
        }

        // CSV書き出し準備
        float timeCount = 1F;
        foreach (string moveCommand in moveList){
            CSV.WriteCSV(moveCommand + "," + commandcount);
            commandcount++;
            timeCount++;
        }
    }


    /// <summary>
    /// まえへすすむパネルの処理
    /// 　　プレイヤーが現在向いている方向へ前進する
    /// </summary>
    private void Move(){
//      ashioto.Play();  // 効果音

        Quaternion q = this.transform.rotation;
        float moveZ = q.eulerAngles.z;
        if (moveZ == 0f){
            pos = transform.position;
            pos.y += 0.05F;
            transform.position = pos;
            moveCount += 0.05F;
            if(moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
            }
            //Debug.Log("a");
        } else if (moveZ == 90f || moveZ == -270f){
            pos = transform.position;
            pos.x += 0.05F;
            transform.position = pos;
            moveCount += 0.05F;

            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
            }
            //Debug.Log("a");
        } else if (moveZ == 180f || moveZ == -180f){
            pos = transform.position;
            pos.y -= 0.05F;
            transform.position = pos;
            moveCount += 0.05F;

            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
            }
            //Debug.Log("a");
        } else if (moveZ == 270f || moveZ == -90f){
            pos = transform.position;
            pos.x -= 0.05F;
            transform.position = pos;
            moveCount += 0.05F;

            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
            }
        }
    }


    /// <summary>
    /// うしろへすすむパネルの処理
    /// 　　プレイヤーが現在向いてる方向と逆方向へ進む
    /// </summary>
    private void Back(){

        Quaternion q = this.transform.rotation;
        float backZ = q.eulerAngles.z;
        if (backZ == 0f){
            pos = transform.position;
            pos.y -= 0.1F;
            transform.position = pos;
            moveCount += 0.1F;
            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
            }
        } else if (backZ == 90f || backZ == -270f){
            pos = transform.position;
            pos.x -= 0.1F;
            transform.position = pos;
            moveCount += 0.1F;

            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
            }
        } else if (backZ == 180f || backZ == -180f){
            pos = transform.position;
            pos.y += 0.1F;
            transform.position = pos;
            moveCount += 0.1F;

            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
            }
        } else if (backZ == 270f || backZ == -90f){
            pos = transform.position;
            pos.x += 0.1F;
            transform.position = pos;
            moveCount += 0.1F;

            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
            }
        }
    }

    /// <summary>
    /// みぎへむくパネルの処理
    /// 　　プレイヤーの向きを右に90度回す
    /// </summary>
    private void Right(){
        moveCount += 0.1F;
        if (moveCount > maxMove){
            moveCount = 0F;
            transform.Rotate(new Vector3(0, 0, 90));
            panelIndex++;
            switch (meDirection){
                case FRONT:
                    this.GetComponent<Renderer>().material = leftmaterial;
                    meDirection = LEFT;
                    break;
                case RIGHT:
                    this.GetComponent<Renderer>().material = frontmaterial;
                    meDirection = FRONT;
                    break;
                case LEFT:
                    this.GetComponent<Renderer>().material = backmaterial;
                    meDirection = BACK;
                    break;
                case BACK:
                    this.GetComponent<Renderer>().material = rightmaterial;
                    meDirection = RIGHT;
                    break;
            }
        }
    }

    /// <summary>
    /// ひだりへむくパネルの処理
    /// 　　プレイヤーの向きを左に90度回す
    /// </summary>
    private void Left(){
        moveCount += 0.1F;

        if (moveCount > maxMove){
            panelIndex++;
            moveCount = 0F;
            transform.Rotate(new Vector3(0, 0, -90));
            switch (meDirection){
                case FRONT:
                    this.GetComponent<Renderer>().material = rightmaterial;
                    meDirection = RIGHT;
                    break;
                case RIGHT:
                    this.GetComponent<Renderer>().material = backmaterial;
                    meDirection = BACK;
                    break;
                case LEFT:
                    this.GetComponent<Renderer>().material = frontmaterial;
                    meDirection = FRONT;
                    break;
                case BACK:
                    this.GetComponent<Renderer>().material = leftmaterial;
                    meDirection = LEFT;
                    break;
            }
        }
    }

    /// <summary>
    /// パネルの種類で実行する関数を呼び分ける処理
    /// </summary>
    /// <param name="panelObj">行動パネル</param>
    private void MoveCommand(GameObject panelObj){
        string moveCommand = panelObj.tag;
        switch (moveCommand){
            case "moveOn":
                Move();
                break;

            case "turnRight":
                Right();
                break;

            case "turnLeft":
                Left();
                break;

            case "backOn":
                Back();
                break;

            case "action":
                Action();
                break;

            case "whilePanel":
                WhileAction(panelObj);
                break;

            default:
                panelIndex++;
                break;
        }

    }

    /// <summary>
    /// アクションパネルの処理
    /// 　　
    /// </summary>
    private void Action(){
        if (!checkpointflag){
            pauseScript.dispCheckPoint(checkPointSprite);
            //moveList.Add(GameObject.Find("speak").tag);
            // for(int i = 0; i < moveList.Count; i++){
            //     if(moveList[i] == "ifPanel"){
            //         moveList.Insert(i,"action");
            //     }
            //
            // }
            // CSV.WriteCSV("action," + commandcount );
            checkpointflag = true;
            ifCheckFlg = false;
            commandcount++;
        }
    }


    /// <summary>
    /// リセットボタンの処理
    /// 　　ボタンが押されたら新しくシーンを読み込む
    /// </summary>
    public void SceneReset(){
        if (buttonFreezFlg){
            return;
        }
        SceneManager.LoadScene(stage);
    }


    /// <summary>
    /// ステージリセット処理
    /// 　　スタートボタンが押されたときに呼び出される
    /// </summary>
    public void StatusReset(){
        moveList.Clear();
        player.transform.position = defaultPos;
        player.transform.localEulerAngles = defaultRot;
        checkpointflag = false;
        meDirection = BACK;
        this.GetComponent<Renderer>().material = backmaterial;
        if(stage == "stage3"){
            Destroy(goalobj);
        }
        Destroy(miss);
        Destroy(goal);
        switching = false;
        buttonFreezFlg = true;

        moveFlg = true;
        panelIndex = 0;
        panelList = panelController.GetPanelList();
        ifActionMap = panelController.GetIfActionMap();
        
        floarController.ResetMaterial();
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "goal"){
                if (checkpointflag){
                //Debug.Log(moveList.Count);
                //Debug.Log(moveObjList.Count);
                /*
                goal = (GameObject)Resources.Load("clear");
                Vector3 postion = new Vector3(x: -5.5F, y: 0F, z: -5F);
                goal = Instantiate(goal, postion, Quaternion.identity);
                */
                pauseScript.DispGoal(goalSprite);
                StartCoroutine(WaiTtime(2));
            } else {
                pauseScript.DispGoal(throwSprite);
            }
        }

        if (other.gameObject.tag == "checkpoint" && !checkpointflag && stage == "stage1" ) {
            checkpointflag = true;
            ifCheckFlg = true;
            pauseScript.dispCheckPoint(checkPointSprite);
        } else if (other.gameObject.tag == "checkpoint" && stage == "stage2") {
            checkpointflag = true;

        } else if (other.gameObject.tag == "checkpoint1" && !checkpointflag && stage == "stage3") {
            stage3check1 = true;
            Debug.Log("checkpoint1 : true");

        } else if (other.gameObject.tag == "checkpoint2" && !checkpointflag && stage == "stage3") {
            stage3check2 = true;
            Debug.Log("checkpoint2 : true");

        } else if (other.gameObject.tag == "checkpoint3" && !checkpointflag && stage == "stage3") {
            stage3check3 = true;
            Debug.Log("checkpoint3 : true");

        }

        if (stage3check1 && stage3check2 && stage3check3) {
            checkpointflag = true;
        }

        //stg3 スタート地点/ゴール地点の切り替え
        if(other.gameObject.tag == "switching" && switching == false) { 
            goalobj = (GameObject)Resources.Load("goal");
            Vector3 goalpos = new Vector3(-9.8f, -6.5f, -2f);
            goalobj = Instantiate(goalobj, goalpos, Quaternion.identity);
            goalobj.name = "goal";
            switching = true;
        }

        if (other.tag == "floar"){
            floarController.ChangeMaterial(other.transform.parent.gameObject);
        }
        if (other.gameObject.tag == "dog"){
            //awa.PlayOneShot(awa.clip);
            miss = (GameObject)Resources.Load("dogAtack");
            Vector3 postion = new Vector3(x: 0F, y: 0.5F, z: -5F);
            miss = Instantiate(miss, postion, Quaternion.identity);
        }

        if (other.gameObject.tag == "dog2"){
            miss = (GameObject)Resources.Load("accident2");
            Vector3 pos = new Vector3(0f, 0.5f, -6.594f);
            miss = Instantiate(miss, pos, Quaternion.identity);
        }
    }

    void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "checkpoint"){
            ifCheckFlg = false;
        }
        // if(other.gameObject.tag == "switching") { 
        //     goalobj = (GameObject)Resources.Load("goal");
        //     Vector3 goalpos = new Vector3(-9.8f, -6.5f, -2f);
        //     goalobj = Instantiate(goalobj, goalpos, Quaternion.identity);
        //     goalobj.SetActive(true);
        // }
    }

    IEnumerator WaiTtime(int num)
    {
        yield return new WaitForSeconds(num);
        commandcount = 1;
        SceneManager.LoadScene(nextScenename);
    }

    public void SetIfCheckFlg(){
        ifCheckFlg = false;
    }

    private void WhileAction(GameObject panelObj){
        button whileButtonCs = panelObj.GetComponent<button>();
        List<GameObject> whilePanelList = whileButtonCs.GetWhilePanelList();
        if (whilePanelList == null){
            panelIndex++;
            return;
        }
        int whileCountMax = whileButtonCs.GetWhileCount();

        if(whileCountMax > whileCount){
            if (whilePanelList.Count > whileIndex){
                GameObject whilePanel = whilePanelList[whileIndex];
                MoveCommand(whilePanel);
                if (moveCount == 0){
                    whileIndex++;
                    panelIndex--;
                }
            } else {
                whileIndex = 0;
                whileCount++;
            }
        } else {
            whileCount = 0;
            panelIndex++;
        }
    }

    public void ChangeMoveFlg(){
        moveFlg = !moveFlg;
    }

    public static List<string> GetMoveList(){
        return moveList;
    }
}