using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// メビウスの輪の挙動
public class MoveMobius : MonoBehaviour
{
    // Start is called before the first frame update
    public float MovePower;                 // 移動力
    bool MoveFlg;                           // 移動判定用
    Rigidbody rb;                           // リジットボディ
    GameObject player;
    public GameObject AttachMobius;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーが乗っているとき
        if (MoveFlg)
        {

            if (Input.GetKey(KeyCode.W))
            {
                this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + MovePower, 0.0f);
                // Debug.Log(MoveFlg);
            }
            if (Input.GetKey(KeyCode.S))
            {
                this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - MovePower, 0.0f);
                // Debug.Log(MoveFlg);
            }
            if (Input.GetKey(KeyCode.A))
            {
                this.gameObject.transform.position = new Vector3(transform.position.x - MovePower, transform.position.y, 0f);
                // Debug.Log(MoveFlg);
            }
            if (Input.GetKey(KeyCode.D))
            {
                this.gameObject.transform.position = new Vector3(transform.position.x + MovePower, transform.position.y, 0f);
                // rb.AddForce( MovePower, 0f, 0f);
                // Debug.Log(MoveFlg);
            }
        }


        MoveFlg = false;

        int num = player.GetComponent<PlayerMove>().GetNowMobiusNum();//プレイヤーオブジェクトから現在のメビウスの輪の数字取得

        if (this.name == "Mobius (" + num + ")")//自分が対象のメビウスの輪なら
        {
            //Debug.Log("Mobius (" + num + "):対象のメビウスの輪");
            MoveFlg = true;
        }

    }

    //// 衝突時
    //private void OnCollisionEnter(Collision other)
    //{
    //    // メビウスの輪同士がぶつかったとき
    //    if (other.gameObject.tag == "Mobius")
    //    {
    //        //MoveFlg = false;
    //        AttachMobius = other.gameObject;
    //        Debug.Log("メビウスの輪同士がぶつかった");
    //    }
    //}

    ////// 離れた時
    //private void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.tag == "Mobius")
    //    {
    //        //MoveFlg = true;
    //        AttachMobius = other.gameObject;
    //        Debug.Log("メビウスの輪同士がぶつかった");
    //    }
    //}





}
