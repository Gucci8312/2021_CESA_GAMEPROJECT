﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    Vector3 LocalPos;//親オブジェクトからの相対的な場所
    CheckPointCount CheckPointUi;
    PlayerMove player;
    public bool ColFlg;
    Vector3 ColPos;

    // Start is called before the first frame update
    void Start()
    {
        LocalPos = this.transform.localPosition;
        CheckPointUi = GameObject.Find("CheckPointCount").GetComponent<CheckPointCount>();
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //何故か親オブジェクトについていかないので、代入してあげて追従させている
        this.transform.localPosition = LocalPos;
    }

    // 衝突時
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーに当たった時
        if (other.gameObject.tag == "Player")
        {
            if (!player.GetStartFlg())
            {
                if (this.transform.parent.name == "Mobius (" + player.GetNowMobiusNum() + ")")//同じメビウス状にいるかどうか
                {
                    Debug.Log("チェックポイント通過");
                    CheckPointUi.CheckPointNum++;
                    Destroy(this.gameObject);
                    ColFlg = true;
                    ColPos = other.ClosestPointOnBounds(this.transform.position);
                }
            }
        }
    }

    // 衝突時
    //private void OnCollisionEnter(Collision other)
    //{
    //    // プレイヤーに当たった時
    //    if (other.gameObject.tag == "Player")
    //    {
    //        if (!player.GetStartFlg())
    //        {
    //            if (this.transform.parent.name == "Mobius (" + player.GetNowMobiusNum() + ")")//同じメビウス状にいるかどうか
    //            {
    //                Debug.Log("チェックポイント通過");
    //                CheckPointUi.CheckPointNum++;
    //                Destroy(this.gameObject);
    //                ColFlg = true;
    //               // ColPos = other.ClosestPointOnBounds(this.transform.position);
    //            }
    //        }
    //    }
    //}

    public bool GetColFlg()
    {
        return ColFlg;
    }

    public Vector3 GetColPos()
    {
        return ColPos;
    }
}
