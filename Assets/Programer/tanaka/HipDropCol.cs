using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipDropCol : MonoBehaviour
{

    PlayerMove player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.GetComponent<EnemyMove>().GetInsideFlg()==player.GetInsideFlg())
            {
                if (other.GetComponent<EnemyMove>().GetNowMobiusNum() == player.GetNowMobiusNum())
                {
                    other.GetComponent<EnemyMove>().StanOn();
                    
                }
                
            }
        }
    }
}
