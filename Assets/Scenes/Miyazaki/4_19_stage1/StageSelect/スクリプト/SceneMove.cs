using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMove : MonoBehaviour
{
	public GameObject Stage1;
	public GameObject Stage2;
	public GameObject Stage3;
	public GameObject Stage4;
	float LightPower;
	StageSelect stage_select;
	int Select_Scene;
	// Start is called before the first frame update
	void Start()
    {
		Select_Scene = 1;
		stage_select = GetComponent<StageSelect>();
		//LightPower = Stage1.GetComponent<Light>().intensity;
		//LightPower = Stage2.GetComponent<Light>().intensity;
		//LightPower = Stage3.GetComponent<Light>().intensity;
		//LightPower = Stage4.GetComponent<Light>().intensity;
		//Stage1 = GameObject.Find("Stage1");
		//Stage2 = GameObject.Find("Stage2");
		//Stage3 = GameObject.Find("Stage3");
		//Stage4 = GameObject.Find("Stage4");
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
					break;
				}
			case 2:
				{
					Stage1.GetComponent<Light>().intensity = b;
					Stage2.GetComponent<Light>().intensity = a;
					Stage3.GetComponent<Light>().intensity = b;
					Stage4.GetComponent<Light>().intensity = b;
					break;
				}
			case 3:
				{
					Stage1.GetComponent<Light>().intensity = b;
					Stage2.GetComponent<Light>().intensity = b;
					Stage3.GetComponent<Light>().intensity = a;
					Stage4.GetComponent<Light>().intensity = b;
					break;
				}
			case 4:
				{
					Stage1.GetComponent<Light>().intensity = b;
					Stage2.GetComponent<Light>().intensity = b;
					Stage3.GetComponent<Light>().intensity = b;
					Stage4.GetComponent<Light>().intensity = a;
					break;
				}
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			stage_select.LoadStage(1);
		}
	}
}
