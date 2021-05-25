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
    public Material[] ColorNum;

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

		StageControl.SetOpenFlg(0);
		Activeflag = true;
       // Select_Scene = 1;
        for (int i = 0; i < stage_picture.Length; i++)
        {
            stage_picture[i].SetActive(false);
        }
        fedeout = GetComponent<FedeOut>();
        Select_Scene = StageControl.GetNowStage();
		

		if (Select_Scene >= 1 && Select_Scene <= 5)
        {
            stageselectcam.StageNum0();
        }
        if (Select_Scene >= 6 && Select_Scene <= 10)
        {
            stageselectcam.StageNum1();
        }
        if (Select_Scene >= 11 && Select_Scene <= 15)
        {
            stageselectcam.StageNum2();
        }
        if (Select_Scene >= 16 && Select_Scene <= 20)
        {
            stageselectcam.StageNum3();
        }
        if (Select_Scene >= 21 && Select_Scene <= 25)
        {
            stageselectcam.StageNum4();
        }
    }

    // Update is called once per frame
    void Update()
    {
		
		// if(Controler.GetRightTriggerFlg()&&Controler.GetLeftTriggerFlg())
		if (Controler.GetXButtonFlg())
        {
            StageControl.AllStageOpen();
        }

        if (!gameObject.GetComponent<AreaSelectManeger>().GetMenuFlg())
        {
            fedeout.FedeOut_Update();

            if (Controler.GetRightButtonFlg())
            //if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (Select_Scene != 25)
                {
                    if (StageControl.GetOpenFlg(Select_Scene))
                    {
                        Select_Scene++;
                    }
                }
                if (Select_Scene == 25)
                {
                    Camera = false;
                }
                if (StageControl.GetOpenFlg(Select_Scene))
                {
                    if (Select_Scene % 5 == 1)
                    {
                        Camera = true;
                    }
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
                if (Select_Scene % 5 == 0)
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
            //stageNum[(Select_Scene - 1)].GetComponent<Light>().intensity = LIGHT_ON;
			stageNum[(Select_Scene - 1)].GetComponentInChildren< Light >().intensity = LIGHT_ON;
			ChangeColor();
			


			//Color cc=Color.
			// stageNum[(Select_Scene - 1)].GetComponent<Material>().color =Color.white;
			//stageNum[(Select_Scene - 1)].GetComponent<Light>().intensity = LIGHT_ON;
			StagePictureActiveTrue(Select_Scene - 1);

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

		Release_Stage();
	}

    // @name   AllStageLightOff
    // @brief  すべてのステージのライトをオフにする
    void AllStageLightOff()
    {
        for (int i = 0; i < stageNum.Length; i++)
        {
			//stageNum[i].GetComponent<Light>().intensity = LIGHT_OFF;
			stageNum[i].GetComponentInChildren<Light>().intensity = LIGHT_OFF;
			
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

    void ChangeColor()
    {
        int num = Select_Scene;
        while (num > 5)
        {
            num = num <= 5 ? num : num += -5;
        }
        for (int i = 0; i < 5; i++)
        {
			if (StageControl.GetOpenFlg(i))
			{
				ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * ((i + 1) == num ? 1.0f : 0.005f));
			}
        }
       
    }

	void Release_Stage()
	{
		for (int i = 0; i < 25; i++)
		{
			int num = i;
			if (StageControl.GetOpenFlg(i))
			{
				while (num > 5)
				{
					num = num <= 5 ? num : num += -5;
				}
				if(num==5)
				{
					num = 0;
				}
				stageNum[i].GetComponent<Renderer>().material = ColorNum[num];

			}
			else
			{
				stageNum[i].GetComponent<Renderer>().material = ColorNum[5];
			}

		}
	}


}



