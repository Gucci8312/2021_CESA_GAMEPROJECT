using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SCENESELECT//シーン選択用クラス
{
    public GameObject SceneObj;       //選択用オブジェクト
    public string SceneName;          //遷移先のシーン名
}

public class Scene : MonoBehaviour
{
    public SCENESELECT[] SceneSelect = new SCENESELECT[1];

    ////要素数（size）は統一させる、SceneObjと同じ要素数を持つSceneNameが呼ばれる
    //public GameObject[] SceneObj = new GameObject[1];       //選択用オブジェクト
    //public string[] SceneName = new string[1];              //シーン名

    int SelectNum;//選択しているオブジェクトの要素数

    float InputRecast=10.0f; //入力可能までの待機時間
    bool InputFlag;         //入力できるかどうか

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");
        //float StickXPower = Input.GetAxis("StickMoveX");
        //float StickYPower = Input.GetAxis("StickMoveY");

        InputFlagOnOff();

        if (InputFlag)
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)||
                Vertical < -0.5 || Horizontal > 0.5 )
            {
                SelectNum++;
                InputFlag = false;
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A)||
                Horizontal < -0.5 || Vertical > 0.5)
            {
                SelectNum--;
                InputFlag = false;
            }

        }

        if (SelectNum >= SceneSelect.Length) { SelectNum = 0; }
        if (SelectNum < 0) { SelectNum = SceneSelect.Length - 1; }


        if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown("joystick button 1")) //決定（エンターキー、Bボタン）
        {
            if (SceneSelect[SelectNum].SceneName == "Exit")//ゲーム終了を選んだら（名前は仮で用意したもの）
            {
#if UNITY_EDITOR    // EDITOR上のデバッグの時にゲームを終了させる
                UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();     // ゲームを終了させる
#endif
            }
            else//シーンがあるものを選んだら
            {
                //選択したオブジェクトへシーン遷移
                SceneManager.LoadScene(SceneSelect[SelectNum].SceneName);
            }
        }


        //==選択している（要素数の）オブジェクトに変化させたい場合==
        SceneSelect[SelectNum].SceneObj.transform.localScale = new Vector3(2, 2, 0);//大きさを変更

        for(int i = 0; i < SceneSelect.Length; i++)
        {
            if (i != SelectNum)
            {
                SceneSelect[i].SceneObj.transform.localScale = new Vector3(1, 1, 0);
            }
        }
        //==============================================
    }

    void InputFlagOnOff()
    {
        float Vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");
        //float StickXPower = Input.GetAxis("StickMoveX");
        //float StickYPower = Input.GetAxis("StickMoveY");

        if (!InputFlag)
        {
            //入力していたら
            if (Vertical < -0.5 || Vertical > 0.5 ||
                Horizontal < -0.5 || Horizontal > 0.5 ||
                Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A))
            {
                if (InputRecast < 10.0f)
                {
                    InputRecast++;
                }
                else
                {
                    //入力可能にする
                    InputFlag = true;
                    InputRecast = 0.0f;
                }
            }
            else
            {
                InputFlag = true;
                InputRecast = 0.0f;
            }
        }

    }
}
