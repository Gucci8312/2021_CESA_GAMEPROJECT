using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipDropCollision : MonoBehaviour
{

    GameObject player;
    bool Jump;//ジャンプしているかどうか
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Jump = player.GetComponent<PlayerMove>().GetJumpNow();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            if (Jump)
            {
                other.GetComponent<EnemyMove>().StanOn();//スタンにする
                player.GetComponent<PlayerMove>().CollisionState = false;//ゲームオーバー処理が先に実行されるかもしれないので、
                Debug.Log("ヒップドロップ成功");

            }
        }

    }

    void OnDrawGizmos()//当たり判定描画
    {
        Gizmos.color = new Vector4(0, 0, 1, 0.5f); //色指定
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().bounds.size.x / 2); //中心点とサイズ
    }
}
