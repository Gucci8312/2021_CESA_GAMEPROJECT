using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManeger : MonoBehaviour
{
    private GameObject[] checkpointobjects;     //配列(チェックポイントのデータを収納)
    GameObject Player;

    public GameObject GameClear;
    public GameObject GameOver;

    public bool GameClearFlg;
    public bool GameOverFlg;

    public GameObject[] JudgeUI;
    CheckPointCount CountScript;
    GameObject[] CheackPointObj;

	public GameObject[] Check_Point_Light;

    GameObject ZoomCameraObj;
    CameraZoom _camera;
    public int ThisStageNum;
    int UINum=0;

    //  public GameObject ClearDai;
    private void Awake()
    {
        Player = GameObject.Find("Player");                                        //全てのメビウス取得
    }

    void Start()
    {

        CountScript = GameObject.Find("CheckPointCount").GetComponent<CheckPointCount>();
        CheackPointObj = GameObject.FindGameObjectsWithTag("CheackPointJudge");

        for (int i = 0; i < CheackPointObj.Length; i++)
        {
            CheackPointObj[i] = GameObject.Find("CheckpointJudge (" + i + ")");                                        //全てのメビウス取得
            //Debug.Log("aa");
        }
        //Debug.Log("bb");

        ZoomCameraObj = GameObject.Find("ZoomCameraObj");
        _camera = ZoomCameraObj.GetComponent<CameraZoom>();
        StageControl.SetNowStage(ThisStageNum);

    }

    void Update()
    {
        checkpointobjects = GameObject.FindGameObjectsWithTag("CheckPoint");    //CheckPointのタグを鉾に入れる
        print(checkpointobjects.Length);

        //if (CountScript.CheckPointNum== CrearNum)  //チェックポイント0になったら
        if (CountScript.CheckPointNum == JudgeUI.Length && !GameOverFlg && !GameClearFlg)  //チェックポイント0になったら
        {
            Debug.Log(" ゲームクリア");
            //ClearDai.SetActive(true);
            GameClearFlg = true;

            GameClear.active = true;
            //_camera.OnZoom();                 //カメラズーム
            //        Time.timeScale = 0.0f;
            StageControl.SetOpenFlg(ThisStageNum);
            Player.GetComponent<PlayerMove>().ClearOn();//プレイヤーをクリアの動きに切り替え
            if (Player.GetComponent<PlayerMove>().GetStop())//プレイヤーのクリアの動き終わった
            {
                PauseManager.OnPause();
            }
            SoundManager.StopBGM();
            SoundManager.PlaySeName("clearmusic");
        }

        if (GameClear.active == true)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SceneManager.LoadScene("TitleScene");
            }
        }

        if (Player.GetComponent<PlayerMove>().GetCollisionState() && !GameClearFlg && !GameOverFlg)   // ゲームオーバー時
        {
            Debug.Log(" ゲームオーバー");
            SoundManager.StopBGM();
            SoundManager.PlaySeName("gameovermusic");

            GameOver.active = true;
            //_camera.OnZoom();                //カメラズーム
            //Time.timeScale = 0.0f;
            GameOverFlg = true;

        }

        if (GameOver.active == true)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SceneManager.LoadScene("TitleScene");
            }
        }

        if(UINum!= CountScript.CheckPointNum)//2 0
        {

			
			for (int i = 0; i < Check_Point_Light.Length; i++)
			{
				if (!Check_Point_Light[i].activeSelf)
				{
					Check_Point_Light[i].SetActive(true);
					break;
				}
			}
			// JudgeUI[CountScript.CheckPointNum].SetActive(true);
			Invoke("Create",1.0f);
			UINum++;
            // Create(UINum);
        }

        //for (int idx = 0; idx < CountScript.CheckPointNum; idx++)
        //{
        //    JudgeUI[idx].SetActive(true);
        //}
    }

    void Create()
    {
       // JudgeUI[UINum-1].SetActive(true);
        for (int idx = 0; idx < UINum; idx++)
        {
            JudgeUI[idx].SetActive(true);
        }
    }
}
