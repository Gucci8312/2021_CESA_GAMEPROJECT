using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipDropCol : MonoBehaviour
{

    PlayerMove player;
    public bool HipDropFlg;

    // Start is called before the first frame update
    void Start()
    {
        HipDropFlg = false;
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
