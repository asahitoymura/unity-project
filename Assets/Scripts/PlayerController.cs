using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour{

    private floorController floarController;

    private AudioSource[] seList;
    private AudioSource ashioto;
    private AudioSource awa;

    public Sprite goalSprite;
    public Sprite throwSprite;

    private const string FRONT = "front";
    private const string RIGHT = "right";
    private const string LEFT = "left";
    private const string BACK = "back";
    private string meDirection;//プレイヤーの向き

    private GameObject player;
    private GameObject miss;
    private GameObject goal;
    private GameObject talk;
    Vector3 pos;
    Vector3 defaultPos;
    Vector3 defaultRot;
    private int count;

    private List<string> moveList;
    List<GameObject> moveObjList;
    private bool checkpointflag;
    private int commandcount = 1;

    public Material rightmaterial;
    public Material leftmaterial;
    public Material frontmaterial;
    public Material backmaterial;
    public string nextScenename;
    private CSVWriter CSV;  //CSVWriterクラスを読み込み

    private panelController panelController;

    private bool buttonFreezFlg;

    private bool ifCheckFlg;

    private PauseScript pauseScript;
    private bool moveFlg;
    private int panelIndex;

    private List<GameObject> panelList;
    IDictionary<string, List<GameObject>> ifActionMap;
    private float moveCount;
    private const float maxMove = 2.0F;
    private const float maxMove2 = 1.5F;
    private List<GameObject> whilePanelList;
    private int whileCount;
    private int whileIndex;
    private bool stage3check1;
    private bool stage3check2;
    private bool stage3check3;
    private string stage;
    private GameObject goalobj;

    // Use this for initialization
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
    }

    // Update is called once per frame
    void Update(){
        if (moveFlg){
            //   IDictionary<string, List<GameObject>> ifActionMap = panelController.GetIfActionMap();
            if (ifActionMap != null && ifCheckFlg){
                List<GameObject> ifActionObjList = ifActionMap["ifPanel"];
                MoveCommand(ifActionObjList[0]);
            }else if (panelList.Count > panelIndex){
                GameObject tmpObj = panelList[panelIndex];
                MoveCommand(tmpObj);
            } else {
                moveFlg = false;
                buttonFreezFlg = false;
            }
        }
    }

    public void MoveStart(){
        if (buttonFreezFlg){
            return;
        }
        float timeCount = 1F;
        foreach (string moveCommand in moveList){
            CSV.WriteCSV(moveCommand + "," + commandcount);
            commandcount++;
            timeCount++;
        }
        StatusReset();
    }

    private void Move(){
//        ashioto.Play();
        Quaternion q = this.transform.rotation;
        float moveZ = q.eulerAngles.z;
        if (count > 0 && moveZ == 0f){
            pos = transform.position;
            pos.y += 0.05F;
            transform.position = pos;
            moveCount += 0.05F;
            if(moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
                count++;
            }
        } else if (count <= 0 && moveZ == 0f){
            pos = transform.position;
            pos.y += 0.05F;
            transform.position = pos;
            moveCount += 0.05F;

            if (moveCount > maxMove2){
                panelIndex++;
                moveCount = 0F;
                count++;
            }
        } else if (count > 0 && moveZ == 90f || moveZ == -270f){
            pos = transform.position;
            pos.x += 0.05F;
            transform.position = pos;
            moveCount += 0.05F;

            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
                count++;
            }
        } else if (count > 0 && moveZ == 180f || moveZ == -180f){
            pos = transform.position;
            pos.y -= 0.05F;
            transform.position = pos;
            moveCount += 0.05F;

            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
                count++;
            }
        } else if (count > 0 && moveZ == 270f || moveZ == -90f){
            pos = transform.position;
            pos.x -= 0.05F;
            transform.position = pos;
            moveCount += 0.05F;

            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
                count++;
            }
        }
    }

    private void Back(){

        Quaternion q = this.transform.rotation;
        float backZ = q.eulerAngles.z;
        if (count > 0 && backZ == 0f){
            pos = transform.position;
            pos.y -= 0.1F;
            transform.position = pos;
            moveCount += 0.1F;
            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
                count++;
            }
        } else if (count <= 0 && backZ == 0f){
            pos = transform.position;
            pos.y -= 0.1F;
            transform.position = pos;
            moveCount += 0.1F;

            if (moveCount > maxMove2){
                panelIndex++;
                moveCount = 0F;
                count++;
            }
        } else if (count > 0 && backZ == 90f || backZ == -270f){
            pos = transform.position;
            pos.x -= 0.1F;
            transform.position = pos;
            moveCount += 0.1F;

            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
                count++;
            }
        } else if (count > 0 && backZ == 180f || backZ == -180f){
            pos = transform.position;
            pos.y += 0.1F;
            transform.position = pos;
            moveCount += 0.1F;

            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
                count++;
            }
        } else if (count > 0 && backZ == 270f || backZ == -90f){
            pos = transform.position;
            pos.x += 0.1F;
            transform.position = pos;
            moveCount += 0.1F;

            if (moveCount > maxMove){
                panelIndex++;
                moveCount = 0F;
                count++;
            }
        }
    }

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

    private void Action(){
        if (!checkpointflag){
            pauseScript.dispCheckPoint();
            checkpointflag = true;
            ifCheckFlg = false;
        }
    }

    public void SceneReset(){
        if (buttonFreezFlg){
            return;
        }
        SceneManager.LoadScene(stage);
    }

    public void StatusReset(){
        moveList.Clear();
        player.transform.position = defaultPos;
        player.transform.localEulerAngles = defaultRot;
        count = 0;
        checkpointflag = false;
        meDirection = BACK;
        this.GetComponent<Renderer>().material = backmaterial;
        Destroy(miss);
        Destroy(goal);

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
            ifCheckFlg = true;

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

        if(stage3check1 && stage3check2 && stage3check3){
            checkpointflag = true;
            goalobj = (GameObject)Resources.Load("goal");
            Vector3 goalpos = new Vector3(-9.8f, -6.5f, -2f);
            goalobj = Instantiate(goalobj, goalpos, Quaternion.identity);
        }

        /*
        if (other.gameObject.tag == "wall"){
            //awa.PlayOneShot(awa.clip);
            miss = (GameObject)Resources.Load("miss");
            Vector3 postion = new Vector3(x: -5.5F, y: 0F, z: -5F);
            miss = Instantiate(miss, postion, Quaternion.identity);
         }
         */
        if (other.tag == "floar"){
            floarController.ChangeMaterial(other.transform.parent.gameObject);
        }
        //        if (other.gameObject.tag == "wall")
        //        {
        //            awa.PlayOneShot(awa.clip);
        //            miss = (GameObject)Resources.Load("miss");
        //            Vector3 postion = new Vector3(x: -5.5F, y: 0F, z: -5F);
        //            miss = Instantiate(miss, postion, Quaternion.identity);
        //        }

        //        if (other.gameObject.tag == "checkpoint")
        //        {
        //            checkpointflag = true;
        //
        //            Debug.Log("talkを表示するよ");
        //            talk = (GameObject)Resources.Load("talk");
        //            Vector3 postion = new Vector3(x: -5.5F, y: 0F, z: -5F);
        //            talk = Instantiate(talk, postion, Quaternion.identity);
        //            Debug.Log("talkを表示したよ");
        //        }
        if (other.gameObject.tag == "dog"){
            //awa.PlayOneShot(awa.clip);
            miss = (GameObject)Resources.Load("dogAtack");
            Vector3 postion = new Vector3(x: -5.5F, y: 0F, z: -5F);
            miss = Instantiate(miss, postion, Quaternion.identity);
        }

        if (other.gameObject.tag == "dog2"){
            miss = (GameObject)Resources.Load("accident2");
            Vector3 pos = new Vector3(-4.32f, -0.68f, -6.594f);
            miss = Instantiate(miss, pos, Quaternion.identity);
        }
    }

    IEnumerator WaiTtime(int num){
        yield return new WaitForSeconds(num);
        commandcount = 1;
        SceneManager.LoadScene(nextScenename);
    }

    void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "checkpoint"){
            ifCheckFlg = false;
        }
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
}