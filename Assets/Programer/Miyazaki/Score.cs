using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
	//int a;
	//public GameObject[] game;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("すり抜けた！");
		if (other.tag == "Wa")
		{
			//a++;
			SupureManager.get_supure++;
			Debug.Log(SupureManager.GetScore());
			Debug.Log("GUCCI_BOKE");
			this.gameObject.SetActive(false);
			
		}
		
	}

}
