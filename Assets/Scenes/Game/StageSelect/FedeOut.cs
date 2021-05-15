using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FedeOut : MonoBehaviour
{


	public GameObject stage1;
	public GameObject stage2;
	public GameObject stage3;
	public GameObject stage4;
	public GameObject stage5;

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
			if(cnt2==0)
			{
				stage1.SetActive(false);
				stage2.SetActive(false);
				stage3.SetActive(false);
				stage4.SetActive(false);
				stage5.SetActive(false);
			}
			if (cnt2 == 20)
			{
				stage1.SetActive(true);
				stage2.SetActive(true);
				stage3.SetActive(true);
				stage4.SetActive(true);
				stage5.SetActive(true);
			}
			if (cnt2 == 40)
			{
				stage1.SetActive(false);
				stage2.SetActive(false);
				stage3.SetActive(false);
				stage4.SetActive(false);
				stage5.SetActive(false);
				
			}
			if (cnt2 == 60)
			{
				stage1.SetActive(true);
				stage2.SetActive(true);
				stage3.SetActive(true);
				stage4.SetActive(true);
				stage5.SetActive(true);
				
			}
			if (cnt2 == 80)
			{
				stage1.SetActive(false);
				stage2.SetActive(false);
				stage3.SetActive(false);
				stage4.SetActive(false);
				stage5.SetActive(false);
				flag = true;
			}
			cnt2++;
			
		}
		
		cnt++;
		//return false;
	}
}
