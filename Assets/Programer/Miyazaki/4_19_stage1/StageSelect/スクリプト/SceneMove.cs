using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMove : MonoBehaviour
{
    public GameObject Stage1;
    public GameObject Stage2;
    public GameObject Stage3;
    public GameObject Stage4;

    public GameObject Stage5;
    public GameObject Stage6;
    public GameObject Stage7;
    public GameObject Stage8;

    public GameObject Stage9;
    public GameObject Stage10;
    public GameObject Stage11;
    public GameObject Stage12;

    public GameObject Stage13;
    public GameObject Stage14;
    public GameObject Stage15;
    public GameObject Stage16;

    public GameObject Stage17;
    public GameObject Stage18;
    public GameObject Stage19;
    public GameObject Stage20;

    public StageSelectCamera stageselectcam;

    bool Camera = false;
    bool Camera1 = false;

    int StageNum;

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
        float a = 10;
        float b = 1;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Select_Scene != 20)
            {
                Select_Scene++;
            }
            if (Select_Scene % 4 == 1)
            {
                Camera = true;
            }
            if (Select_Scene == 20)
            {
                Camera = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && Camera == true)
        {
            stageselectcam.OnPlus();
            Camera = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Select_Scene != 0)
            {
                Select_Scene--;

            }
            if (Select_Scene % 4 == 0)
            {
                Camera1 = true;
            }
            if (Select_Scene == 0)
            {
                Select_Scene = 1;
                Camera1 = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && Camera1 == true)
        {
            stageselectcam.OnMinus();
            Camera1 = false;
        }

        switch (Select_Scene)
        {
            case 1:
                {
                    Stage1.GetComponent<Light>().intensity = a;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    StagePictureActiveTrue(0);
                    break;
                }
            case 2:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = a;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    StagePictureActiveTrue(1);

                    break;
                }
            case 3:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = a;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    StagePictureActiveTrue(2);
                    break;
                }
            case 4:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = a;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 5:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = a;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 6:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = a;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 7:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = a;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 8:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = a;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 9:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = a;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 10:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = a;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 11:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = a;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 12:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = a;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 13:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = a;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 14:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = a;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 15:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = a;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 16:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = a;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 17:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = a;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 18:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = a;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 19:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = a;
                    Stage20.GetComponent<Light>().intensity = b;
                    AllStagePictureSetActiveFlase();
                    break;
                }
            case 20:
                {
                    Stage1.GetComponent<Light>().intensity = b;
                    Stage2.GetComponent<Light>().intensity = b;
                    Stage3.GetComponent<Light>().intensity = b;
                    Stage4.GetComponent<Light>().intensity = b;
                    Stage5.GetComponent<Light>().intensity = b;
                    Stage6.GetComponent<Light>().intensity = b;
                    Stage7.GetComponent<Light>().intensity = b;
                    Stage8.GetComponent<Light>().intensity = b;
                    Stage9.GetComponent<Light>().intensity = b;
                    Stage10.GetComponent<Light>().intensity = b;
                    Stage11.GetComponent<Light>().intensity = b;
                    Stage12.GetComponent<Light>().intensity = b;
                    Stage13.GetComponent<Light>().intensity = b;
                    Stage14.GetComponent<Light>().intensity = b;
                    Stage15.GetComponent<Light>().intensity = b;
                    Stage16.GetComponent<Light>().intensity = b;
                    Stage17.GetComponent<Light>().intensity = b;
                    Stage18.GetComponent<Light>().intensity = b;
                    Stage19.GetComponent<Light>().intensity = b;
                    Stage20.GetComponent<Light>().intensity = a;
                    AllStagePictureSetActiveFlase();
                    break;
                }
        }

        if (Controler.SubMitButtonFlg())
        {
            StageSelect.LoadStage(Select_Scene, this);
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
