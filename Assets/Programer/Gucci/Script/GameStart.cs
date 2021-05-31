using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] enemy;

	public GameObject[] obj;
	public GameObject plane;


	//public float StartTime;
	public float StartTime=1.5f;
	int cnt = 0;
	public bool Blinking_Flag;
	// Start is called before the first frame update
	void Start()
    {
		Blinking_Flag = false;
		Blinking_False();
		plane.SetActive(false);
		//player.GetComponent<PlayerMove>().enabled = false;
		//      for (int idx = 0; idx < enemy.Length; idx++)
		//      {
		//          enemy[idx].GetComponent<EnemyMove>().enabled = false;
		//      }
		PauseManager.OnPause();

		Invoke("Active", StartTime);
	}

    // Update is called once per frame
    void Update()
    {
	
		if (cnt == 50)
		{

			SoundManager.PlaySeName("batibati");
			
			Blinking_True();
		}
		if (cnt == 60)
		{
			Blinking_False();
		}
		if (cnt == 70)
		{
			
			Blinking_True();
		}
		if (cnt == 80)
		{

			Blinking_False();
		}
		if (cnt == 90)
		{
			
			Blinking_True();
		}
		if (cnt == 100)
		{
			SoundManager.StopSE();
			Blinking_False();
		}
		if (cnt == 150)
		{
			
			Blinking_True();
			PauseManager.OffPause();
			
		}
		if (cnt == 151)
		{
			Blinking_Flag = true;
		}
			cnt++;
		
    }

    void Active()
    {
		//player.GetComponent<PlayerMove>().enabled = true;
		//for (int idx = 0; idx < enemy.Length; idx++)
		//{
		//    enemy[idx].GetComponent<EnemyMove>().enabled = true;
		//}
	}

	void Blinking_True()
	{
		for (int idx = 0; idx < obj.Length; idx++)
		{
			obj[idx].SetActive(true);
		}
	}
	void Blinking_False()
	{
		for (int idx = 0; idx < obj.Length; idx++)
		{
			obj[idx].SetActive(false);
		}
		PauseManager.OnPause();
	}
}
