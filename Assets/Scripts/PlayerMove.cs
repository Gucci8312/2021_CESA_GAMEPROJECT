using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの挙動
public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;                                                                                   // リジットボディ
    GameObject Mobius;                                                                              // メビウスの輪
    public float MovePower;                                                                         // 移動力
    float Angle;                                                                                    // 角度
    float syahen;
    Vector2 MobiusPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();                                                             // リジットボディを格納
    }

    // Update is called once per frame  
    //void FixedUpdate()
    void Update()
    {
        if (Mobius != null)
        {
            // float hankei = Mobius.GetComponent<SphereCollider>().bounds.size.x / 2 +                // 円の半径を取得
            // GetComponent<SphereCollider>().bounds.size.x / 2;
           // MobiusPos = Mobius.GetComponent<SphereCollider>().bounds.center;                // メビウスの輪の位置を取得
            MobiusPos = Mobius.transform.position;                // メビウスの輪の位置を取得

            this.gameObject.transform.position = new Vector3(MobiusPos.x + Mathf.Cos(Angle) *       // 位置の指定
                syahen, MobiusPos.y + Mathf.Sin(Angle) * syahen, 0.0f);

            Angle -= Mathf.Deg2Rad * (MovePower / 10);                                                //1度分ををラジアンに変換して加える
            
            // 360度を越えると初期化
            if (Angle > 360)
            {
                Angle -= 360;
            }
            else if(Angle<0)
            {
                Angle += 360;
            }
            //Debug.Log(Angle);
        }
    }

    // 衝突時
   // private void OnTriggerEnter(Collider other)
    private void OnCollisionEnter(Collision other)
    {
        // 衝突したものがメビウスの輪の場合
        if (other.gameObject.tag == "Mobius")
        {
           // if (other.gameObject)
            if (other.gameObject.GetComponent<MoveMobius>().GetAttacthMobius())
            {
                Debug.Log("aaaaaaaaaaa");
                if (other.gameObject.name == other.gameObject.GetComponent<MoveMobius>().GetAttacthMobius().name)
                {
                    Debug.Log("プレイヤーが隣接しているメビウスの輪に当たった");
                }
            }
            Mobius = other.gameObject;                                                              // 衝突したメビウスの輪を格納
            this.gameObject.transform.parent = other.gameObject.transform;                          // プレイヤーをメビウスの輪の子にする
            Vector2 PlayerPos = gameObject.transform.position;                                      // プレイヤーの位置を取得
            MobiusPos = Mobius.transform.position;                                                  // メビウスの輪の位置を取得
            syahen = Mobius.GetComponent<SphereCollider>().bounds.size.x / 2 +                  // 斜辺を円のサイズから取得
            (GetComponent<SphereCollider>().bounds.size.x / 2*0.9f);
            float teihen = PlayerPos.x - MobiusPos.x;                                               // 底辺をプレイヤーとメビウスの輪から取得
            Angle = Mathf.Acos(teihen / syahen);                                                    // プレイヤーの現在のいる角度を取得
            rb.useGravity = false;                                                                  // 重力をOFFにする
            Mobius.GetComponent<SphereCollider>().isTrigger = true;
            Debug.Log("メビウスの輪に当たった");
        }
    }

    // 離れた時
    private void OnTriggerExit(Collider other)
    //private void OnCollisionExit(Collision other)
    {
        // 離れたものがメビウスの輪の場合
        if (other.gameObject.tag == "Mobius")
        {
            Mobius.GetComponent<SphereCollider>().isTrigger = false;
            //this.gameObject.transform.parent = null;                                               // 親を未選択にする
            // rb.useGravity = true;                                                                   // 重力をONにする
        }
    }

}
