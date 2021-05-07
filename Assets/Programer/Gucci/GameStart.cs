using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] enemy;
    //public float StartTime;
     float StartTime=3.0f;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<PlayerMove>().enabled = false;
        for (int idx = 0; idx < enemy.Length; idx++)
        {
            enemy[idx].GetComponent<EnemyMove>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("Active",StartTime);
    }

    void Active()
    {
        player.GetComponent<PlayerMove>().enabled = true;
        for (int idx = 0; idx < enemy.Length; idx++)
        {
            enemy[idx].GetComponent<EnemyMove>().enabled = true;
        }
    }
}
