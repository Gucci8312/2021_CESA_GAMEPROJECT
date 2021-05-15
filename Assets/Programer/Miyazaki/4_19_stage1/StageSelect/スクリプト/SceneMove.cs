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
    public FedeOut fedeout;


    bool Camera = false;
    bool Camera1 = false;
    [SerializeField] GameObject[] stage_picture;

    public int Select_Scene;
    bool Activeflag;

    // Start is called before the first frame update
    void Start()
    {
        Activeflag = true;
        Select_Scene = 1;
        for (int i = 0; i < stage_picture.Length; i++)
        {
            stage_picture[i].SetActive(false);
        }
        fedeout = GetComponent<FedeOut>();

    }

    // Update is called once per frame
    void Update()
    {
        fedeout.FedeOut_Update();

        if (Controler.GetRightButtonFlg())
        //if(Input.GetKeyDown(KeyCode.RightArrow))
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
        else if (Controler.GetLeftButtonFlg())
        // else if (Input.GetKeyDown(KeyCode.LeftArrow))
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
        if (Camera == true)
        {
            stageselectcam.OnPlus();
            Camera = false;
        }

        if (Camera1 == true)
        {
            stageselectcam.OnMinus();
            Camera1 = false;
        }

        AllStageLightOff();
        stageNum[(Select_Scene - 1)].GetComponent<Light>().intensity = LIGHT_ON;
        StagePictureActiveTrue(Select_Scene - 1);

        if (!gameObject.GetComponent<AreaSelectManeger>().GetMenuFlg())
        {
            if (Controler.SubMitButtonFlg())
            {
                Activeflag = false;
                AllStagePictureSetActiveFlase();
                fedeout.FedeOut_On();

            }

            if (Controler.GetCanselButtonFlg())
            {
                StageSelect.GoTitleScene(this);
            }
            if (fedeout.Getflag())
            {
                StageSelect.LoadStage(Select_Scene, this);
            }
        }
    }

    // @name   AllStageLightOff
    // @brief  すべてのステージのライトをオフにする
    void AllStageLightOff()
    {
        for (int i = 0; i < stageNum.Length; i++)
        {
            stageNum[i].GetComponent<Light>().intensity = LIGHT_OFF;
        }
    }

    // @name   AllStagePictureSetActiveFlase
    // @brief  背景の絵柄を全部非表示にする
    void AllStagePictureSetActiveFlase()
    {
        for (int i = 0; i < stage_picture.Length; i++)
        {
            stage_picture[i].SetActive(false);
        }
    }

    // @name   StagePictureActiveTrue
    // @brief  特定の背景の絵柄を全部表示にする
    void StagePictureActiveTrue(int _num)
    {

        if (!stage_picture[_num].activeSelf && Activeflag == true)
        {
            AllStagePictureSetActiveFlase();
            stage_picture[_num].GetComponentInChildren<MaterialChangeColor>().Init();
            stage_picture[_num].SetActive(true);
        }
    }
}
