using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] enemy;

	public GameObject[] obj;



	//public float StartTime;
	float StartTime=3.0f;
	int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {

		Blinking_False();

		player.GetComponent<PlayerMove>().enabled = false;
        for (int idx = 0; idx < enemy.Length; idx++)
        {
            enemy[idx].GetComponent<EnemyMove>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

		if (cnt == 0)
		{

			Blinking_True();
		}
		if (cnt == 20)
		{
			Blinking_False();
		}
		if (cnt == 40)
		{

			Blinking_True();
		}
		if (cnt == 60)
		{

			Blinking_False();
		}
		if (cnt == 80)
		{

			Blinking_True();
		}
		cnt++;
		//Invoke("Active",StartTime);
    }

    void Active()
    {
        player.GetComponent<PlayerMove>().enabled = true;
        for (int idx = 0; idx < enemy.Length; idx++)
        {
            enemy[idx].GetComponent<EnemyMove>().enabled = true;
        }
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

	}
}
