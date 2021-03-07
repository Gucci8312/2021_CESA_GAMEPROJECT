using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの挙動
public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] Mobius = new GameObject[4];                                                        // メビウスの輪
    public float MovePower;                                                                         // 移動力
    public int NowMobius;                                                                           //現在のメビウスの添え字　初期のメビウスの輪
    int SaveMobius;                                                                                 //１つ前にいたメビウスの添え字
    float InsideLength;                                                                             //内側に入ったときの位置を調整する値
    public bool RotateLeftFlg;                                                                      //回転方向が左右のどちらかを判定　true:左　false:右
    public bool InsideFlg;                                                                          //メビウスの輪の内側か外側かを判定　true:内側　false:外側
    int SideCnt;                                                                                    //メビウスの輪に沿った動きにするためメビウスの輪を何回切り替えたかをカウント  2以上で外側内側入れ替える
    float counter;                                                                                  //乗り移るとき、元のメビウスの輪に戻らないようにカウントする値
    Vector2 MobiusSavePos;                                                                          //移動前のメビウスの位置を保存する変数
    Vector2 MoveMobiusSum;                                                                          //移動前と移動後の差分を格納する計算用変数


    public float SlowTime;                                                                          //スローモーション中の時間
    public float NormalTime;                                                                        //通常の時間
    float time;                                                                                     //時間を格納
    Vector2 vecX, vecY;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();                                                             // リジットボディを格納

        for (int i = 0; i < 4; i++)
        {
            Mobius[i] = GameObject.Find("Mobius (" + i + ")");                                        //全てのメビウス取得
        }
        SideCnt = 2;
        SaveMobius = -1;
        time = NormalTime;

        ApproachMobius();//対象のメビウスの輪に近づける
    }

    // Update is called once per frame  
    //void FixedUpdate()
    void Update()
    {
        
        if (Input.GetKey(KeyCode.Space))//スローモーション
        {
            time = SlowTime;
        }
        else//通常
        {
            time = NormalTime;
        }


        if (InsideFlg)//メビウスの輪の内側
        {
            InsideLength = 22;//内側までの距離
        }
        else//外側
        {
            InsideLength = 0;
        }

        if (Mobius[NowMobius] != null)
        {

            ApproachMobius();//対象のメビウスの輪に近づける



            //軌道に沿って左右移動
            if (RotateLeftFlg)//左回転
            {
                this.gameObject.transform.position = new Vector3(this.transform.position.x - vecX.x * MovePower * Time.deltaTime * time, this.transform.position.y - vecX.y * MovePower * Time.deltaTime * time, 0);         //対象のメビウスの周りをまわる
            }
            else//右回転
            {
                this.gameObject.transform.position = new Vector3(this.transform.position.x + vecX.x * MovePower * Time.deltaTime * time, this.transform.position.y + vecX.y * MovePower * Time.deltaTime * time, 0);         //対象のメビウスの周りをまわる
            }

            this.gameObject.transform.position = new Vector3(this.transform.position.x + MoveMobiusSum.x, this.transform.position.y + MoveMobiusSum.y, 0);         //メビウスの動きについていく

            CollisonMobius();//移り先のメビウスの輪を探す

            //移ったときに元のメビウスの輪に戻らないようにカウントする
            if (counter > 0)
            {
                counter += Time.deltaTime;
                if (counter > 2)
                {
                    //移り変わることができるようにする
                    SaveMobius = NowMobius;
                    counter = 0;

                }//if (counter > 2)

            }//if (counter > 0)
            

        }//if (Mobius[NowMobius] != null)

    }//void Update()


    private void ApproachMobius()//対象のメビウスの輪に近づける
    {
        //対象のメビウスの輪を元にプレイヤーの3軸直交単位ベクトルを求める
        Vector2 MobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;                // メビウスの輪の位置を取得
        float hankei = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 +                // メビウスの輪の円の半径を取得
            GetComponent<SphereCollider>().bounds.size.x / 2;

        MoveMobiusSum = MobiusPos - MobiusSavePos;

        MobiusSavePos = MobiusPos;

        Vector2 PlayerPos = this.GetComponent<SphereCollider>().bounds.center;                             //プレイヤーの位置取得
        float PlayHankei = this.GetComponent<SphereCollider>().bounds.size.x / 2 +                     // プレイヤーの円の半径を取得
            GetComponent<SphereCollider>().bounds.size.x / 2;

        vecY = MobiusPos - PlayerPos;                                                              //プレイヤーの位置から対象のメビウスへのベクトルを求める(Y軸ベクトル)
        
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


        //対象のメビウスの軌道に乗せる
        if (veclength > hankei - InsideLength)//対象のメビウスから離れている場合
        {
            this.gameObject.transform.position = new Vector3(this.transform.position.x + vecY.x * MovePower * Time.deltaTime, this.transform.position.y + vecY.y * MovePower * Time.deltaTime, 0);     //対象のメビウスに近づける
        }
    }//private void ApproachMobius()//対象のメビウスの輪に近づける


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

            Vector2 Vec = NextMobiusPos - NowMobiusPos;//対象のメビウスの輪と
            float VecLength = Mathf.Sqrt(Vec.x * Vec.x + Vec.y * Vec.y);
            if (hankei + hankei > VecLength)//メビウスの輪同士の当たり判定
            {
                Vec.x = Vec.x / VecLength;
                Vec.y = Vec.y / VecLength;

                Vector2 CollisonPos;//当たっている場所を計算(接点)
                CollisonPos.x = NowMobiusPos.x + Vec.x * hankei;
                CollisonPos.y = NowMobiusPos.y + Vec.y * hankei;

                NextVec = CollisonPos - PlayerPos;//メビウスの輪同士の接点とプレイヤーの位置のベクトルを計算
                NextLength = Mathf.Sqrt(NextVec.x * NextVec.x + NextVec.y * NextVec.y);//メビウスの輪同士の接点とプレイヤーの位置の長さ計算


                if ((hankei/3)+(InsideLength/2) > NextLength)//プレイヤーと移り先のメビウスの輪が当たった
                {
                    SaveMobius = NowMobius;
                    NowMobius = i;
                    counter = 1;


                    if (SideCnt>=2)//2回切り替えると
                    {
                        //内側と外側を反転させる
                        if (InsideFlg)
                        {
                            InsideFlg = false;
                        }
                        else
                        {
                            InsideFlg = true;
                        }
                        
                        SideCnt = 0;
                    }//if (SideCnt>=2)//2回切り替えると

                    //メビウスの輪を切り替えると左右移動を反転させる
                    if (RotateLeftFlg)
                    {
                        RotateLeftFlg = false;
                    }
                    else
                    {
                        RotateLeftFlg = true;
                    }

                    //Debug.Log("メビウスの輪を切り替えた");
                    SideCnt++;

                    break;
                }//if ((hankei/3)+(InsideLength/2) > NextLength)//プレイヤーと移り先のメビウスの輪が当たった

            }//if (hankei + hankei > VecLength)//メビウスの輪同士の当たり判定

        }//for (int i = 0; i < 4; i++)


    }//private void CollisonMobius()//プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定


    public int GetNowMobiusNum()//現在の乗っているメビウスの輪の数字を返す
    {
        return NowMobius;
    }

    // 衝突時
    // private void OnTriggerEnter(Collider other)
    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("敵と当たった");
        }
    }

    // 離れた時
    private void OnTriggerExit(Collider other)
    //private void OnCollisionExit(Collision other)
    {
       
    }

}
