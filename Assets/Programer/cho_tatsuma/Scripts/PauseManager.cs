using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    //static private GameObject m_player;
    //static private GameObject[] m_enemy;
    //static private GameObject[] m_mobius;
    // Start is called before the first frame update
    static public bool pause_value = false;
    void Start()
    {
    }
    static public void GameObjectFindInit()
    {
        //m_player = GameObject.FindGameObjectWithTag("Player");
        //m_enemy = GameObject.FindGameObjectsWithTag("Enemy");
        //m_mobius = GameObject.FindGameObjectsWithTag("Mobius");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    static public void OnPause()
    {
        pause_value = true;
        PlayerMove.PauseOn();
        EnemyMove.PauseOn();
        MoveLine.StopFlagSet(true);
        MoveMobius.StopFlagSet(true);
    }

    static public void OffPause()
    {
        pause_value = false;
        PlayerMove.PauseOff();
        EnemyMove.PauseOff();
        MoveLine.StopFlagSet(false);
        MoveMobius.StopFlagSet(false);
    }
}
