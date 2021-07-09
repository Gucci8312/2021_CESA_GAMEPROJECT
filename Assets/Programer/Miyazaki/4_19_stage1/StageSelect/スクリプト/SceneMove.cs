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
    public GameObject[] TimeAttackObj;
    public GameObject[] stageNum;
    public Material[] ColorNum;
    bool TimeAttackFlg;
    public GameObject Score;

    public GameObject[] UI;
    bool UI_Flag;
    float UI_Time;

    const int LIGHT_OFF = 1;
    const int LIGHT_ON = 10;

    public FedeOut fedeout;
    public DollyDriver dollyDriver;


    bool StopCamera = false;
    bool StopCamera1 = false;
    [SerializeField] GameObject[] stage_picture;

    public int Select_Scene = 1;
    bool Activeflag;
    GameObject TimeAttackStage;
    GameObject NormalStage;
    GameObject TimeAttackClear;

    //GameObject stageringrotate;
    //StageRingRotate srr;
    // Start is called before the first frame update
    void Start()
    {
        NumControl.InitNum();

        StageControl.SetOpenFlg(0);
        Activeflag = true;
        for (int i = 0; i < stage_picture.Length; i++)
        {
            stage_picture[i].SetActive(false);
        }
        fedeout = GetComponent<FedeOut>();
        Select_Scene = StageControl.GetNowStage();

        for (int i = 0; i < 25; i++)
        {
            //TimeAttackObj[i] = GameObject.Find("TimeAttackStage(" + i + 1 + ")");
            //TimeAttackObj[i].SetActive(false);
            if (!StageControl.GetTimeAttackClearFlg(i))
            {
                stageNum[i].GetComponent<StageRingRotate>().SetRotateFlg(true);
                TimeAttackObj[i].SetActive(false);
            }
        }

        //srr=stageringrotate.GetComponent<StageRingRotate>();
        TimeAttackStage = GameObject.Find("TimeAttackStage");
        NormalStage = GameObject.Find("NormalStage");
        TimeAttackClear = GameObject.Find("TimeAttackClear");
        TimeAttackClear.SetActive(false);
    }
	
	// Update is called once per frame
	void Update()
    {
        if (TimeAttackFlg)
        {
            if (StageControl.GetTimeAttackClearFlg(Select_Scene - 1))
            {
                TimeAttackClear.SetActive(true);
            }
            Score.SetActive(false);
        }
        else
        {
            TimeAttackClear.SetActive(false);
            Score.SetActive(true);
        }

        if (Select_Scene == 1 || Select_Scene == 2)
        {
            Score.SetActive(false);
        }
        //else
        //{
        //    Score.SetActive(true);
        //}

        //+Debug.Log(Select_Scene);
        // スコア表示
        NumControl.DrawScore(StageControl.GetStageParsent(Select_Scene - 1));
        //Debug.Log(Select_Scene);

        if (Select_Scene >= 1 && Select_Scene <= 5)
        {
            dollyDriver.StageNum0();
        }
        if (Select_Scene >= 6 && Select_Scene <= 10)
        {
            dollyDriver.StageNum1();
        }
        if (Select_Scene >= 11 && Select_Scene <= 15)
        {
            dollyDriver.StageNum2();
        }
        if (Select_Scene >= 16 && Select_Scene <= 20)
        {
            dollyDriver.StageNum3();
        }
        if (Select_Scene >= 21 && Select_Scene <= 25)
        {
            dollyDriver.StageNum4();
        }

        if (Controler.GetLBButtonFlg())
        {
            Debug.Log("前のエリアへ");
            if (StageControl.GetOpenFlg(Select_Scene))
            {
                Select_Scene -= 5;
                if (Select_Scene < 1)
                {
                    Select_Scene = 1;
                }
            }
            SoundManager.PlaySeName("選択する際のカーソルが移動する時");
        }
        else if (Controler.GetRBButtonFlg())
        {
            Debug.Log("次のエリアへ");
            if (StageControl.GetOpenFlg(Select_Scene))
            {
                Select_Scene += 5;
                if (!StageControl.GetOpenFlg(Select_Scene))
                {
                    Select_Scene -= 5;
                }
                if (Select_Scene > 25)
                {
                    Select_Scene = 25;
                }
            }
            SoundManager.PlaySeName("選択する際のカーソルが移動する時");
        }

        if (Controler.GetXButtonFlg() && Controler.GetYButtonFlg())
        {
            StageControl.AllStageOpen();
        }

        if (!gameObject.GetComponent<AreaSelectManeger>().GetMenuFlg())
        {
            fedeout.FedeOut_Update();

            if (Controler.GetRightButtonFlg())
            {
                //TimeAttackObj[Select_Scene - 1].GetComponent<StageRingRotate>().SetRotateFlg(false);
                //stageNum[Select_Scene - 1].GetComponent<StageRingRotate>().SetRotateFlg(false);
                TimeAttackFlg = false;

                if (Select_Scene != 25)
                {
                    if (StageControl.GetOpenFlg(Select_Scene))
                    {
                        Select_Scene++;
                    }
                }
                if (Select_Scene == 25)
                {
                    StopCamera = false;
                }
                if (StageControl.GetOpenFlg(Select_Scene))
                {
                    if (Select_Scene % 5 == 1)
                    {
                        StopCamera = true;
                    }
                }
                SoundManager.PlaySeName("選択する際のカーソルが移動する時");
            }
            else if (Controler.GetLeftButtonFlg())
            {
                //TimeAttackObj[Select_Scene - 1].GetComponent<StageRingRotate>().SetRotateFlg(false);
                //stageNum[Select_Scene - 1].GetComponent<StageRingRotate>().SetRotateFlg(false);
                TimeAttackFlg = false;

                if (Select_Scene != 0)
                {
                    Select_Scene--;

                }
                if (Select_Scene == 0)
                {
                    Select_Scene = 1;
                    StopCamera1 = false;
                }
                if (Select_Scene % 5 == 0)
                {
                    StopCamera1 = true;
                }
                SoundManager.PlaySeName("選択する際のカーソルが移動する時");
            }
            if (StopCamera == true)
            {
                dollyDriver.OnPlus();
                StopCamera = false;
            }

            if (StopCamera1 == true)
            {
                dollyDriver.OnMinus();
                StopCamera1 = false;
            }
            if (Controler.GetUpButtonFlg())
            {
                if (StageControl.GetTimeAttackClearFlg(Select_Scene - 1))
                {
                    TimeAttackFlg = !TimeAttackFlg;
                }
            }
            else if (Controler.GetDownButtonFlg())
            {
                if (StageControl.GetTimeAttackClearFlg(Select_Scene - 1))
                {
                    TimeAttackFlg = !TimeAttackFlg;
                }
            }

            if (Controler.GetXButtonFlg() && Controler.GetZButtonFlg())
            {
                StageControl.AllStageOpen();
                for (int i = 0; i < 25; i++)
                {
                    //TimeAttackObj[i] = GameObject.Find("TimeAttackStage(" + i + 1 + ")");
                    //TimeAttackObj[i].SetActive(false);
                    if (StageControl.GetTimeAttackClearFlg(i))
                    {
                        TimeAttackObj[i].SetActive(true);
                    }
                }
            }

            AllStageLightOff();
            //stageNum[(Select_Scene - 1)].GetComponent<Light>().intensity = LIGHT_ON;
            stageNum[(Select_Scene - 1)].GetComponentInChildren<Light>().intensity = LIGHT_ON;
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
                if (TimeAttackFlg == true)
                {
                    Select_Scene += 25;
                }
                StageSelect.LoadStage(Select_Scene, this);
            }
        }

        Release_Stage();
        Blinking_UI();
        if (TimeAttackFlg == true)
        {
            TimeAttackObj[Select_Scene - 1].GetComponent<StageRingRotate>().SetRotateFlg(true);
            stageNum[Select_Scene - 1].GetComponent<StageRingRotate>().SetRotateFlg(false);
            TimeAttackStage.SetActive(true);
            NormalStage.SetActive(false);
        }
        else if (TimeAttackFlg == false)
        {
            TimeAttackObj[Select_Scene - 1].GetComponent<StageRingRotate>().SetRotateFlg(false);
            stageNum[Select_Scene - 1].GetComponent<StageRingRotate>().SetRotateFlg(true);
            TimeAttackStage.SetActive(false);
            NormalStage.SetActive(true);
        }

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
            num += -5;
        }


		if(Select_Scene>=0&&Select_Scene <= 5)
		{
			for (int i = 0; i < 5; i++)
			{


				if (i + 1 == num)
				{
					if (stageNum[i].GetComponent<StageRingRotate>().GetRotateFlg())
					{
						ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 1.0f);
						ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 0.005f);
					}
					else
					{
						ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 1.0f);
						ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 0.005f);
					}

				}
				else
				{
					ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 0.005f);
					ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 0.005f);
				}

			}
		}
		else if (Select_Scene >= 5&&Select_Scene <= 10)
		{
			for (int i = 0; i < 5; i++)
			{


				if (i + 1 == num)
				{
					if (stageNum[i+5].GetComponent<StageRingRotate>().GetRotateFlg())
					{
						ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 1.0f);
						ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 0.005f);
					}
					else
					{
						ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 1.0f);
						ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 0.005f);
					}

				}
				else
				{
					ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 0.005f);
					ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 0.005f);
				}

			}
		}
		else if (Select_Scene >= 10 && Select_Scene <= 15)
		{
			for (int i = 0; i < 5; i++)
			{


				if (i + 1 == num)
				{
					if (stageNum[i + 10].GetComponent<StageRingRotate>().GetRotateFlg())
					{
						ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 1.0f);
						ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 0.005f);
					}
					else
					{
						ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 1.0f);
						ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 0.005f);
					}

				}
				else
				{
					ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 0.005f);
					ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 0.005f);
				}

			}
		}
		else if (Select_Scene >= 15 && Select_Scene <= 20)
		{
			for (int i = 0; i < 5; i++)
			{


				if (i + 1 == num)
				{
					if (stageNum[i + 15].GetComponent<StageRingRotate>().GetRotateFlg())
					{
						ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 1.0f);
						ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 0.005f);
					}
					else
					{
						ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 1.0f);
						ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 0.005f);
					}

				}
				else
				{
					ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 0.005f);
					ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 0.005f);
				}

			}
		}
		else if (Select_Scene >= 20 && Select_Scene <= 25)
		{
			for (int i = 0; i < 5; i++)
			{


				if (i + 1 == num)
				{
					if (stageNum[i + 20].GetComponent<StageRingRotate>().GetRotateFlg())
					{
						ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 1.0f);
						ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 0.005f);
					}
					else
					{
						ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 1.0f);
						ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 0.005f);
					}

				}
				else
				{
					ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * 0.005f);
					ColorNum[i + 5].SetColor("_EmissionColor", ColorNum[i + 5].color * 0.005f);
				}

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
                while (num >= 5)
                {
                    num += -5;
                }
                stageNum[i].GetComponent<Renderer>().material = ColorNum[num];

            }
            else
            {
                stageNum[i].GetComponent<Renderer>().material = ColorNum[10];
            }

        }
    }

    void Blinking_UI()
    {

        UI_Time -= Time.deltaTime;
        if (UI_Time <= 0.0)
        {
            UI_Time = 1.0f;

            if (UI_Flag)
            {
                for (int i = 0; i < UI.Length; i++)
                {
                    UI[i].SetActive(UI_Flag);
                }

                UI_Flag = false;
            }
            else
            {
                for (int i = 0; i < UI.Length; i++)
                {
                    UI[i].SetActive(UI_Flag);
                }
                UI_Flag = true;
            }


            //ここに処理
        }


    }


}



