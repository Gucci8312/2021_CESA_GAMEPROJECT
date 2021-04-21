﻿using System.Collections;
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

    void Start()
    {
        Player = GameObject.Find("Player");                                        //全てのメビウス取得
        CountScript = GameObject.Find("CheckPointCount").GetComponent<CheckPointCount>();
        CheackPointObj = GameObject.FindGameObjectsWithTag("CheackPointJudge");

        for (int i = 0; i < CheackPointObj.Length; i++)
        {
            CheackPointObj[i] = GameObject.Find("CheckpointJudge (" + i + ")");                                        //全てのメビウス取得
        }
    }

    void Update()
    {
        checkpointobjects = GameObject.FindGameObjectsWithTag("CheckPoint");    //CheckPointのタグを鉾に入れる
        print(checkpointobjects.Length);

        if (CountScript.CheckPointNum==3)  //チェックポイント0になったら
        {
            Debug.Log(" ゲームクリア");

            GameClear.active = true;
            Time.timeScale = 0.0f;
        }

        if (GameClear.active == true)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SceneManager.LoadScene("TittleScene");
            }
        }

        if (Player.GetComponent<PlayerMove>().GetCollisionState())  //チェックポイント0になったら
        {
            Debug.Log(" ゲームオーバー");

            GameOver.active = true;
            Time.timeScale = 0.0f;
        }

        if (GameOver.active == true)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SceneManager.LoadScene("TittleScene");
            }
        }

        for (int idx = 0; idx < CountScript.CheckPointNum; idx++)
        {
            JudgeUI[idx].SetActive(true);
        }
    }
}