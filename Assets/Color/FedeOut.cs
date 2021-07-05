using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FedeOut : MonoBehaviour
{


	public GameObject w1_stage1;
	public GameObject w1_stage2;
	public GameObject w1_stage3;
	public GameObject w1_stage4;
	public GameObject w1_stage5;
	public GameObject w1_Light1;
	public GameObject w1_Light2;

	public GameObject w2_stage1;
	public GameObject w2_stage2;
	public GameObject w2_stage3;
	public GameObject w2_stage4;
	public GameObject w2_stage5;
	public GameObject w2_Light1;
	public GameObject w2_Light2;

	public GameObject w3_stage1;
	public GameObject w3_stage2;
	public GameObject w3_stage3;
	public GameObject w3_stage4;
	public GameObject w3_stage5;
	public GameObject w3_Light1;
	public GameObject w3_Light2;

	public GameObject w4_stage1;
	public GameObject w4_stage2;
	public GameObject w4_stage3;
	public GameObject w4_stage4;
	public GameObject w4_stage5;
	public GameObject w4_Light1;
	public GameObject w4_Light2;

	public GameObject w5_stage1;
	public GameObject w5_stage2;
	public GameObject w5_stage3;
	public GameObject w5_stage4;
	public GameObject w5_stage5;
	public GameObject w5_Light1;
	public GameObject w5_Light2;

	int cnt;
	int cnt2;
	bool FedeFlag;
	bool flag;
	// Start is called before the first frame update
	void Start()
	{
		cnt = 0;
		FedeFlag = false;
		flag = false;

	}
	void Update()
	{


	}
	public void FedeOut_On()
	{
		FedeFlag = true;
	}

	public bool Getflag()
	{
		return flag;
	}

	// Update is called once per frame
	public void FedeOut_Update()
	{
		//消して点けて消して点けて消して
		if (FedeFlag)
		{
			if (cnt2 == 0)
			{
				SoundManager.PlaySeName("batibati");
				Blinking_False();

			}
			if (cnt2 == 20)
			{
				Blinking_True();
			}
			if (cnt2 == 40)
			{
				Blinking_False();
			}
			if (cnt2 == 60)
			{
				Blinking_True();
				SoundManager.StopSE();
			}
			if (cnt2 == 80)
			{
				Blinking_False();
				flag = true;
			}
			cnt2++;



			cnt++;
			//return false;
			
		}
	}


	void Blinking_True()
	{
		w1_stage1.SetActive(true);
		w1_stage2.SetActive(true);
		w1_stage3.SetActive(true);
		w1_stage4.SetActive(true);
		w1_stage5.SetActive(true);
		w1_Light1.SetActive(true);
		w1_Light2.SetActive(true);

		w2_stage1.SetActive(true);
		w2_stage2.SetActive(true);
		w2_stage3.SetActive(true);
		w2_stage4.SetActive(true);
		w2_stage5.SetActive(true);
		w2_Light1.SetActive(true);
		w2_Light2.SetActive(true);

		w3_stage1.SetActive(true);
		w3_stage2.SetActive(true);
		w3_stage3.SetActive(true);
		w3_stage4.SetActive(true);
		w3_stage5.SetActive(true);
		w3_Light1.SetActive(true);
		w3_Light2.SetActive(true);

		w4_stage1.SetActive(true);
		w4_stage2.SetActive(true);
		w4_stage3.SetActive(true);
		w4_stage4.SetActive(true);
		w4_stage5.SetActive(true);
		w4_Light1.SetActive(true);
		w4_Light2.SetActive(true);

		w5_stage1.SetActive(true);
		w5_stage2.SetActive(true);
		w5_stage3.SetActive(true);
		w5_stage4.SetActive(true);
		w5_stage5.SetActive(true);
		w5_Light1.SetActive(true);
		w5_Light2.SetActive(true);
	}


	void Blinking_False()
	{
		w1_stage1.SetActive(false);
		w1_stage2.SetActive(false);
		w1_stage3.SetActive(false);
		w1_stage4.SetActive(false);
		w1_stage5.SetActive(false);
		w1_Light1.SetActive(false);
		w1_Light2.SetActive(false);
							
		w2_stage1.SetActive(false);
		w2_stage2.SetActive(false);
		w2_stage3.SetActive(false);
		w2_stage4.SetActive(false);
		w2_stage5.SetActive(false);
		w2_Light1.SetActive(false);
		w2_Light2.SetActive(false);
							
		w3_stage1.SetActive(false);
		w3_stage2.SetActive(false);
		w3_stage3.SetActive(false);
		w3_stage4.SetActive(false);
		w3_stage5.SetActive(false);
		w3_Light1.SetActive(false);
		w3_Light2.SetActive(false);
							
		w4_stage1.SetActive(false);
		w4_stage2.SetActive(false);
		w4_stage3.SetActive(false);
		w4_stage4.SetActive(false);
		w4_stage5.SetActive(false);
		w4_Light1.SetActive(false);
		w4_Light2.SetActive(false);
							
		w5_stage1.SetActive(false);
		w5_stage2.SetActive(false);
		w5_stage3.SetActive(false);
		w5_stage4.SetActive(false);
		w5_stage5.SetActive(false);
		w5_Light1.SetActive(false);
		w5_Light2.SetActive(false);
	}
}

	
	
