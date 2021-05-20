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

    public GameObject[] JudgeUI;
    CheckPointCount CountScript;
    GameObject[] CheackPointObj;

    GameObject ZoomCameraObj;
    CameraZoom _camera;
  //  public GameObject ClearDai;

    void Start()
    {
        Player = GameObject.Find("Player");                                        //全てのメビウス取得
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

    }

    void Update()
    {
        checkpointobjects = GameObject.FindGameObjectsWithTag("CheckPoint");    //CheckPointのタグを鉾に入れる
        print(checkpointobjects.Length);

        //if (CountScript.CheckPointNum== CrearNum)  //チェックポイント0になったら
        if (CountScript.CheckPointNum == JudgeUI.Length)  //チェックポイント0になったら
        {
            Debug.Log(" ゲームクリア");
            //ClearDai.SetActive(true);



            GameClear.active = true;
            //_camera.OnZoom();                 //カメラズーム
            //        Time.timeScale = 0.0f;

            Player.GetComponent<PlayerMove>().ClearOn();//プレイヤーをクリアの動きに切り替え
            if (Player.GetComponent<PlayerMove>().GetStop())//プレイヤーのクリアの動き終わった
            {
                PauseManager.OnPause();
            }

            SoundManager.StopBGM();
        }

        if (GameClear.active == true)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SceneManager.LoadScene("TitleScene");
            }
        }

        if (Player.GetComponent<PlayerMove>().GetCollisionState())  //チェックポイント0になったら
        {
            Debug.Log(" ゲームオーバー");
            SoundManager.PlayBgmName("gameovermusic");

            GameOver.active = true;
            //_camera.OnZoom();                //カメラズーム
            //Time.timeScale = 0.0f;
        }

        if (GameOver.active == true)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SceneManager.LoadScene("TitleScene");
            }
        }

        for (int idx = 0; idx < CountScript.CheckPointNum; idx++)
        {
            JudgeUI[idx].SetActive(true);
        }
    }
}
