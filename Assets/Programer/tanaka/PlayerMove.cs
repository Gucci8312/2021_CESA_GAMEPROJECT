﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// プレイヤーの挙動
public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] Mobius;                                                        // メビウスの輪
    public int NowMobiusColor;                                                                      //松井君に渡す用　MobiusuColorから取得した変数格納
    public int NowMobius;                                                                           //現在のメビウスの添え字　初期のメビウスの輪
    int SaveMobius;                                                                                 //１つ前にいたメビウスの添え字
    float InsideLength;                                                                             //内側に入ったときの位置を調整する値
    public bool RotateLeftFlg;                                                                      //回転方向が左右のどちらかを判定　true:左　false:右
    public bool InsideFlg;                                                                          //メビウスの輪の内側か外側かを判定　true:内側　false:外側
    int SideCnt;                                                                                    //メビウスの輪に沿った動きにするためメビウスの輪を何回切り替えたかをカウント  2以上で外側内側入れ替える
    float counter;                                                                                  //乗り移るとき、元のメビウスの輪に戻らないようにカウントする値
    Vector2 MobiusSavePos;                                                                          //移動前のメビウスの位置を保存する変数
    Vector2 MoveMobiusSum;                                                                          //移動前と移動後の差分を格納する計算用変数
    Vector2 vecX, vecY;

    bool TimingInput;                                                                               //タイミング入力を管理する変数　true:入力あり　false:入力なし
    [SerializeField, Range(0, 7)] public int StartPoint;                                            //メビウス上の点の番号
    float MoveAngle;                                                                                //移動量
    int MobiusPointNum;                                                                             //メビウス上の点の総数　今後、点の数を増やす場合publicにする
    bool StartFlg;                                                                                  //初期位置設定用フラグ　最初の一回だけ通る
    GameObject RythmObj;                                                                            //リズムオブジェクト
    Rythm rythm;                                                                                    //リズムスクリプト取得用


    bool CollisionState;                                                                             //当たり判定を外部に渡す変数　treu:当たっている　false:当たっていない

    int EnemyMax;
    int EnemyUpdateCount;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();                                                             // リジットボディを格納

        Mobius = GameObject.FindGameObjectsWithTag("Mobius");

        for (int i = 0; i < Mobius.Length; i++)
        {
            Mobius[i] = GameObject.Find("Mobius (" + i + ")");                                        //全てのメビウス取得
        }

        RythmObj = GameObject.Find("rythm_circle");                                                   //リズムオブジェクト取得
        this.rythm = RythmObj.GetComponent<Rythm>();                                                  //リズムのコード


        MobiusPointNum = 8;
        MoveAngle = 360.0f / MobiusPointNum;


        SideCnt = 2;
        SaveMobius = -1;
        TimingInput = false;
        StartFlg = true;
        counter = 0;
        CollisionState = false;

        EnemyMax = GameObject.FindGameObjectsWithTag("Enemy").Length;
        EnemyUpdateCount = 0;

        //初期位置設定
        Vector2 MobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;
        this.gameObject.transform.position = new Vector3(MobiusPos.x, MobiusPos.y, 0);
        this.gameObject.transform.position += new Vector3(0, 50.0f, 0);



        if (InsideFlg)//メビウスの輪の内側
        {
            InsideLength = 22;//内側までの距離
        }
        else//外側
        {
            InsideLength = 0;
        }

    }

    // Update is called once per frame  
    //void FixedUpdate()
    void Update()
    {
        NowMobiusColor = Mobius[NowMobius].GetComponent<MobiusColor>().GetNowColorNum();//松井君のスクリプトから変数取得


        if (Input.GetKeyDown(KeyCode.R))//リセット処理
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 現在シーンのリロード
        }



        if (StartFlg)
        {
            ApproachMobius();//対象のメビウスの輪に近づける

            for (int i = 0; i < StartPoint; i++)//入力されたポイントの場所に移動させる
            {
                transform.RotateAround(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center, -this.transform.forward, MoveAngle);//右移動
            }



            StartFlg = false;
        }
        else
        {

            if (!Mobius[NowMobius].GetComponent<MoveMobius>().GetFlickMoveFlag())//メビウスが止まっているとき
            {
                Vector2 MobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;                // メビウスの輪の位置を取得

                MoveMobiusSum = MobiusPos - MobiusSavePos;

                MobiusSavePos = MobiusPos;


                float hankei = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 +                // メビウスの輪の円の半径を取得
                   GetComponent<SphereCollider>().bounds.size.x / 2;
                this.gameObject.transform.position = new Vector3(MobiusPos.x, MobiusPos.y, 0);
                this.gameObject.transform.position += new Vector3(0, (hankei - InsideLength), 0);

                for (int i = 0; i < StartPoint; i++)//ポイントの場所に移動させる
                {
                    transform.RotateAround(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center, -this.transform.forward, MoveAngle);//右移動
                }

                TimingInput = this.rythm.checkPlayerMove;//ノーツに合わせられたかを取得



                if (Mobius[NowMobius] != null)
                {

                    if (TimingInput)//キー入力あり
                    {
                        //ずれてしまうバグがあったためコメント化　万が一の際使えるように残しておく
                        //ApproachMobius();//軌道に乗せる

                        if (RotateLeftFlg)
                        {
                            //ずれてしまうバグがあったためコメント化　万が一の際使えるように残しておく
                            //transform.RotateAround(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center, this.transform.forward, MoveAngle);//左移動
                            StartPoint--;
                            if (StartPoint < 0)
                            {
                                StartPoint = 7;
                            }
                        }
                        else
                        {
                            //ずれてしまうバグがあったためコメント化　万が一の際使えるように残しておく
                            //transform.RotateAround(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center, -this.transform.forward, MoveAngle);//右移動
                            StartPoint++;
                            if (StartPoint > 7)
                            {
                                StartPoint = 0;
                            }
                        }

                        this.rythm.checkPlayerMove = false;
                        //this.rythm.rythmCheckFlag = false;
                        counter++;
                        //Debug.Log("移動");
                        //Debug.Log(StartPoint);
                    }//if (TimingInput)



                    this.gameObject.transform.position = new Vector3(this.transform.position.x + MoveMobiusSum.x, this.transform.position.y + MoveMobiusSum.y, 0);         //メビウスの動きについていく


                    CollisonMobius();//移り先のメビウスの輪を探す

                    //移ったときに元のメビウスの輪に戻らないようにカウントする
                    if (counter > 1)//
                    {
                        //移り変わることができるようにする
                        SaveMobius = NowMobius;
                        counter = 0;

                    }//if (counter > 2)

                }//if (Mobius[NowMobius] != null)
            }
        }//else if(StartFlg)

        //Debug.Log(CollisionState);
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

        vecY = PlayerPos - MobiusPos;                                                              //プレイヤーの位置から対象のメビウスへのベクトルを求める(Y軸ベクトル)

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

        this.gameObject.transform.position = new Vector3(MobiusPos.x + (vecY.x * (hankei - InsideLength)), MobiusPos.y + (vecY.y * (hankei - InsideLength)), 0);     //対象のメビウスに近づける



    }//private void ApproachMobius()//対象のメビウスの輪に近づける


    private void CollisonMobius()//プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定
    {

        Vector2 NowMobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;                // 現在のメビウスの輪の位置を取得
        Vector2 PlayerPos = this.GetComponent<SphereCollider>().bounds.center;                             //プレイヤーの位置取得

        Vector2 NowVec = NowMobiusPos - PlayerPos;
        float NowLength = Mathf.Sqrt(NowVec.x * NowVec.x + NowVec.y * NowVec.y);

        float hankei = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.size.x / 2 +                // 円の半径を取得
                GetComponent<SphereCollider>().bounds.size.x / 2;
        float PlayerHankei = this.GetComponent<SphereCollider>().bounds.size.x / 2 +                // 円の半径を取得
                GetComponent<SphereCollider>().bounds.size.x / 2;

        Vector2 NextMobiusPos;//次のメビウスの場所
        Vector2 NextVec;
        float NextLength = 0;
        for (int i = 0; i < Mobius.Length; i++)
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


                if ((PlayerHankei / 2) + InsideLength > NextLength)//プレイヤーと移り先のメビウスの輪が当たった
                {
                    transform.RotateAround(Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center, -this.transform.forward, 180);
                    if (StartPoint >= 4)
                    {
                        StartPoint -= 4;
                    }
                    else
                    {
                        StartPoint += 4;
                    }
                    //Debug.Log(StartPoint);
                    SaveMobius = NowMobius;
                    NowMobius = i;
                    counter = 0;


                    if (SideCnt >= 2)//2回切り替えると
                    {
                        //内側と外側を反転させる
                        if (InsideFlg)
                        {
                            InsideFlg = false;
                            InsideLength = 0;//内側までの距離

                            Debug.Log("外側");
                        }
                        else
                        {
                            InsideLength = 22;//内側までの距離
                            InsideFlg = true;
                            Debug.Log("内側");
                        }


                        SideCnt = 0;
                    }//if (SideCnt>=2)//2回切り替えると
                    ApproachMobius();

                    //メビウスの輪を切り替えると左右移動を反転させる
                    if (RotateLeftFlg)
                    {
                        RotateLeftFlg = false;
                    }
                    else
                    {
                        RotateLeftFlg = true;
                    }

                    Debug.Log("メビウスの輪を切り替えた");
                    SideCnt++;

                    break;
                }//if ((hankei/3)+(InsideLength/2) > NextLength)//プレイヤーと移り先のメビウスの輪が当たった


            }//if (hankei + hankei > VecLength)//メビウスの輪同士の当たり判定

        }//for (int i = 0; i < Mobius.Length; i++)


    }//private void CollisonMobius()//プレイヤーと対象のメビウスの輪以外の一番近いメビウスの輪との判定


    private void StartPosSet()
    {
        Vector2 MobiusPos = Mobius[NowMobius].GetComponent<SphereCollider>().bounds.center;                // メビウスの輪の位置を取得
    }

    public int GetNowMobiusNum()//現在の乗っているメビウスの輪の数字を返す
    {
        return NowMobius;
    }

    // 衝突時
    // private void OnTriggerEnter(Collider other)
    // private void OnCollisionEnter(Collision other)
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            if (!StartFlg)//シーンを読み込むと、実行されてしまうので回避するために1ループ実行されるまで当たり判定は取らない
            {
                CollisionState = true;
                Debug.Log("敵と当たった");
            }
        }
    }

    // 離れた時
    //  private void OnTriggerEnter(Collider other)
    //private void OnCollisionExit(Collision other)
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            CollisionState = false;
            Debug.Log("敵と離れた");
        }
    }



    public bool GetCollisionState()//敵と当たっているかどうかを返す
    {
        return CollisionState;
    }
    public int GetNowMobiusColor()//松井君に渡すための関数
    {
        return NowMobiusColor;
    }

    public void EnemyUpdateCountUp()//敵が何体更新処理が実行されたかカウントする　
    {
        EnemyUpdateCount++;
        if (EnemyMax <= EnemyUpdateCount)//最後の敵の更新処理が終わればrythmSendCheckFlagをfalse
        {
            EnemyUpdateCount = 0;
            this.rythm.rythmSendCheckFlag = false;
        }
    }
    public bool GetStartFlg()
    {
        return StartFlg;
    }
}