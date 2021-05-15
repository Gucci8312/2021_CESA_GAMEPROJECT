using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FedeOut : MonoBehaviour
{


	public GameObject stage1;
	public GameObject stage2;
	public GameObject stage3;
	public GameObject stage4;

	int cnt;
	// Start is called before the first frame update
	void Start()
    {
		cnt = 0;
    }
	void Update()
	{


	}
	// Update is called once per frame
	public bool FedeOut_Update()
    {

		//if (cnt == 20)
		//{
			
		//}
		//if (cnt == 40)
		//{
		//	stage2.SetActive(false);
		//}
		//if (cnt == 60)
		//{
		//	stage3.SetActive(false);
		//}
		//if (cnt == 80)
		//{
		//	stage4.SetActive(false);
		//	return true;
		//}
		//cnt++;
		stage1.SetActive(false);
		stage2.SetActive(false);
		stage3.SetActive(false);
		stage4.SetActive(false);
		return true;
	}
}
