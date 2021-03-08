using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    GameObject[] Mobius = new GameObject[4];                                                          // メビウスの輪
    public float MovePower;                                                                         // 移動力
    public int NowMobius;                                                                           //現在のメビウスの添え字　
    int SaveMobius;                                                                                 //１つ前にいたメビウスの添え字
    public float InsideLength;                                                                      //内側に入ったときの位置を調整する値
    public bool RotateLeftFlg;                                                                      //回転方向が左右のどちらかを判定　true:左　false:右
    public bool InsideFlg;                                                                          //メビウスの輪の内側か外側かを判定　true:内側　false:外側
    public float MobiusCollisonLength;                                                              //メビウスの輪同士で当たったときの判定用の長さ
    int counter;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();                                                             // リジットボディを格納

        for (int i = 0; i < 4; i++)
        {
            Mobius[i] = GameObject.Find("Mobius (" + i + ")");                                        //全てのメビウス取得
        }
        SaveMobius = -1;
    }

    // Update is called once per frame  
    //void FixedUpdate()
    void Update()
    {
        if (InsideFlg)//メビウスの輪の内側
        {
            InsideLength = 22;
        }
        else//外側
        {
            InsideLength = 0;
        }

        if (Mobius[NowMobius] != null)
        {

            Vector2 MobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;                // メビウスの輪の位置を取得
            float hankei = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 +                // 円の半径を取得
                GetComponent<SphereCollider>().bounds.size.x / 2;

            Vector2 PlayerPos = this.GetComponent<SphereCollider>().bounds.center;                             //プレイヤーの位置取得


            Vector2 vecY = MobiusPos - PlayerPos;                                                              //プレイヤーの位置から対象のメビウスへのベクトルを求める(Y軸ベクトル)

            Vector2 vecX;//(X軸ベクトル)
            vecX.x = 0.0f * 0.0f - 1.0f * vecY.y;                                                              //Y軸とZ軸からX軸を求める
            vecX.y = 1.0f * vecY.x - 0.0f * 0.0f;

            float veclength = Mathf.Sqrt(vecY.x * vecY.x + vecY.y * vecY.y);                                   //Yベクトルの長さ計算
            //単位ベクトルにする
            vecY.x = vecY.x / veclength;
            vecY.y = vecY.y / veclength;

            veclength = Mathf.Sqrt(vecX.x * vecX.x + vecX.y * vecX.y);                                         //Xベクトルの長さ計算
            //単位ベクトルにする
            vecX.x = vecX.x / veclength;
            vecX.y = vecX.y / veclength;

            if (veclength > hankei - InsideLength)//対象のメビウスから離れている場合
            {
                this.gameObject.transform.position = new Vector3(this.transform.position.x + vecY.x * MovePower * Time.deltaTime, this.transform.position.y + vecY.y * MovePower * Time.deltaTime, 0);     //対象のメビウスに近づける

            }

            if (RotateLeftFlg)//左回転
            {
                this.gameObject.transform.position = new Vector3(this.transform.position.x - vecX.x * MovePower * Time.deltaTime, this.transform.position.y - vecX.y * MovePower * Time.deltaTime, 0);         //対象のメビウスの周りをまわる

            }
            else//右回転
            {
                this.gameObject.transform.position = new Vector3(this.transform.position.x + vecX.x * MovePower * Time.deltaTime, this.transform.position.y + vecX.y * MovePower * Time.deltaTime, 0);         //対象のメビウスの周りをまわる

            }


            CollisonMobius();


            if (counter > 0)
            {
                counter++;
                if (counter > 500)
                {
                    SaveMobius = NowMobius;
                    counter = 0;
                }
            }
        }
    }


    private void CollisonMobius()//プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定
    {

        Vector2 NowMobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;                // 現在のメビウスの輪の位置を取得
        Vector2 PlayerPos = this.GetComponent<SphereCollider>().bounds.center;                             //プレイヤーの位置取得

        Vector2 NowVec = NowMobiusPos - PlayerPos;
        float NowLength = Mathf.Sqrt(NowVec.x * NowVec.x + NowVec.y * NowVec.y);

        float hankei = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 +                // 円の半径を取得
                GetComponent<SphereCollider>().bounds.size.x / 2;

        Vector2 NextMobiusPos;//次のメビウスの場所
        Vector2 NextVec;
        float NextLength = 0;
        for (int i = 0; i < 4; i++)
        {
            if (i == NowMobius) continue;//現在のメビウスの位置は処理を飛ばす
            if (i == SaveMobius) continue;

            NextMobiusPos = Mobius[i].GetComponent<SphereCollider>().bounds.center;

            Vector2 Vec = NextMobiusPos - NowMobiusPos;
            float VecLength = Mathf.Sqrt(Vec.x * Vec.x + Vec.y * Vec.y);
            if (hankei + hankei > VecLength)
            {
                Vec.x = Vec.x / VecLength;
                Vec.y = Vec.y / VecLength;

                Vector2 CollisonPos;//当たっている場所を計算(接点)
                CollisonPos.x = NowMobiusPos.x + Vec.x * hankei;
                CollisonPos.y = NowMobiusPos.y + Vec.y * hankei;

                NextVec = CollisonPos - PlayerPos;
                NextLength = Mathf.Sqrt(NextVec.x * NextVec.x + NextVec.y * NextVec.y);

                //this.transform.position = new Vector3(CollisonPos.x, CollisonPos.y, 0);


                if ((hankei / 2) + (InsideLength / 2) > NextLength)
                {
                    SaveMobius = NowMobius;
                    NowMobius = i;
                    counter = 1;

                    //内側と外側を反転させる
                    if (InsideFlg)
                    {
                        InsideFlg = false;
                    }
                    else
                    {
                        InsideFlg = true;
                    }

                    //左右移動を反転させる
                    if (RotateLeftFlg)
                    {
                        RotateLeftFlg = false;
                    }
                    else
                    {
                        RotateLeftFlg = true;
                    }
                    Debug.Log("メビウスの輪を切り替えた");

                    break;
                }
            }



            if (MobiusCollisonLength + InsideLength > NowLength + NextLength)
            {

            }
        }


    }


    public int GetNowMobiusNum()
    {
        return NowMobius;
    }

}
