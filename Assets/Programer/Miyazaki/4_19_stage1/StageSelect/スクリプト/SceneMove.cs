// @file   SceneMove.cs
// @brief  シーンセレクトのマネージャークラスの定義
// @author T,Cho & M.Miyazaki & H.Wakabayashi
// @date   2021/05/?? 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @name   SceneMove
// @brief  シーンセレクト時のマネージャークラス
public class SceneMove : MonoBehaviour
{
    public GameObject[] stageNum;
    const int LIGHT_OFF = 1;
    const int LIGHT_ON = 10;

    public StageSelectCamera stageselectcam;

    bool Camera = false;
    bool Camera1 = false;
    [SerializeField] GameObject[] stage_picture;
    //public Material Stage1_color;
    //public Material Stage2_color;
    //public Material Stage3_color;
    //public Material Stage4_color;


    float LightPower;

    //StageSelect stage_select;

    public int Select_Scene;
    // Start is called before the first frame update
    void Start()
    {
        Select_Scene = 1;
        //	stage_select = GetComponent<StageSelect>();
        //LightPower = Stage1.GetComponent<Light>().intensity;
        //LightPower = Stage2.GetComponent<Light>().intensity;
        //LightPower = Stage3.GetComponent<Light>().intensity;
        //LightPower = Stage4.GetComponent<Light>().intensity;
        //Stage1 = GameObject.Find("Stage1");
        //Stage2 = GameObject.Find("Stage2");
        //Stage3 = GameObject.Find("Stage3");
        //Stage4 = GameObject.Find("Stage4");
        for (int i = 0; i < stage_picture.Length; i++)
        {
            stage_picture[i].SetActive(false);
        }
        //stage4_picture.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Select_Scene != 20)
            {
                Select_Scene++;
            }
            if (Select_Scene == 20)
            {
                Camera = false;
            }
            if (Select_Scene % 4 == 1)
            {
                Camera = true;
            }

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Select_Scene != 0)
            {
                Select_Scene--;

            }
            if (Select_Scene == 0)
            {
                Select_Scene = 1;
                Camera1 = false;
            }
            if (Select_Scene % 4 == 0)
            {
                Camera1 = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && Camera == true)
        {
            stageselectcam.OnPlus();
            Camera = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && Camera1 == true)
        {
            stageselectcam.OnMinus();
            Camera1 = false;
        }

        AllStageLightOff();
        stageNum[(Select_Scene - 1)].GetComponent<Light>().intensity = LIGHT_ON;
        AllStagePictureSetActiveFlase();
        //StagePictureActiveTrue(Select_Scene - 1);

        if (Controler.SubMitButtonFlg())
        {
            StageSelect.LoadStage(Select_Scene, this);
        }
    }

    void AllStageLightOff()
    {
        for (int i = 0; i < stageNum.Length; i++)
        {
            stageNum[i].GetComponent<Light>().intensity = LIGHT_OFF;
        }
    }


    void AllStagePictureSetActiveFlase()
    {
        for (int i = 0; i < stage_picture.Length; i++)
        {
            stage_picture[i].SetActive(false);
        }
    }

    void StagePictureActiveTrue(int _num)
    {
        if (!stage_picture[_num].activeSelf)
        {
            AllStagePictureSetActiveFlase();
            stage_picture[_num].GetComponentInChildren<MaterialChangeColor>().Init();
            stage_picture[_num].SetActive(true);
        }
    }
}
