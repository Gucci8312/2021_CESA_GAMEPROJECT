using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//メビウスが揺れる処理
public class ShakeMobius : MonoBehaviour
{
    private GameObject ShakeMobiusObj;                  //揺らすメビウスオブジェクト
    Vector2 InitPos;                                    //初期位置
    public bool ShakeFlag=false;                        //揺らすかどうか
    public float MaxShakeTime = 0.3f;                   //揺らす最大時間（秒）
    float NowShakeTime;                                 //現在の時間（秒）
    public float ShakePower = 2.0f;                     //揺らす力

    float ThisR;                                        //半径
    // Start is called before the first frame update
    void Start()
    {
        ShakeMobiusObj = this.transform.Find("メビウス.001").gameObject;
        InitPos = ShakeMobiusObj.transform.localPosition;
        NowShakeTime = MaxShakeTime;

        ThisR = (this.GetComponent<SphereCollider>().bounds.size.x + this.GetComponent<SphereCollider>().bounds.size.y) / 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)//時間が止まっていなければ
        {
            ShakeUpdate();
        }
    }

    //ゆれの更新
    private void ShakeUpdate()
    {
        if (ShakeFlag)//揺れているとき
        {
            if (NowShakeTime >= 0)
            {
                ShakeMobiusObj.transform.localPosition = InitPos + ShakeSpherePos(ShakePower);
                NowShakeTime -= Time.deltaTime;
            }
            else
            {
                ShakeFlag = false;
            }
        }
        else//揺れてないとき
        {
            ShakeMobiusObj.transform.localPosition = InitPos;
            NowShakeTime = MaxShakeTime;
        }
    }

    //メビウスをゆれさせる関数（MoveMobiusが呼ぶ）
    public void ShakeOn()
    {
        if (!ShakeFlag)
        {
            ShakeFlag = true;
        }
    }

    //ランダムで座標をずらす
    private Vector2 ShakePos()
    {
        Vector2 pos = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
        return pos;
    }

    //ランダムで座標をずらす（円形）
    private Vector2 ShakeSpherePos(float Radius)
    {
        Vector2 pos = Random.insideUnitSphere*Radius;
        return pos;
    }
}
