using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipDropCol : MonoBehaviour
{
    [SerializeField] GameObject AlertObj;   //アラートオブジェクト
    PlayerMove player;                      //プレイヤー
    GameObject[] enemy;                     //敵
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemy.Length; i++)
        {
            if (i == 0)
            {
                enemy[i] = GameObject.Find("Enemy");
            }
            else
            {
                enemy[i] = GameObject.Find("Enemy (" + i + ")");
            }
        }
        AlertObj.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            if (other.GetComponent<EnemyMove>().GetInsideFlg() == player.GetInsideFlg())
            {
                if (other.GetComponent<EnemyMove>().GetNowMobiusNum() == player.GetNowMobiusNum())
                {
                    if (!player.GetJumpNow())
                    {
                        other.GetComponent<EnemyMove>().SetAlert(true);
                        AlertObj.SetActive(true);
                    }
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!player.GetJumpNow())
            {
                AlertObj.SetActive(false);
                other.GetComponent<EnemyMove>().SetAlert(false);
            }

        }
    }

    //範囲にいる敵をスタンにする
    public void EnemyStanOn()
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].GetComponent<EnemyMove>().GetAlertCollision())
            {
                enemy[i].GetComponent<EnemyMove>().StanOn();
            }
        }
    }
}
