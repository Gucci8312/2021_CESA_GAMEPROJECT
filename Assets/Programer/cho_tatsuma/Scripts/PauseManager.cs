using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    static private GameObject m_player;
    static private GameObject[] m_enemy;
    // Start is called before the first frame update
    void Start()
    {
    }
    static public void GameObjectFindInit()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_enemy = GameObject.FindGameObjectsWithTag("Enemy");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    static public void OnPause()
    {
        m_player.GetComponent<PlayerMove>().enabled = false;
        for(int i = 0; i < m_enemy.Length; i++)
        {
            m_enemy[i].GetComponent<EnemyMove>().enabled = false;
        }
    }

    static public void OffPause()
    {
        m_player.GetComponent<PlayerMove>().enabled = true;
        for (int i = 0; i < m_enemy.Length; i++)
        {
            m_enemy[i].GetComponent<EnemyMove>().enabled = true;
        }
    }
}
