﻿using System.Collections;
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
