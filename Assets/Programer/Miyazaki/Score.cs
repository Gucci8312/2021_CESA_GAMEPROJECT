using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    //int a;
    //public GameObject[] game;
    // Start is called before the first frame update
    GameObject scoreObj;
    void Start()
    {
        scoreObj = GameObject.Find("Num");
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
            this.gameObject.SetActive(false);
			
		}
		
	}

    public void Collision()
    {
        //a++;
        SupureManager.get_supure++;
        scoreObj.GetComponent<ExpantionShrink>().isExpantion = true;
        this.gameObject.SetActive(false);
    }
}
