using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMove : MonoBehaviour
{
	public GameObject Stage1;
	public GameObject Stage2;
	public GameObject Stage3;
	public GameObject Stage4;

	[SerializeField] GameObject[] stage_picture;
	//public Material Stage1_color;
	//public Material Stage2_color;
	//public Material Stage3_color;
	//public Material Stage4_color;
	

	float LightPower;

	//StageSelect stage_select;

<<<<<<< HEAD

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
        for (int i = 0; i < 5; i++)
        {
            float a = (i + 1) == num ? 1.0f : 0.005f;


            ColorNum[i].SetColor("_EmissionColor", ColorNum[i].color * a);
            ColorNum[i].SetColor("_EmissionColor", ColorNum[i + 6].color * a);
        }


    }

    void Release_Stage()
=======
	int Select_Scene;
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
		for(int i= 0; i < stage_picture.Length; i++)
        {
			stage_picture[i].SetActive(false);
		}
		//stage4_picture.SetActive(false);

	}

	// Update is called once per frame
	void Update()
	{
		float a = 10;
		float b = 1;
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (Select_Scene != 4)
			{
				Select_Scene++;
			}

		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if (Select_Scene != 0)
			{
				Select_Scene--;
			}

		}

		switch (Select_Scene)
		{
			case 1:
				{
					Stage1.GetComponent<Light>().intensity = a;
					Stage2.GetComponent<Light>().intensity = b;
					Stage3.GetComponent<Light>().intensity = b;
					Stage4.GetComponent<Light>().intensity = b;
					StagePictureActiveTrue(0);
					break;
				}
			case 2:
				{
					Stage1.GetComponent<Light>().intensity = b;
					Stage2.GetComponent<Light>().intensity = a;
					Stage3.GetComponent<Light>().intensity = b;
					Stage4.GetComponent<Light>().intensity = b;
					StagePictureActiveTrue(1);

					break;
				}
			case 3:
				{
					Stage1.GetComponent<Light>().intensity = b;
					Stage2.GetComponent<Light>().intensity = b;
					Stage3.GetComponent<Light>().intensity = a;
					Stage4.GetComponent<Light>().intensity = b;
					StagePictureActiveTrue(2);
					break;
				}
			case 4:
				{
					Stage1.GetComponent<Light>().intensity = b;
					Stage2.GetComponent<Light>().intensity = b;
					Stage3.GetComponent<Light>().intensity = b;
					Stage4.GetComponent<Light>().intensity = a;
					AllStagePictureSetActiveFlase();
					break;
				}
		}

		if (Controler.SubMitButtonFlg())
		{
			StageSelect.LoadStage(1,this);
		}
	}

	void AllStagePictureSetActiveFlase()
>>>>>>> f5ab7ec7c429af89752733e72fb77e5076247480
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
