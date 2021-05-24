using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    static private GameObject m_player;
    static private GameObject[] m_enemy;
    static private GameObject[] m_mobius;
    // Start is called before the first frame update
    void Start()
    {
    }
    static public void GameObjectFindInit()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_enemy = GameObject.FindGameObjectsWithTag("Enemy");
        m_mobius = GameObject.FindGameObjectsWithTag("Mobius");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    static public void OnPause()
    {
        PlayerMove.PauseOn();
        for(int i = 0; i < m_enemy.Length; i++)
        {
            m_enemy[i].GetComponent<EnemyMove>().enabled = false;
        }
        foreach(var obj in m_mobius)
        {
            if (obj.GetComponent<MoveLine>() != null)
            {
                obj.GetComponent<MoveLine>().enabled = false;
            }
            obj.GetComponent<MoveMobius>().enabled = false;
            obj.GetComponent<EnemyMobius>().enabled = false;
        }
    }

    static public void OffPause()
    {
        PlayerMove.PauseOff();
        for (int i = 0; i < m_enemy.Length; i++)
        {
            m_enemy[i].GetComponent<EnemyMove>().enabled = true;
        }
        foreach (var obj in m_mobius)
        {
            if (obj.GetComponent<MoveLine>() != null)
            {
                obj.GetComponent<MoveLine>().enabled = true;
            }
            obj.GetComponent<MoveMobius>().enabled = true;
            obj.GetComponent<EnemyMobius>().enabled = true;
        }
    }
}
